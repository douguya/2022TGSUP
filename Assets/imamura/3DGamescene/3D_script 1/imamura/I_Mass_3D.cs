using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class I_Mass_3D : MonoBehaviour
{

    public bool Open;//�}�X���󂢂Ă邩�ǂ���
    public bool Goal;//�}�X���S�[�����ǂ���
    public bool warp;//�}�X�����[�v�o����}�X��
    public bool decision;//�}�X�͈ړ����肳��Ă邩
    public bool On_Click;
    public string Day = "null";//�}�X�͉�����
    public bool Present_Item;


    public GameObject goal_flag;//�S�[���̊ە\���p
    public GameObject warp_point;//�ړ��ł���}�X�̕\���p
    public GameObject hideCover;//�}�X�̕\���p
    public GameObject select;//�ړ��ł���}�X�̕\���p
    public GameObject decision_Mass;//���肵���}�X�̕\���p
    public GameObject DayText;//�}�X�̓��t�p
    public GameObject Present;

    void Start()
    {

    }

    void Update()
    {

    }

    public void hideCover_setting()
    {
        hideCover.SetActive(true);
        Open = false;
    }

    public void hideCover_Clear()
    {
        hideCover.SetActive(false);
        Open = true;
    }

    public void warp_setting()
    {
        warp = true;
        warp_point.SetActive(true);
    }

    public void Goal_setting()
    {
        Goal = true;
        goal_flag.SetActive(true);
    }

    public void Goal_Clear()
    {
        Goal = false;
        goal_flag.SetActive(false);
    }

    public void select_display()
    {
        select.SetActive(true);
    }

    public void select_hidden()
    {
        select.SetActive(false);
    }

    public void decision_display()
    {
        decision_Mass.SetActive(true);
        decision = true;
    }

    public void decision_hidden()
    {
        decision_Mass.SetActive(false);
        decision = false;
    }

    public void OnClick()
    {
        if (select.activeSelf == true)
        {
            select_hidden();
            decision_display();
            On_Click = true;
            GameObject.Find("I_game_manager").GetComponent<I_game_manager>().Player_select();
        }

    }
    public void DayText_setting(int day)
    {
        DayText.GetComponent<TextMesh>().text = "" + day;
    }

    public void Present_setting()
    {
        Present.SetActive(true);
        Present_Item = true;
    }

    public void Present_hid()
    {
        Present.SetActive(false);
        Present_Item = false;
    }
}

