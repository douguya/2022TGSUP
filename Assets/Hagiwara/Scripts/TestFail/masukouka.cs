using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class masukouka : MonoBehaviour
{
    public GameObject BuckGround;
    public GameObject Icon;
    public GameObject[] Player = new GameObject[4];
    public float speed = 0.5f;                      //�v���C���[�ړ����x
    private float currentTime = 0f;
    private int Count = 0;
    private int SwitchCount = 0;
    private bool Go = false;
    private int[] Xplay = new int[4];
    private int[] Yplay = new int[4];
    public Sprite yuurei;//�H��̉摜
    public Sprite Tsyatu;//T�V���c�̉摜
    public Sprite bikini;//�r�L�j�̉摜
    public Sprite Default;//�ʏ�摜

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Effects(string day)
    {
        switch (Daykouka.DayEffectictDictionary[day][0, 1])
        {
            case "�A�C�e��":
                GetItem(Daykouka.DayEffectictDictionary[day][0, 2]);
                break;

            case "�A�C�R��":
                Icon.GetComponent<BuckGroundChange>().Change(day);
                break;

            case "BGM":
                //BGM�؂�ւ�
                break;

            case "�w�i":
                BuckGround.GetComponent<BuckGroundChange>().Change(day);//�����̓��݂̂Ȃ̂ł�������
                GetItem(Daykouka.DayEffectictDictionary[day][0, 2]);
                break;

            case "�w�i��BGM":
                BuckGround.GetComponent<BuckGroundChange>().Change(day);
                //BGM�؂�ւ�
                break;

            case "����":
                for (int i = 5; i < 10; i++)
                {
                    for (int l = 7; l < 14; l++)
                    {
                        GetComponent<PlayerStatus>().week[i].width[l].GetComponent<Mass>().invalid = true;
                        if (GetComponent<PlayerStatus>().week[i].width[l].GetComponent<Mass>().Goal == true)
                        {
                            GetComponent<PlayerStatus>().week[i].width[l].GetComponent<Mass>().Goal = false;
                        }
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    if (6 < Player[i].GetComponent<PlayerStatus>().PlayerX() && 4 < Player[i].GetComponent<PlayerStatus>().PlayerY())
                    {
                        int Week, Day;
                        do
                        {
                            Week = Random.Range(0, 10);
                            Day = Random.Range(0, 14);
                        } while (GetComponent<PlayerStatus>().week[Week].width[Day].GetComponent<Mass>().invalid == true);
                        Player[i].GetComponent<PlayerStatus>().PlayerMass(Day,Week);
                    }
                }
                GoalDecision();
                break;

            case "����������":
                for (int i = 0; i < 4; i++)
                {
                    if (6 < Player[i].GetComponent<PlayerStatus>().PlayerX() && 4 < Player[i].GetComponent<PlayerStatus>().PlayerY())
                    {
                        Player[i].GetComponent<PlayerStatus>().Itemobtain("����������");
                    }
                }
                
                break;

            case "���":
                if (GetComponent<PlayerStatus>().week[8].width[3].GetComponent<Mass>().Open == true)
                {
                    GetComponent<PlayerStatus>().Itemobtain("�Ή��F���+");
                }
                else
                {
                    GetComponent<PlayerStatus>().Itemobtain("�Ή��F���");
                }
                break;

            case "�P�[�L":
                GetComponent<PlayerStatus>().Itemobtain("�V���[�g�P�[�L");
                if (GetComponent<PlayerStatus>().week[7].width[11].GetComponent<Mass>().Open == true)
                {
                    GetComponent<PlayerStatus>().PlayerMass(11,7);
                }
                break;

            case "�C�`�S":
                int have = 0;
                int list = GetComponent<PlayerStatus>().HabItem.Count;
                for (int i = 0; i < list; i++)
                {
                    if (GetComponent<PlayerStatus>().HabItem[i] == "�S�[��")
                    {
                        have = 1;
                    }
                }
                if (have < 1)
                {
                    GetComponent<PlayerStatus>().Itemobtain("�C�`�S+");
                }
                else
                {
                    GetComponent<PlayerStatus>().Itemobtain("�C�`�S");
                }
                break;

            case "�g":
                int[,] Loot = new int[,] { { 1, 8 }, { 1, 9 }, { 1, 10 } };
                currentTime += Time.deltaTime;      //�v���C���[�̈ړ���������i�ނ悤��
                if (currentTime > speed)
                {
                    GetComponent<PlayerStatus>().PlayerMass(Loot[Count,1], Loot[Count,0]); //����i�߂�
                    Count++;
                    currentTime = 0f;
                }
                if (Count > 2)
                {
                    Count = 0;
                }
                break;

            case "���؂�":
                int[] Xplay = new int[4];
                int[] Yplay = new int[4];

                switch (SwitchCount)
                {
                    case 0:
                        for (int i = 0; i < 4; i++)
                        {
                            Xplay[i] = Player[i].GetComponent<PlayerStatus>().PlayerX();
                            Yplay[i] = Player[i].GetComponent<PlayerStatus>().PlayerY();
                            GetComponent<PlayerStatus>().week[Yplay[i]].width[Xplay[i]].GetComponent<Mass>().Selecton();

                        }
                        SwitchCount = 1;
                        break;
                    case 1:

                        Move();

                        for (int i = 0; i < GetComponent<PlayerStatus>().week.Length; i++)
                        {
                            for (int l = 0; l < GetComponent<PlayerStatus>().week[0].width.Length; l++)
                            {
                                for (int P = 0; P < 4; P++)
                                {
                                    if (Yplay[P] == i && Xplay[P] == l && GetComponent<PlayerStatus>().week[i].width[l].GetComponent<Mass>().walk == true)
                                    {
                                        Player[P].GetComponent<PlayerStatus>().PlayerMass(GetComponent<PlayerStatus>().PlayerX(), GetComponent<PlayerStatus>().PlayerY());
                                        GetComponent<PlayerStatus>().PlayerMass(l, i);
                                        Go = true;
                                    }
                                }
                            }
                        }
                        if (Go)
                        {
                            Go = false;
                            clearSelect();
                            SwitchCount = 0;
                        }

                        break;
                }
                break;

            case "���l":
                switch (SwitchCount)
                {
                    case 0:
                        for (int i = 0; i < 4; i++)
                        {
                            GetComponent<PlayerStatus>().week[Player[i].GetComponent<PlayerStatus>().PlayerY()].width[Player[i].GetComponent<PlayerStatus>().PlayerX()].GetComponent<Mass>().Selecton();
                        }
                        SwitchCount = 1;
                        break;
                    case 1:
                        Move();
                        break;
                }
                break;

            case "�o�X":

                switch (SwitchCount)
                {
                    case 0:
                        GetComponent<PlayerStatus>().week[7].width[9].GetComponent<Mass>().Selecton();
                        GetComponent<PlayerStatus>().week[9].width[9].GetComponent<Mass>().Selecton();
                        GetComponent<PlayerStatus>().week[8].width[8].GetComponent<Mass>().Selecton();
                        GetComponent<PlayerStatus>().week[8].width[10].GetComponent<Mass>().Selecton();
                        SwitchCount = 1;
                        break;
                    case 1:
                        Move();
                        break;
                }
                break;

            case "UFO":

                switch (SwitchCount)
                {
                    case 0:
                        GetComponent<PlayerStatus>().week[3].width[5].GetComponent<Mass>().Selecton();
                        GetComponent<PlayerStatus>().week[4].width[7].GetComponent<Mass>().Selecton();
                        GetComponent<PlayerStatus>().week[8].width[3].GetComponent<Mass>().Selecton();
                        GetComponent<PlayerStatus>().week[8].width[13].GetComponent<Mass>().Selecton();
                        SwitchCount = 1;
                        break;
                    case 1:
                        Move();
                        break;
                }
                break;

            case "���E�ό�":

                switch (SwitchCount)
                {
                    case 0:
                        GetComponent<PlayerStatus>().week[1].width[0].GetComponent<Mass>().Selecton();
                        GetComponent<PlayerStatus>().week[0].width[13].GetComponent<Mass>().Selecton();
                        GetComponent<PlayerStatus>().week[9].width[0].GetComponent<Mass>().Selecton();
                        GetComponent<PlayerStatus>().week[9].width[12].GetComponent<Mass>().Selecton();
                        SwitchCount = 1;
                        break;
                    case 1:
                        Move();
                        break;
                }
                break;

            case "��":

                switch (SwitchCount)
                {
                    case 0:
                        for (int i = 0; i < GetComponent<PlayerStatus>().week.Length; i++)
                        {
                            for (int l = 0; l < GetComponent<PlayerStatus>().week[0].width.Length; l++)
                            {
                                if (GetComponent<PlayerStatus>().week[i].width[l].GetComponent<Mass>().invalid == false)
                                {
                                    GetComponent<PlayerStatus>().week[i].width[l].GetComponent<Mass>().Selecton();
                                }
                            }
                        }
                        SwitchCount = 1;
                        break;
                    case 1:
                        Move();
                        break;
                }
                break;

            case "�p�����g��":
                GetComponent<PlayerStatus>().PlayerMass(5, 7);
                break;

            case "����":
                for (int i = 0; i < 4; i++)
                {
                    Player[i].GetComponent<PlayerStatus>().PlayerMass(1, 4);
                }
                break;

            case "":
                break;

            default:
                break;
        }



    }

    private void step()
    {
        //GetComponent<PlayerStatus>().stopon();
    }

    private void GetItem(string Iname)
    {
        GetComponent<PlayerStatus>().Itemobtain(Iname);
    }

    private void GoalDecision()
    {
        int Week, Day;
        do
        {
            Week = Random.Range(0, 10);
            Day = Random.Range(0, 14);
        } while (GetComponent<PlayerStatus>().week[Week].width[Day].GetComponent<Mass>().invalid == true);
        GetComponent<PlayerStatus>().week[Week].width[Day].GetComponent<Mass>().GoalOn();

    }

    private void Move()
    {
        for (int i = 0; i < GetComponent<PlayerStatus>().week.Length; i++)
        {
            for (int l = 0; l < GetComponent<PlayerStatus>().week[0].width.Length; l++)
            {
                if (GetComponent<PlayerStatus>().week[i].width[l].GetComponent<Mass>().walk == true)
                {
                    GetComponent<PlayerStatus>().PlayerMass(l, i);
                    Go = true;
                }
            }
        }
        if (Go)
        {
            Go = false;
            clearSelect();
            SwitchCount = 0;
        }
    }

    private void clearSelect()//�I���ł���}�X�̑S����
    {
        for (int i = 0; i < GetComponent<PlayerStatus>().week.Length; i++)
        {
            for (int l = 0; l < GetComponent<PlayerStatus>().week[0].width.Length; l++)
            {
                GetComponent<PlayerStatus>().week[i].width[l].GetComponent<Mass>().Selectoff();//�}�X��I���o����Ƃ���image������
                GetComponent<PlayerStatus>().week[i].width[l].GetComponent<Mass>().walk = false;//�N���b�N���ꂽ�Ƃ������������
            }
        }
    }
}
