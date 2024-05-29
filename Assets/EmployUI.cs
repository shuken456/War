using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployUI : MonoBehaviour
{
    //�eUI
    public EmploymentUI EmUI;
    public GameObject EmploymentAfterUI;

    //�\������e�L�X�g
    public Text InfoText;
    public Text MoneyText;

    // Start is called before the first frame update
    void Start()
    {
        InfoText.text = Common.FighterType(EmUI.SelectType) + "���ق��܂��B\n��낵���ł����H";
        MoneyText.text = "�i�K�v����:"+ EmUI.FighterMoney(EmUI.SelectType)+"���j";
    }

    public void YesButtonClick()
    {
        this.gameObject.SetActive(false);
        EmploymentAfterUI.SetActive(true);
    }

    public void NoButtonClick()
    {
        this.gameObject.SetActive(false);
    }
}
