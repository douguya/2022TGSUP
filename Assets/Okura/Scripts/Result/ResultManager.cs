using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ResultManager : MonoBehaviourPunCallbacks
{
    //----------------------------------�ϐ�----------------------------------
    [SerializeField]
    int playersnum;         //�v���C���[�̐l��
    [SerializeField]
    Text textItems;           //�R�s�[���̃e�L�X�g
    [SerializeField]
    Text textPoints;           //�R�s�[���̃e�L�X�g
    [SerializeField]
    Text[] total;           //�g�[�^���X�R�A
    [SerializeField]
    Transform Canvas;       //Canvas
    [SerializeField]
    Transform[] ScoreBackGround;      //TextUI�̃R�s�[�̐e�ɂ���I�u�W�F�N�g(�ȉ�SBG)
    [SerializeField]
    GameObject[] PlayerBackGround;    //�v���C���[�̏����o���ꏊ(�ȉ�PBG)
    [SerializeField]
    Button display;
    [SerializeField]
    GameObject[] ScrollBar;
    [SerializeField]
    float interval = -25.0f;        //PBG�Ő��������e�L�X�g�{�b�N�X�̊Ԋu
    [SerializeField]
    GameObject[] players;             //I_Player_3D�������Ă�I�u�W�F�N�g
    [SerializeField]
    List<Anniversary_Item>[] OriginalItem;    //�v���C���[�������Ă��鏈���O�̃A�C�e��

    Dictionary<string, int> Item0 = new Dictionary<string, int> { };//���בւ��p�̋��List��l����
    Dictionary<string, int> Item1 = new Dictionary<string, int> { };
    Dictionary<string, int> Item2 = new Dictionary<string, int> { };
    Dictionary<string, int> Item3 = new Dictionary<string, int> { };

    List<Dictionary<string, int>> Items;//���dictionary�𑽎������������́Bplayer1�̃A�C�e����1�Ɠ��͂���

    GameObject BGM;
    GameObject SE;
    //----------------------------------�֐�----------------------------------
    private void Awake()
    {
        BGM = GameObject.FindGameObjectWithTag("BGM");
        SE = GameObject.FindGameObjectWithTag("SE");

        Cursor.visible = true;

        //���[���h�ϐ�����
        playersnum = PhotonNetwork.PlayerList.Length;
        total = new Text[playersnum];
        Canvas = GameObject.Find("Canvas").transform;
        ScoreBackGround = new Transform[playersnum];
        OriginalItem = new List<Anniversary_Item>[playersnum];
        Items = new List<Dictionary<string, int>> {{ Item0 },{ Item1 },{ Item2 },{ Item3 }};
        players = GameObject.FindGameObjectsWithTag("Player");
        ScrollBar = new GameObject[playersnum];


        //��������v���n�u�𐶐����邽�߂̉�����
        PlayerBackGround = new GameObject[playersnum];
        float[] PBGinitpos = new float[2] {-235.0f, -16.9f};//��������ۂ̏����ʒuxy
        float PBGinterval = 160.0f;//��������ۂ�x���W�̊Ԋu

        for (int i = 0; i < playersnum; i++)
        {
            //�v���n�u�ƃv���C���[�̏������[�h
            PlayerBackGround[i] = Resources.Load<GameObject>("PlayerItems" + i);

            //�v���n�u�𐶐�����
            GameObject CopyedPBG = Instantiate(PlayerBackGround[i],new Vector3(PBGinitpos[0] + (PBGinterval * i),PBGinitpos[1],0.0f), Quaternion.identity);
            CopyedPBG.name = "PlayerItems" + i;//���O��ύX
            CopyedPBG.transform.SetParent(Canvas, false);//canvas�̎q�ɐݒ肵�ĕ\��

            //�v���C���[�̖��O���Q�Ƃ��ݒ�
            Text Playername = GameObject.Find("Playername" + i).GetComponent<Text>();
            Playername.text = ReferencePlayername(players[i]);

            //�\�����Ɏg��SBG�ƃg�[�^���X�R�A���o���e�L�X�g�{�b�N�X���Q�Ƃ��ݒ�
            ScoreBackGround[i] = GameObject.Find("Content" + i).transform;
            total[i] = GameObject.Find("Total" + i).GetComponent<Text>();

            //���ёւ��O�̃v���C���[�̎��������Q��
            OriginalItem[i] = players[i].GetComponent<I_Player_3D>().Hub_Items;

            //�X�N���[���o�[�̑傫�������ŕK�v�ȎQ��
            ScrollBar[i] = GameObject.Find("Scroll View" + i);
        }

        DisplayItems();
    }


    private void Start()
    {
        BGM.GetComponent<AudioSource>().loop = false;

        BGM.GetComponent<BGMManager>().BGMsetandplay("ResultJingle");
        SE.GetComponent<SEManager>().SEsetandplay("HandClapJingle");
    }

    //���O�̎Q��
    public string ReferencePlayername(GameObject Player)
    {
        string name = "";
        foreach (var PList in PhotonNetwork.PlayerList)//�v���C���[���X�g�̓��e�����ԂɊi�[
        {
            if (PList.ActorNumber == Player.GetComponent<PhotonView>().CreatorActorNr) //���X�g�̃v���C���[��ID�ƃI�u�W�F�N�g�̍쐬�҂�AD���r
            {
                name = PList.NickName;
            }
        }
        return name;
    }


    //�A�C�e���̕\��
    public void DisplayItems()
    {
        textItems = GameObject.Find("Items").GetComponent<Text>();//�R�s�[�����Q��
        textPoints = GameObject.Find("Points").GetComponent<Text>();//�R�s�[�����Q��
        float[] initpos = new float[4] { textItems.transform.localPosition.x, textItems.transform.localPosition.y ,
                                         textPoints.transform.localPosition.x, textPoints.transform.localPosition.y };//�e�L�X�g�̏����ʒu,[0][1]�̓A�C�e��,[2][3]�̓|�C���g
        int[] totalpoint = new int[playersnum];//�g�[�^���X�R�A�i�[�p
        int count = 0;//���[�v��
        

        DuplicateItem();

        foreach(Transform i in ScoreBackGround)
        {
            int dupcount = 0;//�A�C�e���̕\����
            float TotalBordSize = 0.0f;
            Text LastCopy = null;
            TextLineAdjust LastCopyLineAdjust = null;

            foreach (string j in Items[count].Keys)
            {
                //�ŏ��͊l���������̂��������ŕ\��
                Text Copytext = Instantiate(textItems, new Vector2(initpos[0], initpos[1]), Quaternion.identity);
                Copytext.transform.SetParent(i, false);
                Copytext.text = j;
                var CopyLineAdjust = Copytext.GetComponent<TextLineAdjust>();
                CopyLineAdjust.LineAdjust();

                var linenum = CopyLineAdjust.LineNum;
                if (dupcount >= 1) {

                    var CopyTextPosition = Copytext.GetComponent<RectTransform>().anchoredPosition;

                    

                    var dump = (linenum - 1)* Copytext.fontSize;
                    CopyTextPosition.y = (LastCopy.GetComponent<RectTransform>().anchoredPosition.y + -(LastCopyLineAdjust.Rectheight / 2 )- dump) + interval;
                    Copytext.GetComponent<RectTransform>().anchoredPosition = CopyTextPosition;
                    TotalBordSize = -((CopyTextPosition.y + -(CopyLineAdjust.Rectheight / 2) - dump) + interval);
                    Debug.Log(LastCopy.GetComponent<RectTransform>().anchoredPosition.y - Copytext.GetComponent<RectTransform>().anchoredPosition.y);
                }

                LastCopy = Copytext;
                LastCopyLineAdjust = LastCopy.GetComponent<TextLineAdjust>();

                //���Ɋl���������̂̃|�C���g���E�����ŕ\��
                Text point = Instantiate(textPoints, new Vector2(initpos[2], initpos[3]), Quaternion.identity);
                    
                point.transform.SetParent(Copytext.transform, false);
                point.alignment = TextAnchor.MiddleRight;
                point.text = Items[count][j] + "P";
                var pointRect = point.GetComponent<RectTransform>();

                var PointHight= Copytext.GetComponent<RectTransform>().sizeDelta.y;
                pointRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, PointHight);

                var point_spot = pointRect.anchoredPosition;
                point_spot.y= -linenum*(CopyLineAdjust.ItemNameTextBox.fontSize)/2;
                pointRect.anchoredPosition=point_spot;

                totalpoint[count] += Items[count][j];
                    dupcount++;
            }

            BordSizeAdjust(Items[count], i, ScrollBar[count], TotalBordSize);
            total[count].text = totalpoint[count].ToString() + "P";//�g�[�^���X�R�A�̕\��
            count++;
        }


        //���ʕ\��,totalpoint�̂����Ə���
        int[] tp = new int[playersnum];
        int[] Rank = new int[playersnum];

        //�l����
        for (int i = 0; i < playersnum; ++i)
        {
            tp[i] = totalpoint[i];
            Rank[i] = i;
        }

        //�~����
        for (int i = 0; i < playersnum; ++i)
        {
            for (int j = 0; j < playersnum; ++j)
            {
                if(tp[i] > tp[j])
                {
                    int tmp;
                    tmp = Rank[i];
                    Rank[i] = Rank[j];
                    Rank[j] = tmp;

                    tmp = tp[i];
                    tp[i] = tp[j];
                    tp[j] = tmp;
                }
            }
        }

        //���(Rank[0])�̂݉�����\��
        Image RImage = GameObject.Find("Rank" + Rank[0]).GetComponent<Image>();
        RImage.sprite = Resources.Load<Sprite>("1st");
    }


    //�d���A�C�e�����܂Ƃ߂Ă������肳����
    void DuplicateItem()
    {
        for (int num = 0; num < OriginalItem.Length; num++)//�v���C���[�̐��Ɠ����񐔍s��
        {
            for (int i = 0; i < OriginalItem[num].Count; i++)//num�Ԗڂ̃v���C���[�̎������̐������s��
            {
                if (Items[num].ContainsKey(OriginalItem[num][i].ItemName))//�A�C�e���̒��ɏd������҂������
                {
                    Items[num][OriginalItem[num][i].ItemName] += OriginalItem[num][i].ItemPoint;//�|�C���g�𑝂₷
                }
                else
                {
                    Items[num].Add(OriginalItem[num][i].ItemName, OriginalItem[num][i].ItemPoint);//�Ȃ���ΐV�������ڂ����
                }
            }
        }
    }



    //�{�[�h�̃T�C�Y����
    public void BordSizeAdjust(Dictionary<string, int> PlayerItems, Transform SBG, GameObject Bar, float TotalBordY)
    {
        int BlockQuantity;

        if (PlayerItems.Keys.Count <= 9)
        {

            BlockQuantity = 9;
            Bar.GetComponent<ScrollRect>().vertical = false;

        }
        else
        {

            BlockQuantity = PlayerItems.Keys.Count;//�A�C�e���̐�
            Bar.GetComponent<ScrollRect>().vertical = true;

        }


        var BordSize_y = TotalBordY;//�{�[�h�̃T�C�Y���擾�@�i�߂�l�̂��߁j

        SBG.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, BordSize_y);
    }


    public void GoTitle()
    {
        BGM.GetComponent<AudioSource>().loop = true;
        BGM.GetComponent<BGMManager>().BGMsetandplay("TitleBGM");
    }
}