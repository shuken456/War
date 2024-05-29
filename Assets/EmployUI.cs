using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployUI : MonoBehaviour
{
    //各UI
    public EmploymentUI EmUI;
    public GameObject EmploymentAfterUI;

    //表示するテキスト
    public Text InfoText;
    public Text MoneyText;

    // Start is called before the first frame update
    void Start()
    {
        InfoText.text = Common.FighterType(EmUI.SelectType) + "を雇います。\nよろしいですか？";
        MoneyText.text = "（必要資金:"+ EmUI.FighterMoney(EmUI.SelectType)+"両）";
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
