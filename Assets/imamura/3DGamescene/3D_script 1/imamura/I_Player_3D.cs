using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class I_Player_3D : MonoBehaviourPunCallbacks
{
    public int PlayerNumber;                      //�v���C���[�ԍ�


    public int XPlayer_position;                  //�v���C���[�̌��݂̉��̈ʒu
    public int YPlayer_position;                  //�v���C���[�̌��݂̏c�̈ʒu

    int Xcenter, Ycenter;                         //�I���ł���}�X�̒��S�}�X

    public List<Anniversary_Item> Hub_Items = new List<Anniversary_Item>();

    public int Move_Point = 0;                    //�v���C���[�̈ړ��ł������ 
    private int select_Point = 0;                 //�}�X��I���ł��鐔
    private bool[] Player_warpMove = new bool[51];//�v���C���[�̈ړ����@

    public int Goalcount = 0;

    private int[] XPlayer_Loot = new int[51];     //�I�������}�X���L������
    private int[] YPlayer_Loot = new int[51];


    public GameObject GameManager;                //GameManager�I�u�W�F�N�g�̎擾
    private I_game_manager Manager;                 //I_game_manager���擾

    public GameObject DiceButton;                           //�_�C�X���~�߂�ׂ̃I�u�W�F�N�g�擾
    public GameObject ButtonText;                           //�_�C�X�̃e�L�X�g�I�u�W�F�N�g�擾
    private bool DiceStrat = true;                          //�{�^�����_�C�X�̊J�n���X�g�b�v��
    public bool Effect;                           //�G�t�F�N�g�ɂ��ړ����ǂ��� 

    public bool Exchange;                         //�ꏊ�������邩�ǂ��� 
    public bool Turn_change;                      //�^�[����ς��邩�ǂ��� 
    public int OneMore_Dice;

    public int DiceAdd = 0;

    public int DiceMultiply = 0;
    public int MoveAdd_point = 0;
    public int MoveStop_point = 0;
    private int MovePoint_Count = 0;
    public bool selectwark;
    private bool Effect_ready;

    private bool Effect_start = true;

    private bool MoveStop_Push;

    public bool Guide_on = true;
    private bool Guide_one = true;

    private bool OneMore;

    private bool consecutive_hits;

    // �ȉ�MannequinPlayer��̈��p=====================================================================
    public Anniversary_Item_Master ItemMaster;
    public GameObject ItemBlock;//�A�C�e�����X�g��UGI

    // =====================================================================

    private void Awake()
    {
        GameManager= GameObject.FindWithTag("GameController");
        DiceButton=GameObject.FindWithTag("Dice");
        ButtonText=DiceButton.transform.GetChild(0).gameObject;
        Manager = GameManager.GetComponent<I_game_manager>();
        
        DontDestroyOnLoad(this.gameObject);

        if (SceneManager.GetActiveScene().name=="T1")
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        name=""+GetComponent<PhotonView>().CreatorActorNr;
        
    }


    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Result")
        {
            transform.position = new Vector3(0, 600, 0);
        }
    }

    //�v���C���[�̕\��
    public void Player_indicate()
    {
        gameObject.SetActive(true);
    }



    //�v���C���[�̏����ʒu�ݒ�
    public void Player_position_setting(int Y_position, int X_position)
    {
        Manager = GameManager.GetComponent<I_game_manager>();
        XPlayer_position = X_position;//�v���C���[�̌��݂̏c�E���ʒu
        YPlayer_position = Y_position;
        transform.position = Manager.Week[Y_position].Day[X_position].GetComponent<I_Mass_3D>().transform.position;//�v���C���[�̈ړ�
        //Debug.Log("�v���C���[�̏����ʒu "+Y_position + " : "+ X_position);
    }


    public void Turn_your()
    {
        string Log = Manager.PlayerColouradd("�M��")+"�̃^�[���ł��B";
       
        Manager.Log_Mine(Log);
        Log =Manager.PlayerColouradd(PhotonNetwork.NickName)+"�̃^�[���ł��B";
        Manager.Log_connection_Oter(Log);

        Manager.HowMyTurn=true;
        Manager.Camera.GetComponent<Camera_Mouse>().CameraOwnership();
        Manager.Camera.GetComponent<Camera_Mouse>().Permission_Zoom = true;
        photonView.RPC(nameof(ApartmentEffect), RpcTarget.All);
        consecutive_hits = false;
        Dice_ready();
    }



    //�_�C�X���񂷏���
    public void Dice_ready()
    {
        string Text_Announce;

        if (Manager.Player_Turn==PlayerNumber)
        {
            var tran = new Vector3(this.transform.position.x-1.8f, this.transform.position.y+9, this.transform.position.z-6.5f);
            var rate = new Vector3(47, 00, 0);


            if (Manager.GoalPut)//�S�[�����ݒu���ꂽ�΂���̎�
            {

                GameObject Goal = Manager.Week[Manager.YGoal].Day[Manager.XGoal];
                var tranGoal = new Vector3(Goal.transform.position.x-1.8f, Goal.transform.position.y+9, Goal.transform.position.z-6.5f);
                var rateGoal = new Vector3(47, 00, 0);


                Manager.Camera.GetComponent<Camera_Mouse>().CameraGoal( tranGoal, rateGoal, tran, rate);
                Manager. GoalPutFalse();
            }
            else
            {
                Manager.Camera.GetComponent<Camera_Mouse>().CameraPlayer(tran, rate);
            }
        }
        if (Guide_on == true && Guide_one == true)
        {
            GameManager.GetComponent<Guide>().Dice_BottonStart();
            GameManager.GetComponent<Guide>().scroll_Start();
        }
        DiceButton.GetComponent<Button>().interactable = true;
        ButtonText.GetComponent<Text>().text = "�_�C�X����";
        gameObject.GetComponent<I_Day_Effect>().DiceSetting();
        if (selectwark)

        {

            ButtonText.GetComponent<Text>().text = "�i��";

            Text_Announce = Manager.PlayerColouradd(PhotonNetwork.NickName) + "��" + MoveAdd_point + "�}�X�܂Ői��ł������ł��B";
            Manager.Log_connection(Text_Announce);
        }
        Turn_change = false;
        if(MoveAdd_point != 0 && selectwark == false)
        {
            Text_Announce = Manager.PlayerColouradd(PhotonNetwork.NickName) + "�̓_�C�X��+" + MoveAdd_point + "�}�X�܂Ői��ł������ł��B";
            Manager.Log_connection(Text_Announce);
        }
        if (OneMore_Dice > 1 && OneMore == false)
        {
            OneMore = true;
            Text_Announce = Manager.PlayerColouradd(PhotonNetwork.NickName) + "�̓_�C�X��" + OneMore_Dice + "��U��܂��B";
            Manager.Log_connection(Text_Announce);
        }
        if (DiceAdd != 0 && OneMore_Dice < 1)
        {
            Text_Announce = Manager.PlayerColouradd(PhotonNetwork.NickName) + "�̃_�C�X�̏o�ڂ�+" + DiceAdd + "���܂��B";
            Manager.Log_connection(Text_Announce);
        }
        if (DiceMultiply != 0)
        {
            Text_Announce = Manager.PlayerColouradd(PhotonNetwork.NickName) + "�̃_�C�X�̏o�ڂ��~" + DiceMultiply + "���܂��B";
            Manager.Log_connection(Text_Announce);
        }
    }




  [PunRPC] public void ApartmentEffect()
    {
        foreach (var Item in Hub_Items)
        {
           if(Item.ItemName=="�}���V����")
            {
                Item.ItemPoint++;
                Debug.Log("<color=red>�}���V��������</color>");
                ItemBlock.GetComponent<ItemBlock_List_Script>().PuintUpdate();
                var Log =Manager.PlayerColouradd(PhotonNetwork.NickName)+"�}���V�����̎��Y���l���オ��A1�|�C���g�������܂����B";
                Manager.Log_connection_Oter(Log);

            }
            if (Item.ItemName=="�������卪")
            {
                Item.ItemPoint++;
                Debug.Log("<color=red>�J�C�����卪</color>");
                ItemBlock.GetComponent<ItemBlock_List_Script>().PuintUpdate();
                var Log = Manager.PlayerColouradd(PhotonNetwork.NickName)+"�������卪���������A1�|�C���g�������܂����B";
                Manager.Log_connection_Oter(Log);
            }

        }

    }
    //�_�C�X���~�߂Ēl���󂯎��
    private void Dice_Stop()
    {
        if (OneMore_Dice >= 2)

        {

            OneMore_Dice--;

            DiceAdd += Manager.Output_DiceStop();

            Dice_ready();

        }

        else

        {
            if (consecutive_hits == false)
            {
                if (Guide_on == true && Guide_one == true)
                {
                    GameManager.GetComponent<Guide>().Dice_BottonFinish();
                    GameManager.GetComponent<Guide>().chat_Finish();
                }
                Move_Point = Manager.Output_DiceStop() + DiceAdd;

                if (DiceMultiply != 0)

                {

                    Move_Point *= DiceMultiply;

                }

                if (DiceAdd != 0 || DiceMultiply != 0)
                {
                    string Text_Announce = Manager.PlayerColouradd(PhotonNetwork.NickName) + "�̈ړ��o���鍇�v��..." + Move_Point + "�ł��B";
                    Manager.Log_connection(Text_Announce);
                }

                MoveStop_point = Move_Point;

                Move_Point += MoveAdd_point;

                MovePoint_Count = 0;

                MoveAdd_point = 0;

                DiceAdd = 0;

                DiceMultiply = 0;

                OneMore = false;

                //Debug.Log("�����F" + Move_Point); 
                MoveSelect();
            }
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
                            photonView.RPC(nameof(Output_SelectClear), RpcTarget.All, week, day);
                        }
                    }
                    StartCoroutine(PlayerMove_Coroutine(MovePoint_Count, true));//�v���C���[�̈ړ��J�n 
                }
                else
                {          
                        ButtonText.GetComponent<Text>().text = "�ړ���I��";
                        if (OneMore_Dice <= 1)
                        {
                            DiceButton.GetComponent<Button>().interactable = false;
                        }
                        else
                        {
                            ButtonText.GetComponent<Text>().text = "�_�C�X����";
                        }

                        StartCoroutine(Dice_Coroutine());

                        DiceStrat = true;
                }
            }
        }
    }


    IEnumerator Dice_Coroutine()
    {
        Manager.Dice.GetComponent<newRotate>().Dice_shuffle();

        yield return new WaitForSeconds(0.5f);     //1�b�҂�

        Dice_Stop();//�_�C�X���~�߂Ēl���󂯎�� 
    }

    public void another_turn()
    {
        ButtonText.GetComponent<Text>().text = "���v���C���[�̃^�[��";
    }





    //�I���ł���}�X�̕\���̏����ݒ�
    public void MoveSelect()
    {
        consecutive_hits = true;
        if (Guide_on == true && Guide_one == true)
        {
            GameManager.GetComponent<Guide>().MassSelecet_Start();
            GameManager.GetComponent<Guide>().warp_BottonStart();
        }
        Effect_start = true;
        Xcenter = XPlayer_position;                 //�I���̒��S�ƂȂ�}�X��ݒ�
        Ycenter = YPlayer_position;
        YPlayer_Loot[0] = Ycenter;                  //�v���C���[�̌��݂̃}�X���L������
        XPlayer_Loot[0] = Xcenter;
        photonView.RPC(nameof(Output_decisionSetting), RpcTarget.All, Ycenter, Xcenter);//���݂̃}�X���ړ����肵���}�X�ɂ���
        select_Point = Move_Point;                  //�I���ł��鐔�Ƀ_�C�X�̖ڂ�����
        MoveSelect_Display();                       //�I���ł���}�X�̕\��
    }

    //�I���ł���}�X�̕\��
    private void MoveSelect_Display()
    {
        int[] Select = new int[4];                                              //�I���̒��S�ƂȂ�}�X�̎l����ݒ肷��

                            
        photonView.RPC(nameof(Output_SelectClear), RpcTarget.All, Ycenter, Xcenter);//�I���̒��S�ƂȂ�}�X�̑I���}�X(���F���)��\��
        Select[0] = Xcenter - 1; Select[1] = Xcenter + 1;                       //�I���̒��S�ƂȂ�}�X�̍��E������
        for (int way = 0; way < 2; way++)
        {
            //�I���̒��S�ƂȂ�}�X�̍��E�����݂��ړ����肳�ꂽ�}�X�łȂ���
            if (0 <= Select[way] && Select[way] < Manager.Week[0].Day.Length && Manager.Week[Ycenter].Day[Select[way]].GetComponent<I_Mass_3D>().decision == false)
            {
                Manager.Week[Ycenter].Day[Select[way]].layer = LayerMask.NameToLayer("Default");
                photonView.RPC(nameof(Output_SelectSetting), RpcTarget.All, Ycenter, Select[way]);
            }
        }

        Select[2] = Ycenter - 1; Select[3] = Ycenter + 1;                        //�I���̒��S�ƂȂ�}�X�̏㉺������
        for (int way = 2; way < Select.Length; way++)
        {
            //�I���̒��S�ƂȂ�}�X�̏㉺�����݂��ړ����肳�ꂽ�}�X�łȂ���
            if (0 <= Select[way] && Select[way] < Manager.Week.Length && Manager.Week[Select[way]].Day[Xcenter].GetComponent<I_Mass_3D>().decision == false)
            {
                Manager.Week[Select[way]].Day[Xcenter].layer = LayerMask.NameToLayer("Default");                     //�ړ����肵���}�X��\��
                photonView.RPC(nameof(Output_SelectSetting), RpcTarget.All, Select[way], Xcenter);
            }
        }

        if (Manager.Week[Ycenter].Day[Xcenter].GetComponent<I_Mass_3D>().warp == true)//�I���̒��S�ƂȂ�}�X�����[�v�}�X�Ȃ�
        {
            for (int week = 0; week < Manager.Week.Length; week++)
            {
                for (int day = 0; day < Manager.Week[0].Day.Length; day++)
                {
                    if (Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().warp == true && (Xcenter, Ycenter) != (day, week))
                    {
                        Manager.Week[week].Day[day].layer = LayerMask.NameToLayer("Default");
                                           //�I���ł���}�X��\��
                        photonView.RPC(nameof(Output_SelectSetting), RpcTarget.All, week, day);
                    }
                }
            }
        }
    }

    //�I���ł���}�X����ړ����肷��
    public void MoveSelect_Clicked()
    {
        if (Guide_on == true && Guide_one == true)
        {
            Guide_one = false;
            GameManager.GetComponent<Guide>().MassSelecet_Finish();
            GameManager.GetComponent<Guide>().warp_BottonFinish();
        }
        if (Effect == false)

        {
            for (int week = 0; week < Manager.Week.Length; week++)
            {
                for (int day = 0; day < Manager.Week[0].Day.Length; day++)
                {

                    photonView.RPC(nameof(Output_SelectClear), RpcTarget.All, week, day);                                          //�S�Ă̑I���}�X(���F���)��\��
                    if (Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().On_Click)        //�}�X���N���b�N���ꂽ���̂�
                    {
                        //Debug.Log("���肵���}�X�I");
                      
                        photonView.RPC(nameof(Output_decisionSetting), RpcTarget.AllViaServer, week, day);
                        select_Point--;                                                      //�v���C���[�̈ړ��ł��������1���炷
                        MoveStop_point--;
                        MovePoint_Count++;
                        Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().On_Click = false;//�N���b�N���ꂽ����������
                        YPlayer_Loot[Move_Point - select_Point] = week;                      //�ړ����肵���}�X���L������
                        XPlayer_Loot[Move_Point - select_Point] = day;
                        //���S�}�X�����[�v�}�X�ł������烏�[�v�}�X�Ɉړ�������
                        if (Manager.Week[Ycenter].Day[Xcenter].GetComponent<I_Mass_3D>().warp == true && Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().warp == true)
                        {
                            Player_warpMove[Move_Point - select_Point] = true;              //���[�v�̃��[�V����������悤�ɂ���
                                                                                            //   Debug.Log("���[�V����");
                        }
                        //Debug.Log("�s���:"+ (Move_Point - select_Point));
                        Ycenter = week; Xcenter = day;                                      //�I���̒��S�}�X���N���b�N���ꂽ�}�X�Ɉڂ�
                        if (Move_Point - select_Point - 2 >= 0)
                        {
                            Manager.Week[YPlayer_Loot[Move_Point - select_Point - 2]].Day[XPlayer_Loot[Move_Point - select_Point - 2]].GetComponent<I_Mass_3D>().decision = false;
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
                // Debug.Log("�s���I��");
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

                 
                    photonView.RPC(nameof(Output_SelectClear), RpcTarget.All, week, day);

                    if (Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().On_Click)        //�}�X���N���b�N���ꂽ���̂� 

                    {

                        Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().On_Click = false;//�N���b�N���ꂽ���������� 

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
                photonView.RPC(nameof(Output_AnimationWarpUp), RpcTarget.AllViaServer);  //���[�v�̃A�j���[�V����
                yield return new WaitForSeconds(0.4f);     //1�b�҂�
                photonView.RPC(nameof(Output_AnimationStop), RpcTarget.AllViaServer);     //�r�f�I�̍Đ� 

                photonView.RPC(nameof(Output_PlayerMove), RpcTarget.AllViaServer, YPlayer_Loot[Move], XPlayer_Loot[Move]);//���[�v�̃A�j���[�V�����ƈړ� 

                yield return new WaitForSeconds(0.1f);     //0.1�b�҂� 
            }
            else
            {
                if (YPlayer_Loot[Move] < YPlayer_Loot[Move - 1] && YPlayer_Loot[Move - 1] != 5)
                {
                    photonView.RPC(nameof(Output_AnimationUp), RpcTarget.AllViaServer); //��ړ��̃A�j���[�V����
                }
                else if (YPlayer_Loot[Move] < YPlayer_Loot[Move - 1] && YPlayer_Loot[Move - 1] == 5)
                {
                    photonView.RPC(nameof(Output_AnimationUpMonth), RpcTarget.AllViaServer); //��ړ��Ō����ׂ��A�j���[�V����
                }
                if (YPlayer_Loot[Move] > YPlayer_Loot[Move - 1] && YPlayer_Loot[Move - 1] != 4)
                {
                    photonView.RPC(nameof(Output_AnimationDown), RpcTarget.AllViaServer); //���ړ��̃A�j���[�V����
                }
                else if (YPlayer_Loot[Move] > YPlayer_Loot[Move - 1] && YPlayer_Loot[Move - 1] == 4)
                {
                    photonView.RPC(nameof(Output_AnimationDownMonth), RpcTarget.AllViaServer);//���ړ��Ō����ׂ��A�j���[�V����
                }
                if (XPlayer_Loot[Move] > XPlayer_Loot[Move - 1] && XPlayer_Loot[Move - 1] != 6)
                {
                    photonView.RPC(nameof(Output_AnimationRight), RpcTarget.AllViaServer);//�E�ړ��̃A�j���[�V����
                }
                else if (XPlayer_Loot[Move] > XPlayer_Loot[Move - 1] && XPlayer_Loot[Move - 1] == 6)
                {
                    photonView.RPC(nameof(Output_AnimationRightMonth), RpcTarget.AllViaServer);//�E�ړ��Ō����ׂ��A�j���[�V����
                }
                if (XPlayer_Loot[Move] < XPlayer_Loot[Move - 1] && XPlayer_Loot[Move - 1] != 7)
                {
                    photonView.RPC(nameof(Output_AnimationLeft), RpcTarget.AllViaServer);//�E�ړ��Ō����ׂ��A�j���[�V��//���ړ��̃A�j���[�V����
                }
                else if (XPlayer_Loot[Move] < XPlayer_Loot[Move - 1] && XPlayer_Loot[Move - 1] == 7)
                {
                    photonView.RPC(nameof(Output_AnimationLeftMonth), RpcTarget.AllViaServer);//���ړ��Ō����ׂ��A�j���[�V����
                }

            }
            photonView.RPC(nameof(Output_Postion), RpcTarget.AllViaServer, XPlayer_Loot[Move], YPlayer_Loot[Move]);
        
            XPlayer_position = XPlayer_Loot[Move];  //�v���C���[�̌��݂̏c�E���ʒu��ݒ�
            YPlayer_position = YPlayer_Loot[Move];
            photonView.RPC(nameof(Output_Playerloot), RpcTarget.OthersBuffered, YPlayer_position, XPlayer_position);

            photonView.RPC(nameof(Output_BGM), RpcTarget.AllViaServer);
            yield return new WaitForSeconds(0.4f);     //1�b�҂�

            photonView.RPC(nameof(Output_AnimationStop), RpcTarget.AllViaServer);  //�S�ẴA�j���[�V�������~�߂� 

            photonView.RPC(nameof(Output_PlayerMove), RpcTarget.AllViaServer, YPlayer_Loot[Move], XPlayer_Loot[Move]);                //���W�ړ� 

            yield return new WaitForSeconds(0.3f);     //0.1�b�҂� 
        }
        for (int week = 0; week < Manager.Week.Length; week++)
        {
            for (int day = 0; day < Manager.Week[0].Day.Length; day++)
            {
                photonView.RPC(nameof(Output_decisionClear), RpcTarget.AllViaServer, week, day);

            }
        }


        if (Exchange)

        {

            Exchange = false;

            gameObject.GetComponent<I_Day_Effect>().Exchange_Position();


        }
        Manager.Player_Same();

        if (Manager.Week[YPlayer_position].Day[XPlayer_position].GetComponent<I_Mass_3D>().Present_Item == true)
        {
            //�����ɃI���G���e�[�����O�̍����_�A�C�e���̎擾���������
            
                
            foreach (var item in ItemMaster.Anniversary_Items)
            {
               if(item.ItemName=="�v���[���g")
                {
                    photonView.RPC(nameof(ItemAdd), RpcTarget.All, ItemMaster.Anniversary_Items.IndexOf(item)); //�A�C�e�����Z
                 
                    string Log = Manager.PlayerColouradd(PhotonNetwork.NickName)+"���I���G���e�[�����O��"+item.ItemName+"����肵�܂����B";
                    Manager.Log_connection(Log);

                }
            }
            Manager.Week[YPlayer_position].Day[XPlayer_position].GetComponent<I_Mass_3D>().Present_hid();
        }

        if (Manager.Week[YPlayer_position].Day[XPlayer_position].GetComponent<I_Mass_3D>().Goal == true)
        {
            Player_Goal();//�S�[�������Ƃ��̏���
        }
        else
        {
            if (Turn_change == false)

            {

                if (Effect == true)
                {
                    Effect_start = false;
                    StopI_Day_Effect(); //�~�܂����}�X�̏��� 

                }

            }
        }
        
        if (GetComponent<I_Day_Effect>().Effect_ON)
        {
            GetComponent<I_Day_Effect>().Move_end=true;
        }
              
       
        Turn_change = false;



        //=================================================================================================


    }
    [PunRPC]
    private void Output_Playerloot(int Y, int X)
    {
        XPlayer_position = X;  //�v���C���[�̌��݂̏c�E���ʒu��ݒ�
        YPlayer_position =Y;

    }

    [PunRPC]
    private void Output_BGM()
    {
        Manager.SE.GetComponent<SEManager>().SEsetandplay("WalkSE");

    }


    [PunRPC]  //�ړ��̍ۂ̍��W�ړ����o��
    private void Output_PlayerMove(int YPlayer_Loot_Move, int XPlayer_Loot_Move)
    {
        transform.position = Manager.Week[YPlayer_Loot_Move].Day[XPlayer_Loot_Move].GetComponent<I_Mass_3D>().transform.position;//�v���C���[�̈ړ�
    }





    [PunRPC]//��ړ��̃A�j���[�V�������o��
    private void Output_AnimationUp()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_up", true);
    }
    [PunRPC] //��ړ��Ō����ׂ��A�j���[�V�������o��
    private void Output_AnimationUpMonth()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_upMonth", true);
    }
    [PunRPC]//���ړ��̃A�j���[�V�������o��
    private void Output_AnimationDown()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_down", true);
    }
    [PunRPC]//���ړ��Ō����ׂ��A�j���[�V�������o��
    private void Output_AnimationDownMonth()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_downMonth", true);
    }
    [PunRPC]//�E�ړ��̃A�j���[�V�������o��
    private void Output_AnimationRight()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_right", true);
    }
    [PunRPC]//�E�ړ��Ō����ׂ��A�j���[�V�������o��
    private void Output_AnimationRightMonth()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_rightMonth", true);
    }
    [PunRPC] //���ړ��̃A�j���[�V�������o��
    private void Output_AnimationLeft()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_left", true);
    }
    [PunRPC] //���ړ��Ō����ׂ��A�j���[�V�������o��
    private void Output_AnimationLeftMonth()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_leftMonth", true);
    }
    [PunRPC] //���[�v�̃A�j���[�V�������o��
    private void Output_AnimationWarpUp()
    {
        gameObject.GetComponent<Animator>().SetBool("Move_warpup", true);
    }




    [PunRPC] //�ړ��A�j���[�V�������~�߂���o��
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



    //�ړ������ɕ������i��(����(�㉺���E), ����)
    public void Player_wayMove(string way, int step)
    {
        YPlayer_Loot[0] = YPlayer_position;                  //�v���C���[�̌��݂̃}�X���L������
        XPlayer_Loot[0] = XPlayer_position;
        int Step_Count = step;
        //  Debug.Log(0 + " : " + YPlayer_Loot[0] + ":" + XPlayer_Loot[0]);
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
            //     Debug.Log(Move + " : " + YPlayer_Loot[Move] + ":" + XPlayer_Loot[Move]);
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
            //     Debug.Log(Move + " : " + YPlayer_Loot[Move] + ":" + XPlayer_Loot[Move]);
        }
        StartCoroutine(PlayerMove_Coroutine(Step_Count, false));//�v���C���[�̈ړ��J�n
    }





    [PunRPC]  //�I���ł���}�X��\�����ďo��(���L����Ƒ��v���C���[���N���b�N�o����\��������׋��L���Ȃ��悤�ɗ���)
    private void Output_SelectSetting(int week, int day)
    {
        Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().select_display();
    }

    [PunRPC] //�I���ł���}�X���\���ɂ��ďo��
    private void Output_SelectClear(int week, int day)
    {
        Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().select_hidden();
    }

    [PunRPC] //�ړ����肵���}�X��\�����ďo��
    private void Output_decisionSetting(int week, int day)
    {
        Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().decision_display();
    }

    [PunRPC]//�ړ����肵���}�X���\���ɂ��ďo��
    private void Output_decisionClear(int week, int day)
    {
        Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().decision_hidden();
    }



    [PunRPC ] private void Output_Postion(int Xposition, int Yposition)

    {

        XPlayer_position = Xposition;

        YPlayer_position = Yposition;

    }

    //�~�܂����}�X�̏���






    //�S�[���������̏���
    public void Player_Goal()
    {
        StartCoroutine(Goal_Coroutine());
        
        photonView.RPC(nameof(Output_GoalCount), RpcTarget.All); //�S�[���������Z
        Manager.Goal_Add();//�Q�[���S�̂̃S�[�����ɉ��Z
        var loop = ItemMaster.Anniversary_Items.Count-1;//�Ō�̈ʒu���擾
        photonView.RPC(nameof(ItemAdd), RpcTarget.All, loop); //�A�C�e�����Z
        
        string Log = Manager.PlayerColouradd(PhotonNetwork.NickName)+"��"+ItemMaster.Anniversary_Items[loop].ItemName+"����肵�܂����B";
        Manager.Log_connection(Log);
    }

    IEnumerator Goal_Coroutine()
    {
        Output_GoalAnimation();
        yield return new WaitForSeconds(2.0f);     //1�b�҂�
        Manager.GameFinish();

        GoalAnimation_After();

    }

      //�S�[���������̃S�[�������o��
      [PunRPC]private void Output_GoalCount()
    {
        Goalcount++;
    }

    //�S�[���̃A�j���[�V�����̋��L���肢���܂�
    private void Output_GoalAnimation()
    {
        photonView.RPC(nameof(Output_GoalAnimation_RPC), RpcTarget.All);
    }
    [PunRPC]
    private void�@Output_GoalAnimation_RPC()
    {
        Manager.SE.GetComponent<SEManager>().SEsetandplay("GoalSE");
        Manager.GoalAnimation.GetComponent<Animator>().SetBool("GoalAnimation", true);
    }





    public void GoalAnimation_After()
    {
        if (Manager.Player_Turn == PlayerNumber)
        {
            //Debug.Log(PlayerNumber + "GoalAnimation_After()");
            if (Turn_change == false)
            {
                if (Effect_start == true)
                {
                    Debug.Log("StopDay_Effect()" + PlayerNumber);
                    StopI_Day_Effect(); //�~�܂����}�X�̏���
                }
                //Manager.PlayerTurn_change();
            }
            Turn_change = false;
            Effect_start = true;
        }
    }


    //���t�̌��ʔ���
    public void Player_DayEffect()
    {
       
        string day = Manager.Week[YPlayer_position].Day[XPlayer_position].GetComponent<I_Mass_3D>().Day;//����������t���擾
        StartCoroutine(Day_Animation(day));     //�r�f�I�̍Đ��ƃz�b�v�A�b�v�̕\��
        Effect_ready = true;
        Item_Get(day);                               //�����ɓ��t�̌��ʓ����
        
        //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
    }

    public void Item_Get(string day)
    {
      //  Debug.Log("WWWWWWWWWWWWWWWWWWWWWWWWWWWWW"+day);
        int loop = 0;
        int Itemunum = 0;
          foreach (var Item in ItemMaster.Anniversary_Items)//�A�C�e�����X�g�̓��e�����ԂɊi�[
          {
            if (Item.Day==day)
            {
                if (Item.ItemName!="�����_���ȃA�C�e��"&&Item.ItemName!="���D�i") {
                    photonView.RPC(nameof(ItemAdd), RpcTarget.All, loop); //�A�C�e�����Z
                    Itemunum=loop;
                    string Log = Manager.PlayerColouradd(PhotonNetwork.NickName)+"��"+Item.ItemName+"����肵�܂����B";
                    Manager.Log_connection(Log);
                }
          
            }
            loop++;
          }
        //   Debug.Log("�����Ăނ�������������������������������������");
        if (ItemMaster.Anniversary_Items[Itemunum].ItemName!="�����_���ȃA�C�e��")
        {
            

           
        }



    }






   //�J�����}�X���\���ɂ��ďo��
   [PunRPC] private void Output_hideCoverClear(int week, int day)
    {
        Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().hideCover_Clear();//�}�X���J�����\���ɂ���
    }

    //�r�f�I�̍Đ��ƃz�b�v�A�b�v�̕\��
  

    //�r�f�I�̍Đ��ƃz�b�v�A�b�v�̕\��
    IEnumerator Day_Animation(string day)
    {
        
        photonView.RPC(nameof(Day_Animation_RPC_1), RpcTarget.AllViaServer, day);//�R���[�`�����p
        yield return new WaitForSeconds(8);     //8�b�҂�
        photonView.RPC(nameof(Day_Animation_RPC_2), RpcTarget.AllViaServer, day);//�R���[�`�����p
                                                        
    }


    [PunRPC]public void Day_Animation_RPC_1(string day)//�R���[�`�����p
    {
        
        //   Debug.Log("���[�r�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[");
        Manager.Output_VideoSetting();  //�r�f�I��\�� 
        Manager.Video_obj.GetComponent<VideoPlayer>().clip = gameObject.GetComponent<I_Day_Effect>().Output_VideoClip(day); //�r�f�I���I�u�W�F�N�gVideo�ɓ���� 
        Manager.Output_VideoStart();     //�r�f�I�̍Đ� 
    }

    [PunRPC]
    public void Day_Animation_RPC_2(string day)//�R���[�`�����p
    {
        
        if (Manager.Video_obj.activeInHierarchy == true)

        {
            if (Guide_on == true)
            {
                GameManager.GetComponent<Guide>().Hopup_Start();
            }

            Manager.Output_HopUp();         //�z�b�v�A�b�v��\������ 

            gameObject.GetComponent<I_Day_Effect>().Output_HopUp_Setting(day);        //�z�b�v�A�b�v����t�̂��̂ɕύX���� 

        }

        Manager.Output_VideoFinish();     //�r�f�I�̔�\�� 



    }








    // �ȉ�MannequinPlayer��̈��p=====================================================================
    [PunRPC] public void ItemAdd(int ItemNum)//ItemNum���}�X�^�[�o�^���̔ԍ�
    {
        Hub_Items.Add(ScriptableObject.Instantiate(ItemMaster.Anniversary_Items[ItemNum]));//�}�X�^�[�ɂ���ItemNum�̃A�C�e���𐶐����AHub�ɒǉ�
        ItemBlock.GetComponent<ItemBlock_List_Script>().AddItem(ItemNum);
    }

    [PunRPC] public void ItemLost(int HubItemNum)//HubItemNum�������A�C�e���o�^���̔ԍ�
    {

        Hub_Items.RemoveAt(HubItemNum);//��������HubItemNum�Ԗڂ̃A�C�e��������
        ItemBlock.GetComponent<ItemBlock_List_Script>().LostItem(HubItemNum);


    }


    public void ItemAdd_ToConnect(int HubItemNum)//ItemNum���}�X�^�[�o�^���̔ԍ�
    {

        photonView.RPC(nameof(ItemAdd), RpcTarget.All, HubItemNum); //�A�C�e�����Z


    }
    public void ItemLost_ToConnect(int HubItemNum)//HubItemNum�������A�C�e���o�^���̔ԍ�
    {
       
        photonView.RPC(nameof(ItemLost), RpcTarget.All, HubItemNum); //�A�C�e�����Z

    }



    // =====================================================================





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

                    if (Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().Day == Day)

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


    public void HopUp_Setting(string day)

    {
        if (Guide_on == true)
        {
            GameManager.GetComponent<Guide>().Hopup_Start();
        }

        Manager.Output_HopUp();         //�z�b�v�A�b�v��\������ 

        gameObject.GetComponent<I_Day_Effect>().Output_HopUp_Setting(day);        //�z�b�v�A�b�v����t�̂��̂ɕύX���� 

    }
  

    private void StopI_Day_Effect()//�~�܂����}�X�̏���
    {
    
        if (Manager.Week[YPlayer_position].Day[XPlayer_position].GetComponent<I_Mass_3D>().Open == false)//�܂��J���ĂȂ��}�X�Ȃ�
        {
             photonView.RPC(nameof(Output_hideCoverClear), RpcTarget.All, YPlayer_position, XPlayer_position); //�}�X���J�����\���ɂ���
             Player_DayEffect();//���t�̌���
        }
        else
        {
            Debug.Log("�^�[���`�����W�I�I�I");
          Manager.PlayerTurn_change();
    
           
        }
    }


    public void StertDayEffect()

    {
        if (Guide_on == true)
        {
            GameManager.GetComponent<Guide>().Hopup_Finish();
            Guide_on = false;
            if(Manager.Player_Turn == PlayerNumber)
            {
                GameManager.GetComponent<Guide>().Item_Cstart();
            }
            
        }

        if (Effect_ready)

        {

            Effect_ready = false;

            string day = Manager.Week[YPlayer_position].Day[XPlayer_position].GetComponent<I_Mass_3D>().Day;//����������t���擾 

            gameObject.GetComponent<I_Day_Effect>().Day_EffectReaction(day);

        }

    }


}


