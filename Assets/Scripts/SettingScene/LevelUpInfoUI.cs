using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpInfoUI : MonoBehaviour
{
    public FighterEditUI FeUI;

    //レベルアップ後表示UI
    public GameObject LevelUpAfterUI;

    //Level数
    private int DefaultLevel;
    public int UpLevel;

    //必要資金
    private int NeedMoney;

    //各表示テキスト
    public Text NameText;
    public Text DefaultLevelText;
    public Text UpLevelText;
    public Text MoneyText;

    //レベルアップボタン
    public Button LvUpButton;

    //レベル下げボタン
    public GameObject MinusButton;

    void OnEnable()
    {
        //初期状態
        MinusButton.SetActive(false);
        UpLevel = 1;
        DefaultLevel = FeUI.SelectFighterStatus.Level;
        NeedMoney = ((DefaultLevel + UpLevel) / 5) + 1;

        //テキスト表示
        NameText.text = FeUI.SelectFighterStatus.FighterName;
        DefaultLevelText.text = "Lv" + DefaultLevel.ToString();
        UpLevelText.text = "Lv" + (DefaultLevel + UpLevel).ToString();
        MoneyText.text = "（必要資金: " + NeedMoney.ToString() + "両）";

        //必要資金に足りなければレベルアップボタンを押せないように
        if (NeedMoney > Common.Money)
        {
            LvUpButton.interactable = false;
        }
    }

    //→ボタンクリック
    public void PlusButtonClick()
    {
        //レベル増加
        UpLevel += 1;
        UpLevelText.text = "Lv" + (DefaultLevel + UpLevel).ToString();

        NeedMoney += ((DefaultLevel + UpLevel) / 5) + 1;
        MoneyText.text = "（必要資金: " + NeedMoney.ToString() + "両）";

        MinusButton.SetActive(true);

        //必要資金に足りなければレベルアップボタンを押せないように
        if (NeedMoney > Common.Money)
        {
            LvUpButton.interactable = false;
        }
    }

    //←ボタンクリック
    public void MinusButtonClick()
    {
        //レベル減少
        UpLevel -= 1;
        UpLevelText.text = "Lv" + (DefaultLevel + UpLevel).ToString();

        NeedMoney -= ((DefaultLevel + UpLevel + 1) / 5) + 1; 
        MoneyText.text = "（必要資金: " + NeedMoney.ToString() + "両）";

        if (UpLevel == 1)
        {
            MinusButton.SetActive(false);
        }

        //必要資金があればレベルアップボタンを押せるように
        if (NeedMoney <= Common.Money)
        {
            LvUpButton.interactable = true;
        }
    }

    //レベルアップボタンクリック
    public void LvUpButtonClick()
    {
        Common.Money -= NeedMoney;
        FeUI.SeManager.MoUI.TextWrite();

        LevelUpAfterUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //キャンセル
    public void CancelButtonClick()
    {
        this.gameObject.SetActive(false);
    }
}
