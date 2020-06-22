using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BlockShapeSpawner : MonoBehaviour
{
    public static BlockShapeSpawner Instance;


    [HideInInspector] public ShapeBlockList activeShapeBlockModule;         //소환 블록 기본 정보 카피 용도

    [SerializeField] Transform[] shapeContainers;                           //소환 블록 위치

    List<int> shapeBlockProbabilityPool = new List<int>();                  //스테이지 별 소환 블록 저장

    int shapeBlockPoolCount = 1;

    public List<GameObject> spawn_Blocks = new List<GameObject>();          //스폰된 하단 블록

    public Stack<List<int[]>> recover_Block = new Stack<List<int[]>>();     //되돌리는 하단 블럭 정보


    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        Instance = this;

        activeShapeBlockModule = UIManager.Instance.shapeBlockList;
    }

    /// <summary>
    /// 블록 소환
    /// </summary>
    public void Spawn_Block()
    {
        isTutoSpawn = false;
        isTutoChange = false;
        CreateShapeBlockProbabilityList();
        FillShapeContainer();
        CheckOnBoardShapeStatus();

    }

    /// <summary>
    /// 소환 블록 리스트 만들기
    /// </summary>
    public void CreateShapeBlockProbabilityList()
    {
        int Max_Block = 0;

        shapeBlockProbabilityPool.Clear();

        switch (GamePlay.instance.gameMode)
        {
            case GameMode.Classic:
            case GameMode.Multi:

                Max_Block = 39;

                break;
            case GameMode.Stage:
                if (GamePlay.instance.Play_Stage_Num < 30)
                {
                    Max_Block = 22;
                }
                else if (GamePlay.instance.Play_Stage_Num < 60)
                {
                    Max_Block = 31;
                }
                else
                {
                    Max_Block = 39;
                }
                break;
            case GameMode.Timer:

                if (ScoreManager.Instance.GetScore() >= 10000)
                {
                    Max_Block = 39;
                }
                else if (ScoreManager.Instance.GetScore() >= 5000)
                {
                    Max_Block = 31;
                }
                else if (ScoreManager.Instance.GetScore() < 5000)
                {
                    Max_Block = 22;
                }

                break;
            default:
                break;
        }


        if (activeShapeBlockModule != null)
        {
            for (int i = 0; i < Max_Block; i++)
            {
                AddShapeInProbabilityPool(activeShapeBlockModule.ShapeBlocks[i].BlockID, activeShapeBlockModule.ShapeBlocks[i].spawnProbability);

            }

        }

        shapeBlockProbabilityPool.Shuffle();

    }

    /// <summary>
    /// 소환 블록 리스트 저장
    /// </summary>
    /// <param name="blockID">Block I.</param>
    /// <param name="probability">Probability.</param>
    void AddShapeInProbabilityPool(int blockID, int probability)
    {
        int probabiltyTimesToAdd = shapeBlockPoolCount * probability;

        for (int index = 0; index < probabiltyTimesToAdd; index++)
        {
            shapeBlockProbabilityPool.Add(blockID);
        }
    }

    bool isTutoSpawn = false;
    bool isTutoChange = false;

    /// <summary>
    /// Fills the shape container.
    /// </summary>
    public void FillShapeContainer()
    {

        //옆에서 나오는 애니메이션
        //ReorderShapes();

        bool isAllEmpty = true;

        if (spawn_Blocks.Count > 0)
        {
            isAllEmpty = false;
        }

        if (isAllEmpty)
        {
            spawn_Blocks.Clear();

            for (int i = 0; i < shapeContainers.Length; i++)
            {
                Dictionary<string, object> stage_Info = DataManager.Instance.stage_data[GamePlay.instance.Play_Stage_Num];

                if (GamePlay.instance.gameMode.Equals(GameMode.Stage) && (int)stage_Info["stage_Type"] == 0 && !isTutoSpawn)
                {
                    //튜토리얼/////////////////////////////////////////////////////////
                    Dictionary<string, object> tuto_Info = DataManager.Instance.tutorial_data[GamePlay.instance.Play_Stage_Num];

                    string block_Num = (string)tuto_Info["spawn_block"];

                    if (block_Num != "")
                    {
                        string[] str_block = block_Num.Split('_');
                        CreateShapeWithID(shapeContainers[i], int.Parse(str_block[i]));
                    }
                    else
                    {
                        GamePlay.instance.isSpawn = true;

                        AddRandomShapeToContainer(shapeContainers[i]);

                    }
                    /////////////////////////////////////////////////////////////////////
                }
                else if ((int)stage_Info["stage_Type"] == 0 && isTutoChange)
                {
                    //튜토리얼/////////////////////////////////////////////////////////

                    Dictionary<string, object> tuto_Info = DataManager.Instance.tutorial_data[GamePlay.instance.Play_Stage_Num];

                    string block_Num = (string)tuto_Info["change_block"];

                    if (block_Num != "")
                    {
                        string[] str_block = block_Num.Split('_');
                        CreateShapeWithID(shapeContainers[i], int.Parse(str_block[i]));
                    }
                    else
                    {
                        GamePlay.instance.isSpawn = true;

                        AddRandomShapeToContainer(shapeContainers[i]);

                    }
                    /////////////////////////////////////////////////////////////////////

                }
                else
                {
                    //일반/////////////////////////////////////////////////////////

                    GamePlay.instance.isSpawn = true; //소환할때 놓을때 있는지 확인

                    AddRandomShapeToContainer(shapeContainers[i]);
                    /////////////////////////////////////////////////////////////////////

                }
            }

            isTutoSpawn = true;

        }



    }


    /// <summary>
    /// 랜덤 블록 소환
    /// </summary>
    /// <param name="shapeContainer">Shape container.</param>
    public void AddRandomShapeToContainer(Transform shapeContainer)
    {
        if (shapeBlockProbabilityPool == null || shapeBlockProbabilityPool.Count <= 0)
        {
            CreateShapeBlockProbabilityList();
        }

        int RandomShape = shapeBlockProbabilityPool[0];
        shapeBlockProbabilityPool.RemoveAt(0);

        Debug.Log("1 소환!!!!");
        GameObject spawningShapeBlock = ObjectPool.Spawn(activeShapeBlockModule.ShapeBlocks.Find(o => o.BlockID == RandomShape).shapeBlock);
        spawningShapeBlock.transform.SetParent(shapeContainer);
        spawningShapeBlock.transform.localScale = Vector3.one;
        spawningShapeBlock.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        spawningShapeBlock.GetComponent<ShapeInfo>().CrateBlock(0, RandomShape);

        spawningShapeBlock.transform.LeanMoveLocal(Vector3.zero, 0.3F);
        spawn_Blocks.Add(spawningShapeBlock);

    }

    /// <summary>
    /// 블록 ID로 블록 소환
    /// </summary>
    /// <param name="shapeContainer">Shape container.</param>
    /// <param name="shapeID">Shape I.</param>
    void CreateShapeWithID(Transform shapeContainer, int shapeID)
    {
        Debug.Log("2 소환!!!!");
        GameObject spawningShapeBlock = ObjectPool.Spawn(activeShapeBlockModule.ShapeBlocks.Find(o => o.BlockID == shapeID).shapeBlock);
        spawningShapeBlock.transform.SetParent(shapeContainer);
        spawningShapeBlock.transform.localScale = Vector3.one;
        spawningShapeBlock.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        spawningShapeBlock.GetComponent<ShapeInfo>().CrateBlock(0, shapeID);

        spawningShapeBlock.transform.LeanMoveLocal(Vector3.zero, 0.3F);
        spawn_Blocks.Add(spawningShapeBlock);

    }

    /// <summary>
    /// 블록 ID와 컬러로 블록 소환
    /// </summary>
    /// <param name="shapeContainer">Shape container.</param>
    /// <param name="shapeID">Shape I.</param>
    void CreateShapeWithID(Transform shapeContainer, int shapeID, int color_id)
    {
        Debug.Log("3 소환!!!!");

        GameObject spawningShapeBlock = ObjectPool.Spawn(activeShapeBlockModule.ShapeBlocks.Find(o => o.BlockID == shapeID).shapeBlock);
        spawningShapeBlock.transform.SetParent(shapeContainer);
        spawningShapeBlock.transform.localScale = Vector3.one;
        spawningShapeBlock.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);

        spawningShapeBlock.transform.LeanMoveLocal(Vector3.zero, 0.3F);
        spawningShapeBlock.GetComponent<ShapeInfo>().CrateBlock(color_id, shapeID);

        spawn_Blocks.Add(spawningShapeBlock);

    }

    /// <summary>
    /// 놓을때 없는지 체크
    /// </summary>
    public void CheckOnBoardShapeStatus()
    {
        List<ShapeInfo> OnBoardBlockShapes = new List<ShapeInfo>();

        foreach (GameObject shapeContainer in spawn_Blocks)
        {
            OnBoardBlockShapes.Add(shapeContainer.GetComponent<ShapeInfo>());
            shapeContainer.GetComponent<ShapeInfo>().OneBlockAlpha(1);
        }

        //놓을때 없는지 체크
        bool canExistingBlocksPlaced = GamePlay.instance.CanExistingBlocksPlaced(OnBoardBlockShapes);
        GamePlay.instance.CheckOneBlock();

        if (canExistingBlocksPlaced == false && UIManager.Instance.game_Stat != Game_Stat.End)
        {
            GamePlay.instance.OnUnableToPlaceShape();
        }
        else
        {
            //있으면 스폰 체크 풀기
            GamePlay.instance.isSpawn = false;
        }


    }


    //void ReorderShapes()
    //{
    //    List<Transform> EmptyShapes = new List<Transform>();

    //    foreach (Transform shapeContainer in shapeContainers)
    //    {
    //        if (shapeContainer.childCount == 0)
    //        {
    //            EmptyShapes.Add(shapeContainer);
    //        }
    //        else
    //        {
    //            if (EmptyShapes.Count > 0)
    //            {
    //                Transform emptyContainer = EmptyShapes[0];
    //                shapeContainer.GetChild(0).SetParent(emptyContainer);
    //                EmptyShapes.RemoveAt(0);

    //                emptyContainer.GetChild(0).LeanMoveLocal(Vector3.zero, 0.3F);

    //                EmptyShapes.Add(shapeContainer);
    //            }
    //        }
    //    }
    //}


    /// <summary>
    /// 블록 리셋
    /// </summary>
    public void Block_Reset()
    {
        foreach (var item in spawn_Blocks)
        {
            ObjectPool.Recycle(item);
        }

        spawn_Blocks.Clear();
        shapeBlockProbabilityPool.Clear();
        shapeBlockPoolCount = 1;
    }



    /// <summary>
    /// 블록 체인지 아이템
    /// </summary>
    public void Block_Change()
    {
        foreach (var item in spawn_Blocks)
        {
            ObjectPool.Recycle(item);
        }

        spawn_Blocks.Clear();
        shapeBlockProbabilityPool.Clear();
        shapeBlockPoolCount = 1;

        isTutoChange = true;

        FillShapeContainer();
        CheckOnBoardShapeStatus();

    }

    /// <summary>
    /// 블록 되돌리기
    /// </summary>
    public void Recover()
    {
        Block_Reset();
        List<int[]> spawn_Blcok = recover_Block.Pop();

        for (int i = 0; i < spawn_Blcok.Count; i++)
        {
            Debug.Log(spawn_Blcok[i][0]);

            CreateShapeWithID(shapeContainers[i], spawn_Blcok[i][0], spawn_Blcok[i][1]);

        }

    }

}

