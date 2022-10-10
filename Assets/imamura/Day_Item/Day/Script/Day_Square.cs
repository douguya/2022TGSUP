using System;
using UnityEngine;
using UnityEngine.Video;


[Serializable]

[CreateAssetMenu(fileName = "Day_Square", menuName = "Day_Square")]
public class Day_Square : ScriptableObject
{





    [Tooltip("�L�O���̖��O")]
    public string Anniversary;
    [Tooltip("�L�O���̓��t")]
    public string Day;
    [Tooltip("���o�A�j���[�V����")]
    public VideoClip Staging;    
    [Tooltip("�z�b�v�A�b�v�摜")]
    public Sprite HopUp;   �@ 
    [Tooltip("���t�̐���")][Multiline(7)]
    public string Commentary;
    
    


    [Header("")]
    [Header("���t�ɊY��������ʂ��Ȃ��ꍇ��Noon�Ɠ��͂���")]
    [Header("���t���ʁA�Y�����镨����������")]
      



    [Tooltip("�A�C�e���̃I�u�W�F�N�g")]
    public Anniversary_Item Anniversary_Item;�@�@ �@�@�@ 
    [Tooltip("���̏u�Ԃ̈ړ�")]
    public string Move;�@�@�@ �@   
    [Tooltip("�ύX����BGM")]
    public string BGM;             
    [Tooltip("�o������I�u�W�F�N�g")]
    public string Instance;
    [Tooltip("�A�C�R���̕ύX")]
    public Sprite Icon;
    [Tooltip("�p�[�e�B�N���ɂ��񎟉��o")]
    public string particle;        
    [Tooltip("���̎����̃^�[���ł̃_�C�X")]
    public string NextDice;        
    [Tooltip("���̎����̃^�[���ł̈ړ�")]
    public string NextMove;        
    [Tooltip("�A�C�e��������")]
    public string ItemLost;        
    [Tooltip("�����t���̃|�C���g�t�^")]
    public string ConditionalPoint;
    [Tooltip("���̑��̌���")]
    public string�@OtherEffects;

}
