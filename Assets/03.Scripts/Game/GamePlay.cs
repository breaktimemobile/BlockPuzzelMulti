using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum GameMode
{
    Classic,
    Stage,
    Multi,
    Timer
}

public enum End_Stat
{
    timer,
    block,
    boom
}

public class GamePlay : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler
{
    public static GamePlay instance;

    public GameMode gameMode = GameMode.Classic;
    public End_Stat end_Stat = End_Stat.block;


    public List<Block> blockGrid;

    [HideInInspector]
    public Stack<List<BlockInfo>> restoreGrid = new Stack<List<BlockInfo>>();

    ShapeInfo currentShape = null;
    Transform hittingBlock = null;

    List<Block> highlightingBlocks = new List<Block>();

    [SerializeField] private int MaxAllowedRescuePerGame = 0;

    [SerializeField] private int MaxAllowedVideoWatchRescue = 0;

    [HideInInspector]
    public int TotalFreeRescueDone = 0;

    [HideInInspector]
    public int TotalRescueDone = 0;

    public int MoveCount = 0;

    public Sprite BombSprite;
    public Sprite CurrentSprite;

    //public Timer timeSlider;

    public bool isHelpOnScreen = false;

    public int currentID;

    public int BlockCombo = 0;

    [SerializeField] private GameObject LineAnimator;
    [SerializeField] private Text txtAnimatedText;
    [SerializeField] private GameObject ComboAnimator;
    [SerializeField] private Text Txt_Img_Combo_Val;
    [SerializeField] private Text Txt_Combo_Val;

    Animator anim;

    public int Play_Stage_Num = 0;

    public List<int> Mission_Block_Num = new List<int>();
    public List<int> Mission_Block_Val = new List<int>();

    public Stack<List<int>> ReMission_Block_Val = new Stack<List<int>>();

    public List<List<Block>> breakingBlock = new List<List<Block>>();

    bool isplaying = false;
    bool isMulti_Check = false;
    bool isMulti_Double = false;

    public Vector3 score_pos;
    public GameObject bomb_fx;
    int current_color = -1;

    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
    }

    public void Start_Game()
    {

        TinySauce.OnGameStarted();

        switch (gameMode)
        {

            case GameMode.Stage:
            case GameMode.Multi:

                PlayerPrefs.SetInt("size", 8);

                break;

            default:
                break;
        }

        AudioManager.instance.musicSource.UnPause();

        AudioManager.instance.Play_Music_Sound(Music_Sound.ingame_bg);

        isPause = false;

        UIManager.Instance.Touch_active(true);

        if (gameMode.Equals(GameMode.Stage) && Play_Stage_Num < 7) UIManager.Instance.Check_Tuto();

        BlockCombo = 0;
        MoveCount = 0;

        UIManager.Instance.game_Stat = Game_Stat.Game;
        UIManager.Instance.Set_PlayUi();
        GetComponent<GameBoardGenerator>().Set_Size(PlayerPrefs.GetInt("size",8));
        GetComponent<GameBoardGenerator>().GenerateBoard();
        BlockShapeSpawner.Instance.Spawn_Block();

        //스테이지일 경우 미션 생성
        if (gameMode.Equals(GameMode.Stage))
            Set_Mission();

        isplaying = false;
        isMulti_Check = false;
        isMulti_Double = false;

        UIManager.Instance.Img_Multi_Item_Time.fillAmount = 0;

    }

    void Set_Mission()
    {

        //미션 초기화
        Mission_Block_Num.Clear();
        Mission_Block_Val.Clear();
        ReMission_Block_Val.Clear();

        Dictionary<string, object> stage_Info = DataManager.Instance.stage_data[Play_Stage_Num];
        Clear_Stage_Info Find_Stage_Info = DataManager.Instance.state_Player.clear_Stage.Find(x => x.Stage_Id.Equals(Play_Stage_Num));

        int stage_Star = Find_Stage_Info != null ? Find_Stage_Info.Stage_Star : 0;

        switch (stage_Star)
        {

            case 1:
                FireBaseManager.Instance.LogEvent("Stage_Star_1");
                break;
            case 2:
                FireBaseManager.Instance.LogEvent("Stage_Star_2");
                break;

            case 3:
                FireBaseManager.Instance.LogEvent("Stage_Star_3");
                break;
            default:
                break;
        }

        for (int i = 0; i < 3; i++)
        {
            int missionNum = stage_Info["mission_" + (i + 1)].ToString().TryParseInt();
            int clearNum = stage_Info["clear_" + (i + 1)].ToString().TryParseInt();

            Mission_Block_Num.Add(missionNum);

            if (missionNum == 10) clearNum = 0;

            //별개수에 따른 난이도 조정 

            if (Play_Stage_Num >= 7)
            {

                switch (stage_Star)
                {
                    case 0:
                        break;
                    case 1:
                        clearNum += Mathf.RoundToInt(clearNum * 0.3f);
                        break;
                    case 2:
                    case 3:

                        clearNum += Mathf.RoundToInt(clearNum * 0.7f);

                        break;
                    default:
                        break;
                }
            }
            Mission_Block_Val.Add(clearNum);

        }

        //미션 내용 표시
        UIManager.Instance.Set_Mission_Block_Num();

    }

    #region IBeginDragHandler implementation

    /// <summary>
    /// Raises the begin drag event.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentShape != null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
            pos.z = currentShape.transform.localPosition.z;
            currentShape.transform.localPosition = pos;
        }
    }

    #endregion

    #region IPointerDownHandler implementation
    /// <summary>
    /// Raises the pointer down event.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (UIManager.Instance.game_Stat.Equals(Game_Stat.End) || UIManager.Instance.game_Stat.Equals(Game_Stat.Wait))
            return;

        if (eventData.pointerCurrentRaycast.gameObject != null && !isplaying)
        {

            Transform clickedObject = eventData.pointerCurrentRaycast.gameObject.transform;

            if (clickedObject.GetComponent<ShapeInfo>() != null)
            {
                isplaying = true;

                if (clickedObject.transform.childCount > 0)
                {
                    currentShape = clickedObject.GetComponent<ShapeInfo>();
                    Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
                    currentShape.transform.localScale = Vector3.one * 1.75f;
                    currentShape.transform.localPosition = new Vector3(pos.x, pos.y, 0);
                    AudioManager.instance.Play_Effect_Sound(Effect_Sound.block_touch);
                    CurrentSprite = currentShape.blockImage;

                    if (gameMode.Equals(GameMode.Stage) && Play_Stage_Num < 7)
                    {
                        UIManager.Instance.Touch_active(false);
                    }
                }
            }
        }
    }
    #endregion

    bool isTuto = true;

    #region IPointerUpHandler implementation
    /// <summary>
    /// Raises the pointer up event.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        isTuto = true;

        if (currentShape != null && UIManager.Instance.game_Stat.Equals(Game_Stat.Game))
        {
            current_color = currentShape.colorID - 1;

            //튜토리얼 블럭 위치 체크
            //위치 아니면 isTuto false
            if (gameMode.Equals(GameMode.Stage) && Play_Stage_Num < 7 && highlightingBlocks.Count > 0)
            {
                Check_Tuto_Block();

            }

            if (gameMode.Equals(GameMode.Stage) && Play_Stage_Num < 7)
            {
                UIManager.Instance.Touch_active(true);

            }

            Debug.Log("왜 안돌아가닝");

            if (highlightingBlocks.Count > 0 && isTuto)
            {
                if (gameMode.Equals(GameMode.Stage) && Play_Stage_Num == 4)
                {
                    if (UIManager.Instance.tuto_Step == 0)
                    {
                        UIManager.Instance.tuto_Step += 1;
                        UIManager.Instance.Check_Tuto_Over();
                    }

                }

                if (breakingBlock.Count >= 1)
                {
                    foreach (var item in breakingBlock[0])
                    {
                        item.CheckOneBlock(false);
                    }
                }

                //되돌리기 기능 바닥블록 저장/////////////////////////

                List<BlockInfo> blockInfos = new List<BlockInfo>();

                foreach (var item in blockGrid)
                {
                    BlockInfo blockInfo = new BlockInfo
                    {
                        block_Type = item.block_Type,
                        blockID = item.blockID,
                        colorID = item.colorID,
                        durability = item.durability,
                        Ice_durability = item.Ice_durability,
                        isFilled = item.isFilled,
                        isCopyFilled = item.isCopyFilled,
                        Save_block_sprite = item.Save_block_sprite
                    };

                    blockInfos.Add(blockInfo);
                }

                //////////////////////////////////////////////////////


                SetImageToPlacingBlocks();

                restoreGrid.Push(blockInfos);

                //되돌리기 기능 바닥블록 저장/////////////////////////

                List<int[]> reco = new List<int[]>();

                foreach (var item in BlockShapeSpawner.Instance.spawn_Blocks)
                {

                    int[] co = { item.GetComponent<ShapeInfo>().shapeID, item.GetComponent<ShapeInfo>().colorID };
                    reco.Add(co);
                }

                BlockShapeSpawner.Instance.recover_Block.Push(reco);

                ////////////////////////////////////////////////////////


                List<int> add = new List<int>();
                foreach (var item in Mission_Block_Val)
                {
                    add.Add(item);
                }

                ReMission_Block_Val.Push(add);


                BlockShapeSpawner.Instance.spawn_Blocks.Remove(currentShape.gameObject);
                score_pos = currentShape.transform.position;

                ObjectPool.Recycle(currentShape.gameObject);
                currentShape = null;
                CheckBoardStatus();


            }
            else
            {
                isplaying = false;

                currentShape.transform.localPosition = Vector3.zero;
                currentShape.transform.localScale = Vector3.one;

                currentShape = null;
                CurrentSprite = null;

                Debug.Log("왜 안돌아가닝");

            }
        }
    }
    #endregion

    #region IDragHandler implementation
    /// <summary>
    /// Raises the drag event.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnDrag(PointerEventData eventData)
    {
        if (currentShape != null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
            pos = new Vector3(pos.x, (pos.y + 1F), 0F);

            currentShape.transform.position = pos;

            RaycastHit2D hit = Physics2D.Raycast(currentShape.GetComponent<ShapeInfo>().firstBlock.block.position, Vector2.zero, 1);

            if (hit.collider != null)
            {
                if (hittingBlock == null || hit.collider.transform != hittingBlock)
                {

                    hittingBlock = hit.collider.transform;
                    CanPlaceShape(hit.collider.transform);

                }
            }
            else
            {
                foreach (var item in blockGrid)
                {
                    item.ReturnBlockImg();
                }

                StopHighlighting();
            }
        }
    }
    #endregion


    /// <summary>
    /// 스테이지 랜덤 블록 세팅
    /// </summary>
    public void TimerBoom_Spawn()
    {
        List<Block> NoneFill = blockGrid.FindAll(x => x.block_Type.Equals(Block_Type.Basic));

        int Ran = Random.Range(0, NoneFill.Count);

        NoneFill[Ran].SetBlockImage(UIManager.Instance.Sp_Blocks[9 - 1], 1, 9, Random.Range(5, 10));

    }


    /// <summary>
    /// Sets the image to placing blocks.
    /// 이미지를 블록에 배치
    /// </summary>
    void SetImageToPlacingBlocks()
    {
        if (highlightingBlocks != null && highlightingBlocks.Count > 0)
        {
            foreach (Block c in highlightingBlocks)
            {
                c.SetBlockImage(currentShape.blockImage, currentShape.shapeID, currentShape.colorID);
            }
        }

        currentID = currentShape.colorID;
        AudioManager.instance.Play_Effect_Sound(Effect_Sound.block_drop);
    }

    /// <summary>
	/// Determines whether this instance can place shape the specified currentHittingBlock.
    /// 모양을 배치할수 있는지 체크
	/// </summary>
	public bool CanPlaceShape(Transform currentHittingBlock)
    {
        foreach (var item in blockGrid)
        {
            item.ReturnBlockImg();
        }

        Block currentCell = currentHittingBlock.GetComponent<Block>();

        int currentRowID = currentCell.rowID;
        int currentColumnID = currentCell.columnID;

        StopHighlighting();

        bool canPlaceShape = true;
        foreach (ShapeBlock c in currentShape.shapeBlocks)
        {
            Block checkingCell = blockGrid.Find(o => o.rowID == currentRowID + (c.rowID + currentShape.startOffsetX) && o.columnID == currentColumnID + (c.columnID - currentShape.startOffsetY));

            if ((checkingCell == null) || (checkingCell != null && checkingCell.isFilled))
            {
                canPlaceShape = false;
                SetCopyToPlacingBlocks(false);
                highlightingBlocks.Clear();
                break;
            }
            else
            {
                if (!highlightingBlocks.Contains(checkingCell))
                {
                    highlightingBlocks.Add(checkingCell);

                }
            }
        }

        //여기에 놓을수 있다!
        if (canPlaceShape)
        {
            Line_Change_Img();
            SetHighLightImage();
        }

        return canPlaceShape;
    }

    /// <summary>
    /// 미리 보여주기
    /// </summary>
    void SetHighLightImage()
    {
        foreach (Block c in highlightingBlocks)
        {
            c.SetHighlightImage(currentShape.blockImage);
        }
    }

    /// <summary>
    /// 미리 보여주기 멈추기
    /// </summary>
    public void StopHighlighting()
    {
        if (highlightingBlocks != null && highlightingBlocks.Count > 0)
        {
            foreach (Block c in highlightingBlocks)
            {
                c.StopHighlighting();
            }
        }
        hittingBlock = null;
        SetCopyToPlacingBlocks(false);
        highlightingBlocks.Clear();
    }

    public bool isSpawn = false;

    /// <summary>
    /// Raises the unable to place shape event.
    /// </summary>
    public void OnUnableToPlaceShape()
    {

        if ((TotalRescueDone < MaxAllowedRescuePerGame) || MaxAllowedRescuePerGame < 0)
        {
            //GamePlayUI.Instance.ShowRescue(GameOverReason.OUT_OF_MOVES);

        }
        else
        {
            if (isSpawn)
            {
                BlockShapeSpawner.Instance.Block_Change();
            }
            else
            {
                if (UIManager.Instance.game_Stat != Game_Stat.End)
                {
                    end_Stat = End_Stat.block;
                    OnGameOver();
                    Debug.Log("자리없음 게임오버");
                }


            }

        }
    }

    /// <summary>
    /// Raises the game over event.
    /// </summary>
    public void OnGameOver(bool isClear = false)
    {
        if (UIManager.Instance.game_Stat != Game_Stat.End)
        {
            StartCoroutine("Co_GameOver", isClear);
        }
    }

   IEnumerator Co_GameOver(bool isClear = false)
    {

        TinySauce.OnGameFinished(ScoreManager.Instance.GetScore());

        UIManager.Instance.game_Stat = Game_Stat.End;

        List<Block> shuffleBlock = new List<Block>();

        if (!isClear)
        {
            foreach (var item in blockGrid)
            {
                if (item.colorID != -1)
                    shuffleBlock.Add(item);

            }

            shuffleBlock.Shuffle();

            foreach (var item in shuffleBlock)
            {
                item.GameOver();
                yield return new WaitForSeconds(0.02f);
            }

            yield return new WaitForSeconds(0.5f);
        }


        //게임 오버 팝업 오픈 정보 세팅
        switch (gameMode)
        {
            case GameMode.Classic:

                ScoreManager.Instance.Check_HighScore();
                UIManager.Instance.Set_GameOver_UI();

                break;
            case GameMode.Stage:
                //AudioManager.instance.musicSource.Stop();

                int baseStar = Play_Stage_Num >= 7 ? 1 : 3;
                Clear_Stage_Info clear_Stage_Info = new Clear_Stage_Info()
                {
                    Stage_Id = Play_Stage_Num,
                    Stage_Star = baseStar
                };

                UIManager.Instance.Txt_Over_Stage_Item_Val.gameObject.SetActive(isClear);
                UIManager.Instance.Img_Over_Stage_Item.gameObject.SetActive(isClear);

                if (isClear)
                {
                    AudioManager.instance.Play_Effect_Sound(Effect_Sound.popup_result_clear);

                    Clear_Stage_Info Find_Stage_Info = DataManager.Instance.state_Player.clear_Stage.Find(x => x.Stage_Id.Equals(Play_Stage_Num));
                    Dictionary<string, object> stage_Info = DataManager.Instance.stage_data[Play_Stage_Num];

                    Color color;

                    if ((int)stage_Info["star_" + clear_Stage_Info.Stage_Star] - 1 == 0)
                    {
                        ColorUtility.TryParseHtmlString("#CA4EF9", out color);

                    }
                    else
                    {
                        ColorUtility.TryParseHtmlString("#50FC43", out color);

                    }

                    UIManager.Instance.Txt_Over_Stage_Item_Val.color = color;

                    if (Find_Stage_Info == null)
                    {
                        DataManager.Instance.state_Player.clear_Stage.Add(clear_Stage_Info);


                        DataManager.Instance.Get_Item((Item)(int)stage_Info["star_" + clear_Stage_Info.Stage_Star] - 1,
                            (int)stage_Info["reward_" + clear_Stage_Info.Stage_Star]);

                        UIManager.Instance.Txt_Over_Stage_Item_Val.text = "x" + (int)stage_Info["reward_" + clear_Stage_Info.Stage_Star];
                        UIManager.Instance.Img_Over_Stage_Item.sprite = UIManager.Instance.sp_Items[(int)stage_Info["star_" + clear_Stage_Info.Stage_Star] - 1];

                 
                    }
                    else
                    {
                        if (Find_Stage_Info.Stage_Star < 3)
                        {
                            Find_Stage_Info.Stage_Star += 1;

                            DataManager.Instance.Get_Item((Item)(int)stage_Info["star_" + Find_Stage_Info.Stage_Star] - 1,
                          (int)stage_Info["reward_" + Find_Stage_Info.Stage_Star]);


                            UIManager.Instance.Txt_Over_Stage_Item_Val.text = "x" + (int)stage_Info["reward_" + Find_Stage_Info.Stage_Star];
                            UIManager.Instance.Img_Over_Stage_Item.sprite = UIManager.Instance.sp_Items[(int)stage_Info["star_" + Find_Stage_Info.Stage_Star] - 1];

                        }
                        else
                        {

                            UIManager.Instance.Txt_Over_Stage_Item_Val.gameObject.SetActive(false);
                            UIManager.Instance.Img_Over_Stage_Item.gameObject.SetActive(false);
                        }
                    }

                    FireBaseManager.Instance.LogEvent("Stage_Clear");


                    //클리어시 플레이어 정보 저장
                    DataManager.Instance.Save_Player_Data();
                    UIManager.Instance.Set_Stage_Info();
                    UIManager.Instance.Set_Total_Star_Ui();

                }
                else
                {
                    AudioManager.instance.Play_Effect_Sound(Effect_Sound.popup_result_fail);
                    FireBaseManager.Instance.LogEvent("Stage_Fail");

                }

                UIManager.Instance.Set_GameOver_UI(isClear);

                break;
            case GameMode.Multi:
                //블럭이 놓을때 없을때
                Hashtable customRoomProperties = new Hashtable() { { "Score", -9999 } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(customRoomProperties);

                PhotonManager.Instance.Rpc_Game_Over();

                break;

            case GameMode.Timer:
                ScoreManager.Instance.Check_HighScore();

                UIManager.Instance.Set_GameOver_UI();

                break;
            default:
                break;
        }

    }
    /// <summary>
    /// Sets the image to placing blocks.
    /// 이미지를 블록에 배치
    /// </summary>
    void SetCopyToPlacingBlocks(bool stat)
    {
        if (highlightingBlocks != null && highlightingBlocks.Count > 0)
        {
            foreach (Block c in highlightingBlocks)
            {
                c.CopyFill(stat);
            }
        }

    }

    public void Line_Change_Img()
    {

        SetCopyToPlacingBlocks(true);

        int placingShapeBlockCount = highlightingBlocks.Count;

        List<int> updatedRows = new List<int>();
        List<int> updatedColumns = new List<int>();

        List<List<Block>> breakingRows = new List<List<Block>>();
        List<List<Block>> breakingColumns = new List<List<Block>>();

        foreach (Block b in highlightingBlocks)
        {
            if (!updatedRows.Contains(b.rowID))
            {
                updatedRows.Add(b.rowID);
            }
            if (!updatedColumns.Contains(b.columnID))
            {
                updatedColumns.Add(b.columnID);
            }
        }

        foreach (int rowID in updatedRows)
        {
            List<Block> currentRow = GetCopyEntireRow(rowID);
            if (currentRow != null)
            {
                breakingRows.Add(currentRow);

            }
        }

        foreach (int columnID in updatedColumns)
        {
            List<Block> currentColumn = GetCopyEntireColumn(columnID);
            if (currentColumn != null)
            {
                breakingColumns.Add(currentColumn);

            }
        }

        foreach (var item in breakingRows)
        {
            foreach (var blocks in item)
            {
                blocks.ChangeBlockImg(CurrentSprite);

            }
        }

        foreach (var item in breakingColumns)
        {
            foreach (var blocks in item)
            {
                blocks.ChangeBlockImg(CurrentSprite);

            }
        }
    }

    /// <summary>
    /// Checks the board status.
    /// </summary>
    void CheckBoardStatus()
    {
        int placingShapeBlockCount = highlightingBlocks.Count;
        List<int> updatedRows = new List<int>();
        List<int> updatedColumns = new List<int>();

        List<List<Block>> breakingRows = new List<List<Block>>();
        List<List<Block>> breakingColumns = new List<List<Block>>();

        foreach (Block b in highlightingBlocks)
        {
            if (!updatedRows.Contains(b.rowID))
            {
                updatedRows.Add(b.rowID);
            }
            if (!updatedColumns.Contains(b.columnID))
            {
                updatedColumns.Add(b.columnID);
            }
        }

        SetCopyToPlacingBlocks(false);

        highlightingBlocks.Clear();

        foreach (int rowID in updatedRows)
        {
            List<Block> currentRow = GetEntireRow(rowID);
            if (currentRow != null)
            {
                breakingRows.Add(currentRow);

            }
        }

        foreach (int columnID in updatedColumns)
        {
            List<Block> currentColumn = GetEntireColumn(columnID);
            if (currentColumn != null)
            {
                breakingColumns.Add(currentColumn);
            }
        }

        if (breakingRows.Count > 0 || breakingColumns.Count > 0)
        {
            StartCoroutine(BreakAllCompletedLines(breakingRows, breakingColumns, placingShapeBlockCount));

        }
        else
        {
            BlockCombo = 0;

            int double_item = isMulti_Double ? 2 : 1;

            ScoreManager.Instance.AddScore(10 * placingShapeBlockCount * double_item);


            switch (gameMode)
            {
                case GameMode.Classic:
                    break;
                case GameMode.Stage:
                    Set_Mission_Block(1, 10 * placingShapeBlockCount);

                    UIManager.Instance.Set_Mission_Block_Num();
                    Cheek_Mission_Clear();

                    break;
                case GameMode.Multi:
                    break;
                case GameMode.Timer:
                    MoveCount += 1;

                    if (UIManager.Instance.game_Stat != Game_Stat.End)
                    {
                        foreach (var item in blockGrid)
                        {
                            if (item.block_Type.Equals(Block_Type.boom))
                            {
                                item.Check_Boom();
                            }
                        }
                    }

                    if (MoveCount >= 10)
                    {
                        TimerBoom_Spawn();

                        MoveCount = 0;
                    }
                    break;

                default:
                    break;
            }


            BlockShapeSpawner.Instance.FillShapeContainer();
            BlockShapeSpawner.Instance.CheckOnBoardShapeStatus();

        }

        GamePlay.instance.isplaying = false;

        //if (GameController.gameMode == GameMode.BLAST || GameController.gameMode == GameMode.CHALLENGE)
        //{
        //    Invoke("UpdateBlockCount", 0.5F);
        //}
    }

    /// <summary>
	/// Gets the entire row.
	/// </summary>
	/// <returns>The entire row.</returns>
	/// <param name="rowID">Row I.</param>
	List<Block> GetEntireRow(int rowID)
    {
        List<Block> thisRow = new List<Block>();
        for (int columnIndex = 0; columnIndex < GameBoardGenerator.instance.TotalColumns; columnIndex++)
        {
            Block block = blockGrid.Find(o => o.rowID == rowID && o.columnID == columnIndex);

            if (block.isFilled)
            {
                thisRow.Add(block);
            }
            else
            {
                return null;
            }

            if (block.block_Type.Equals(Block_Type.balloon))
            {
                return null;

            }
        }
        return thisRow;
    }

    /// <summary>
    /// Gets the entire row for rescue.
    /// </summary>
    /// <returns>The entire row for rescue.</returns>
    /// <param name="rowID">Row I.</param>
    List<Block> GetEntireRowForRescue(int rowID)
    {
        List<Block> thisRow = new List<Block>();
        for (int columnIndex = 0; columnIndex < GameBoardGenerator.instance.TotalColumns; columnIndex++)
        {
            Block block = blockGrid.Find(o => o.rowID == rowID && o.columnID == columnIndex);
            thisRow.Add(block);
        }
        return thisRow;
    }

    /// <summary>
	/// Gets the entire column.
	/// </summary>
	/// <returns>The entire column.</returns>
	/// <param name="columnID">Column I.</param>
	List<Block> GetEntireColumn(int columnID)
    {
        List<Block> thisColumn = new List<Block>();
        for (int rowIndex = 0; rowIndex < GameBoardGenerator.instance.TotalRows; rowIndex++)
        {
            Block block = blockGrid.Find(o => o.rowID == rowIndex && o.columnID == columnID);
            if (block.isFilled)
            {
                thisColumn.Add(block);
            }
            else
            {
                return null;
            }

            if (block.block_Type.Equals(Block_Type.balloon))
            {
                return null;

            }
        }
        return thisColumn;
    }


    /// <summary>
    /// Gets the entire row.
    /// </summary>
    /// <returns>The entire row.</returns>
    /// <param name="rowID">Row I.</param>
    List<Block> GetCopyEntireRow(int rowID)
    {
        List<Block> thisRow = new List<Block>();
        for (int columnIndex = 0; columnIndex < GameBoardGenerator.instance.TotalColumns; columnIndex++)
        {
            Block block = blockGrid.Find(o => o.rowID == rowID && o.columnID == columnIndex);

            if (block.isCopyFilled || block.isFilled)
            {
                thisRow.Add(block);
            }
            else
            {
                return null;
            }

            if (block.block_Type.Equals(Block_Type.balloon))
            {
                return null;

            }
        }
        return thisRow;
    }

    /// <summary>
    /// Gets the entire column.
    /// </summary>
    /// <returns>The entire column.</returns>
    /// <param name="columnID">Column I.</param>
    List<Block> GetCopyEntireColumn(int columnID)
    {
        List<Block> thisColumn = new List<Block>();
        for (int rowIndex = 0; rowIndex < GameBoardGenerator.instance.TotalRows; rowIndex++)
        {
            Block block = blockGrid.Find(o => o.rowID == rowIndex && o.columnID == columnID);
            if (block.isCopyFilled || block.isFilled)
            {
                thisColumn.Add(block);
            }
            else
            {
                return null;
            }

            if (block.block_Type.Equals(Block_Type.balloon))
            {
                return null;

            }
        }
        return thisColumn;
    }

    /// <summary>
	/// Breaks all completed lines.
    /// 라인 클리어 체크
	/// </summary>
	/// <returns>The all completed lines.</returns>
	/// <param name="breakingRows">Breaking rows.</param>
	/// <param name="breakingColumns">Breaking columns.</param>
	/// <param name="placingShapeBlockCount">Placing shape block count.</param>
	IEnumerator BreakAllCompletedLines(List<List<Block>> breakingRows, List<List<Block>> breakingColumns, int placingShapeBlockCount)
    {
        int TotalBreakingLines = breakingRows.Count + breakingColumns.Count;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

       // LineAnimator.transform.position = score_pos + new Vector3(0,0.4F,0);

        int addLine = 10 * placingShapeBlockCount;
        int TotalScore = 0;

        if (TotalBreakingLines >= 1)
        {
            Debug.Log("current_color " + current_color);
            AudioManager.instance.Play_Effect_Sound(Effect_Sound.destruction);

            //anim.Play("LineBoom");

            if (gameMode.Equals(GameMode.Timer))
            UIManager.Instance.AddSlider(5 * TotalBreakingLines);
        }

        int double_item = isMulti_Double ? 2 : 1;

        SystemLanguage language = (SystemLanguage)DataManager.Instance.state_Player.language;

        if (TotalBreakingLines == 1)
        {
            TotalScore = 80 + (80 * BlockCombo * 2) + addLine;
            ScoreManager.Instance.AddScore(TotalScore * double_item);
            BlockCombo += 1;

            LineAnimator.transform.Find("Img_Good").gameObject.SetActive(false);
            LineAnimator.transform.Find("Img_Awesome").gameObject.SetActive(false);
            LineAnimator.transform.Find("Img_Excelleent").gameObject.SetActive(false);
            LineAnimator.transform.Find("Txt_Good").gameObject.SetActive(false);
            LineAnimator.transform.Find("Txt_Awesome").gameObject.SetActive(false);
            LineAnimator.transform.Find("Txt_Excelleent").gameObject.SetActive(false);

        }
        else if (TotalBreakingLines == 2)
        {
            TotalScore = 180 + (180 * BlockCombo * 2) + addLine;

            ScoreManager.Instance.AddScore(TotalScore * double_item);
            BlockCombo += 1;

            if (language.Equals(SystemLanguage.Japanese))
            {
                LineAnimator.transform.Find("Txt_Good").gameObject.SetActive(true);
                LineAnimator.transform.Find("Txt_Awesome").gameObject.SetActive(false);
                LineAnimator.transform.Find("Txt_Excelleent").gameObject.SetActive(false);
            }
            else
            {
                LineAnimator.transform.Find("Img_Good").gameObject.SetActive(true);
                LineAnimator.transform.Find("Img_Awesome").gameObject.SetActive(false);
                LineAnimator.transform.Find("Img_Excelleent").gameObject.SetActive(false);

            }

            //AudioManager.instance.Play_Effect_Sound(Effect_Sound.Good);



        }
        else if (TotalBreakingLines == 3)
        {
            TotalScore = 280 + (280 * BlockCombo * 2) + addLine;

            ScoreManager.Instance.AddScore(TotalScore * double_item);
            BlockCombo += 1;

            if (language.Equals(SystemLanguage.Japanese))
            {
                LineAnimator.transform.Find("Txt_Good").gameObject.SetActive(false);
                LineAnimator.transform.Find("Txt_Awesome").gameObject.SetActive(true);
                LineAnimator.transform.Find("Txt_Excelleent").gameObject.SetActive(false);
            }
            else
            {

                LineAnimator.transform.Find("Img_Good").gameObject.SetActive(false);
                LineAnimator.transform.Find("Img_Awesome").gameObject.SetActive(true);
                LineAnimator.transform.Find("Img_Excelleent").gameObject.SetActive(false);

            }

            AudioManager.instance.Play_Effect_Sound(Effect_Sound.Awesome);


        }
        else if (TotalBreakingLines >= 4)
        {

            TotalScore = 380 + (380 * BlockCombo * 2) + addLine;

            ScoreManager.Instance.AddScore(TotalScore * double_item);

            if (language.Equals(SystemLanguage.Japanese))
            {
                LineAnimator.transform.Find("Txt_Good").gameObject.SetActive(false);
                LineAnimator.transform.Find("Txt_Awesome").gameObject.SetActive(false);
                LineAnimator.transform.Find("Txt_Excelleent").gameObject.SetActive(true);
            }
            else
            {

                LineAnimator.transform.Find("Img_Good").gameObject.SetActive(false);
                LineAnimator.transform.Find("Img_Awesome").gameObject.SetActive(false);
                LineAnimator.transform.Find("Img_Excelleent").gameObject.SetActive(true);

            }

            AudioManager.instance.Play_Effect_Sound(Effect_Sound.Excelleent);


            BlockCombo += 1;

        }

        if (BlockCombo >= 2)
        {

            if (language.Equals(SystemLanguage.Japanese))
            {
                Txt_Combo_Val.text = " X" + BlockCombo;
                Txt_Combo_Val.transform.parent.gameObject.SetActive(true);
                Txt_Img_Combo_Val.transform.parent.gameObject.SetActive(false);

            }
            else
            {
                Txt_Img_Combo_Val.text = "X" + BlockCombo;
                Txt_Img_Combo_Val.transform.parent.gameObject.SetActive(true);
                Txt_Combo_Val.transform.parent.gameObject.SetActive(false);

            }

            ComboAnimator.SetActive(true);

        }

        Set_Mission_Block(1, TotalScore);

        LineAnimator.SetActive(true);


        yield return 0;

        
        if (breakingRows.Count > 0)
        {
            foreach (List<Block> thisLine in breakingRows)
            {
                foreach (var item in thisLine)
                {

                    if (item.block_Type.Equals(Block_Type.ice))
                    {

                        Set_Mission_Block(8);
                        Set_Mission_Block(item.colorID + 1);

                    }
                    else
                    {
                        Set_Mission_Block(item.colorID + 1);

                    }
                }

                StartCoroutine(BreakThisLine(thisLine));

              
            }
        }

        if (breakingColumns.Count > 0)
        {
            foreach (List<Block> thisLine in breakingColumns)
            {
                foreach (var item in thisLine)
                {
                    if (item.block_Type.Equals(Block_Type.ice))
                    {
                        Set_Mission_Block(8);
                        Set_Mission_Block(item.colorID + 1);

                    }
                    else
                    {
                        Set_Mission_Block(item.colorID + 1);
                    }
                }
            }
        }

        if (breakingRows.Count > 0)
        {
            yield return new WaitForSeconds(0.1F);
        }

        if (breakingColumns.Count > 0)
        {
            foreach (List<Block> thisLine in breakingColumns)
            {
              StartCoroutine(BreakThisLine(thisLine));

            }       
        }

        if (gameMode.Equals(GameMode.Stage))
        {
            bool isClear = true;

            for (int i = 0; i < Mission_Block_Val.Count; i++)
            {
                if (Mission_Block_Val[i] >= 1)
                    isClear = false;
            }

            if (Play_Stage_Num < 7)
            {

                if (isClear)
                {
                    UIManager.Instance.tuto_Step += 1;
                }

                UIManager.Instance.Check_Tuto_Over();

            }

            UIManager.Instance.Btn_Puase.gameObject.SetActive(!isClear);

        }


        yield return new WaitForSeconds(1.0f);

        LineAnimator.SetActive(false);
        ComboAnimator.SetActive(false);


        switch (gameMode)
        {
            case GameMode.Classic:
                break;
            case GameMode.Stage:

                UIManager.Instance.Set_Mission_Block_Num();

                Cheek_Mission_Clear();
    
                break;
            case GameMode.Multi:
                break;

            case GameMode.Timer:
                MoveCount += 1;

                if (UIManager.Instance.game_Stat != Game_Stat.End)
                {
                    foreach (var item in blockGrid)
                    {
                        if (item.block_Type.Equals(Block_Type.boom))
                        {
                            item.Check_Boom();
                        }
                    }
                }

                if (MoveCount >= 10)
                {
                    TimerBoom_Spawn();

                    MoveCount = 0;
                }


                break;
            default:
                break;
        }

        BlockShapeSpawner.Instance.FillShapeContainer();
        BlockShapeSpawner.Instance.CheckOnBoardShapeStatus();

        isplaying = false;

    }

    /// <summary>
    /// Breaks the this line.
    /// 라인 지우기
    /// </summary>
    /// <returns>The this line.</returns>
    /// <param name="breakingLine">Breaking line.</param>
    IEnumerator BreakThisLine(List<Block> breakingLine)
    {
        foreach (Block b in breakingLine)
        {
            b.ClearBlock (current_color);
            yield return new WaitForSeconds(0.1F);
        }

        yield return 0;
    }

    /// <summary>
    /// Determines whether this instance can existing blocks placed the specified OnBoardBlockShapes.
    /// </summary>
    /// <returns><c>true</c> if this instance can existing blocks placed the specified OnBoardBlockShapes; otherwise, <c>false</c>.</returns>
    /// <param name="OnBoardBlockShapes">On board block shapes.</param>
    public bool CanExistingBlocksPlaced(List<ShapeInfo> OnBoardBlockShapes)
    {
        foreach (Block block in blockGrid)
        {
            if (!block.isFilled)
            {
                foreach (ShapeInfo info in OnBoardBlockShapes)
                {
                    bool canPlace = CheckShapeCanPlace(block, info);
                    if (canPlace)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 블럭 한개 남았는지 체크
    /// </summary>
    /// <param name="OnBoardBlockShapes"></param>
    public void CheckOneBlock()
    {

        int Count = 0;

        List<GameObject> shape = new List<GameObject>();
        breakingBlock.Clear();

        foreach (Block block in blockGrid)
        {
            block.CheckOneBlock(false);

            if (!block.isFilled)
            {
                foreach (GameObject info in BlockShapeSpawner.Instance.spawn_Blocks)
                {
                    bool canPlace = CheckShapeCanPlace(block, info.GetComponent<ShapeInfo>());
                    if (canPlace)
                    {
                        shape.Add(info);
                        Count += 1;
                    }
                }
            }
        }

        foreach (GameObject info in BlockShapeSpawner.Instance.spawn_Blocks)
        {
            info.GetComponent<ShapeInfo>().OneBlockAlpha(1);

        }

        //같은 위치 중복제거
        shape = shape.Distinct().ToList();

        if (Count == 1 && shape.Count == 1)
        {
            foreach (GameObject info in BlockShapeSpawner.Instance.spawn_Blocks)
            {
                info.GetComponent<ShapeInfo>().OneBlockAlpha(info.Equals(shape[0]) ? 1 : 0.5f);

            }

            //하나씩 밖에 없당

            //블럭 블러
            //위치 점등
            foreach (var item in breakingBlock[0])
            {
                item.CheckOneBlock(true);
            }
        }
    }

    /// <summary>
    /// Checks the shape can place.
    /// </summary>
    /// <returns><c>true</c>, if shape can place was checked, <c>false</c> otherwise.</returns>
    /// <param name="placingBlock">Placing block.</param>
    /// <param name="placingBlockShape">Placing block shape.</param>
    bool CheckShapeCanPlace(Block placingBlock, ShapeInfo placingBlockShape)
    {
        int currentRowID = placingBlock.rowID;
        int currentColumnID = placingBlock.columnID;
        List<Block> OneBlocks = new List<Block>();

        if (placingBlockShape != null && placingBlockShape.shapeBlocks != null)
        {
            foreach (ShapeBlock c in placingBlockShape.shapeBlocks)
            {
                Block checkingCell = blockGrid.Find(o => o.rowID == currentRowID + (c.rowID + placingBlockShape.startOffsetX) && o.columnID == currentColumnID + (c.columnID - placingBlockShape.startOffsetY));

                if ((checkingCell == null) || (checkingCell != null && checkingCell.isFilled))
                {
                    OneBlocks.Clear();
                    return false;
                }
                else
                {
                    if (!OneBlocks.Contains(checkingCell))
                    {
                        OneBlocks.Add(checkingCell);

                    }
                }
            }
        }

        breakingBlock.Add(OneBlocks);
        return true;
    }

    /// <summary>
    /// 게임 정보 리셋
    /// </summary>
    public void GameReset()
    {
        currentShape = null;
        hittingBlock = null;

        foreach (var item in blockGrid)
        {
            ObjectPool.Recycle(item.gameObject);
        }

        blockGrid.Clear();
        highlightingBlocks.Clear();

        BlockShapeSpawner.Instance.Block_Reset();
    }

    /// <summary>
    /// 재시작
    /// </summary>
    public void Restart()
    {
        string Mode = "";

        switch (gameMode)
        {
            case GameMode.Classic:
                Mode = "Classic";
                break;
            case GameMode.Stage:
                Mode = "Stage";
                break;
            case GameMode.Multi:
                Mode = "Multi";

                break;
            case GameMode.Timer:
                Mode = "Timer";

                break;
            default:
                break;
        }

        FireBaseManager.Instance.LogEvent(Mode+"_Pause_Restart", "Stage", GamePlay.instance.Play_Stage_Num);

        GameReset();

        Start_Game();
    }

    /// <summary>
    /// 다음 스테이지
    /// </summary>
    public void Next_Stage()
    {
        Play_Stage_Num += 1;

        GamePlay.instance.GameReset();

    }


    /// <summary>
    /// 미션 블록 정보 세팅
    /// </summary>
    /// <param name="colorId"></param>
    /// <param name="val"></param>
    public void Set_Mission_Block(int colorId, int val = 1)
    {
        for (int i = 0; i < Mission_Block_Num.Count; i++)
        {
            if (Mission_Block_Num[i].Equals(colorId))
            {
                Mission_Block_Val[i] -= val;

            }
        }

        UIManager.Instance.Set_Mission_Block_Num();
    }


    /// <summary>
    /// 미션 클리어 여부 체크
    /// </summary>
    public void Cheek_Mission_Clear()
    {
        bool isClear = true;

        for (int i = 0; i < Mission_Block_Val.Count; i++)
        {
            if (Mission_Block_Val[i] >= 1)
            {
                isClear = false;
            }
        }

        if (isClear)
        {
            OnGameOver(isClear);

        }
  
        if (UIManager.Instance.game_Stat != Game_Stat.End)
        {
            foreach (var item in blockGrid)
            {
                if (item.block_Type.Equals(Block_Type.boom))
                {
                    item.Check_Boom();
                }
            }
        }



    }

    /// <summary>
    /// 아이템 사용
    /// </summary>
    /// <param name="item"></param>
    public void Use_Item(Item item)
    {
        string Mode = "";

        switch (gameMode)
        {
            case GameMode.Classic:
                Mode = "Classic";
                break;
            case GameMode.Stage:
                Mode = "Stage";
                break;
            case GameMode.Multi:
                Mode = "Multi";

                break;
            case GameMode.Timer:
                Mode = "Timer";

                break;
            default:
                break;
        }

        if (item != Item.multi_double)
        {
            if (gameMode == GameMode.Stage && Play_Stage_Num <= 6)
            {
                UIManager.Instance.tuto_Step += 1;
                UIManager.Instance.Check_Tuto_Over();

            }
            else
            {

                if (DataManager.Instance.state_Player.item_info.Items[(int)item - 1] <= 0)
                {
                    switch (item)
                    {
                        case Item.crystal:
                            break;
                        case Item.rotation:
                            FireBaseManager.Instance.LogEvent(Mode + "_Rotation_Item_Shop");
                            break;
                        case Item.Recover:
                            FireBaseManager.Instance.LogEvent(Mode + "_Undo_Item_Shop");
                            break;
                        case Item.change:
                            FireBaseManager.Instance.LogEvent(Mode + "_Change_Item_Shop");
                            break;
                        case Item.boom:
                            FireBaseManager.Instance.LogEvent(Mode + "_Boom_Item_Shop");
                            break;
                        case Item.multi_double:
                            break;
                        default:
                            break;
                    }
                    UIManager.Instance.No_Item();

                    return;
                }

            }
        }

        foreach (var Grid in blockGrid)
        {
            Grid.CheckOneBlock(false);
        }

        switch (item)
        {
            case Item.crystal:
                break;
            case Item.rotation:

                if ((gameMode == GameMode.Stage && Play_Stage_Num >= 7) || gameMode.Equals(GameMode.Classic) || gameMode.Equals(GameMode.Timer))
                {
                    DataManager.Instance.state_Player.item_info.Items[(int)item - 1] -= 1;
                }

                UIManager.Instance.Set_Item_Txt(item);

                foreach (var block in BlockShapeSpawner.Instance.spawn_Blocks)
                {
                    block.GetComponent<ShapeInfo>().Set_Rotation();
                }

                BlockShapeSpawner.Instance.CheckOnBoardShapeStatus();

                FireBaseManager.Instance.LogEvent(Mode + "_Rotation_Item_Use");
                AudioManager.instance.Play_Effect_Sound(Effect_Sound.rotation_item);

                break;
            case Item.Recover:

                if (restoreGrid.Count == 0)
                    return;

                if ((gameMode == GameMode.Stage && Play_Stage_Num >= 7) || gameMode.Equals(GameMode.Classic) || gameMode.Equals(GameMode.Timer))
                {
                    DataManager.Instance.state_Player.item_info.Items[(int)item - 1] -= 1;
                }
                UIManager.Instance.Set_Item_Txt(item);

                Recover();

                FireBaseManager.Instance.LogEvent(Mode + "_Undo_Item_Use");
                AudioManager.instance.Play_Effect_Sound(Effect_Sound.undo_item);


                break;
            case Item.change:


                isSpawn = true;
                if ((gameMode == GameMode.Stage && Play_Stage_Num >= 7) || gameMode.Equals(GameMode.Classic) || gameMode.Equals(GameMode.Timer))
                {
                    DataManager.Instance.state_Player.item_info.Items[(int)item - 1] -= 1;
                }
                UIManager.Instance.Set_Item_Txt(item);

                BlockShapeSpawner.Instance.Block_Change();

                BlockShapeSpawner.Instance.CheckOnBoardShapeStatus();

                FireBaseManager.Instance.LogEvent(Mode + "_Change_Item_Use");
                AudioManager.instance.Play_Effect_Sound(Effect_Sound.refresh_item);
                break;
            case Item.boom:

                StartCoroutine(Co_Boom(item,Mode));
                AudioManager.instance.Play_Effect_Sound(Effect_Sound.bomb_item);

                break;
            case Item.multi_double:


                if (!isMulti_Check)
                {
                    isMulti_Check = true;
                    StartCoroutine("Co_Multi_Double");
                    BlockShapeSpawner.Instance.CheckOnBoardShapeStatus();
                    FireBaseManager.Instance.LogEvent("Multi_Double_Free_Item");
                    AudioManager.instance.Play_Effect_Sound(Effect_Sound.score_x2_item);

                }
                else
                {
                    if (!isMulti_Double && DataManager.Instance.Check_Crystal(10))
                    {
                        BlockShapeSpawner.Instance.CheckOnBoardShapeStatus();

                        DataManager.Instance.state_Player.crystal -= 10;
                        DataManager.Instance.Save_Player_Data();
                        UIManager.Instance.Set_Item_Txt(Item.crystal);
                        StartCoroutine("Co_Multi_Double");
                        FireBaseManager.Instance.LogEvent("Multi_Double_Item");
                        AudioManager.instance.Play_Effect_Sound(Effect_Sound.score_x2_item);

                    }
                }

                break;
            default:
                break;
        }

        DataManager.Instance.Save_Player_Data();
    }

    IEnumerator Co_Boom(Item item,string Mode)
    {
        bomb_fx.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        if ((gameMode == GameMode.Stage && Play_Stage_Num >= 7) || gameMode.Equals(GameMode.Classic) || gameMode.Equals(GameMode.Timer))
        {
            DataManager.Instance.state_Player.item_info.Items[(int)item - 1] -= 1;
        }

        UIManager.Instance.Set_Item_Txt(item);

        if(blockGrid.Count > 0)  AudioManager.instance.Play_Effect_Sound(Effect_Sound.destruction);

        foreach (var block in blockGrid)
        {
            block.Boom_Item();
        }

        BlockShapeSpawner.Instance.CheckOnBoardShapeStatus();
        
        FireBaseManager.Instance.LogEvent(Mode + "_Boom_Item_Use");

        bomb_fx.SetActive(false);


    }

    IEnumerator Co_Multi_Double()
    {
        UIManager.Instance.Start_Multi_Item();

        isMulti_Double = true;

        float Item_Time = 10;

        while (Item_Time > 0)
        {
            UIManager.Instance.Txt_Multi_Sec.text = ((int)Item_Time).ToString();
            UIManager.Instance.Img_Multi_Item_Time.fillAmount = Item_Time / 10.0f;
            yield return new WaitForSeconds(0.1f);
            Item_Time -= 0.1f;
        }

        UIManager.Instance.Img_Multi_Item_Time.fillAmount = 0;
        isMulti_Double = false;

        UIManager.Instance.Change_Multi_Item();
    }
    /// <summary>
    /// 되돌리기 아이템
    /// </summary>
    public void Recover()
    {

        List<BlockInfo> blockinfos = restoreGrid.Pop();

        for (int i = 0; i < blockinfos.Count; i++)
        {
            blockGrid[i].block_Type = blockinfos[i].block_Type;
            blockGrid[i].blockID = blockinfos[i].blockID;
            blockGrid[i].colorID = blockinfos[i].colorID;
            blockGrid[i].durability = blockinfos[i].durability;
            blockGrid[i].Ice_durability = blockinfos[i].Ice_durability;
            blockGrid[i].isFilled = blockinfos[i].isFilled;
            blockGrid[i].isCopyFilled = false;
            blockGrid[i].Save_block_sprite = blockinfos[i].Save_block_sprite;
            blockGrid[i].Recover();
        }

        if (gameMode.Equals(GameMode.Stage))
        {
            Mission_Block_Val = ReMission_Block_Val.Pop();
            switch (gameMode)
            {
                case GameMode.Classic:
                    break;
                case GameMode.Stage:
                    UIManager.Instance.Set_Mission_Block_Num();

                    break;
                case GameMode.Multi:
                    break;
                default:
                    break;
            }

        }

        ScoreManager.Instance.Recover();
        BlockShapeSpawner.Instance.Recover();
        UIManager.Instance.Set_Mission_Block_Num();

    }

    /// <summary>
    /// 튜토리얼 블럭 위치 체크
    /// 해당 위치가 아니면 못 놓는다
    /// </summary>
    public void Check_Tuto_Block()
    {
        Dictionary<string, object> tuto_info = DataManager.Instance.tutorial_data[Play_Stage_Num];

        if ((string)tuto_info["check_block"] != "" && UIManager.Instance.tuto_Step < (int)tuto_info["step"])
        {

            string[] tuto_block = tuto_info["check_block"].ToString().Split('/');

            switch (tuto_block.Length)
            {
                case 1:
                    string[] block_1 = tuto_block[0].Split('_');
                    if ((highlightingBlocks[0].rowID == int.Parse(block_1[0]) && highlightingBlocks[0].columnID == int.Parse(block_1[1])))
                    {
                        isTuto = true;
                    }
                    else
                    {
                        foreach (var item in blockGrid)
                        {
                            item.ReturnBlockImg();
                        }

                        StopHighlighting();

                        isTuto = false;

                    }
                    break;

                case 2:
                    block_1 = tuto_block[0].Split('_');
                    string[] block_2 = tuto_block[1].Split('_');

                    if ((highlightingBlocks[0].rowID == int.Parse(block_1[0]) && highlightingBlocks[0].columnID == int.Parse(block_1[1]))
                    || (highlightingBlocks[0].rowID == int.Parse(block_2[0]) && highlightingBlocks[0].columnID == int.Parse(block_2[1])))
                    {
                        isTuto = true;
                    }
                    else
                    {
                        foreach (var item in blockGrid)
                        {
                            item.ReturnBlockImg();
                        }

                        StopHighlighting();

                        isTuto = false;

                    }
                    break;
                case 3:
                    block_1 = tuto_block[0].Split('_');
                    block_2 = tuto_block[1].Split('_');
                    string[] block_3 = tuto_block[2].Split('_');

                    if ((highlightingBlocks[0].rowID == int.Parse(block_1[0]) && highlightingBlocks[0].columnID == int.Parse(block_1[1]))
                    || (highlightingBlocks[0].rowID == int.Parse(block_2[0]) && highlightingBlocks[0].columnID == int.Parse(block_2[1]))
                    || (highlightingBlocks[0].rowID == int.Parse(block_3[0]) && highlightingBlocks[0].columnID == int.Parse(block_3[1])))
                    {
                        isTuto = true;
                    }
                    else
                    {
                        foreach (var item in blockGrid)
                        {
                            item.ReturnBlockImg();
                        }

                        StopHighlighting();

                        isTuto = false;

                    }
                    break;
            }
        }
        else
        {

        }
    }

    public bool isPause = false;

    public void Pause()
    {
        string Mode = "";

        switch (gameMode)
        {
            case GameMode.Classic:
                Mode = "Classic";
                break;
            case GameMode.Stage:
                Mode = "Stage";
                break;
            case GameMode.Multi:
                Mode = "Multi";

                break;
            case GameMode.Timer:
                Mode = "Timer";

                break;
            default:
                break;
        }

        if (isPause)
        {
            FireBaseManager.Instance.LogEvent(Mode + "_Pause_Play");

            AudioManager.instance.musicSource.UnPause();

        }
        else
        {
            FireBaseManager.Instance.LogEvent(Mode + "_Pause");

            AudioManager.instance.musicSource.Pause();

        }


        isPause = !isPause;

        //Time.timeScale = isPause ? 0 : 1;
        UIManager.Instance.Touch_active(!isPause);
    }


    IEnumerator Co_End_Check()
    {
        yield return null;
    }


}
