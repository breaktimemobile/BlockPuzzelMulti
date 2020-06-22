using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

#if HBDOTween
using DG.Tweening;
#endif


public class GameBoardGenerator : MonoBehaviour
{

    public static GameBoardGenerator instance;

    /// Total Rows, Configurable from inspector.
    public int TotalRows = 8;

    /// Total Column Count, Configurable from inspector.
    public int TotalColumns = 8;

    /// Space between each blocks, Configurable from inspector.
    public int blockSpace = 5;

    /// The content of the board.
    public GameObject BoardContent;

    /// The empty block template.

    /// The empty block template.
    public GameObject blockPanel;

    int startPosx = 0;
    int startPosy = 0;

    int blockWidth = 0;
    int blockHeight = 0;

    int cellIndex = 0;

    private void Awake()
    {
        instance = this;
    }

    public void Set_Size(int size)
    {
        TotalRows = size;
        TotalColumns = size;
    }

    /// <summary>
    /// 시작시 바닥 블록 뿌려주기
    /// </summary>
    public void GenerateBoard()
    {

        blockHeight = (int)880 / TotalColumns;
        blockWidth = (int)880 / TotalRows;

        startPosx = -(((TotalColumns - 1) * (blockHeight + blockSpace)) / 2);
        startPosy = (((TotalRows - 1) * (blockWidth + blockSpace)) / 2);

        int newPosX = startPosx;
        int newPosY = startPosy;

        for (int row = 0; row < TotalRows; row++)
        {
            List<Block> thisRowCells = new List<Block>();
            for (int column = 0; column < TotalColumns; column++)
            {

                GameObject newCell = GenerateNewBlock(row, column);
                newCell.GetComponent<RectTransform>().sizeDelta = new Vector2(blockWidth, blockHeight);
                newCell.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(newPosX, (newPosY), 0);
                newPosX += (blockWidth + blockSpace);
                Block thisCellInfo = newCell.GetComponent<Block>();
                thisCellInfo.blockImage = newCell.transform.GetChild(0).GetComponent<Image>();
                thisCellInfo.rowID = row;
                thisCellInfo.columnID = column;
                thisRowCells.Add(thisCellInfo);
                thisCellInfo.Init();
                newCell.SetActive(false);
                cellIndex++;
            }

            GamePlay.instance.blockGrid.AddRange(thisRowCells);
            newPosX = startPosx;
            newPosY -= (blockHeight + blockSpace);
        }

        StartCoroutine(Co_Spawn_Anim());

        switch (GamePlay.instance.gameMode)
        {
            case GameMode.Classic:
                break;
            case GameMode.Stage:
                RandomBlock_Spawn();

                break;
            case GameMode.Multi:
                break;
            default:
                break;
        }

    }


    IEnumerator Co_Spawn_Anim()
    {
        AudioManager.instance.Play_Effect_Sound(Effect_Sound.map_tiles_created);

        int Start_Block = TotalColumns - 1;

        List<int> Spawn_block = new List<int>();


        for (int i = 0; i <= Start_Block * 2; i++)
        {
            List<int> Spawn = new List<int>();


            foreach (var item in Spawn_block)
            {
                Spawn.Add(item);
            }

            Spawn_block.Clear();

            int Miu = i <= Start_Block ? 1 : -1;

            for (int j = 0; j < Spawn.Count + Miu; j++)
            {
                if (i == 0)
                {
                    Spawn_block.Add(Start_Block);
                }
                else if (i <= Start_Block)
                {
                    if (j >= Spawn.Count)
                        Spawn_block.Add(Spawn[Spawn.Count - 1] + TotalColumns);
                    else
                        Spawn_block.Add(Spawn[j] - 1);
                }
                else
                {
                    Spawn_block.Add(Spawn[j] + TotalColumns);
                }
            }

            foreach (var item in Spawn_block)
            {
                GamePlay.instance.blockGrid[item].gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(0.02f);

        }
    }

    /// <summary>
    /// 블록 정보 세팅
    /// </summary>
    /// <returns>The new block.</returns>
    /// <param name="rowIndex">Row index.</param>
    /// <param name="columnIndex">Column index.</param>
    GameObject GenerateNewBlock(int rowIndex, int columnIndex)
    {
        GameObject newBlock = ObjectPool.Spawn(UIManager.Instance.emptyBlockTemplate, transform, transform.position);
        newBlock.GetComponent<RectTransform>().sizeDelta = new Vector2((blockWidth), (blockHeight));
        newBlock.transform.SetParent(BoardContent.transform);
        newBlock.transform.localScale = Vector3.one;
        newBlock.transform.SetAsLastSibling();
        newBlock.name = "Block-" + rowIndex.ToString() + "-" + columnIndex.ToString();
        return newBlock;
    }


    /// <summary>
    /// 스테이지 랜덤 블록 세팅
    /// </summary>
    public void RandomBlock_Spawn()
    {

        Dictionary<string, object> stage_Info = DataManager.Instance.stage_data[GamePlay.instance.Play_Stage_Num];

        if ((int)stage_Info["stage_Type"] == 0)
        {
            //튜토리얼////////////////////////////////////////////////////////////////////////
            Dictionary<string, object> tuto_Info = DataManager.Instance.tutorial_data[GamePlay.instance.Play_Stage_Num];

            for (int i = 0; i < 6; i++)
            {

                string block_Num = (string)tuto_Info["block_num_" + i];
                if (block_Num != "")
                {
                    string[] str_block = block_Num.Split('_');

                    int block_color = Random.Range(0, 6);

                    foreach (var item in str_block)
                    {
                        if (i == 4 && GamePlay.instance.Play_Stage_Num == 2) block_color = 6;

                        GamePlay.instance.blockGrid[int.Parse(item)].SetBlockImage(UIManager.Instance.Sp_Blocks[block_color], 1, block_color + 1);

                    }
                }
            }
            //////////////////////////////////////////////////////////////////////////////////
        }
        else
        {
            //일반////////////////////////////////////////////////////////////////////////

            for (int i = 1; i < 4; i++)
            {
                int block_Num = (int)stage_Info["random_block_" + i];
                int block_Val = (int)stage_Info["block_count_" + i];
                int block_Hp = (int)stage_Info["block_hp_" + i];

                List<Block> NoneFill = GamePlay.instance.blockGrid.FindAll(x => x.block_Type.Equals(Block_Type.None));

                //Debug.Log("NoneFill  " + NoneFill.Count);
                Clear_Stage_Info Find_Stage_Info = DataManager.Instance.state_Player.clear_Stage.Find(x => x.Stage_Id.Equals(GamePlay.instance.Play_Stage_Num));

                int stage_Star = Find_Stage_Info != null ? Find_Stage_Info.Stage_Star : 0;


                switch (stage_Star)
                {
                    case 0:
                        break;
                    case 1:
                        block_Val += Mathf.RoundToInt(block_Val * 0.3f);
                        break;
                    case 2:
                    case 3:

                        block_Val += Mathf.RoundToInt(block_Val * 0.7f);

                        break;
                    default:
                        break;
                }

                Debug.Log("block_Num  " + block_Num + " block_Val  " + block_Val + "block_Hp  " + block_Hp);

                int Mission_val = block_Val * (block_Hp + 1);

                // mission 8 block_Num 7
                if (block_Num == 7)
                {
                    int mission = 0;

                    int clearNum = 0;

                    for (int j = 1; j < 3; j++)
                    {
                        mission = (int)stage_Info["mission_" + j];
                        if (mission == 8)
                        {
                            clearNum = (int)stage_Info["clear_" + j];
                            break;
                        }
                    }

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


                    Debug.Log("block " + Mission_val + "clear " + clearNum);

                    int[] Random = getRandomInt(1, 0, NoneFill.Count);

                    foreach (var item in Random)
                    {
                        NoneFill[item].SetBlockImage(UIManager.Instance.Sp_Blocks[block_Num - 1], 1, block_Num, clearNum - Mission_val - 1);
                    }
                }

                //랜덤으로 자리 배치
                if (block_Num >= 1)
                {
                    int[] Random = getRandomInt(block_Val, 0, NoneFill.Count);

                    foreach (var item in Random)
                    {
                        NoneFill[item].SetBlockImage(UIManager.Instance.Sp_Blocks[block_Num - 1], 1, block_Num, block_Hp);
                    }
                }
            }

            //////////////////////////////////////////////////////////////////////////////////

        }

    }

    /// <summary>
    /// 범위안에 랜덤 인트값 가져오기
    /// </summary>
    /// <param name="length"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public int[] getRandomInt(int length, int min, int max)
    {

        int[] randArray = new int[length];
        bool isSame;

        for (int i = 0; i < length; ++i)
        {
            while (true)
            {
                randArray[i] = Random.Range(min, max);
                isSame = false;

                for (int j = 0; j < i; ++j)
                {
                    if (randArray[j] == randArray[i])
                    {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame) break;
            }
        }
        return randArray;
    }
}