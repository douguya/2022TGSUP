using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class ItemBlock_List_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    public List<GameObject> Block_List = new List<GameObject>();//�A�C�e���u���b�N�̃��X�g
    public GameObject BlockSumple;//�u���b�N�̃T���v��
    public GameObject AllBord;//�X�N���[���̑S�̗̈�悤
    public GameObject BlockBox;//�u���b�N�̓��ꕨ�@�e�q�֌W�̑���Ŏg�p
    public GameObject Mask;
    public GameObject Scroll;
    public GameObject ScrollBar;
    public GameObject IcobImage;
    public Text PlayerName;
    public float FarstBlock_Transform;
    public bool Rist_View=false;
    public Hashtable hashtable = new Hashtable();//�J�X�^���v���p�e�B�̃��X�g


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
        GameObject Block = Instantiate(BlockSumple, new Vector3(-1.0f, 0.0f, 0.0f), Quaternion.identity);//�u���b�N�̐���
        Block_List.Add(Block);//�u���b�N�����X�g�ɓ˂�����

        Block.transform.SetParent(BlockBox.transform, false);//�A�C�e�����X�g�̐e�q�֌W�̒���
        Block.GetComponent<ItemBlock>().ItemNumber=ItemNum;
        Block.GetComponent<ItemBlock>().player=Player;
        Block.GetComponent<ItemBlock>().Detail_Switch();
        
       
        MaskSize();
        BordSize();//�{�[�h�̃T�C�Y����
       
        BlockUpdate();//�u���b�N�̏ꏊ�𒲐�
    }

    public void LostItem(int ItemNum)
    {
        Destroy(Block_List[ItemNum]);
        Block_List.Remove(Block_List[ItemNum]);//�u���b�N�����X�g�ɓ˂�����
                
            
        

        MaskSize();
        BordSize();//�{�[�h�̃T�C�Y����

        BlockUpdate();//�u���b�N�̏ꏊ�𒲐�
    }

    public void BordSize()//�{�[�h�̃T�C�Y����
    {

        int BlockQuantity;
        if (Block_List.Count<=3) 
        {
            
            BlockQuantity = 3;

            ScrollBar.GetComponent<ScrollRect>().vertical=false;

        }
        else
        {
          
            BlockQuantity = Block_List.Count;//�u���b�N�̐�
            ScrollBar.GetComponent<ScrollRect>().vertical=true;

        }

        var scale = BlockSumple.GetComponent<RectTransform>().localScale.y;
        var BordSize_x = BlockSumple.GetComponent<RectTransform>().sizeDelta.x;//�{�[�h�̃T�C�Y���擾�@�i�߂�l�̂��߁j
        var BordSize_y = BlockSumple.GetComponent<RectTransform>().sizeDelta.y*BlockQuantity*scale;//�{�[�h�̃T�C�Y���擾�@�i�߂�l�̂��߁j

        AllBord.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,0, BordSize_y);

    }

    public void MaskSize()//�{�[�h�̃T�C�Y����
    {
        
        int BlockPuantity;
        if (Block_List.Count>3)
        {
        
            BlockPuantity = 3;

        }
        else
        {
            
            BlockPuantity = Block_List.Count;//�u���b�N�̐�
        }

        var scale = BlockSumple.GetComponent<RectTransform>().localScale.y;
        var MaskSize_x = BlockSumple.GetComponent<RectTransform>().sizeDelta.x;//�{�[�h�̃T�C�Y���擾�@�i�߂�l�̂��߁j
        var MaskSize_y = BlockSumple.GetComponent<RectTransform>().sizeDelta.y*BlockPuantity* scale;//�{�[�h�̃T�C�Y���擾�@�i�߂�l�̂��߁j
        


        Mask.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, -2, MaskSize_y+(3.5f));
     
        var MasTransform = Mask.GetComponent<RectTransform>().position;
 
        MasTransform.y = MasTransform.y+(1.50f*2f);
        if (Block_List.Count==3) {
            Mask.GetComponent<RectTransform>().position=MasTransform;
           
        }
        else
        {

        }
    }




    public void PuintUpdate()//�u���b�N�X�V��
    {
        
        foreach (var bloc in Block_List)
        {

            
            bloc.GetComponent<ItemBlock>().Detail_Switch();
          
        }
        
    }
    public void BlockUpdate()//�u���b�N�̏ꏊ�𒲐�
    {
        int loop = 0;
        foreach (var Block in Block_List)
        {
            var BlockTransform = Block.GetComponent<RectTransform>().position;//�u���b�N�̏ꏊ���擾
            var scale = BlockSumple.GetComponent<RectTransform>().localScale.y;
            BlockTransform.x=0+1.5f;
            BlockTransform.y=FarstBlock_Transform-(BlockSumple.GetComponent<RectTransform>().sizeDelta.y*loop*scale)-4;//�u���b�N�̏ꏊ������l�߂Ă���
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
