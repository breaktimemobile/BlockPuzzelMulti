using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class ShapeInfo : MonoBehaviour
{
    public int shapeID = 0;
    public ShapeBlock firstBlock;
    public Sprite blockImage = null;
    public int startOffsetX = 0;
    public int startOffsetY = 0;

    public List<ShapeBlock> shapeBlocks; //내 자식 블럭

    public int colorID;

    public void CrateBlock(int color_id = 0 , int _shapeID =0)
    {
        shapeID = _shapeID;
        CreateBlockList(color_id);

        //첫번째 블럭으로 위치 계산
        firstBlock = shapeBlocks[0];
        blockImage = firstBlock.block.GetComponent<Image>().sprite;
        startOffsetX = firstBlock.rowID;
        startOffsetY = firstBlock.columnID;

    }

    public void Set_Rotation()
    {
        Rotate();

        CreateBlockList(this.colorID);

        //첫번째 블럭으로 위치 계산
        firstBlock = shapeBlocks[0];
        blockImage = firstBlock.block.GetComponent<Image>().sprite;
        startOffsetX = firstBlock.rowID;
        startOffsetY = firstBlock.columnID;
    }

    /// <summary>
    /// 블록 회전
    /// </summary>
    public void Rotate()
    {
        int col = shapeBlocks.Max(m => m.columnID);
        int row = shapeBlocks.Max(m => m.rowID);


        int Range = col >= row ? col + 1 : row + 1;

        int[,] NewRotate = new int[Range, Range];

        foreach (var item in shapeBlocks)
        {
            NewRotate[item.rowID, item.columnID] = 1;
        }

        int[,] rotateBlock = RotateRight(NewRotate);

        List<Transform> shapeAllBlocks = transform.GetComponentsInChildren<Transform>().ToList();

        //내 자신 지우기
        if (shapeAllBlocks.Contains(transform))
        {
            shapeAllBlocks.Remove(transform);
        }

        int CheckBlock = 0;

        //짝수인지 홀수인지
        //짝수면 29 홀수면 58

        int maxy = 0;
        int maxx = 0;


        //x좌표 최대값///////////////////////////////////////////
        int[] addx = new int[rotateBlock.GetLength(0)];

        for (int i = 0; i < rotateBlock.GetLength(0); i++)
            for (int j = 0; j < rotateBlock.GetLength(1); j++)
                if (rotateBlock[i, j] == 1)
                    addx[i]++;

        maxy = Array.FindAll(addx, a => a >= 1).Length;
        /////////////////////////////////////////////////////////

        //y좌표 최대값///////////////////////////////////////////
        int[] addy = new int[rotateBlock.GetLength(1)];

        for (int i = 0; i < rotateBlock.GetLength(1); i++)
            for (int j = 0; j < rotateBlock.GetLength(0); j++)
                if (rotateBlock[j, i] == 1)
                    addy[i]++;

        maxx = Array.FindAll(addy, a => a >= 1).Length;
        /////////////////////////////////////////////////////////

        int[,] ClearArr = new int[maxy, maxx];

        //X,Y 최대 범위 계산//////////////////////////////////
        if (maxy > maxx)
        {
            int movey = 0;

            for (int i = 0; i < rotateBlock.GetLength(1); i++)
            {
                bool isUse = false;

                for (int j = 0; j < rotateBlock.GetLength(0); j++)
                {
                    if (rotateBlock[j, i] == 1)
                    {
                        ClearArr[j, movey] = 1;
                        isUse = true;
                    }
                }

                if (isUse) movey++;
            }
        }
        else
        {
            int movey = 0;

            for (int i = 0; i < rotateBlock.GetLength(0); i++)
            {
                bool isUse = false;

                for (int j = 0; j < rotateBlock.GetLength(1); j++)
                {
                    if (rotateBlock[i, j] == 1)
                    {
                        ClearArr[movey, j] = 1;

                        isUse = true;

                    }
                }
                if (isUse) movey++;

            }
        }

        //////////////////////////////////////////////////////////////////

        int x = (maxx - 1) * 29;
        int y = (maxy - 1) * 29;

        #region Old
        //if (maxy == 2 && maxx == 2)
        //{
        //    x = 29;
        //    y = 29;
        //}
        //else if (maxy == 2 && maxx == 3)
        //{
        //    x = 58;
        //    y = 29;
        //}
        //else if (maxy == 3 && maxx == 2)
        //{
        //    x = 29;
        //    y = 58;
        //}
        //else if (maxy == 3 && maxx == 3)
        //{
        //    x = 58;
        //    y = 58;
        //}
        //else if (maxy == 1 && maxx == 2)
        //{
        //    x = 29;
        //    y = 0;
        //}
        //else if (maxy == 2 && maxx == 1)
        //{
        //    x = 0;
        //    y = 29;
        //}
        //else if (maxy == 1 && maxx == 3)
        //{
        //    x = 58;
        //    y = 0;
        //}
        //else if (maxy == 3 && maxx == 1)
        //{
        //    x = 0;
        //    y = 58;
        //}
        //else if (maxy == 1 && maxx == 4)
        //{
        //    x = 58 + 29;
        //    y = 0;
        //}
        //else if (maxy == 4 && maxx == 1)
        //{
        //    x = 0;
        //    y = 58 + 29;
        //}
        //else if (maxy == 1 && maxx == 5)
        //{
        //    x = 58 + 58;
        //    y = 0;
        //}
        //else if (maxy == 5 && maxx == 1)
        //{
        //    x = 0;
        //    y = 58 + 58;
        //}
        #endregion


        for (int i = 0; i < ClearArr.GetLength(0); i++)
        {
            for (int j = 0; j < ClearArr.GetLength(1); j++)
            {
                if (ClearArr[i, j] == 1)
                {
                    shapeAllBlocks[CheckBlock].name = "Block-" + i + "-" + j;

                    shapeAllBlocks[CheckBlock].localPosition = new Vector2((j * 58) - x, (i * -58) + y);
                    CheckBlock += 1;
                }
            }
        }


    }

    /// <summary>

    /// 2차원 배열 오른쪽으로 회전하기

    /// </summary>

    /// <typeparam name="T">배열 타입</typeparam>

    /// <param name="sourceArray">소스 배열</param>

    /// <returns>오른쪽 회전 배열</returns>

    public static T[,] RotateRight<T>(T[,] sourceArray)
    {

        int lengthY = sourceArray.GetLength(0);
        int lengthX = sourceArray.GetLength(1);

        T[,] targetArray = new T[lengthX, lengthY];

        for (int y = 0; y < lengthY; y++)
        {
            for (int x = 0; x < lengthX; x++)
            {
                targetArray[x, y] = sourceArray[lengthY - 1 - y, x];
            }
        }

        return targetArray;

    }



    /// <summary>
    /// 자식 블럭 생성
    /// </summary>
	void CreateBlockList(int color_id = 0)
    {
        shapeBlocks = new List<ShapeBlock>();
        List<Transform> shapeAllBlocks = transform.GetComponentsInChildren<Transform>().ToList();

        //내 자신 지우기
        if (shapeAllBlocks.Contains(transform))
        {
            shapeAllBlocks.Remove(transform);
        }

        int random_colorID = UnityEngine.Random.Range(0, 6);

        colorID = random_colorID + 1;

        if (color_id != 0)
        {
            colorID = color_id;

        }

        //자식 블럭 좌표 저장
        foreach (Transform block in shapeAllBlocks)
        {
            string[] blockNameSplit = block.name.Split('-');

            block.GetComponent<Image>().sprite = UIManager.Instance.Sp_Blocks[colorID - 1];

            if (blockNameSplit.Length == 3)
            {
                int rowID = int.Parse(blockNameSplit[1]);
                int columnID = int.Parse(blockNameSplit[2]);

                ShapeBlock thisBlock = new ShapeBlock(block, rowID, columnID, colorID);
                if (!shapeBlocks.Contains(thisBlock))
                {
                    shapeBlocks.Add(thisBlock);
                }
            }
        }
    }


    /// <summary>
    /// 블럭 알파값
    /// </summary>
    /// <param name="alpha"></param>
    public void OneBlockAlpha(float alpha)
    {

        List<Transform> shapeAllBlocks = transform.GetComponentsInChildren<Transform>().ToList();

        //내 자신 지우기
        if (shapeAllBlocks.Contains(transform))
        {
            shapeAllBlocks.Remove(transform);
        }

        foreach (var item in shapeAllBlocks)
        {
            item.GetComponent<Image>().color = new Color(1, 1, 1, alpha);

        }

    }

}

public class ShapeBlock
{
    public Transform block;
    public int rowID;
    public int columnID;
    public int Color_ID;

    public ShapeBlock(Transform _block, int _rowID, int _columnID, int _colorID)
    {
        this.block = _block;
        this.rowID = _rowID;
        this.columnID = _columnID;
        Color_ID = _colorID;

    }
}


