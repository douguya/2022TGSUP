using System;
using UnityEngine;



[Serializable]

[CreateAssetMenu(fileName = "Anniversary_Item", menuName = "Anniversary_Item")]
public class Anniversary_Item : ScriptableObject
{




    [Tooltip("�A�C�e���̖��O")]
    public string ItemName;
    [Tooltip("�A�C�e���̃|�C���g")]
    public int ItemPoint;
    [Tooltip("�L�O���̖��O")]
    public string Anniversary;
    [Tooltip("�L�O���̓��t")]
    public string Day;
    [Tooltip("�A�C�e���̉摜")]
    public Sprite ItemSprite;
    [Tooltip("�A�C�e���̕���")]
    public string classification;
    [Tooltip("�A�C�e���̎����@�Ȃ��ꍇ��null")]
    public string ItemLifespan;
     

    [Tooltip("�A�C�e���̐����@�g�p���ʂ�����ꍇ�͂�����܂�")]
    [Multiline(7)]
    public string Commentary;

    [Tooltip("�A�C�e���d�l���ʂ̗L�� ����ꍇ��true")]
    public bool ItemEffect;


    
    [Tooltip("���̏u�Ԃ̈ړ�")]
    public string Move;
    [Tooltip("�ύX����BGM")]
    public string BGM;
    [Tooltip("�o������I�u�W�F�N�g")]
    public string Instance;
    [Tooltip("�p�[�e�B�N���ɂ��񎟉��o")]
    public string particle;
    [Tooltip("�_�C�X�ւ̉e��")]
    public string DiceInfluence;
    [Tooltip("�A�C�e��������")]
    public string ItemLost;
    [Tooltip("�����t���̃|�C���g�t�^")]
    public string ConditionalPoint;


}

