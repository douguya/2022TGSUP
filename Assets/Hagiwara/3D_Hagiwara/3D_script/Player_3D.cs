using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Player_3D : MonoBehaviour
{
    public int PlayerNumber;                      //�v���C���[�ԍ�

    public int XPlayer_position;                  //�v���C���[�̌��݂̉��̈ʒu
    public int YPlayer_position;                  //�v���C���[�̌��݂̏c�̈ʒu

    int Xcenter, Ycenter;                         //�I���ł���}�X�̒��S�}�X

    public int Move_Point = 0;                    //�v���C���[�̈ړ��ł������ 
    private int select_Point = 0;                 //�}�X��I���ł��鐔
    private bool[] Player_warpMove = new bool[40];//�v���C���[�̈ړ����@

    public int Goalcount = 0;

    private int[] XPlayer_Loot = new int[40];     //�I�������}�X���L������
    private int[] YPlayer_Loot = new int[40];


    public GameObject GameManager;                //GameManager�I�u�W�F�N�g�̎擾
    private game_manager Manager;                 //game_manager���擾

    public GameObject DiceButton;                           //�_�C�X���~�߂�ׂ̃I�u�W�F�N�g�擾
    public GameObject ButtonText;                           //�_�C�X�̃e�L�X�g�I�u�W�F�N�g�擾
    private bool DiceStrat = true;                          //�{�^�����_�C�X�̊J�n���X�g�b�v��

    public bool Effect;                           //�G�t�F�N�g�ɂ��ړ����ǂ���
    public bool Exchange;                         //�ꏊ�������邩�ǂ���
    public bool Turn_change;                      //�^�[����ς��邩�ǂ���

    public bool OneMore_Dice;
    public int DiceAdd = 0;
    public int DiceMultiply = 0;

    private bool Effect_ready;

    private bool MoveStop_Push;
    public int MoveAdd_point = 0;
    public int MoveStop_point = 0;
    private int MovePoint_Count = 0;
    public bool selectwark;

    void Start()
    {

    }


    void Update()
    {

    }

    //�v���C���[�̕\��
    public void Player_indicate()
    {
        gameObject.SetActive(true);
    }



    //�v���C���[�̏����ʒu�ݒ�
    public void Player_position_setting(int Y_position, int X_position)
    {
        Manager = GameManager.GetComponent<game_manager>();
        XPlayer_position = X_position;//�v���C���[�̌��݂̏c�E���ʒu
        YPlayer_position = Y_position;
        transform.position = Manager.Week[Y_position].Day[X_position].GetComponent<Mass_3D>().transform.position;//�v���C���[�̈ړ�
        //Debug.Log("�v���C���[�̏����ʒu "+Y_position + " : "+ X_position);
    }



    //�_�C�X���񂷏���
    public void Dice_ready()
    {
        DiceButton.GetComponent<Button>().interactable = true;
        ButtonText.GetComponent<Text>().text = "�_�C�X����";
        if (selectwark)
        {
            ButtonText.GetComponent<Text>().text = "�i��";
        }
    }

    //�_�C�X���~�߂Ēl���󂯎��
    private void Dice_Stop()
    {
        if (OneMore_Dice)
        {
            OneMore_Dice = false;
            DiceAdd = Manager.Output_DiceStop();
            Dice_ready();
        }
        else
        {
            Move_Point = Manager.Output_DiceStop() + DiceAdd;
            if (DiceMultiply != 0)
            {
                Move_Point *= DiceMultiply;
            }
            MoveStop_point = Move_Point;
            Move_Point += MoveAdd_point;
            MovePoint_Count = 0;
            MoveAdd_point = 0;
            DiceAdd = 0;
            DiceMultiply = 0;
            //Debug.Log("�����F" + Move_Point);
            MoveSelect();
        }
    }

    public void DicePush()
    {
        if (PlayerNumber == Manager.Player_Turn)
        {
            if (selectwark)
            {
                selectwark = false;
                DiceButton.GetComponent<Button>().interactable = false;
                ButtonText.GetComponent<Text>().text = "�ړ���I��";
                Move_Point = MoveAdd_point;
                MovePoint_Count = 0;
                MoveAdd_point = 0;
                DiceAdd = 0;
                DiceMultiply = 0;
                //Debug.Log("�����F" + Move_Point);
                MoveSelect();
            }
            else
            {
                if (MoveStop_Push)
                {
                    MoveStop_Push = false;
                    select_Point = 0;
                    DiceButton.GetComponent<Button>().interactable = false;
                    ButtonText.GetComponent<Text>().text = "�ړ���I��";
                    for (int week = 0; week < Manager.Week.Length; week++)
                    {
                        for (int day = 0; day < Manager.Week[0].Day.Length; day++)
                        {
                            Output_SelectClear(week, day);
                        }
                    }
                    StartCoroutine(PlayerMove_Coroutine(MovePoint_Count, true));//�v���C���[�̈ړ��J�n
                }
                else
                {
                    if (DiceStrat)
                    {
                        gameObject.GetComponent<Day_Effect>().DiceSetting();
                        //�����Ń_�C�X���񂷏���
                        //Manager.Output_DiceStart();
                        ButtonText.GetComponent<Text>().text = "�_�C�X���~�߂�";
                        DiceStrat = false;
                    }
                    else
                    {
                        Dice_Stop();//�_�C�X���~�߂Ēl���󂯎��
                        DiceButton.GetComponent<Button>().interactable = false;
                        ButtonText.GetComponent<Text>().text = "�ړ���I��";
                        DiceStrat = true;
                    }
                }
            }
        }
    }

    public void another_turn()
    {
        ButtonText.GetComponent<Text>().text = "���v���C���[�̃^�[��";
    }





    //�I���ł���}�X�̕\���̏����ݒ�
    public void MoveSelect()
    {
        Xcenter = XPlayer_position;                 //�I���̒��S�ƂȂ�}�X��ݒ�
        Ycenter = YPlayer_position;
        YPlayer_Loot[0] = Ycenter;                  //�v���C���[�̌��݂̃}�X���L������
        XPlayer_Loot[0] = Xcenter;

        Output_decisionSetting(Ycenter, Xcenter);   //���݂̃}�X���ړ����肵���}�X�ɂ���
        select_Point = Move_Point;                  //�I���ł��鐔�Ƀ_�C�X�̖ڂ�����
        MoveSelect_Display();                       //�I���ł���}�X�̕\��
    }

    //�I���ł���}�X�̕\��
    private void MoveSelect_Display()
    {
        int[] Select = new int[4];                                              //�I���̒��S�ƂȂ�}�X�̎l����ݒ肷��

        Output_SelectClear(Ycenter, Xcenter);                                   //�I���̒��S�ƂȂ�}�X�̑I���}�X(���F���)��\��

        Select[0] = Xcenter - 1; Select[1] = Xcenter + 1;                       //�I���̒��S�ƂȂ�}�X�̍��E������
        for (int way = 0; way < 2; way++)
        {
            //�I���̒��S�ƂȂ�}�X�̍��E�����݂��ړ����肳�ꂽ�}�X�łȂ���
            if (0 <= Select[way] && Select[way] < Manager.Week[0].Day.Length && Manager.Week[Ycenter].Day[Select[way]].GetComponent<Mass_3D>().decision == false)
            {
                Output_SelectSetting(Ycenter, Select[way]);                      //�ړ����肵���}�X��\��
            }
        }

        Select[2] = Ycenter - 1; Select[3] = Ycenter + 1;                        //�I���̒��S�ƂȂ�}�X�̏㉺������
        for (int way = 2; way < Select.Length; way++)
        {
            //�I���̒��S�ƂȂ�}�X�̏㉺�����݂��ړ����肳�ꂽ�}�X�łȂ���
            if (0 <= Select[way] && Select[way] < Manager.Week.Length && Manager.Week[Select[way]].Day[Xcenter].GetComponent<Mass_3D>().decision == false)
            {
                Output_SelectSetting(Select[way], Xcenter);                     //�ړ����肵���}�X��\��
            }
        }

        if (Manager.Week[Ycenter].Day[Xcenter].GetComponent<Mass_3D>().warp == true)//�I���̒��S�ƂȂ�}�X�����[�v�}�X�Ȃ�
        {
            for (int week = 0; week < Manager.Week.Length; week++)
            {
                for (int day = 0; day < Manager.Week[0].Day.Length; day++)
                {
                    if (Manager.Week[week].Day[day].GetComponent<Mass_3D>().warp == true)
                    {
                        Output_SelectSetting(week, day);                        //�I���ł���}�X��\��
                    }
                }
            }
        }
    }

    //�I���ł���}�X����ړ����肷��
    public void MoveSelect_Clicked()
    {
        if (Effect == false)
        {
            for (int week = 0; week < Manager.Week.Length; week++)
            {
                for (int day = 0; day < Manager.Week[0].Day.Length; day++)
                {

                    Output_SelectClear(week, day);                                           //�S�Ă̑I���}�X(���F���)��\��
                    if (Manager.Week[week].Day[day].GetComponent<Mass_3D>().On_Click)        //�}�X���N���b�N���ꂽ���̂�
                    {
                        //Debug.Log("���肵���}�X�I");
                        select_Point--;                                                      //�v���C���[�̈ړ��ł��������1���炷
                        MoveStop_point--;
                        MovePoint_Count++;
                        Manager.Week[week].Day[day].GetComponent<Mass_3D>().On_Click = false;//�N���b�N���ꂽ����������
                        YPlayer_Loot[Move_Point - select_Point] = week;                      //�ړ����肵���}�X���L������
                        XPlayer_Loot[Move_Point - select_Point] = day;
                        //���S�}�X�����[�v�}�X�ł������烏�[�v�}�X�Ɉړ�������
                        if (Manager.Week[Ycenter].Day[Xcenter].GetComponent<Mass_3D>().warp == true && Manager.Week[week].Day[day].GetComponent<Mass_3D>().warp == true)
                        {
                            Player_warpMove[Move_Point - select_Point] = true;              //���[�v�̃��[�V����������悤�ɂ���
                            Debug.Log("���[�V����");
                        }
                        //Debug.Log("�s���:"+ (Move_Point - select_Point));
                        Ycenter = week; Xcenter = day;                                      //�I���̒��S�}�X���N���b�N���ꂽ�}�X�Ɉڂ�
                        if (Move_Point - select_Point - 2 >= 0)
                        {
                            Manager.Week[YPlayer_Loot[Move_Point - select_Point - 2]].Day[XPlayer_Loot[Move_Point - select_Point - 2]].GetComponent<Mass_3D>().decision = false;
                        }
                    }
                }
            }
            if (MoveStop_point <= 0)
            {
                MoveStop_Push = true;
                DiceButton.GetComponent<Button>().interactable = true;
                ButtonText.GetComponent<Text>().text = "�����Ŏ~�܂�";
            }
            if (select_Point > 0)         //�܂��ړ��ł������������Ȃ�
            {
                MoveSelect_Display();     //�I���ł���}�X�̕\��
            }
            else
            {
                MoveStop_Push = false;
                DiceButton.GetComponent<Button>().interactable = false;
                ButtonText.GetComponent<Text>().text = "�ړ�";
                Debug.Log("�s���I��");
                StartCoroutine(PlayerMove_Coroutine(Move_Point, true));//�v���C���[�̈ړ��J�n
            }
        }
        else
        {
            //���t�̌��ʂɂ��ړ�
            for (int week = 0; week < Manager.Week.Length; week++)
            {
                for (int day = 0; day < Manager.Week[0].Day.Length; day++)
                {
                    Output_SelectClear(week, day);
                    if (Manager.Week[week].Day[day].GetComponent<Mass_3D>().On_Click)        //�}�X���N���b�N���ꂽ���̂�
                    {
                        Manager.Week[week].Day[day].GetComponent<Mass_3D>().On_Click = false;//�N���b�N���ꂽ����������
                        Player_warpMove[1] = true;
                        YPlayer_Loot[1] = week;                      //�ړ����肵���}�X���L������
                        XPlayer_Loot[1] = day;
                    }
                }
            }

            Effect = false;
            StartCoroutine(PlayerMove_Coroutine(1, false));//�v���C���[�̈ړ��J�n
        }
    }

    //�v���C���[�̈ړ�
    IEnumerator PlayerMove_Coroutine(int MovePoint, bool Effect)
    {
        for (int Move = 1; Move < MovePoint + 1; Move++)//�v���C���[�̌��蕪�����ړ�
        {
            if (Player_warpMove[Move] == true)      //���[�v���邩
            {
                Player_warpMove[Move] = false;
                Output_AnimationWarpUp();           //���[�v�̃A�j���[�V����
                yield return new WaitForSeconds(1);     //1�b�҂�
                Output_AnimationStop();
                Output_PlayerMove(Move);              //���[�v�̃A�j���[�V�����ƈړ�
                yield return new WaitForSeconds(0.1f);     //0.1�b�҂�
            }
            else
            {
                if (YPlayer_Loot[Move] < YPlayer_Loot[Move - 1] && YPlayer_Loot[Move - 1] != 5)
                {
                    Output_AnimationUp();//��ړ��̃A�j���[�V����
                }
                else if (YPlayer_Loot[Move] < YPlayer_Loot[Move - 1] && YPlayer_Loot[Move - 1] == 5)
                {
                    Output_AnimationUpMonth();//��ړ��Ō����ׂ��A�j���[�V����
                }
                if (YPlayer_Loot[Move] > YPlayer_Loot[Move - 1] && YPlayer_Loot[Move - 1] != 4)
                {
                    Output_AnimationDown();//���ړ��̃A�j���[�V����
                }
                else if (YPlayer_Loot[Move] > YPlayer_Loot[Move - 1] && YPlayer_Loot[Move - 1] == 4)
                {
                    Output_AnimationDownMonth();//���ړ��Ō����ׂ��A�j���[�V����
                }
                if (XPlayer_Loot[Move] > XPlayer_Loot[Move - 1] && XPlayer_Loot[Move - 1] != 6)
                {
                    Output_AnimationRight();//�E�ړ��̃A�j���[�V����
                }
                else if (XPlayer_Loot[Move] > XPlayer_Loot[Move - 1] && XPlayer_Loot[Move - 1] == 6)
                {
                    Output_AnimationRightMonth();//�E�ړ��Ō����ׂ��A�j���[�V����
                }
                if (XPlayer_Loot[Move] < XPlayer_Loot[Move - 1] && XPlayer_Loot[Move - 1] != 7)
                {
                    Output_AnimationLeft();//���ړ��̃A�j���[�V����
                }
                else if (XPlayer_Loot[Move] < XPlayer_Loot[Move - 1] && XPlayer_Loot[Move - 1] == 7)
                {
                    Output_AnimationLeftMonth();//���ړ��Ō����ׂ��A�j���[�V����
                }

            }
            XPlayer_position = XPlayer_Loot[Move];  //�v���C���[�̌��݂̏c�E���ʒu��ݒ�
            YPlayer_position = YPlayer_Loot[Move];
            yield return new WaitForSeconds(1);     //1�b�҂�
            Output_AnimationStop();                 //�S�ẴA�j���[�V�������~�߂�
            Output_PlayerMove(Move);                //���W�ړ�
            yield return new WaitForSeconds(0.1f);     //0.1�b�҂�
        }
        for (int week = 0; week < Manager.Week.Length; week++)
        {
            for (int day = 0; day < Manager.Week[0].Day.Length; day++)
            {
                Output_decisionClear(week, day);    //�S�Ă̈ړ����肵���}�X������
            }
        }
        if (Exchange)
        {
            Exchange = false;
            gameObject.GetComponent<Day_Effect>().Exchange_Position();
        }
        Manager.Player_Same();
        Debug.Log(Turn_change);
        if (Turn_change == false)
        {
            if (Effect == true)
            {
                StopDay_Effect(); //�~�܂����}�X�̏���
            }
            Manager.PlayerTurn_change();
        }
        Turn_change = false;
    }



    //�ړ��̍ۂ̍��W�ړ����o��
    private void Output_PlayerMove(int Move)
    {
        transform.position = Manager.Week[YPlayer_Loot[Move]].Day[XPlayer_Loot[Move]].GetComponent<Mass_3D>().transform.position;//�v���C���[�̈ړ�
    }




    //��ړ��̃A�j���[�V�������o��
    private void Output_AnimationUp()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_up", true);
    }
    //��ړ��Ō����ׂ��A�j���[�V�������o��
    private void Output_AnimationUpMonth()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_upMonth", true);
    }
    //���ړ��̃A�j���[�V�������o��
    private void Output_AnimationDown()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_down", true);
    }
    //���ړ��Ō����ׂ��A�j���[�V�������o��
    private void Output_AnimationDownMonth()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_downMonth", true);
    }
    //�E�ړ��̃A�j���[�V�������o��
    private void Output_AnimationRight()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_right", true);
    }
    //�E�ړ��Ō����ׂ��A�j���[�V�������o��
    private void Output_AnimationRightMonth()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_rightMonth", true);
    }
    //���ړ��̃A�j���[�V�������o��
    private void Output_AnimationLeft()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_left", true);
    }
    //���ړ��Ō����ׂ��A�j���[�V�������o��
    private void Output_AnimationLeftMonth()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_leftMonth", true);
    }
    //���[�v�̃A�j���[�V�������o��
    private void Output_AnimationWarpUp()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_warpup", true);
    }


    //�ړ��A�j���[�V�������~�߂���o��
    private void Output_AnimationStop()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_up", false);
        gameObject.GetComponent<Animator>().SetBool("Move_down", false);
        gameObject.GetComponent<Animator>().SetBool("Move_right", false);
        gameObject.GetComponent<Animator>().SetBool("Move_left", false);
        gameObject.GetComponent<Animator>().SetBool("Move_upMonth", false);
        gameObject.GetComponent<Animator>().SetBool("Move_downMonth", false);
        gameObject.GetComponent<Animator>().SetBool("Move_rightMonth", false);
        gameObject.GetComponent<Animator>().SetBool("Move_leftMonth", false);
        gameObject.GetComponent<Animator>().SetBool("Move_warpup", false);
    }

    public void Player_WarpMove(string Mode, string Day)
    {
        if (Mode == "���[�v")
        {
            YPlayer_Loot[0] = YPlayer_position;                  //�v���C���[�̌��݂̃}�X���L������
            XPlayer_Loot[0] = XPlayer_position;
            for (int week = 0; week < Manager.Week.Length; week++)
            {
                for (int day = 0; day < Manager.Week[0].Day.Length; day++)
                {
                    if (Manager.Week[week].Day[day].GetComponent<Mass_3D>().Day == Day)
                    {
                        YPlayer_Loot[1] = week;
                        XPlayer_Loot[1] = day;
                    }
                }
            }
            if (Manager.Week[YPlayer_Loot[1]].Day[XPlayer_Loot[1]].activeInHierarchy == true)
            {
                Player_warpMove[1] = true;
                StartCoroutine(PlayerMove_Coroutine(1, false));//�v���C���[�̈ړ��J�n
            }
        }
    }

    //�ړ������ɕ������i��(����(�㉺���E), ����)
    public void Player_wayMove(string way, int step)
    {
        YPlayer_Loot[0] = YPlayer_position;                  //�v���C���[�̌��݂̃}�X���L������
        XPlayer_Loot[0] = XPlayer_position;
        int Step_Count = step;
        Debug.Log(0 + " : " + YPlayer_Loot[0] + ":" + XPlayer_Loot[0]);
        for (int Move = 1; Move < step + 1; Move++)
        {
            switch (way)
            {
                case "��":
                    YPlayer_Loot[Move] = YPlayer_Loot[Move - 1] - 1;
                    XPlayer_Loot[Move] = XPlayer_Loot[Move - 1];
                    break;

                case "��":
                    YPlayer_Loot[Move] = YPlayer_Loot[Move - 1] + 1;
                    XPlayer_Loot[Move] = XPlayer_Loot[Move - 1];
                    break;

                case "�E":
                    YPlayer_Loot[Move] = YPlayer_Loot[Move - 1];
                    XPlayer_Loot[Move] = XPlayer_Loot[Move - 1] + 1;
                    break;

                case "��":
                    YPlayer_Loot[Move] = YPlayer_Loot[Move - 1];
                    XPlayer_Loot[Move] = XPlayer_Loot[Move - 1] - 1;
                    break;
            }
            if (YPlayer_Loot[Move] < 0 || Manager.Week.Length < YPlayer_Loot[Move])
            {
                Step_Count--;
                YPlayer_Loot[Move] = YPlayer_Loot[Move - 1];
            }
            if (XPlayer_Loot[Move] < 0 || Manager.Week[0].Day.Length < XPlayer_Loot[Move])
            {
                Step_Count--;
                XPlayer_Loot[Move] = XPlayer_Loot[Move - 1];
            }
            if (Manager.Week[YPlayer_Loot[Move]].Day[XPlayer_Loot[Move]].activeInHierarchy == false)
            {
                Step_Count--;
                YPlayer_Loot[Move] = YPlayer_Loot[Move - 1];
                XPlayer_Loot[Move] = XPlayer_Loot[Move - 1];
            }
            //Debug.Log(Move + " : " + YPlayer_Loot[Move] + ":" + XPlayer_Loot[Move]);
        }
        StartCoroutine(PlayerMove_Coroutine(Step_Count, false));//�v���C���[�̈ړ��J�n
    }





    //�I���ł���}�X��\�����ďo��(���L����Ƒ��v���C���[���N���b�N�o����\��������׋��L���Ȃ��悤�ɗ���)
    private void Output_SelectSetting(int week, int day)
    {
        Manager.Week[week].Day[day].GetComponent<Mass_3D>().select_display();
    }

    //�I���ł���}�X���\���ɂ��ďo��
    private void Output_SelectClear(int week, int day)
    {
        Manager.Week[week].Day[day].GetComponent<Mass_3D>().select_hidden();
    }

    //�ړ����肵���}�X��\�����ďo��
    private void Output_decisionSetting(int week, int day)
    {
        Manager.Week[week].Day[day].GetComponent<Mass_3D>().decision_display();
    }

    //�ړ����肵���}�X���\���ɂ��ďo��
    private void Output_decisionClear(int week, int day)
    {
        Manager.Week[week].Day[day].GetComponent<Mass_3D>().decision_hidden();
    }





    //�~�܂����}�X�̏���
    private void StopDay_Effect()
    {
        if (Manager.Week[YPlayer_position].Day[XPlayer_position].GetComponent<Mass_3D>().Goal == true)
        {
            Player_Goal();//�S�[�������Ƃ��̏���
        }
        else
        {
            if (Manager.Week[YPlayer_position].Day[XPlayer_position].GetComponent<Mass_3D>().Open == false)//�܂��J���ĂȂ��}�X�Ȃ�
            {
                Output_hideCoverClear(YPlayer_position, XPlayer_position);//�}�X���J�����\���ɂ���
                Player_DayEffect();//���t�̌���
            }
        }
    }





    //�S�[���������̏���
    public void Player_Goal()
    {
        Output_GoalCount();//�S�[���������Z
        Manager.Goal_Add();//�Q�[���S�̂̃S�[�����ɉ��Z
        Player_DayEffect();//���t�̌���
    }

    //�S�[���������̃S�[�������o��
    private void Output_GoalCount()
    {
        Goalcount++;
    }

    //���t�̌��ʔ���
    public void Player_DayEffect()
    {
        string day = Manager.Week[YPlayer_position].Day[XPlayer_position].GetComponent<Mass_3D>().Day;//����������t���擾
        StartCoroutine(Day_Animation(day));     //�r�f�I�̍Đ��ƃz�b�v�A�b�v�̕\��
        Effect_ready = true;
        //�����ɓ��t�̌��ʓ����
    }

    //�J�����}�X���\���ɂ��ďo��
    private void Output_hideCoverClear(int week, int day)
    {
        Manager.Week[week].Day[day].GetComponent<Mass_3D>().hideCover_Clear();//�}�X���J�����\���ɂ���
    }

    //�r�f�I�̍Đ��ƃz�b�v�A�b�v�̕\��
    IEnumerator Day_Animation(string day)
    {
        Manager.Output_VideoSetting();  //�r�f�I��\��
        Manager.Video_obj.GetComponent<VideoPlayer>().clip = gameObject.GetComponent<Day_Effect>().Output_VideoClip(day); //�r�f�I���I�u�W�F�N�gVideo�ɓ����
        Manager.Output_VideoStart();     //�r�f�I�̍Đ�
        yield return new WaitForSeconds(8);     //8�b�҂�
        if (Manager.Video_obj.activeInHierarchy == true)
        {
            Manager.Output_HopUp();         //�z�b�v�A�b�v��\������
            gameObject.GetComponent<Day_Effect>().Output_HopUp_Setting(day);        //�z�b�v�A�b�v����t�̂��̂ɕύX����
        }
        Manager.Output_VideoFinish();     //�r�f�I�̔�\��

    }

    public void HopUp_Setting(string day)
    {
        Manager.Output_HopUp();         //�z�b�v�A�b�v��\������
        gameObject.GetComponent<Day_Effect>().Output_HopUp_Setting(day);        //�z�b�v�A�b�v����t�̂��̂ɕύX����
    }


    public void StertDayEffect()
    {
        if (Effect_ready)
        {
            Effect_ready = false;
            string day = Manager.Week[YPlayer_position].Day[XPlayer_position].GetComponent<Mass_3D>().Day;//����������t���擾
            gameObject.GetComponent<Day_Effect>().Day_EffectReaction(day);
        }
    }
}
