using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
public class imamuraDice : MonoBehaviour
{
    [SerializeField]
    Sprite[] DiceImages=new Sprite[6]; //�_�C�X�̉摜

    private Image Dice;�@�@�@�@�@�@�@�@//Imag�ւ̊���
    private bool DiceSpin = true;�@�@�@ //�T�C�R�����񂷋���
    public int DiseSpeed;       //�T�C�R���̐؂�ւ��X�s�[�h
    int loop = 1;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Dice = this.GetComponent<Image>();//�_�C�X�̉摜�R���|�[�l���g�w��
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public async void OnDiceSpin()//�T�C�R�����񂷊֐�
    {
        DiceSpin = true;
        loop =0;�@�@�@�@�@�@�@//�T�C�R���̉摜�w��p
        while (DiceSpin == true) //�T�C�R���̉�]����
        {
            await Task.Delay(DiseSpeed);//�T�C�R���̉摜�ύX�̃f�B���C
            Dice.sprite =DiceImages[loop];//�T�C�R���̉摜�ύX
            loop++;�@�@�@�@�@�@�@�@�@�@
            if (loop > 5)
            { loop = 0; }
         }
    }

    public int  StopDice()//�_�C�X���~�߂Ēl��Ԃ�
    {
        DiceSpin = false;//�_�C�X�X�g�b�v
       
        return loop+1;// �_�C�X�̒l��Ԃ�
    }


    public void StopDiceBotton()//�{�^�����疳�����N�����悤
    {
        StopDice();
        Debug.Log(StopDice());

    }
}
