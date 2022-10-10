using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anniva : MonoBehaviour
{
    [SerializeField]
    string Itemname;
    [SerializeField]
    GameObject Instance;
    [SerializeField]
    float InstanceyValue;
    [SerializeField]
    BGMManager BGM;

    PlayerStatasIMamura Playerstatus;

    private void Awake()
    {
        Playerstatus = GetComponent<PlayerStatasIMamura>();
        BGM = GameObject.Find("BGM").GetComponent<BGMManager>();
    }

    //��{�ƂȂ�A�A�C�e���݂̂��l������^�C�v�̋L�O��
    public void GetItemandPoint() {
        Playerstatus.ItemobtainToRPC(Itemname);
    }

    //Instance���o�Ă�����
    public void AppearInstances()
    {
        StartCoroutine(Instance.GetComponent<AnimationController>().StartAnimation(InstanceyValue));
    }

    //BGM���ς����
    public void BGMChange() {
        BGM.BGMsetandplay(Itemname);
    }
}
