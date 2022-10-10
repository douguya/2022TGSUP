using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Photon.Pun;
using Photon.Realtime;

public class I_Day_Effect : MonoBehaviourPunCallbacks
{
    private I_game_manager game_Manager;
    private GameObject Player;
    private Image DayImage;
    public Day_Square_Master Day_Square_Master;
    public GameObject BGMObject;

    private int DayNumber;
    private int Origin_XMass;
    private int Origin_YMass;

    private bool DiceChange = false;
    private bool[] DiceNumber = new bool[6];
    private bool PlayerTurn_change = true;


    public int  EffectCount=0;
    public int  EndCount = 0;
    //�I������pbool====================================
    public bool Effect_ON=false;
    public bool InsTance_ON = false;
    public bool Move_end = false;
    public bool Dice_end = false;
    public bool NextMove_end = false;
    public bool IconChange_end = false;
    public bool ItemLost_end = false;
    public bool Instance_end = false;
    public bool BGM_end = false;
    public bool Conditional_End = false;
    public bool OtherEffects_End = false;
    //====================================
    void Start()
    {
        game_Manager = GameObject.Find("I_game_manager").GetComponent<I_game_manager>();
        DayImage = GameObject.Find("I_game_manager").GetComponent<I_game_manager>().HopUp.GetComponent<Image>();
        Player=this.gameObject;
        BGMObject = GameObject.FindGameObjectWithTag("BGM");
    }

    // Update is called once per frame
    void Update()
    {
       if(Effect_ON==true)
        {
            EndCounts();
          
            if (EndCount==EffectCount)
            {
                Debug.Log("�v���C���[�̃^�[���`�F���W");
                Effect_ON=false;
                game_Manager.PlayerTurn_change();//�v���C���[�^�[���`�F���W
            }
        }
    }



    public void Output_HopUp_Setting(string Day)
    {
        DaySquare_Search(Day);
        DayImage.sprite = Day_Square_Master.Day_Squares[DayNumber].HopUp;
    }

    public VideoClip Output_VideoClip(string Day)
    {
        DaySquare_Search(Day);
        return Day_Square_Master.Day_Squares[DayNumber].Staging;
    }

    //Day_Square_Master�������̓��t�������̂�T��
    private void DaySquare_Search(string Day)
    {

        for (int num = 0; num < Day_Square_Master.Day_Squares.Count; num++)
        {
            if (Day_Square_Master.Day_Squares[num].Day == Day)
            {
                DayNumber = num;
            }
        }

    }
    private void EffectCounts()
    {
        var Effect = Day_Square_Master.Day_Squares[DayNumber];
        if (Effect.Move!="Noon"){ EffectCount++; }
        if (Effect.BGM !="Noon"){ EffectCount++; }
        if (Effect.NextDice!="Noon"){ EffectCount++; }
        if (Effect.NextMove!="Noon"){ EffectCount++; }
      //  if (Effect.Icon!=null){ EffectCount++; }
        if (Effect.ItemLost!="Noon"){ EffectCount++; }
        if (Effect.Instance!="Noon") { EffectCount++; }
        
        if (Effect.ConditionalPoint!="Noon") { EffectCount++; }
        if (Effect.OtherEffects!="Noon") { EffectCount++; }
    }
    private void EndCounts()
    {
        EndCount=0;

        if (BGM_end == true) { EndCount++; Debug.Log("BGM_end�@" + BGM_end); }
        if (Move_end==true) { EndCount++; Debug.Log("Move_end�@"+Move_end); }
        //BGM�̏ꏊ
        if (Dice_end==true) { EndCount++; Debug.Log("Dice_end�@"+Dice_end); }
        if (NextMove_end ==true) { EndCount++; Debug.Log("NextMove_end�@"+NextMove_end); }
      //  if (IconChange_end==true) { EndCount++; Debug.Log("IconChange_end�@"+IconChange_end); }
        if (ItemLost_end==true) { EndCount++; Debug.Log("ItemLost_end�@"+ItemLost_end); }
        if (Instance_end==true) { EndCount++; Debug.Log("Instance_end�@"+Instance_end); }
        if (Conditional_End==true) { EndCount++; Debug.Log("Instance_end�@"+Instance_end); }
        if (OtherEffects_End==true) { EndCount++; Debug.Log("Instance_end�@"+Instance_end); }



    }

    private void CountReset()
    {
        EffectCount=0;
        EndCount=0;
        BGM_end = false;
        Move_end = false;
        Dice_end = false;
        NextMove_end = false;
      //  IconChange_end = false;
        ItemLost_end = false;
        Instance_end = false;
        Conditional_End=false;
        OtherEffects_End =false;
    }




    public void Day_EffectReaction(string Day)
    {
        
        DaySquare_Search(Day);
        CountReset();

        EffectCounts();
        Effect_ON=true;
        Debug.Log("���t���ʂ̔���");
        Effect_Move();
        Effect_BGM();
        Effect_Dice();
        Effect_NextMove();
        //Effect_IconChange();
        Effect_ItemLost();
        Effect_Instance();
        Effect_Couditional();
        Effcet_OtherEffects();//�m�X�g���_���X�̑�\��

    }

    private void Effect_Move()
    {
  
        string daySquare_Move = Day_Square_Master.Day_Squares[DayNumber].Move;
        if (daySquare_Move != "Noon")
        {
            if (daySquare_Move != "none")
            {
                string Text_Announce;

                int turn = game_Manager.Player_Turn ;
                
                char[] Char_Move = daySquare_Move.ToCharArray(); //Move�̓��e��char�^�ɕϊ�
                if (daySquare_Move.StartsWith("���[�v"))
                {
                    if (daySquare_Move.Substring(3, 2) == "�I��")
                    {
                        //  Debug.Log("�w�肵���v���C���[�Ƀ��[�v");
                        //�I�������v���C���[�̌��ɔ��

                        Text_Announce = game_Manager.PlayerColouradd(PhotonNetwork.NickName) + "���I�������v���C���[�̌��Ƀ��[�v���܂��B";
                        game_Manager.Log_connection(Text_Announce);

                        Output_TurnChange(turn);
                        for (int Player = 0; Player < game_Manager.joining_Player; Player++)
                        {
                            if (turn != Player)
                            {
                                int XMass = game_Manager.Player[Player].GetComponent<I_Player_3D>().XPlayer_position;
                                int YMass = game_Manager.Player[Player].GetComponent<I_Player_3D>().YPlayer_position;
                                gameObject.GetComponent<I_Player_3D>().Effect = true;
                                game_Manager.Week[YMass].Day[XMass].GetComponent<I_Mass_3D>().select_display();
                            }
                        }

                    }
                    else
                    {
                        // Debug.Log("�w��}�X���[�v");
                        //�w��}�X�ւ̃��[�v
                        Output_TurnChange(turn);
                        gameObject.GetComponent<I_Player_3D>().Player_WarpMove("���[�v", daySquare_Move.Remove(0, 3));

                        Text_Announce = game_Manager.PlayerColouradd(PhotonNetwork.NickName) + "��" + daySquare_Move + "�Ƀ��[�v���܂��B";
                        game_Manager.Log_connection(Text_Announce);
                    }

                }
                if (daySquare_Move.StartsWith("�W��"))
                {
                    //  Debug.Log("�W��");
                    for (int Player = 0; Player < game_Manager.joining_Player; Player++)
                    {
                        
                            Output_TurnChange(Player);
                            game_Manager.Player[Player].GetComponent<I_Player_3D>().Player_WarpMove("���[�v", daySquare_Move.Remove(0, 2));
                        
                    }
                    Text_Announce = game_Manager.PlayerColouradd(PhotonNetwork.NickName) + "�̂���}�X�ɏW�����܂��B";
                    game_Manager.Log_connection(Text_Announce);
                    //  Debug.Log("PlayerTurn_change:3");
                }
                if (daySquare_Move.StartsWith("�I��"))
                {
                    //  Debug.Log("�I��");
                    //��������}�X����I�����ă��[�v
                    Output_TurnChange(turn);
                    if (daySquare_Move.Remove(0, 2) == "���[�v�}�X")
                    {
                        Text_Announce = game_Manager.PlayerColouradd(PhotonNetwork.NickName) + "���I���������[�v�}�X�Ƀ��[�v���܂��B";
                        game_Manager.Log_connection(Text_Announce);

                        for (int week = 0; week < game_Manager.Week.Length; week++)
                        {
                            for (int day = 0; day < game_Manager.Week[0].Day.Length; day++)
                            {
                                if (game_Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().warp == true)
                                {
                                    gameObject.GetComponent<I_Player_3D>().Effect = true;
                                    game_Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().select_display();
                                }
                            }
                        }
                    }
                    if (daySquare_Move.Substring(2, 2) == "����")
                    {
                        for (int week = 0; week < game_Manager.Week.Length; week++)
                        {
                            for (int day = 0; day < game_Manager.Week[0].Day.Length; day++)
                            {
                                if (game_Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().Day != "null")
                                {
                                    string[] Day_part = game_Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().Day.Split('/');
                                    if (Day_part[1] == daySquare_Move.Remove(0, 4))
                                    {
                                        gameObject.GetComponent<I_Player_3D>().Effect = true;
                                        game_Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().select_display();
                                    }
                                }
                            }
                        }

                        Text_Announce = game_Manager.PlayerColouradd(PhotonNetwork.NickName) + "���I������" + daySquare_Move + "���Ƀ��[�v���܂��B";
                        game_Manager.Log_connection(Text_Announce);
                    }
                    if (daySquare_Move.Remove(0, 2) == "�S�}�X")
                    {
                        for (int week = 0; week < game_Manager.Week.Length; week++)
                        {
                            for (int day = 0; day < game_Manager.Week[0].Day.Length; day++)
                            {
                                gameObject.GetComponent<I_Player_3D>().Effect = true;
                                game_Manager.Week[week].Day[day].GetComponent<I_Mass_3D>().select_display();
                            }
                        }

                        Text_Announce = game_Manager.PlayerColouradd(PhotonNetwork.NickName) + "���I�������D���ȓ��Ƀ��[�v���܂��B";
                        game_Manager.Log_connection(Text_Announce);
                    }
                }
                if (daySquare_Move.StartsWith("����"))
                {
                    //Debug.Log("����");
                    //�I�������v���C���[�ƃ}�X����������
                    Output_TurnChange(turn);
                    Origin_XMass = gameObject.GetComponent<I_Player_3D>().XPlayer_position;
                    Origin_YMass = gameObject.GetComponent<I_Player_3D>().YPlayer_position;
                    for (int Player = 0; Player < game_Manager.joining_Player; Player++)
                    {
                        if (turn != Player)
                        {
                            int XMass = game_Manager.Player[Player].GetComponent<I_Player_3D>().XPlayer_position;
                            int YMass = game_Manager.Player[Player].GetComponent<I_Player_3D>().YPlayer_position;
                            gameObject.GetComponent<I_Player_3D>().Effect = true;
                            gameObject.GetComponent<I_Player_3D>().Exchange = true;
                            game_Manager.Week[YMass].Day[XMass].GetComponent<I_Mass_3D>().select_display();
                        }
                    }

                    Text_Announce = game_Manager.PlayerColouradd(PhotonNetwork.NickName) + "���I�������v���C���[�Əꏊ���������܂��B";
                    game_Manager.Log_connection(Text_Announce);
                }
                if (daySquare_Move.StartsWith("��") || daySquare_Move.StartsWith("��") || daySquare_Move.StartsWith("�E") || daySquare_Move.StartsWith("��"))
                {
                    //  Debug.Log("�㉺���E");
                    //�㉺���E�A���}�X�̈ړ�
                    Output_TurnChange(turn);
                    //Debug.Log("���t���ʂł̃X���C�h�ړ�" + Char_Move[0] + ":" + Toint(Char_Move[1]));
                    gameObject.GetComponent<I_Player_3D>().Player_wayMove(daySquare_Move.Substring(0, 1), Toint(Char_Move[1]));

                    Text_Announce = game_Manager.PlayerColouradd(PhotonNetwork.NickName) + "��" + daySquare_Move + "��" + Toint(Char_Move[1]) + "�}�X�ړ����܂��B";
                    game_Manager.Log_connection(Text_Announce);
                }
            }
           
        }
    }
    public void Exchange_Position()//��������
    {

        int turn = game_Manager.Player_Turn;
        
        for (int Player = 0; Player < game_Manager.joining_Player; Player++)
        {
            if (game_Manager.Player[Player].GetComponent<I_Player_3D>().XPlayer_position == gameObject.GetComponent<I_Player_3D>().XPlayer_position && game_Manager.Player[Player].GetComponent<I_Player_3D>().YPlayer_position == gameObject.GetComponent<I_Player_3D>().YPlayer_position)
            {
                if (turn != Player)
                {
                    string day = game_Manager.Week[Origin_YMass].Day[Origin_XMass].GetComponent<I_Mass_3D>().Day;

                    photonView.RPC(nameof(Output_TurnChange), RpcTarget.All, Player);
                    game_Manager.Player[Player].GetComponent<I_Player_3D>().Player_WarpMove("���[�v", day);
                    //Debug.Log(day);
                }
            }
        }
    }

    [PunRPC]
    public void Output_TurnChange(int Player)
    {
        game_Manager.Player[Player].GetComponent<I_Player_3D>().Turn_change = true;
    }
    private void Effect_BGM()
    {

        if (Day_Square_Master.Day_Squares[DayNumber].BGM != "Noon")
        {
            if (Day_Square_Master.Day_Squares[DayNumber].BGM != "none")
            {
                string Text_Announce;
                PlayerTurn_change = false;

                photonView.RPC(nameof(EffectBGM_RPC), RpcTarget.All, Day_Square_Master.Day_Squares[DayNumber].Anniversary);

                switch (Day_Square_Master.Day_Squares[DayNumber].BGM)
                {
                    case "���۔M�уf�[":
                        Text_Announce = "BGM���W�����O���ɂȂ�܂��B";
                        game_Manager.Log_connection(Text_Announce);
                        break;

                    case "�I�J���g�L�O��":
                        Text_Announce = "BGM���z���[���ɂȂ�܂��B";
                        game_Manager.Log_connection(Text_Announce);
                        break;

                    case "�s�A�m�̓�":
                        Text_Announce = "BGM���s�A�m���ɂȂ�܂��B";
                        game_Manager.Log_connection(Text_Announce);
                        break;

                    case "���̓�":
                        Text_Announce = "BGM�������ɂȂ�܂��B";
                        game_Manager.Log_connection(Text_Announce);
                        break;
                }
                
                BGM_end = true;
            }

        }
    }
    [PunRPC]
    public void EffectBGM_RPC(string BGM)
    {
        BGMObject.GetComponent<BGMManager>().BGMsetandplay(BGM);
    }









    private void Effect_Dice()
    {
        string daySquare_NextDice = Day_Square_Master.Day_Squares[DayNumber].NextDice;
        char[] Char_NextDice = daySquare_NextDice.ToCharArray();
        if (daySquare_NextDice != "Noon")
        {
            if (daySquare_NextDice != "none")
            {
                string Text_Announce;

                //�_�C�X�̏o�ڂɑ���
                if (daySquare_NextDice.StartsWith("+"))
                {
                    gameObject.GetComponent<I_Player_3D>().DiceAdd += Toint(Char_NextDice[1]);

                    Text_Announce = game_Manager.PlayerColouradd(PhotonNetwork.NickName) + "�̎��̃_�C�X�̏o�ڂ�+" + Toint(Char_NextDice[1]) + "�ɂȂ�܂��B";
                    game_Manager.Log_connection(Text_Announce);

                }
                if (daySquare_NextDice.StartsWith("*"))
                {
                    gameObject.GetComponent<I_Player_3D>().DiceMultiply += Toint(Char_NextDice[1]);

                    Text_Announce = game_Manager.PlayerColouradd(PhotonNetwork.NickName) + "�̎��̃_�C�X�̏o�ڂ��~" + Toint(Char_NextDice[1]) + "�ɂȂ�܂��B";
                    game_Manager.Log_connection(Text_Announce);

                }
                //�_�C�X�̏o�ڂɑ���(�S��)
                if (daySquare_NextDice.StartsWith("�S��"))
                {
                    if (daySquare_NextDice.Substring(2, 1) == "+")
                    {
                        for (int Player = 0; Player < game_Manager.joining_Player; Player++)
                        {

                            photonView.RPC(nameof(Output_DiceAdd), RpcTarget.All, Player, Char_NextDice[1]);
                        }
                    }
                    if (daySquare_NextDice.Substring(2, 1) == "*")
                    {
                        for (int Player = 0; Player < game_Manager.joining_Player; Player++)
                        {

                            photonView.RPC(nameof(Output_DiceMultiply), RpcTarget.All, Player, Char_NextDice[1]);
                        }
                    }
                }
                //�_�C�X�̏o�ڂ̕ω�
                if (daySquare_NextDice.StartsWith("�o��"))
                {
                    DiceChange = true;
                    if (daySquare_NextDice.Contains("1"))
                    {
                        DiceNumber[0] = true;
                    }
                    if (daySquare_NextDice.Contains("2"))
                    {
                        DiceNumber[1] = true;
                    }
                    if (daySquare_NextDice.Contains("3"))
                    {
                        DiceNumber[2] = true;
                    }
                    if (daySquare_NextDice.Contains("4"))
                    {
                        DiceNumber[3] = true;
                    }
                    if (daySquare_NextDice.Contains("5"))
                    {
                        DiceNumber[4] = true;
                    }
                    if (daySquare_NextDice.Contains("6"))
                    {
                        DiceNumber[5] = true;
                    }

                    Text_Announce = game_Manager.PlayerColouradd(PhotonNetwork.NickName) + "�̎��̃_�C�X�̏o�ڂ��ω����܂��B";
                    game_Manager.Log_connection(Text_Announce);
                }
            }
            Dice_end=true;
        }
       
    }
    [PunRPC]
    private void Output_DiceAdd(int Player, char add)
    {
        game_Manager.Player[Player].GetComponent<I_Player_3D>().DiceAdd += Toint(add);
    }

    [PunRPC]
    private void Output_DiceMultiply(int Player, char Multiply)
    {
        game_Manager.Player[Player].GetComponent<I_Player_3D>().DiceAdd += Toint(Multiply);
    }


    public void DiceSetting()
    {
        if (DiceChange)
        {
            game_Manager.Dice.GetComponent<newRotate>().InDiceNum.Clear();
            for (int Dice = 0; Dice < DiceNumber.Length; Dice++)
            {
                if (DiceNumber[Dice] == true)
                {
                    game_Manager.Dice.GetComponent<newRotate>().InDiceNum.Add(Dice + 1);
                }
                DiceNumber[Dice] = false;
            }
            DiceChange = false;
        }
        else
        {
            game_Manager.Dice.GetComponent<newRotate>().resetDice();
        }
    }






    private void Effect_Instance()
    {
        char[] Char_Instance = Day_Square_Master.Day_Squares[DayNumber].Instance.ToCharArray();
        if (Day_Square_Master.Day_Squares[DayNumber].Instance != "Noon")
        {
            if (Day_Square_Master.Day_Squares[DayNumber].Instance != "none")
            {
                PlayerTurn_change = false;
                AppearInstances();
            }
        }
    }

    private int Toint(char self)
    {
        return self - '0';
    }
    private void Effect_NextMove()
    {
        string Text_Announce;

        string daySquare_NextMove = Day_Square_Master.Day_Squares[DayNumber].NextMove;
        char[] Char_NextMove = daySquare_NextMove.ToCharArray();
        if (daySquare_NextMove != "Noon")
        {
            if (daySquare_NextMove != "none")
            {
                if (daySquare_NextMove.Contains("��"))
                {
                    gameObject.GetComponent<I_Player_3D>().OneMore_Dice = Toint(Char_NextMove[0]);

                    Text_Announce = game_Manager.PlayerColouradd(PhotonNetwork.NickName) + "�͎��̃^�[���A�_�C�X��" + Toint(Char_NextMove[0]) + "��U��܂��B";
                    game_Manager.Log_connection(Text_Announce);
                }

                if (daySquare_NextMove.StartsWith("�_�C�X"))
                {
                    Debug.Log("���}�X���܂Ői��ł���");
                    if (daySquare_NextMove.Contains("+"))
                    {
                        gameObject.GetComponent<I_Player_3D>().MoveAdd_point += Toint(Char_NextMove[4]);

                        Text_Announce = game_Manager.PlayerColouradd(PhotonNetwork.NickName) + "�͎��̃^�[���A�_�C�X��+" + Toint(Char_NextMove[4]) + "�����Ă������ł��B";
                        game_Manager.Log_connection(Text_Announce);
                    }
                }

                if (daySquare_NextMove.StartsWith("�I��"))
                {
                    gameObject.GetComponent<I_Player_3D>().selectwark = true;
                    gameObject.GetComponent<I_Player_3D>().MoveAdd_point += Toint(Char_NextMove[2]);

                    Text_Announce = game_Manager.PlayerColouradd(PhotonNetwork.NickName) + "�͎��̃^�[���A" + Toint(Char_NextMove[2]) + "�}�X�܂Ői��ł������ł��B";
                    game_Manager.Log_connection(Text_Announce);
                }

            }
            NextMove_end=true;
        }
       
    }

  


    [PunRPC]
    public void RPC_Effect_IconChange(int DayNumber_to )
    {

        Player.GetComponent<I_Player_3D>().ItemBlock.GetComponent<ItemBlock_List_Script>().IcobImage.GetComponent<Image>().sprite=Day_Square_Master.Day_Squares[DayNumber_to].Icon;//���ǂ肽�ǂ��ăA�C�R����ύX

    }
    public void Effect_ItemLost()
    {
        var ItemuLost = Day_Square_Master.Day_Squares[DayNumber].ItemLost;
        if (ItemuLost!="Noon")
        {

            switch (ItemuLost) 
            {

                case "���������̓�":

                    var e = "�����Ă�H���A�C�e��������";
                    SerchClassification("�H�ו�");

                    break;


                case "�S�X�N���b�v�̓�":

                    var ew = "�S�X�N���b�v�̓������Ă�������A�C�e������X�N���b�v�ɂȂ�X�N���b�v�F�|�C���g�{�P";
                    SerchClassification("����");
                    break;



                case "�����̓�":
                    Conversion();


                    break;


                case "�I�[�N�V�����̓�":


                    Auction();
                    break;
                    /*
                case "�Z�v�e���o�[�o�����^�C��":
                    var itemus = Player.GetComponent<I_Player_3D>().Hub_Items;
                    int loop = itemus.Count;

                    int rnd = Random.Range(0, loop);
                    Player.GetComponent<I_Player_3D>().ItemLost_ToConnect(rnd);

                    break;

                    */


                   

            }







            ItemLost_end=true;
        }
       
    }





    public void SerchClassification(string Category)
    {
        var itemus = Player.GetComponent<I_Player_3D>().Hub_Items;
        int loop = itemus.Count;
      
      

        loop=0;
      


        foreach (var item in itemus)
        {

            if (item.classification==Category)
            {
                loop++;
            }
        }


        int rnd = Random.Range(0, loop);

        var count = 0;
        bool bools = false;

        foreach (var item in itemus)
        {
            
            if (item.classification==Category)
            {
               
                if (count==rnd)
                {
                    if (item.name!="��������"&&item.name!="�S�X�N���b�v" && bools ==true)
                    {
                        
                        break;
                    }
                    else
                    {
                        rnd++;
                    }
                }
                
                count++;

            }
            

        }
        if(bools == true)
        {
            Player.GetComponent<I_Player_3D>().ItemLost_ToConnect(count);
        }
    }




    public void Conversion()//�����@�����_���ȃA�C�e���������_���ȃA�C�e���ɕϊ�����
    {
        Debug.Log("��������");
        var itemus = Player.GetComponent<I_Player_3D>().Hub_Items;
        
        if (itemus.Count!=0) {
            int loop = 0;
            int rnd = Random.Range(0, itemus.Count);
            Debug.Log("�Ȃ����A�C�e��"+itemus[rnd].ItemName);
            string Log = PhotonNetwork.NickName + "�������̓��̌��ʂɂ��" + itemus[rnd].ItemName + "�𕴎����܂����B";
            Player.GetComponent<I_Player_3D>().ItemLost_ToConnect(rnd);
            
            game_Manager.Log_connection(Log);
            var varItem = Player.GetComponent<I_Player_3D>().ItemMaster.Anniversary_Items;
            rnd = Random.Range(0, varItem.Count);
            Debug.Log("rnd = "+rnd);
            //Debug.Log("�Ȃ����A�C�e��"+itemus[rnd].ItemName);
            Player.GetComponent<I_Player_3D>().ItemAdd_ToConnect(rnd);
            Debug.Log(varItem[rnd].ItemName);
            Log = PhotonNetwork.NickName+"�������̓��̌��ʂɂ��"+ varItem[rnd].ItemName+"����肵�܂����B";
            game_Manager.Log_connection(Log);
        }
        else
        {
            string Log = PhotonNetwork.NickName+"���A�C�e���������Ă��Ȃ��������߁A�����𗘗p�ł��܂���ł����B";
            game_Manager.Log_connection(Log);
        }
    }



    public void Auction()//�����@�����_���ȃA�C�e���������_���ȃA�C�e���ɕϊ�����
    {

        var items = Player.GetComponent<I_Player_3D>().Hub_Items;

        if (items.Count!=0)
        {
            int loop = 0;
            int rnd = Random.Range(0, items.Count-1);
            Debug.Log("�Ȃ����A�C�e��"+items[rnd].ItemName);
            var Log = PhotonNetwork.NickName+"��"+items[rnd].ItemName+"������";
            Player.GetComponent<I_Player_3D>().ItemLost_ToConnect(rnd);

           

            var ItemsMaster = Player.GetComponent<I_Player_3D>().ItemMaster.Anniversary_Items;

            foreach (var MItem in ItemsMaster)
            {
                if (MItem.ItemName=="���D�i")
                {
                    Player.GetComponent<I_Player_3D>().ItemAdd_ToConnect(ItemsMaster.IndexOf(MItem));
                    Log= Log+ItemsMaster[ItemsMaster.IndexOf(MItem)].ItemName+"�𗎎D���܂����B";
                    game_Manager.Log_connection(Log);
                }

            }



           
        }
        else
        {
            string Log = PhotonNetwork.NickName+"���A�C�e���������Ă��Ȃ��������߁A�I�[�N�V�����𗘗p�ł��܂���ł����B";
            game_Manager.Log_connection(Log);
        }
    }



        //--------------------------�呠--------------------------------
        //Instance���o�Ă�����
        public void AppearInstances()
    {
        AnimationController InstAnimController;
        Camera_Mouse MainCameraMouse = game_Manager.Camera.GetComponent<Camera_Mouse>();

        foreach (GameObject j in game_Manager.Instance) 
        {
            InstAnimController = j.GetComponent<AnimationController>();
            if (InstAnimController.InstanceDay == Day_Square_Master.Day_Squares[DayNumber]) 
            {
                string Text_Announce;
                switch (Day_Square_Master.Day_Squares[DayNumber].Instance)
                {
                    case "�l�b�V�[�̓�":
                        Text_Announce = "�l�b�V�[���o�����܂��B";
                        game_Manager.Log_connection(Text_Announce);
                        break;

                    case "�x�m�R�����E��Y�ɓo�^���ꂽ��":
                        Text_Announce = "�x�m�R���o�����܂��B";
                        game_Manager.Log_connection(Text_Announce);
                        break;

                    case "�����̓�":
                        Text_Announce = "�����^���[���o�����܂��B";
                        game_Manager.Log_connection(Text_Announce);
                        break;

                    case "�I�J���g�L�O��":
                        Text_Announce = "���[�嗤���o�����܂��B";
                        game_Manager.Log_connection(Text_Announce);
                        break;

                    case "�}�����E�f�[":
                        Text_Announce = "�w�Z���o�����܂��B";
                        game_Manager.Log_connection(Text_Announce);
                        break;

                    case "�R�̓�":
                        Text_Announce = "�R���o�����܂��B";
                        game_Manager.Log_connection(Text_Announce);
                        break;

                    case "���C���{�[�u���b�W�J�ʋL�O��":
                        Text_Announce = "���C���{�[�u���b�W���o�����܂��B";
                        game_Manager.Log_connection(Text_Announce);
                        break;

                    case "����̓�":
                        Text_Announce = "���򂪏o�����܂��B";
                        game_Manager.Log_connection(Text_Announce);
                        break;
                }                
                MainCameraMouse.Camera_highlight_imi(InstAnimController.CameraPos, InstAnimController.CameraRot);
                StartCoroutine(InstAnimController.StartAnimation(InstAnimController.InstanceY));
            }
        }
        InsTance_ON=true;
    }
    //------------------------�����܂�------------------------------

    private void Effcet_OtherEffects()
    {
        var daySquare_Other = Day_Square_Master.Day_Squares[DayNumber].OtherEffects;
        if(daySquare_Other != "Noon")
        {
            if (daySquare_Other != "none")
            {
                switch (daySquare_Other)
                {
                    

                    case "�I���G���e�[�����O�̓�"://�����_�A�C�e���̏o��
                        int XMass = 0;
                        int YMass = 0;
                        do
                        {
                            XMass = Random.Range(0, game_Manager.Week[0].Day.Length);
                            YMass = Random.Range(0, game_Manager.Week.Length);
                        } while (game_Manager.Week[YMass].Day[XMass].activeInHierarchy == false );
                        photonView.RPC(nameof(OutPut_PresentSetting), RpcTarget.All, YMass, XMass);
                       
                        OtherEffects_End=true;
                        break;







                }
            }
        }
    }






    public void Effect_Couditional()
    {
        var daySquare_Condi = Day_Square_Master.Day_Squares[DayNumber].ConditionalPoint;
        Debug.Log("<color=red>Con</color>");
        if (daySquare_Condi != "Noon")
        {
            if (daySquare_Condi != "none")
            {
                switch (daySquare_Condi)
                {
                    case "�đ����̓�":

                        var itemus = Player.GetComponent<I_Player_3D>().Hub_Items;




                        foreach (var item in itemus)
                        {

                            if (item.classification=="�H�ו�")
                            {
                                item.ItemPoint++;
                                Debug.Log("<color=red>�đ���</color>");
                                Player.GetComponent<I_Player_3D>().ItemBlock.GetComponent<ItemBlock_List_Script>().PuintUpdate();

                            }


                        }
                        string Log ="�đ����̓��̌��ʂŁA"+ PhotonNetwork.NickName+"�̐H�ו��n�A�C�e���̃|�C���g���P�������܂����B";
                        game_Manager.Log_connection(Log);
                        Conditional_End=true;
                        break;

                    case "�Z��f�[":

                        Conditional_End=true;
                        break;





                }
            }
        }
    }

    //�����̋��L���肢���܂�
    [PunRPC]private void OutPut_PresentSetting(int YMass, int XMass)
    {
        game_Manager.Week[YMass].Day[XMass].GetComponent<I_Mass_3D>().Present_setting();
    }

}

