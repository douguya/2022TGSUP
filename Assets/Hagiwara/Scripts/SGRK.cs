using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGRK : MonoBehaviour
{
    public GameObject BaseMass;//��ԍ����ray���΂����߂̃I�u�W�F�N�g�̎擾

    public ChildArray[] tate;
    void Start()
    {
        Debug.Log(tate.Length);
        Debug.Log(tate[0].yoko.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[System.Serializable]
public class ChildArray
{
    public int[] yoko;
}
