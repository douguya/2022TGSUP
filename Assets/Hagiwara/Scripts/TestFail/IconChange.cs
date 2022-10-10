using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconChange : MonoBehaviour
{
    public Sprite yuurei;//�H��̉摜
    public Sprite Tsyatu;//T�V���c�̉摜
    public Sprite bikini;//�r�L�j�̉摜
    public Sprite Default;//�ʏ�摜

    private Image m_Image;// Image �R���|�[�l���g���i�[����ϐ�

    void Start()
    {
        m_Image = GetComponent<Image>();
    }

    public Sprite Change(string day)
    {
        switch (Daykouka.DayEffectictDictionary[day][0, 2])
        {
            case "�H��":
                return yuurei;// �H��̉摜�ɐ؂�ւ���

            case "T�V���c":
                return Tsyatu;// T�V���c�̉摜�ɐ؂�ւ���

            case "�r�L�j":
                return bikini;// �r�L�j�̉摜�ɐ؂�ւ���
                
            default:
                return Default;// �ʏ�̉摜�ɐ؂�ւ���
        }

    }
}
