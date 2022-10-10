using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRotate : MonoBehaviour
{
    public float xSpeed, ySpeed, zSpeed;�@//�e���̉�]���x
    public bool rotate; //��]���~�߂�{�^��

    public GameObject[] Dice = new GameObject[6];�@//�_�C�X�̊e�ʂɒ���t���Ă����
    public GameObject max; //��ԏ�̖ʂ̋�
    public int DiceNum; //�o����������̖�

    private float xKeep, yKeep, zKeep; //��]���x�̕ۑ��p

    public float xShow, zShow; //��������̖ڂ�������Ƃ��̊p�x

    // Start is called before the first frame update
    void Start()
    {
        xKeep = xSpeed;
        yKeep = ySpeed;
        zKeep = zSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, zSpeed * Time.deltaTime));
        //��]�����Ă�

        //true�̏ꍇ��ɉ�]����
        if (rotate == true)
        {
            xSpeed = xKeep;
            ySpeed = yKeep;
            zSpeed = zKeep;
        }

        //��]�����̑��x�Ȃ�0�ɂ���
        if (xSpeed < 0)
        {
            xSpeed = 0;
        }

        if (ySpeed < 0)
        {
            ySpeed = 0;
        }

        if (zSpeed < 0)
        {
            zSpeed = 0;
        }

        //�~�߂�{�^����������Ă������]���x�𗎂Ƃ��Ă���
        if (rotate == false)
        {
            if (xSpeed > 0)
            {
                xSpeed -= 30f * Time.deltaTime;
            }

            if (ySpeed > 0)
            {
                ySpeed -= 30f * Time.deltaTime;
            }

            if (zSpeed > 0)
            {
                zSpeed -= 30f * Time.deltaTime;
            }
        }

        //0�ɂȂ����琔���𔻒�
        if (xSpeed == 0 && ySpeed == 0 && zSpeed == 0) {
            DiceStop();
            Debug.Log(max);

            //�o���ڂ̐��ɂ���Č����������Z�b�g
            switch (DiceNum)
            {
                case 1:
                    xShow = -30; zShow = 0;
                    break;
                case 2:
                    xShow = -30; zShow = 90;
                    break;
                case 3:
                    xShow = 60; zShow = 0;
                    break;
                case 4:
                    xShow = -120; zShow = 0;
                    break;
                case 5:
                    xShow = -30; zShow = -90;
                    break;
                case 6:
                    xShow = 150; zShow = 0;
                    break;
            }

            //�o���ڂ̖ʂ�������������
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(xShow, 0, zShow), 1.0f * Time.deltaTime);

        }

    }

    public void DiceStop()
    {
        max = Dice[0];
        DiceNum = 1;
        for (int i = 1; i < 6; i++)
        {
            //�e�ʂɒ���t�����󔠂̍���(y)���ׂĈ�ԍ������̂�Ԃ�
            if(max.transform.position.y < Dice[i].transform.position.y)
            {
                max = Dice[i];
                DiceNum = i + 1;
            }
        }


    }
}
