 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MannequinPlayer : MonoBehaviour
{
    public int PlayerNumber;
    // Start is called before the first frame update

    public Day_Square_Master DayMaster;
    public Anniversary_Item_Master ItemMaster;
    public List<Anniversary_Item> Hub_Items = new List<Anniversary_Item>();
    public GameObject blocs;
    public GameObject Log;
    public Text JujeText;
    void Start()
    {

    } 

    // Update is called once per frame
    void Update()
    {
       
    }
    public void ItemAdd(int ItemNum)//ItemNum���}�X�^�[�o�^���̔ԍ�
    {
        Hub_Items.Add(ScriptableObject.Instantiate(ItemMaster.Anniversary_Items[ItemNum]));//�}�X�^�[�ɂ���ItemNum�̃A�C�e���𐶐����AHub�ɒǉ�
        blocs.GetComponent<ItemBlock_List_Script>().AddItem(ItemNum);
    }

    public void ItemLost(int HubItemNum)//HubItemNum�������A�C�e���o�^���̔ԍ�
    {
      
        Hub_Items.RemoveAt(HubItemNum);//��������HubItemNum�Ԗڂ̃A�C�e��������
        blocs.GetComponent<ItemBlock_List_Script>().LostItem(HubItemNum);


    }

    public void NextMyTurn()
    {
        Debug.Log("AAA"+Hub_Items.Count);
        int num = Hub_Items.Count;
       
        for (int loop=num-1;loop>=0;loop--)
        {
            
            if (Hub_Items[loop].ItemLifespan!="null") 
            {
                Hub_Items[loop].ItemLifespan=(int.Parse(Hub_Items[loop].ItemLifespan)-1).ToString();
                if (int.Parse(Hub_Items[loop].ItemLifespan)<=0){ Hub_Items.Remove(Hub_Items[loop]); }
            }
        }

    }

    public void textadd(string LogText)//���{��50����
    {
      
      Text texts=  Log.GetComponent<Text_Log>().TextObj.GetComponent<Text>();//���O�̃e�L�X�g���Q�ƂŎ擾
        /*
      JujeText.text=LogText;

      float height = JujeText.preferredWidth;
         Debug.Log(height);
        */

        texts.text=texts.text+"\n"+LogText;//���O�̃e�L�X�g���e�ɒǉ�
    }

   public void Dayicon(int num)
    {
        Debug.Log("QQQQQQQQQQQQQQQQQQQ"+DayMaster.Day_Squares[num].Icon);
        Debug.Log("QQQQQQQQQQQQQQQQQQQ"+DayMaster.Day_Squares[num+1].Icon);

        if (DayMaster.Day_Squares[num+1].Icon!=null)
        {
            Debug.Log("MMMMMMMMMMMMMMMMMMM"+DayMaster.Day_Squares[num+1].Icon);
        }
        else
        {
            Debug.Log("TTTTTTTTTTTTTTTTTTTTT"+DayMaster.Day_Squares[num+1].Icon);
        }


    }

}
