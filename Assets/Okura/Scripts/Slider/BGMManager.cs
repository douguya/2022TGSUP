using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private AudioSource audiosource;
    musiclist Music;
    [SerializeField]
    string BGMname;

    //�V�[���J�ڂ��Ă����v
    public static BGMManager Instance
    {
        get; private set;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //������
    private void Start()
    {
        audiosource = this.GetComponent<AudioSource>();
        Music = this.GetComponent<musiclist>();
        audiosource.volume = PlayerPrefs.GetFloat("BGMValue", 0.434f);
        Debug.Log(PlayerPrefs.GetFloat("BGMValue", 999.9f));

        BGMsetandplay(BGMname);
    }


    //�X���C�_�[�ŉ��ʒ���
    public void BGMSlider(float Value)
    {
        audiosource.volume = Db2Pa(Value);
        PlayerPrefs.SetFloat("BGMValue", Db2Pa(Value));
        PlayerPrefs.Save();
    }

    //�f�V�x�����特���ɕϊ�
    private float Db2Pa(float db)
    {
        db = Mathf.Clamp(db, -80f, 20f);
        return Mathf.Pow(10f, db / 20f);
    }

    //BGM��炷�@�\
    public void BGMsetandplay(string BGMname) {
        switch (BGMname)
        {
            case "TitleBGM": audiosource.clip = Music.TitleBGM; break;
            case "LobbyBGM": audiosource.clip = Music.LobbyBGM; break;
            case "IngameBGM": audiosource.clip = Music.IngameBGM; break;
            case "ResultBGM": audiosource.clip = Music.ResultBGM; break;
            case "���̓�": audiosource.clip = Music.EnvironmentBGM; break;
            case "���۔M�уf�[": audiosource.clip = Music.JangleBGM; break;
            case "�s�A�m�̓�": audiosource.clip = Music.PianoBGM; break;
            case "�I�J���g�L�O��": audiosource.clip = Music.HorrorBGM; break;

            case "DecisionSE": audiosource.clip = Music.DecisionSE; break;
            case "MoveOn": audiosource.clip = Music.MoveOn; break;
            case "BackSE": audiosource.clip = Music.BackSE; break;
            case "SceneSwitching": audiosource.clip = Music.SceneSwitching; break;
            case "LogSE": audiosource.clip = Music.LogSE; break;

            case "GameStartSE": audiosource.clip = Music.GameStartSE; break;
            case "DiceSE": audiosource.clip = Music.DiceSE; break;
            case "WalkSE": audiosource.clip = Music.WalkSE; break;
            case "WarpSE": audiosource.clip = Music.WarpSE; break;
            case "OpenSE": audiosource.clip = Music.OpenSE; break;
            case "GetSE": audiosource.clip = Music.GetSE; break;
            case "GoalSE": audiosource.clip = Music.GoalSE; break;
            case "GameEndSE": audiosource.clip = Music.GameEndSE; break;
            case "TextSE": audiosource.clip = Music.TextSE; break;

            case "resultSE": audiosource.clip = Music.resultSE; break;

            case "HandClapJingle": audiosource.clip = Music.HandClapJingle; break;
            case "ResultJingle": audiosource.clip = Music.ResultJingle; break;
            case "FinalResultJingle": audiosource.clip = Music.FinalResultJingle; break;
        }

        audiosource.Play();
    }
}
