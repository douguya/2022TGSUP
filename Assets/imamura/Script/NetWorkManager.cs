using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class NetWorkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public string[] Room = new string[5];

    public GameObject SceneManagerObj;//�V�[���}�l�[�W���[�̃I�u�W�F�N�g
                                      // public GameObject I_game_manager;//�Q�[���}�l�[�W���[�̃I�u�W�F�N�g;
    public I_game_manager I_game_Manager_Script;//�Q�[���}�l�[�W���[�̃I�u�W�F�N�g�̃X�N���v�g;

    //  public GameObject ReadyButton;//�Q�[���}�l�[�W���[�̃I�u�W�F�N�g;
    public ReadyButton ReadyButton_Script;//�Q�[���}�l�[�W���[�̃I�u�W�F�N�g�̃X�N���v�g;

    public InputField InputField;     //���O���͗�
    public Text PlayerName;           //���͂��ꂽ�v���C���[�̖��O
    public Text[] RoomText;           //���[���̖��O�ƃe�L�X�g
    public GameObject[] RoomBotton;   //���[���{�^���̃I�u�W�F�N�g

    public GameObject ItemListBox;//�A�C�e�����X�g�̐e
    public GameObject LoadImage;//���[�h��ʂ̂��ǂ�

    public GameObject ItemList_Moment;//�A�C�e�����X�g�̐e

    [SerializeField]
    public int PlayerIdVew;
    public string PlayerNameVew;

    public Hashtable hashtable = new Hashtable();//�J�X�^���v���p�e�B�̃��X�g
    public bool[] CanJoinRoom = new bool[5] { true, true, true, true, true };

    private byte MaxRoomPeople = 4;//��̃��[���̍ő�l��
    string GameVersion = "Ver1.0";

    public GameObject[] myArray;
    public Material[] MaterialsList = new Material[4];
    public Material[] CursorColor_List = new Material[4];


    public 

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        InputField = GetComponent<InputField>();
        LoadImage.SetActive(true);
        // I_game_Manager_Script=I_game_manager.GetComponent<I_game_manager>();
        // ReadyButton_Script=ReadyButton.GetComponent<ReadyButton>();

    }
    private void Update()
    {

    }

    public override void OnConnectedToMaster()//�}�X�^�[�T�[�o�ɐڑ����ꂽ���ɌĂ΂��
    {
        PhotonNetwork.JoinLobby();//���r�[�ɓ���
    }
    // Update is called once per frame
    public override void OnJoinedLobby()
    {
        Debug.Log("���r�[�֎Q�����܂���");
        LoadImage.SetActive(false);

    }




    public override void OnRoomListUpdate(List<RoomInfo> roomList)//���[�����X�g�X�V�� �X�V���ꂽ���[���݂̂��󂯎��
    {

        foreach (var info in roomList)//���[�����X�g�̎擾
        {
            int RoomNum = int.Parse(Regex.Replace(info.Name, @"[^0-9]", ""));//�ύX���ꂽ���[���̔ԍ��𒊏o

            RoomText[RoomNum-1].text = info.Name + "A " + info.PlayerCount + "/" + MaxRoomPeople;

        }


    }


    public void JoineLoom(int RoomNum)//�����ɓ��鏈��
    {
        SceneManagerObj.GetComponent<SceneManagaer>().TransitionToGame();//�Q�[���V�[���֑J��
        StartCoroutine(JoineLoom_Coroutine(RoomNum));
    }

    public IEnumerator JoineLoom_Coroutine(int RoomNum)//�����ɓ��鏈��,�R���[�`��
    {
        yield return new WaitForSeconds(0.4f);
        var roomOptions = new RoomOptions();//���[���I�v�V�����̐ݒ�
        roomOptions.MaxPlayers = MaxRoomPeople;
        PhotonNetwork.JoinOrCreateRoom("���[��" + RoomNum, roomOptions, TypedLobby.Default);

        yield break;
    }




    public override void OnJoinedRoom()//�����ɓ��ꂽ���̏���
    {
        StartCoroutine(OnJoinedRoom_Coroutine());
    }

    public IEnumerator OnJoinedRoom_Coroutine()///�����ɓ��ꂽ���̏����@�R���[�`��
    {

        MaterialChange();
     
        var position = new Vector3(0.28f, -3.37f, -0.73f);
        GameObject Player = PhotonNetwork.Instantiate("Player3D", position, Quaternion.identity);//�v���C���[�̐���

        GameObject ItemList_UGI = PhotonNetwork.Instantiate("ItemBlock_List", position, Quaternion.identity);//�v���C���[�̃A�C�e�����X�g�̍쐬
        ReadyButton_Script.JoinedRoom_Jointed();
        GameObject PlayerCarsol = PhotonNetwork.Instantiate("Cursor", position, Quaternion.identity);//�v���C���[�̐���


        
        IconChange();

        Player.GetComponent<I_Player_3D>().DiceButton.GetComponent<Button>().onClick.AddListener(Player.GetComponent<I_Player_3D>().DicePush);
 

        hashtable["PlayerSetUP"]=true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);//�ύX�����J�X�^���v���p�e�B�̍X�V

        yield return new WaitForSeconds(0.4f);
        string Log = I_game_Manager_Script.PlayerColouradd(PhotonNetwork.NickName)+"���������܂����B";
        I_game_Manager_Script.Log_connection(Log);

        Playerlist_Update();
        
        LoadImage.SetActive(false);
        
        GameObject.Find("I_game_manager").GetComponent<Guide>().rady_BottonStart();
        GameObject.Find("I_game_manager").GetComponent<Guide>().option_BottonStart();
        GameObject.Find("I_game_manager").GetComponent<Guide>().chat_Start();

        yield break;

    }

    

    public void MaterialChange()//�v���C���[�̃}�e���A����ύX����
    {
        
        foreach (var Materials in MaterialsList)//�}�e���A���̓��e�����ԂɊi�[
        {
            bool juje = false;
            foreach (var PList in PhotonNetwork.PlayerList)//�v���C���[���X�g�̓��e�����ԂɊi�[
            {
                if (PList.CustomProperties.ContainsValue(System.Array.IndexOf(MaterialsList, Materials)))//���̃}�e���A�����v���p�e�B�Ɏ��v���C���[�������ꍇ
                {
                    juje=true;
                }
            }

            if (juje == false)
            {
              
                hashtable["PlayerNumMaterial"]=System. Array.IndexOf(MaterialsList, Materials);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);//�ύX�����J�X�^���v���p�e�B�̍X�V
                break;
            }


           
        }

    }

    public void IconChange()//�v���C���[�̃A�C�R���̃v���p�e�B��ύX����
    {



        for (int loop=0;loop<4;loop++) {
            bool juje = false;
            foreach (var PList in PhotonNetwork.PlayerList)//�v���C���[���X�g�̓��e�����ԂɊi�[
            {
                if (PList.CustomProperties.ContainsValue(PList.CustomProperties["iconNum"]))//���̃}�e���A�����v���p�e�B�Ɏ��v���C���[�������ꍇ
                {
                    if ((int)PList.CustomProperties["iconNum"]==loop) {
                        juje=true;

                    }
                }
            }

            if (juje == false)
            {

                hashtable["iconNum"]=loop;
                PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);//�ύX�����J�X�^���v���p�e�B�̍X�V

                break;
            }



        }

    }







    public void ItemList_UGI_Transform(GameObject ItemList)
    {
        ItemList_Moment=ItemList;
        photonView.RPC(nameof(ItemList_UGI_Transform_RPC), RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void ItemList_UGI_Transform_RPC()
    {
        ItemList_Moment.transform.parent=ItemListBox.transform;
    }



    [PunRPC]
    public void PlayerAppearance(GameObject Player)
    {
        Player.SetActive(true);
    }







    public override void OnJoinRoomFailed(short returnCode, string message)//�����ɓ���Ȃ������Ƃ�
    {
        if (SceneManager.GetActiveScene().name == SceneManagaer.Gamesend)//�Q�[���V�[���ɓ����Ă��܂����ꍇ
        {
            SceneManager.LoadScene(SceneManagaer.Lobysend);//���r�[�V�[���ɕԂ�
            PhotonNetwork.JoinLobby();//���r�[�ɕԂ�
        }
    }



    public void FinishInputName()//���O�����͂��ꂽ�Ƃ�
    {
        PhotonNetwork.NickName = PlayerName.text;//�v���C���[�̖��O��ύX����
        PlayerNameVew = PlayerName.text;//�v���C���[�̖��O���C���X�y�N�^�[���猩����悤�ɂ���
    }



    public void Leave_the_room()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerLeftRoom(Player player)//�v���C���[���������Ƃ��̏���
    {
        StartCoroutine(OnPlayerLeftRoom_Coroutine());
        string Log = "�v���C���[�F"+ player.NickName+"���ޏo���܂����B";
        I_game_Manager_Script.Log_Mine(Log);
    }

    public IEnumerator OnPlayerLeftRoom_Coroutine()//�v���C���[���������Ƃ��̏��� �R���[�`��
    {
        ReadyButton_Script.PlayerLeftRoom_Jointed();
        yield return new WaitForSeconds(0.4f);
        Playerlist_Update();//�v���C���[�̃I�u�W�F�N�g�i�[�p/�����ʒu�ւ̈ړ����܂�
       

        yield break;
    }




    public override void OnPlayerEnteredRoom(Player newPlayer)//���g�����[���ɓ������Ƃ�
    {
        StartCoroutine(OnPlayerEnteredRoom_Coroutine());

    }

    public IEnumerator OnPlayerEnteredRoom_Coroutine()//���҂����[���ɓ������Ƃ��R���[�`��
    {



        yield return new WaitForSeconds(0.6f);
     
        yield break;
    }




    public void Playerlist_Update()//�v���C���[�̃I�u�W�F�N�g�i�[�p/�����ʒu�ւ̈ړ����܂�
    {
  
        GameObject[] Players_spot = GameObject.FindGameObjectsWithTag("Player");//�v���C���[�I�u�W�F�N�g�̈ꎞ�ۑ��ꏊ�@�^�O�Ō����݂Ƃ�
        GameObject[] ItemListSpot = GameObject.FindGameObjectsWithTag("ItemList_UGI");//�v���C���[�I�u�W�F�N�g�̈ꎞ�ۑ��ꏊ�@�^�O�Ō����݂Ƃ�

     

        I_game_Manager_Script.Player.Clear();


        int loop = 0;//�A�C�e�����X�g�̏����l
        foreach (var PList in PhotonNetwork.PlayerList)//�v���C���[���X�g�̓��e�����ԂɊi�[
        {
            
            foreach (GameObject obj in Players_spot)//�v���C���[���X�g�̒��g�ƁA�ꎞ�ۑ������v���C���[�I�u�W�F�N�g��˂����킹��
            {
                if (PList.ActorNumber==obj.GetComponent<PhotonView>().CreatorActorNr) //���X�g�̃v���C���[��ID�ƃI�u�W�F�N�g�̍쐬�҂�AD���r
                {
                    
                    I_game_Manager_Script.Player.Add(obj);//���̏����ŁA�v���C���[���X�g�̏��Ԃǂ���Ƀv���C���[�I�u�W�F�N�g��ۑ��ł���@���Ԃ�ς�����悤�ɂ������Ȃ�ύX�̗]�n����
                    I_game_Manager_Script.Player_setting(loop);//�v���C��\������̈ʒu�Ɉړ�
                    obj.GetComponent<I_Player_3D>().PlayerNumber=loop;

                   
                }
            }

            foreach (GameObject obj in ItemListSpot)//�v���C���[���X�g�̒��g�ƁA�ꎞ�ۑ������v���C���[�I�u�W�F�N�g��˂����킹��
            {
                obj.transform.parent=ItemListBox.transform;
                string name= obj.GetComponent<PhotonView>().Owner.NickName;
          
                obj.GetComponent<ItemBlock_List_Script>().Namedisplay(name);
                if (PList.ActorNumber==obj.GetComponent<PhotonView>().CreatorActorNr) //���X�g�̃v���C���[��ID�ƃI�u�W�F�N�g�̍쐬�҂�AD���r
                {
            

                    foreach (GameObject Player in Players_spot)//�Ή�����v���C���[�Ƀv���C���[���X�g��˂�����
                    {
                        if (Player.GetComponent<PhotonView>().CreatorActorNr==obj.GetComponent<PhotonView>().CreatorActorNr) //���X�g�̃v���C���[��ID�ƃI�u�W�F�N�g�̍쐬�҂�AD���r
                        {
                            Player.GetComponent<I_Player_3D>().ItemBlock=obj;//�u���b�N���X�g���i�[
                 
                            obj.GetComponent<ItemBlock_List_Script>().Player=Player;
                            I_game_Manager_Script.ItemList_setting(obj, loop);//�v���C��\������̈ʒu�Ɉړ�

                           

                        }
                    }

                }
              
            }
            loop++;
            I_game_Manager_Script.joining_Player = PhotonNetwork.PlayerList.Length;
            if (I_game_Manager_Script.Player.Count!=loop)
            {
                Debug.LogError("��蔭���B���������Ȃ����ĉ�����");
            }
        }

        GameObject[] Cursors = GameObject.FindGameObjectsWithTag("cursor");
       
        foreach (GameObject Cursor in Cursors)//�Ή�����v���C���[�Ƀv���C���[���X�g��˂�����
        {
            Cursor.transform.parent=I_game_Manager_Script.cursors.transform;

            Cursor.GetComponent<Mouse_Cursor>().RotateChange();
        }


    }



    public string Playername(GameObject Player)//�v���C���\�̖��O���Ƃ�
    {
        string name="";
        foreach (var PList in PhotonNetwork.PlayerList)//�v���C���[���X�g�̓��e�����ԂɊi�[
        {
            if (PList.ActorNumber== Player.GetComponent<PhotonView>().CreatorActorNr) //���X�g�̃v���C���[��ID�ƃI�u�W�F�N�g�̍쐬�҂�AD���r
            {
                name=PList.NickName;
            }
        }
        return name;
    }


    public override void OnPlayerPropertiesUpdate(Player player, Hashtable propertiesThatChanged)
    {
       
        foreach (var prop in propertiesThatChanged)
        {
           
            if ((string)prop.Key=="PlayerNumMaterial")//�ύX���ꂽ�v���C���[�v���p�e�B���}�e���A���Ɋւ�����̂������ꍇ
            {

                Playerlist_Material_Update();

            }
            if ((string)prop.Key=="iconNum")//�ύX���ꂽ�v���C���[�v���p�e�B���}�e���A���Ɋւ�����̂������ꍇ
            {

               Icon_Update();

            }
            if ((string) prop.Key=="PlayerSetUP")
            {
                Playerlist_Update();//�v���C���[�̃I�u�W�F�N�g�i�[�p/�����ʒu�ւ̈ړ����܂�
                Playerlist_Material_Update();
                Icon_Update();
            }

        }
    }

    public void Playerlist_Material_Update()
    {
       
        GameObject[] Players_spot = GameObject.FindGameObjectsWithTag("Player");//�v���C���[�I�u�W�F�N�g�̈ꎞ�ۑ��ꏊ�@�^�O�Ō����݂Ƃ�
        GameObject[] Cursors = GameObject.FindGameObjectsWithTag("cursor");
        foreach (var PList in PhotonNetwork.PlayerList)//�v���C���[���X�g�̓��e�����ԂɊi�[
        {
           

            foreach (GameObject obj in Players_spot)//�v���C���[���X�g�̒��g�ƁA�ꎞ�ۑ������v���C���[�I�u�W�F�N�g��˂����킹��
            {
              
                if (PList.ActorNumber==obj.GetComponent<PhotonView>().CreatorActorNr) //���X�g�̃v���C���[��ID�ƃI�u�W�F�N�g�̍쐬�҂�AD���r
                {

                    Transform children = obj.GetComponentInChildren<Transform>();

                    foreach (Transform ob in children)
                    {
          
                        ob.GetComponent<Renderer>().material=MaterialsList[(int)PList.CustomProperties["PlayerNumMaterial"]];
                    }

                   
                }
            }


            foreach (GameObject obj in Cursors)//�v���C���[���X�g�̒��g�ƁA�ꎞ�ۑ������v���C���[�I�u�W�F�N�g��˂����킹��
            {

                if (PList.ActorNumber==obj.GetComponent<PhotonView>().CreatorActorNr) //���X�g�̃v���C���[��ID�ƃI�u�W�F�N�g�̍쐬�҂�AD���r
                {

                  
                        obj.GetComponent<Image>().material=CursorColor_List[(int)PList.CustomProperties["PlayerNumMaterial"]];
                    


                }
            }











        }
    }
    public void Icon_Update()
    {
        GameObject[] ItemListSpot = GameObject.FindGameObjectsWithTag("ItemList_UGI");//�v���C���[�I�u�W�F�N�g�̈ꎞ�ۑ��ꏊ�@�^�O�Ō����݂Ƃ�
      
        foreach (var PList in PhotonNetwork.PlayerList)//�v���C���[���X�g�̓��e�����ԂɊi�[
        {


            foreach (GameObject obj in ItemListSpot)//�v���C���[���X�g�̒��g�ƁA�ꎞ�ۑ������v���C���[�I�u�W�F�N�g��˂����킹��
            {

                if (PList.ActorNumber==obj.GetComponent<PhotonView>().CreatorActorNr) //���X�g�̃v���C���[��ID�ƃI�u�W�F�N�g�̍쐬�҂�AD���r
                {

                    var Image = obj.GetComponent<ItemBlock_List_Script>().IcobImage.GetComponent<Image>();
                    int num = (int)PList.CustomProperties["iconNum"];
                    Image.sprite= I_game_Manager_Script.IconSprits.Icons[num];//�}�e���A���Ɠ����菇�ŃA�C�e�����X�g�̉摜��ύX
                    Debug.Log("AAAAAAAAAAAA"+num);
                }
            }
        }
    }
    public void Voidlobby()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        OnLeftLobby();
    }

    public override  void OnLeftLobby()
    {
      
        SceneManagerObj.GetComponent<SceneManagaer>().TransitionToMain();
    }




    public void VoidRoom()
    {        
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }


    public override void OnLeftRoom()
    {
        Cursor.visible = true;
        SceneManagerObj.GetComponent<SceneManagaer>().TransitionToMain();
    }
}
