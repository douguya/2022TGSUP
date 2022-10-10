using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckGroundChange : MonoBehaviour
{
    public Sprite mutairiku;//���[�嗤�̉摜
    public Sprite ToKyoTower;//�����^���[�̉摜

    SpriteRenderer sr;// �摜�`��p�̃R���|�[�l���g
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();// Sprite��SpriteRenderer�R���|�[�l���g���擾
    }

    public void Change(string day)
    {
        switch (Daykouka.DayEffectictDictionary[day][0, 2])
        {
            case "���[�嗤":
                sr.sprite = mutairiku;// ���[�嗤�̉摜�ɐ؂�ւ���
                break;

            case "�����^���[":
                sr.sprite = ToKyoTower;// �����^���[�̉摜�ɐ؂�ւ���
                break;
        }
    }



}
