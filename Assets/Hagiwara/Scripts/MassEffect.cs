using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassEffect : MonoBehaviour
{
    //public GameObject[] Player = new GameObject[4];//�v���C���[�I�u�W�F�N�g�擾
    public GameObject daycomment;

    public float speed = 0.5f;                      //�v���C���[�ړ����x
    private float currentTime = 0f;
    private int Count = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Effects(string day)
    {

        GetComponent<PlayerStatus>().daycomment(day);
        
        Debug.Log(DictionaryManager.DayEffectictDictionary[day][0]);
        Debug.Log("�E��"+DictionaryManager.EffectictCategoryDictionary[DictionaryManager.DayEffectictDictionary[day][0]][0]); 
       // Debug.Log("RRRRRRRRRRRRR" + DictionaryManager.DayEffectictDictionary[day][0,0]);
        switch (DictionaryManager.EffectictCategoryDictionary [DictionaryManager.DayEffectictDictionary[day][0]] [0])
        {
            case "�A�C�e��":
                Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                GetItem( DictionaryManager.EffectictCategoryDictionary[DictionaryManager.DayEffectictDictionary[day][0]][1]);
                break;

           // case "�A�C�R��":

                //break;

          //  case "BGM":
           //     break;


         //   case "�w�i":
         //       break;

         //   case "�g":
                
         //       break;

         //   case "":
         //       break;

            default:
                GetItem("�ėp�J�����_�[");
                break;
        }
        
        

    }
    
    private void step()
    {
       // GetComponent<PlayerStatus>().stopon();
    }

    private void GetItem(string Iname)
    {
        Debug.Log("aaaaaaaaaaaaaaaaaaaaa"+Iname);
        GetComponent<PlayerStatus>().Itemobtain(Iname);
    }

}
