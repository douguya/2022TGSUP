using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Photon.Pun;
public class newRotate : MonoBehaviourPunCallbacks
{
    //public float xSpeed, ySpeed, zSpeed;�@//�e���̉�]���x
    //public bool rotate; //��]���~�߂�{�^��

    public int DiceNum; //�o����������̖�

    //private float xKeep, yKeep, zKeep; //��]���x�̕ۑ��p
    //private float xShow, zShow; //��������̖ڂ�������Ƃ��̊p�x
    private bool shuffle;

    public Sprite[] Dise_sprite = new Sprite[6];

    public List<int> InDiceNum = new List<int> { 1, 2, 3, 4, 5, 6 }; //�w�肳�ꂽ�����������납��o��

    //public int DiseSpeed = 2;       //�T�C�R���̐؂�ւ��X�s�[�h
    private float seconds;


    //------------------------------------�呠-----------------------------
    SEManager SE;

    // Start is called before the first frame update
    void Start()
    {
        //xKeep = xSpeed;
        //yKeep = ySpeed;
        //zKeep = zSpeed;
        //newDiceStop();
        SE = GameObject.FindGameObjectWithTag("SE").GetComponent<SEManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, zSpeed * Time.deltaTime));
        ////��]�����Ă�

        ////true�̏ꍇ��ɉ�]����
        //if (rotate == true)
        //{
        //    xSpeed = xKeep;
        //    ySpeed = yKeep;
        //    zSpeed = zKeep;
        //}
        seconds += Time.deltaTime;
        if (shuffle == true && seconds >= 0.1)
        {
            seconds = 0;
            gameObject.GetComponent<Image>().sprite = Dise_sprite[Random.Range(0, 6)];
        }

    }

    //�񂷂Ƃ��̌Ăяo���p�֐�
    //public void RotateStart()
    //{
    //    rotate = true;
    //}
    public void Dice_shuffle()
    {
        photonView.RPC(nameof(OutPut_DiceShuffle), RpcTarget.All);
    }

  [PunRPC]  public void OutPut_DiceShuffle()
    {
        SE.SEsetandplay("DiceSE");
        shuffle = true;
    }

    [PunRPC]
    public void OutPut_DiceShuffle_Stop()
    {
        shuffle = false;
    }

    //�X�g�b�v����Ƃ��̌Ăяo���p�֐�
    public void newDiceStop()
    {
        
        photonView.RPC(nameof(OutPut_DiceShuffle_Stop), RpcTarget.All);
        //�o���ڂ̐��������_���Ő���
        for (; ; )
        {
            DiceNum = Random.Range(1, 7);
            if (InDiceNum.Contains(DiceNum) == true)
            {
                break;//������������Ă���Βʂ�
            }
        }
        
        photonView.RPC(nameof(Output_LookDice), RpcTarget.All, DiceNum);
        //����������~
        //rotate = false;
        //xSpeed = 0;
        //ySpeed = 0;
        //zSpeed = 0;

        //�o���ڂ���������Z�b�g
        //switch (DiceNum)
        //{
        //    case 1:

        //        xShow = 0; zShow = 0;
        //        break;
        //    case 2:
        //        xShow = 0; zShow = 90;
        //        break;
        //    case 3:
        //        xShow = -90; zShow = 0;
        //        break;
        //    case 4:
        //        xShow = 90; zShow = 0;
        //        break;
        //    case 5:
        //        xShow = 0; zShow = -90;
        //        break;
        //    case 6:
        //        xShow = 180; zShow = 0;
        //        break;
        //}
        //��������̖ڂ�������
        //transform.rotation = Quaternion.Euler(xShow, 0, zShow);
    }


    
  [PunRPC]private void Output_LookDice(int Num)
    {
        gameObject.GetComponent<Image>().sprite = Dise_sprite[Num - 1];
    }

    public void OddDice() //��_�C�X�ɂȂ�
    {
        InDiceNum.Clear();
        InDiceNum.Add(1);
        InDiceNum.Add(3);
        InDiceNum.Add(5);
    }

    public void EvenDice()�@//�����_�C�X�ɂȂ�
    {
        InDiceNum.Clear();
        InDiceNum.Add(2);
        InDiceNum.Add(4);
        InDiceNum.Add(6);
    }

    public void resetDice()
    {
        InDiceNum.Clear();
        InDiceNum.Add(1);
        InDiceNum.Add(2);
        InDiceNum.Add(3);
        InDiceNum.Add(4);
        InDiceNum.Add(5);
        InDiceNum.Add(6);
    }
}
