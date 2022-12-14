using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class Tesy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    public List<GameObject> Block_List = new List<GameObject>();//アイテムブロックのリスト
    public GameObject BlockSumple;//ブロックのサンプル
    public GameObject AllBord;//スクロールの全体領域よう
    public GameObject BlockBox;//ブロックの入れ物　親子関係の操作で使用
    public GameObject Mask;
    public GameObject Scroll;
    public GameObject ScrollBar;
    public GameObject IcobImage;
    public Text PlayerName;
    public float FarstBlock_Transform;
    public bool Rist_View = false;
    public Hashtable hashtable = new Hashtable();//カスタムプロパティのリスト


    public void Start()
    {
        BlockUpdate();
        MaskSize();
        BordSize();
        Scroll.SetActive(Rist_View);


    }
    private void Update()
    {

    }

    public void AddItem(int ItemNum)
    {
        GameObject Block = Instantiate(BlockSumple, new Vector3(-1.0f, 0.0f, 0.0f), Quaternion.identity);//ブロックの生成
        Block_List.Add(Block);//ブロックをリストに突っ込む

        Block.transform.SetParent(BlockBox.transform, false);//アイテムリストの親子関係の調整
        Block.GetComponent<ItemBlock>().ItemNumber=ItemNum;
        Block.GetComponent<ItemBlock>().player=Player;
        Block.GetComponent<ItemBlock>().Detail_Switch();


        MaskSize();
        BordSize();//ボードのサイズ調整

        BlockUpdate();//ブロックの場所を調整
    }

    public void LostItem(int ItemNum)
    {
        Destroy(Block_List[ItemNum]);
        Block_List.Remove(Block_List[ItemNum]);//ブロックをリストに突っ込む




        MaskSize();
        BordSize();//ボードのサイズ調整

        BlockUpdate();//ブロックの場所を調整
    }

    public void BordSize()//ボードのサイズ調整
    {

        int BlockQuantity;
        if (Block_List.Count<=3)
        {

            BlockQuantity = 3;

            ScrollBar.GetComponent<ScrollRect>().vertical=false;

        }
        else
        {

            BlockQuantity = Block_List.Count;//ブロックの数
            ScrollBar.GetComponent<ScrollRect>().vertical=true;

        }

        var scale = BlockSumple.GetComponent<RectTransform>().localScale.y;
        var BordSize_x = BlockSumple.GetComponent<RectTransform>().sizeDelta.x;//ボードのサイズを取得　（戻り値のため）
        var BordSize_y = BlockSumple.GetComponent<RectTransform>().sizeDelta.y*BlockQuantity*scale;//ボードのサイズを取得　（戻り値のため）

        AllBord.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, BordSize_y);

    }

    public void MaskSize()//ボードのサイズ調整
    {

        int BlockPuantity;
        if (Block_List.Count>3)
        {

            BlockPuantity = 3;

        }
        else
        {

            BlockPuantity = Block_List.Count;//ブロックの数
        }

        var scale = BlockSumple.GetComponent<RectTransform>().localScale.y;
        var MaskSize_x = BlockSumple.GetComponent<RectTransform>().sizeDelta.x;//ボードのサイズを取得　（戻り値のため）
        var MaskSize_y = BlockSumple.GetComponent<RectTransform>().sizeDelta.y*BlockPuantity* scale;//ボードのサイズを取得　（戻り値のため）



        Mask.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, -2, MaskSize_y+(3.5f));

        var MasTransform = Mask.GetComponent<RectTransform>().position;

        MasTransform.y = MasTransform.y+(1.50f*2f);
        if (Block_List.Count==3)
        {
            Mask.GetComponent<RectTransform>().position=MasTransform;

        }
        else
        {

        }
    }




    public void PuintUpdate()//ブロック更新時
    {

        foreach (var bloc in Block_List)
        {


            bloc.GetComponent<ItemBlock>().Detail_Switch();

        }

    }
    public void BlockUpdate()//ブロックの場所を調整
    {
        int loop = 0;
        foreach (var Block in Block_List)
        {
            var BlockTransform = Block.GetComponent<RectTransform>().position;//ブロックの場所を取得
            var scale = BlockSumple.GetComponent<RectTransform>().localScale.y;
            BlockTransform.x=0+1.5f;
            BlockTransform.y=FarstBlock_Transform-(BlockSumple.GetComponent<RectTransform>().sizeDelta.y*loop*scale)-4;//ブロックの場所を一つずつ詰めていく
            if (Block_List.Count==3)
            {
                BlockTransform.y-=0.6f;
            }
            Block.GetComponent<RectTransform>().anchoredPosition=BlockTransform;
            loop++;




        }
    }

    public void ItemRist_View()
    {
        GameObject.Find("I_game_manager").GetComponent<Guide>().Item_Finish();
        Rist_View =!Rist_View;
        Scroll.SetActive(Rist_View);
    }

    public void Namedisplay(string name)
    {
        PlayerName.text=name;

    }
}
