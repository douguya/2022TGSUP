using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*
        �A�C�e�����X�g�i���ݑ��݂��Ă��镪�j
        8��
        �͂��҂T
        ���a�ȐS�H�R
        ��������ԉ΂S
        �p�p�H�P
        ���a�̐S�H�W
        �X�q�R
        �R�X
        ���k�b�S
        �ʉ������W
        �p�C���R
        ���A�p���t���b�g�T
        �M���@�R
        �p���c�P
        ���V���`�S
        �E�N�����H
        �h���b�V���O�Q
        �`�L�����[�����Q
        ���C���{�[�u���b�W�P
        �W�F���[�g�R
        �x�[�g�[���F������Q
        �����R
        �n�b�s�[�T

        */


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�|�C���g�ǉ��p�C�x���g
    void ConditionalPoint(string x,int y)
    {
        int num = int.Parse(x);
        y += num;
    }


}
