using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if HBDOTween
using DG.Tweening;
#endif

public enum Block_Type
{
    None = 0,
    Basic = 1,
    durability = 111,
    ice = 7,
    noBreak = 8,
    boom = 9,
    balloon = 10,
    bear = 11,
    line = 12
}

public class BlockInfo
{
    public Block_Type block_Type = Block_Type.None;

    [HideInInspector] public int blockID = -1;

    [HideInInspector] public int colorID = -1;

    [HideInInspector] public int durability = 1;

    [HideInInspector] public int Ice_durability = 1;

    //Status whether block is empty or filled.
    [HideInInspector] public bool isFilled = false;

    [HideInInspector] public bool isCopyFilled = false;

    public Sprite Save_block_sprite = null;
}


public class Block : MonoBehaviour
{

    //Row Index of block.
    public int rowID;

    //Column Index of block.
    public int columnID;

    //Block image instance.
    [HideInInspector] public Image blockImage;

    Text txtCounter;
    Image Img_CheckOne;
    Image Img_IceBlock;
    Image Img_GameOver;
    Text Boom_Counter;

    public Block_Type block_Type = Block_Type.None;

    public int blockID = -1;

    public int colorID = -1;

    public int durability = 1;

    public int Ice_durability = 1;

    //Status whether block is empty or filled.
    public bool isFilled = false;

    public bool isCopyFilled = false;

    public Sprite Save_block_sprite = null;

    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void Awake()
    {

        txtCounter = transform.Find("Image/Text-Counter").GetComponent<Text>();
        Img_CheckOne = transform.Find("Image/Img_CheckOne").GetComponent<Image>();
        Img_IceBlock = transform.Find("Image/Img_IceBlock").GetComponent<Image>();
        Img_GameOver = transform.Find("Image/Img_GameOver").GetComponent<Image>();
        Boom_Counter = transform.Find("Image/Boom_Counter").GetComponent<Text>();

    }

    public void Init()
    {
        block_Type = Block_Type.None;

        blockID = -1;

        colorID = -1;

        durability = 1;

        Ice_durability = 1;

        isFilled = false;

        isCopyFilled = false;

        Save_block_sprite = null;

        blockImage.sprite = null;
        blockImage.color = new Color(1, 1, 1, 0);

        Img_CheckOne.gameObject.SetActive(false);
        Img_IceBlock.gameObject.SetActive(false);
        Img_GameOver.gameObject.SetActive(false);
        txtCounter.gameObject.SetActive(false);
        Boom_Counter.gameObject.SetActive(false);

    }


    /// <summary>
    /// 남아 있는 자리 이미지 온오프
    /// </summary>
    /// <param name="state"></param>
    public void CheckOneBlock(bool state)
    {
        Img_CheckOne.gameObject.SetActive(state);
    }

    /// <summary>
    /// 블럭 상태 되돌리기
    /// </summary>
    public void Recover()
    {

        if (colorID == -1)
        {
            blockImage.sprite = null;
            blockImage.color = new Color(1, 1, 1, 0);

        }
        else
        {

            switch (block_Type)
            {
                //노말 타입
                default:
                    blockImage.sprite = UIManager.Instance.Sp_Blocks[colorID - 1];

                    blockImage.color = new Color(1, 1, 1, 1);
                    txtCounter.gameObject.SetActive(false);

                    break;

                case Block_Type.ice:

                    Img_IceBlock.gameObject.SetActive(true);

                    if (colorID == 7)
                    {
                        blockImage.sprite = null;
                        blockImage.color = new Color(1, 1, 1, 0);
                    }
                    else
                    {
                        blockImage.sprite = UIManager.Instance.Sp_Blocks[colorID - 1];
                        blockImage.color = new Color(1, 1, 1, 1);

                    }

                    break;

                case Block_Type.noBreak:
                case Block_Type.boom:
                case Block_Type.durability:
                case Block_Type.balloon:

                    blockImage.sprite = UIManager.Instance.Sp_Blocks[colorID - 1];
                    blockImage.color = new Color(1, 1, 1, 1);

                    break;

            }
        }

        Set_durability_Txt();

    }

    /// <summary>
    /// 블럭 이미지 및 상태 세팅
    /// </summary>
    /// <param name="sprite">Sprite.</param>
    /// <param name="_blockID">Block I.</param>
    public void SetBlockImage(Sprite sprite, int _blockID, int _colorID, int Hp = 0)
    {
        colorID = _colorID;

        if(block_Type.Equals(Block_Type.None))
            Set_TxtColor(colorID);

        switch (colorID)
        {
            default:

                //blockImage.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                if (block_Type.Equals(Block_Type.None))
                {
                    block_Type = Block_Type.Basic;

                }

                durability = 1 + Hp;
                blockID = _blockID;
                isFilled = true;
                isCopyFilled = true;
                Save_block_sprite = sprite;
                blockImage.sprite = sprite;
                blockImage.color = new Color(1, 1, 1, 1);
                txtCounter.gameObject.SetActive(false);

                LeanTween.scale(blockImage.gameObject, Vector3.one, 0.2f);

                break;
            case (int)Block_Type.durability:

                break;

            case (int)Block_Type.ice:
                block_Type = Block_Type.ice;

                blockImage.sprite = null;

                Ice_durability = 1 + Hp;
                isFilled = false;
                Img_IceBlock.gameObject.SetActive(true);
                blockImage.color = new Color(0, 0, 0, 0);

                break;

            case (int)Block_Type.noBreak:

                block_Type = Block_Type.noBreak;

                durability = 9999;
                blockID = _blockID;
                isFilled = true;
                isCopyFilled = true;
                Save_block_sprite = sprite;
                blockImage.sprite = sprite;
                blockImage.color = new Color(1, 1, 1, 1);
                txtCounter.gameObject.SetActive(false);

                break;

            case (int)Block_Type.boom:

                block_Type = Block_Type.boom;

                durability = 1 + Hp;
                blockID = _blockID;
                isFilled = true;
                isCopyFilled = true;
                Save_block_sprite = sprite;
                blockImage.sprite = sprite;
                blockImage.color = new Color(1, 1, 1, 1);
                Boom_Counter.gameObject.SetActive(true);

                break;

            case (int)Block_Type.balloon:

                block_Type = Block_Type.balloon;

                durability = 1 + Hp;
                blockID = _blockID;
                isFilled = true;
                isCopyFilled = true;
                Save_block_sprite = sprite;
                blockImage.sprite = sprite;
                blockImage.color = new Color(1, 1, 1, 1);
                txtCounter.gameObject.SetActive(false);

                break;
        }

        Set_durability_Txt();

    }

    private readonly string[] txtColor = new string[] {"#FF2628", "#6CBA38", "#526BFF", "#FBD01C", "#D83675", "#67CBFF",
                                                        "#72D3EE", "#767772", "#E3E3E3", "#E3E3E3"};

    public void Set_TxtColor(int Block)
    {
        Color color;

        ColorUtility.TryParseHtmlString(txtColor[Block - 1],out color);

        txtCounter.color = color;
        Boom_Counter.color = color;

    }
    /// <summary>
    /// 블럭 이미지 교체
    /// </summary>
    /// <param name="sprite"></param>
    public void ChangeBlockImg(Sprite sprite)
    {
        txtCounter.gameObject.SetActive(false);
        Boom_Counter.gameObject.SetActive(false);
        blockImage.sprite = sprite;
        blockImage.color = new Color(1, 1, 1, 1);

        Set_durability_Txt();

    }

    /// <summary>
    /// 전 이미지로 되돌리기
    /// </summary>
    public void ReturnBlockImg()
    {
        if (Save_block_sprite != null)
        {
            Set_durability_Txt();

            blockImage.sprite = Save_block_sprite;
            blockImage.color = new Color(1, 1, 1, 1);
        }

    }

    /// <summary>
    /// 전 이미지로 되돌리기
    /// </summary>
    public void Timer_continue()
    {
        Img_GameOver.gameObject.SetActive(false);

    }
    
    #region 놓을 자리 미리 보여주기

    /// <summary>
    /// 미리 보여주기 이미지
    /// </summary>
    /// <param name="sprite">Sprite.</param>
    public void SetHighlightImage(Sprite sprite)
    {
        blockImage.sprite = sprite;
        blockImage.color = new Color(1, 1, 1, 0.5F);
    }

    /// <summary>
    /// 미리 보여주기 이미지 중지
    /// </summary>
    public void StopHighlighting()
    {

        blockImage.sprite = null;
        blockImage.color = new Color(1, 1, 1, 0);
    }


    /// <summary>
    /// 미리보이기 상태 세팅
    /// </summary>
    /// <param name="stat"></param>
    public void CopyFill(bool stat)
    {
        isCopyFilled = stat;
    }

    #endregion

    /// <summary>
    /// 부서진 블럭
    /// </summary>
    public void ClearBlock(int block)
    {

        if (block_Type == Block_Type.boom)
        {
            durability = 0;
            block_Type = Block_Type.None;
        }
        else if (block_Type == Block_Type.balloon)
        {
            ReturnBlockImg();
            return;
        }

        Check_Balloon();

        durability -= 1;
        Ice_durability -= 1;


        if (block_Type.Equals(Block_Type.ice))
        {
            Debug.Log("아이스 삭제 " + Ice_durability);

            if (Ice_durability <= 0)
            {
                Debug.Log("아이스 삭제");
                Img_IceBlock.gameObject.SetActive(false);
                block_Type = Block_Type.None;

                GameObject effect = ObjectPool.Spawn(UIManager.Instance.explosion[block], transform, transform.position);
                effect.transform.localScale = Vector3.one;
                effect.transform.localPosition = Vector3.zero;

            }
            else
            {
                Img_IceBlock.gameObject.SetActive(true);

            }
        }
        else
        {
            if (durability <= 0)
            {
                block_Type = Block_Type.None;
            }
        }


        if (durability <= 0)
        {
            //gen block panel
            if (colorID != -1)
            {
                //gen block panel

                GameObject effect = ObjectPool.Spawn(UIManager.Instance.explosion[block], transform, transform.position);
                effect.transform.localScale = Vector3.one;
                effect.transform.localPosition = Vector3.zero;

            }
            blockImage.transform.localScale = Vector3.one;
            blockImage.sprite = null;
            blockImage.color = new Color(0, 0, 0, 0);



            blockID = -1;
            colorID = -1;

            isFilled = false;
            isCopyFilled = false;
            Save_block_sprite = null;

        }
        else
        {
            ReturnBlockImg();
        }

        Set_durability_Txt();

    }

    /// <summary>
    /// 폭탄 아이템
    /// </summary>
    public void Boom_Item()
    {

        durability -= 1;
        //Ice_durability -= 1;


        if (block_Type.Equals(Block_Type.ice))
        {
            Debug.Log("아이스 삭제 " + Ice_durability);

            if (Ice_durability <= 0)
            {

            }
            else
            {
                if (durability <= 0)
                {

                    //gen block panel

                    blockImage.transform.localScale = Vector3.one;
                    blockImage.sprite = null;
                    blockImage.color = new Color(0, 0, 0, 0);

                    blockID = -1;

                    isFilled = false;
                    isCopyFilled = false;
                    Save_block_sprite = null;

                }

                Img_IceBlock.gameObject.SetActive(true);

            }
        }
        else
        {

            if (durability <= 0)
            {
                if (colorID != -1 && colorID != 9)
                {
                    //gen block panel

                    GameObject effect = ObjectPool.Spawn(UIManager.Instance.explosion[colorID - 1], transform, transform.position);
                    effect.transform.localScale = Vector3.one;
                    effect.transform.localPosition = Vector3.zero;

                }


                block_Type = Block_Type.None;

    
                blockImage.transform.localScale = Vector3.one;
                blockImage.sprite = null;
                blockImage.color = new Color(0, 0, 0, 0);



                blockID = -1;
                colorID = -1;

                isFilled = false;
                isCopyFilled = false;
                Save_block_sprite = null;

            }
            else
            {
                ReturnBlockImg();
            }



        }

        Set_durability_Txt();

    }

    /// <summary>
    /// 폭탄 블럭 내구도 체크
    /// </summary>
    public void Check_Boom()
    {
        durability -= 1;
        Set_durability_Txt();

        //내구도가 0이되면 게임오버
        if (durability <= 0)
        {
            GamePlay.instance.end_Stat = End_Stat.boom;
            GamePlay.instance.OnGameOver(false);
            Debug.Log("폭탄 게임오버");


        }

    }


    /// <summary>
    /// 블럭 내구도 텍스트 세팅
    /// </summary>
    public void Set_durability_Txt()
    {
        txtCounter.gameObject.SetActive(false);
        Boom_Counter.gameObject.SetActive(false);
        
        switch (block_Type)
        {
            case Block_Type.Basic:
                if (durability >= 2)
                {
                    txtCounter.gameObject.SetActive(true);
                    txtCounter.text = durability.ToString();
                }
                break;
            case Block_Type.durability:
                if (durability >= 2 && colorID != 9)
                {
                    txtCounter.gameObject.SetActive(true);
                    txtCounter.text = durability.ToString();
                }
                break;
            case Block_Type.ice:
                if (Ice_durability >= 2)
                {
                    if (blockImage.sprite == null)
                    {
                        txtCounter.gameObject.SetActive(true);
                        txtCounter.text = Ice_durability.ToString();
                    }

                }

                break;
            case Block_Type.boom:

                if (durability >= 1)
                {
                    Boom_Counter.gameObject.SetActive(true);
                    Boom_Counter.text = durability.ToString();
                }

                break;
            case Block_Type.noBreak:
                break;
            default:
                if (durability >= 2)
                {
                    txtCounter.gameObject.SetActive(true);
                    txtCounter.text = durability.ToString();
                }
                break;
        }

    }


    public void GameOver()
    {
        Img_GameOver.color = new Color(1, 1, 1, 0);
        StartCoroutine("Co_GameOver");
    }

    IEnumerator Co_GameOver()
    {
        Img_GameOver.gameObject.SetActive(true);
        float Alapa = 0;

        while (Alapa < 1)
        {
            yield return new WaitForSeconds(0.01f);
            Alapa += 0.02f;
            Img_GameOver.color = new Color(1, 1, 1, Alapa);

        }
    }

    public void Check_Balloon()
    {
        List<Block> blocks = new List<Block>();

        blocks.Add(GamePlay.instance.blockGrid.Find(o => o.rowID == rowID && o.columnID == columnID - 1));
        blocks.Add(GamePlay.instance.blockGrid.Find(o => o.rowID == rowID && o.columnID == columnID + 1));
        blocks.Add(GamePlay.instance.blockGrid.Find(o => o.rowID == rowID - 1 && o.columnID == columnID));
        blocks.Add(GamePlay.instance.blockGrid.Find(o => o.rowID == rowID + 1 && o.columnID == columnID));

        foreach (var item in blocks)
        {
            if (item != null && item.block_Type.Equals(Block_Type.balloon))
                item.Balloon_clear();
        }

    }

    public void Balloon_clear()
    {
        block_Type = Block_Type.None;

        if (colorID != -1)
        {
            //gen block panel

            GameObject effect = ObjectPool.Spawn(UIManager.Instance.explosion[2], transform, transform.position);
            effect.transform.localScale = Vector3.one;
            effect.transform.localPosition = Vector3.zero;

        }
        blockImage.transform.localScale = Vector3.one;
        blockImage.sprite = null;
        blockImage.color = new Color(0, 0, 0, 0);

        GamePlay.instance.Set_Mission_Block(11);


        blockID = -1;
        colorID = -1;

        isFilled = false;
        isCopyFilled = false;
        Save_block_sprite = null;
    }

}
