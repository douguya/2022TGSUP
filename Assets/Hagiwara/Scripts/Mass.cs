using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Mass : MonoBehaviour
{
    public bool Open;//�}�X���󂢂Ă邩�ǂ���
    public bool Goal;//�}�X���S�[�����ǂ���
    public bool invalid;//���̃}�X�����݂��邩�ǂ���
    public bool walk;//onClick���ꂽ���ǂ������ׂ�
    public string Day;//�}�X�͉�����

    public GameObject GoalFlag;//�S�[���̊ە\���p
    public GameObject hako;//�}�X�̕\���p
    public GameObject select;//�ړ��ł���}�X�̕\���p
    public GameObject decision;//�ړ��ł���}�X�̕\���p

    void Start()
    {
        if (invalid == true)
        {
            hako.SetActive(false);
        }
    }

   
    void Update()
    {
        
    }

    public void GoalOn()//�S�[����\��������
    {
        GoalFlag.SetActive(true);
        Goal = true;
    }

    public void GoalOff()//�S�[��������
    {
        GoalFlag.SetActive(false);
        Goal = false;
    }

    public void Selecton()//�ړ��ł���}�X�̕\���p
    {
        select.SetActive(true);
    }

    public void Decisionon()//�ړ��ł���}�X�̌���\���p
    {
        decision.SetActive(true);
    }

    public void Selectoff()//�ړ��ł���}�X�̔�\���p
    {
        select.SetActive(false);
    }

    public void Decisionoff()//�ړ��ł���}�X�̌����\���p
    {
        decision.SetActive(false);
    }

    public void onClick()
    {
        Debug.Log("AAA");
        if (select.activeSelf == true)
        {
           
            Selectoff();
            Decisionon();
            walk = true;
        }
        
    }

}
