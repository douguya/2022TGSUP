using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Threading.Tasks;


public class PlayerStatasIMamura : MonoBehaviourPunCallbacks
{

    public List<string> HabItem;//�����Ă���A�C�e��
    private int Goalcount = 0;  //�S�[��������
    private int PX, PY;         //�v���C���[�̃}�X���W
    public GameObject Play;     //�v���C���[�̃Q�[���I�u�W�F�N�g


    [SerializeField]
    private Dropdown dropdown;�@//�A�C�e���ꗗ�ւ̃A�N�Z�X�p
    public int PlayerIdVew;�@�@ //�v���C���[��ID
    public string PlayerNameVew;//�v���C���[�̖��O
    public int HowPlayer;       //���l�v���C���[�����邩�̉{���p
    public Button Botton;�@�@�@ //����e�X�g�p�{�^���ւ̃A�N�Z�X�p

    void Start()
    {
        PlayerIdVew = photonView.OwnerActorNr;�@�@//�v���C���[��ID�̓���
        PlayerNameVew = photonView.Owner.NickName;//�v���C���[�̖��O�̓���
        SetPlayernumShorten();                    //�A�C�e�����X�gUI�ƃv���C���[�̓���[

        this.name = photonView.Owner.NickName;

       


    }

    public async void SetPlayernumShorten()//�A�C�e�����X�gUI�̓���
    {
        await Task.Delay(50);//��C�ɕ����̃v���C���[�ƃA�C�e�����X�g�̓��������邽�߂̈ꎞ��~






        int loop = 1;//�A�C�e�����X�g�̏����l
        foreach (var PList in PhotonNetwork.PlayerList)//�v���C���[���X�g�̓��e�����ԂɊi�[
        {
            if (photonView.CreatorActorNr == PList.ActorNumber)//�����̍쐬�҂�ID��PList��ID�ƃC�R�[���Ȃ�
            {
                dropdown.ClearOptions();//�ڍs�O�̃��X�g����
                dropdown = GameObject.Find("Dropdown:Player" + loop).GetComponent<Dropdown>();//�v���C���[�̏��ԂɑΉ������A�C�e�����X�gUI�Ƃ̓���
                dropdown.ClearOptions();//���������A�C�e�����X�g�̏�����

                dropdown.options.Add(new Dropdown.OptionData { text = "" + PlayerNameVew });//�A�C�e�����X�g�̃��x���t��
                dropdown.RefreshShownValue();//�A�C�e�����X�gUI�̍X�V

                //�e�X�g�p�{�^���̃Z�b�e�B���O
                if (photonView.IsMine)//PList���v���C���[�̂��̂Ȃ�
                {
                    Botton = GameObject.Find("traffic_lights").GetComponent<Button>();//�e�X�g�p�{�^���ւ̃A�N�Z�X�p
                    Botton.onClick.AddListener(() => Itemobtain("�M���@"));//�e�X�g�p�{�^���ւ̊֐��ǉ�
                }
            }
            loop++;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)//���̃v���C���[�����Ȃ��Ȃ������̃R�[���o�b�N
    {
        //�A�C�e�����X�gUI�̍X�V�̂��߂̑S�̏�����
        for (int loop = 1; loop < 5; loop++)
        {
            GameObject.Find("Dropdown:Player" + loop).GetComponent<Dropdown>().ClearOptions();//�폜
            GameObject.Find("Dropdown:Player" + loop).GetComponent<Dropdown>().RefreshShownValue();//�X�V

        }

        SetPlayernumShorten();//���߂ẴA�C�e�����X�gUI�̓���
    }


    public void Itemobtain(string Item)//�A�C�e������ɓ��ꂽ�ꍇ�̊֐����Ăяo��
    {
        photonView.RPC(nameof(ItemobtainToRPC), RpcTarget.All, Item);
    }

    [PunRPC]
    public void ItemobtainToRPC(string Item)//�A�C�e������ɓ��ꂽ�ꍇ�̊֐�
    {
        HabItem.Add(Item);//Item�̃��X�g�ւ̒ǉ�
        dropdown.options.Add(new Dropdown.OptionData { text = Item + DictionaryManager.ItemDictionary[Item][0] + "P" });//�A�C�e���Ƃ��̃|�C���g���A�C�e�����X�gUI�ɒǉ�
        dropdown.RefreshShownValue();//�A�C�e�����X�gUI�̍X�V
    }

}


