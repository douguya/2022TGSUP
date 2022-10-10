
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using Photon.Pun;

public class Text_Log : MonoBehaviourPunCallbacks
{
    public GameObject TextObj;//�e�L�X�g�̃I�u�W�F�N�g
    public GameObject TextBoard;//�e�L�X�g�̃I�u�W�F�N�g
    public GameObject nowText;//�e�L�X�g�̃I�u�W�F�N�g
    public I_game_manager Game_Manager;
   
    public InputField InputField;     //���O���͗�
    public Text TestText;

    public float Scloll;//�X�N���[���̈ړ���
    public float Scloll_Coefficient;//�X�N���[���̌W��(�t�H���g�T�C�Y�̑���ɔ�Ⴕ�đ傫���Ȃ�@�v�Z������Ȃ��������ߓ���)
    private int SclollCount = 0;//�X�N���[���񐔂̃J�E���g
    private bool Text_View = false;
    private Vector2 Initial_Value;//
    private float FastBorad;

    

    public int textVar = 0;

    public int Large_Sclol = 0;
    public int FallLineCount = 0;

    public string Password;
    // Start is called before the first frame update
    void Start()
    {
        var Text = TextObj.GetComponent<Text>();
        Scloll=Text.fontSize*Text.lineSpacing* Scloll_Coefficient;//�X�N���[���ʂ̎Z�o
        Initial_Value=TextObj.GetComponent<RectTransform>().anchoredPosition;//�����ʒu���o��
        FastBorad= nowText.GetComponent<RectTransform>().anchoredPosition.y;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnScroll()
    {
        var BlockTransform = TextObj.GetComponent<RectTransform>().anchoredPosition;//�u���b�N�̏ꏊ���擾
        var nowPosi = nowText.GetComponent<RectTransform>().anchoredPosition;
        var Mousewheel = Input.GetAxis("Mouse ScrollWheel");//�}�E�X�z�C�[���l�̕ۑ�
        var TextDami = TextObj.GetComponent<Text>().text;
        var num = TextDami.Count(f => f == '\n')+1+FallLineCount;//���s�����R�[�h�̐�����s�����Z�o
        textVar=num;
      

        var TextBordSize = TextBoard.GetComponent<RectTransform>().sizeDelta;
        int Large_SclollCount =33;

        Large_Sclol=Large_SclollCount;

        if (Mousewheel!=0)//�}�E�X�z�C�[�����s��ꂽ�ꍇ
        {



            if (Mousewheel>0)//������z�C�[��
            {
               
                    if (SclollCount< textVar-Large_SclollCount)
                    {
                        SclollCount++;
                        BlockTransform.y-=(Scloll);
                        nowPosi.y-=Scloll;
                        TextObj.GetComponent<RectTransform>().anchoredPosition=BlockTransform;

                        nowText.GetComponent<RectTransform>().anchoredPosition= nowPosi;
                    }
                


            }
            if (Mousewheel<0)//�������z�C�[��
            {

                if (SclollCount>0)
                {
                    SclollCount--;
                    BlockTransform.y+=(Scloll);
                    TextObj.GetComponent<RectTransform>().anchoredPosition=BlockTransform;
                    nowPosi.y+=Scloll;
                    nowText.GetComponent<RectTransform>().anchoredPosition= nowPosi;
                }


            }

            Debug.Log(" SclollCount"+SclollCount);
        }




    }
   

    public void textadd(string LogText)//���{��50����
    {
        SclollCount=0;

        TextObj.GetComponent<RectTransform>().anchoredPosition= Initial_Value;

        Text texts = TextObj.GetComponent<Text>();//���O�̃e�L�X�g���Q�ƂŎ擾
        NowTextBord(LogText);

        texts.text=texts.text+"\n"+LogText;//���O�̃e�L�X�g���e�ɒǉ�
    }






    public void NowTextBord(string LogText)//���{��50����
    {
        TestText.text=LogText;
        var BordSize = nowText.GetComponent<RectTransform>().sizeDelta;

       int SizeCount = Mathf.CeilToInt(TestText.preferredWidth/BordSize.x);
        if (SizeCount>1)
        {
            FallLineCount+=(SizeCount-1);
        }

        BordSize.y = Scloll*SizeCount;

        nowText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, BordSize.y);

        var nowPosi = nowText.GetComponent<RectTransform>().anchoredPosition;

        nowPosi.y= FastBorad+(9.73f*(SizeCount-1));
        nowText.GetComponent<RectTransform>().anchoredPosition= nowPosi;

    }








    public void Direct_Log_InputField()//�`���b�g�ɓ���
    {
        GameObject.Find("I_game_manager").GetComponent<Guide>().chat_Finish();
        if (Input.GetKey(KeyCode.Return))
        {
            if (InputField.GetComponent<InputField>().text==Password)
            {
                Game_Manager.ConnectGameFinish();
            }
            if (InputField.GetComponent<InputField>().text=="888")
            {
                Game_Manager.Player[0].GetComponent<I_Player_3D>().ItemAdd_ToConnect(92);
            }

            if (InputField.GetComponent<InputField>().text == "PTC")
            {
                Game_Manager.PlayerTurn_change();
            }

            var name = PlayerColouradd(PhotonNetwork.NickName);
            string Chat = name+":" +InputField.GetComponent<InputField>().text;

            if (Chat!="")
            {

                Debug.Log(name);

                //���̓t�H�[���̃e�L�X�g����ɂ���
                // textadd(Chat);
                photonView.RPC(nameof(Direct_Log_RPC__InputField), RpcTarget.AllViaServer, Chat);
                InputField.text = "";
            }
        }

    }
    public string PlayerColouradd(string Chat)
    {
        var ColourNum = PhotonNetwork.LocalPlayer.CustomProperties["PlayerNumMaterial"];
        var Text = Chat;
        switch (ColourNum)
        {

            case 0:
                Text="<color=red>"+Text+"</color>";

                break;
            case 1:
                Text="<color=blue>"+Text+"</color>";

                break;
            case 2:
                Text="<color=orange>"+Text+"</color>";

                break;
            case 3:
                Text="<color=lime>"+Text+"</color>";

                break;

        }
        return Text;
    }






    [PunRPC]
    public void Direct_Log_RPC__InputField(string LogText)//�S�̂Ƀ��O�𑗂�
    {
        Game_Manager.SE.GetComponent<SEManager>().SEsetandplay("TextSE");
        textadd(LogText);
    }
    public string SistemrColouradd()
    {

        var system = "<color=purple>"+"�V�X�e��:"+"</color>";


        return system;
    }


}