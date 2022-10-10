using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditButton : MonoBehaviour
{
    [SerializeField]
    Sprite[] Pages;
    [SerializeField]
    GameObject CreditScreen;
    [SerializeField]
    Image Creditpage;
    [SerializeField]
    GameObject NextButton;
    [SerializeField]
    int nowpage;

    SEManager manager;

    private void Start()
    {
        nowpage = 0;
        manager = SEManager.Instance;
    }

    public void OnClick(string objectName)
    { // �I�u�W�F�N�g�̐����������𕪊�
        if ("Credit".Equals(objectName)) this.CreditClick();
        else if ("Back".Equals(objectName)) this.BackClick();
        else if ("Next".Equals(objectName)) this.NextClick();
        else if ("Cancel".Equals(objectName)) this.CancelClick();
        else throw new System.Exception("Not implemented!!");
    }

    public void CreditClick() {
        Resetall();
        CreditScreen.SetActive(true);
    }

    public void BackClick()
    {
        manager.SEsetandplay("MoveOn");

        if (nowpage > 0){
            nowpage--;
            Creditpage.sprite = Pages[nowpage];
        }
        else {
            CreditScreen.SetActive(false);
            nowpage = 0;
        }

        if (nowpage + 1 < Pages.Length) {
            NextButton.SetActive(true);
        }
    }

    public void NextClick() {
        manager.SEsetandplay("MoveOn");
        
        if (nowpage + 2 < Pages.Length)
        {
            nowpage++;
            Creditpage.sprite = Pages[nowpage];
        }
        else
        {
            nowpage++;
            Creditpage.sprite = Pages[nowpage];
            NextButton.SetActive(false);
        }
    }

    public void CancelClick()
    {
        manager.SEsetandplay("DecisionSE");
        CreditScreen.SetActive(false);
    }

    public void Resetall()
    {
        nowpage = 0;
        Creditpage.sprite = Pages[0];
        NextButton.SetActive(true);
    }
}