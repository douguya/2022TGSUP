using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeManager : MonoBehaviour
{
    public int[] month = new int[4];                //�ݒu���錎���󂯎��
    public changes_Width[] changes_height = new changes_Width[10]; //sugorokuManagre��height[]width[]�̑���

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public class changes_Width//week�̎q�E����̃I�u�W�F�N�g�̎擾
    {
        public GameObject[] changes_width;




    }

    //�ǉ�
    private void MonthSetting()//�}�X�Ɍ��Ɠ��t������
    {
        int Xmonth = 0;//�ݒu����}�b�v��X�̔z������炷
        int Ymonth = 0;//�ݒu����}�b�v��Y�̔z������炷

        for (int block = 0; block < this.month.Length; block++)//�w�肷�錎���ǂ̃u���b�N�ɂ��邩����
        {
            switch (block)//���ꂼ��̃u���b�N�Ɏw�肵�����t������悤�ɂ���
            {
                case 0:
                    Xmonth = 0; Ymonth = 0;
                    break;
                case 1:
                    Xmonth = changes_height[0].changes_width.Length / 2; Ymonth = 0;
                    break;
                case 2:
                    Xmonth = 0; Ymonth = changes_height.Length / 2;
                    break;
                case 3:
                    Xmonth = changes_height[0].changes_width.Length / 2; Ymonth = changes_height.Length / 2;
                    break;
            }
            for (int month = 0; month < 12; month++)//month�ɉ����������
            {
                if (this.month[block] == month + 1)//�w�肵���������������ʂ���
                {
                    DaySetting(month, Ymonth, Xmonth);//�}�X�ɓ��t������
                }
            }
        }

    }

    //�ǉ�
    private void DaySetting(int month, int Ymonth, int Xmonth)//�}�X�ɓ��t������
    {
        int nullday = 0;//�󔒂̓��t
        int countday = 0;//�������t
        int Maxday = 0;//���̌��̍ő���t

        switch (month + 1)
        {
            case 1:
                nullday = 6; Maxday = 31;
                break;
            case 2:
                nullday = 2; Maxday = 28;
                break;
            case 3:
                nullday = 2; Maxday = 31;
                break;
            case 4:
                nullday = 5; Maxday = 30;
                break;
            case 5:
                nullday = 0; Maxday = 31;
                break;
            case 6:
                nullday = 3; Maxday = 30;
                break;
            case 7:
                nullday = 5; Maxday = 31;
                break;
            case 8:
                nullday = 1; Maxday = 31;
                break;
            case 9:
                nullday = 4; Maxday = 30;
                break;
            case 10:
                nullday = 6; Maxday = 31;
                break;
            case 11:
                nullday = 2; Maxday = 30;
                break;
            case 12:
                nullday = 4; Maxday = 31;
                break;
        }
        for (int n = 0; n < 7 - nullday; n++)//���T�ڂɓ��t������
        {
            countday++;
            changes_height[Ymonth].changes_width[Xmonth + nullday + n].GetComponent<Mass>().Day = month + 1 + "/" + countday;
        }

        for (int h = 1; h < changes_height.Length / 2; h++)//���T�ȍ~�̓��t������
        {
            for (int w = 0; w < changes_height[0].changes_width.Length / 2; w++)
            {
                if (countday < Maxday)
                {
                    countday++;
                    changes_height[Ymonth + h].changes_width[Xmonth + w].GetComponent<Mass>().Day = month + 1 + "/" + countday;
                }
            }
        }
    }
}
