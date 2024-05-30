using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpInfoUI : MonoBehaviour
{
    public FighterEditUI FeUI;

    //Level数
    private int DefaultLevel;
    private int UpLevel;

    //必要資金
    private int NeedMoney;

    //各表示テキスト
    public Text NameText;
    public Text DefaultLevelText;
    public Text UpLevelText;
    public Text MoneyText;

    //レベル下げボタン
    public GameObject MinusButton;

    void OnEnable()
    {
        MinusButton.SetActive(false);

        UpLevel = 1;
        DefaultLevel = FeUI.SelectFighterStatus.Level;
        NeedMoney = ((DefaultLevel + UpLevel) / 5) + 1;

        NameText.text = FeUI.SelectFighterStatus.FighterName;
        DefaultLevelText.text = "Lv" + DefaultLevel.ToString();
        UpLevelText.text = "Lv" + (DefaultLevel + UpLevel).ToString();
        MoneyText.text = "（必要資金: " + NeedMoney.ToString() + "両）";
    }

    public void PlusButtonClick()
    {
        UpLevel += 1;
        UpLevelText.text = "Lv" + (DefaultLevel + UpLevel).ToString();

        NeedMoney += ((DefaultLevel + UpLevel) / 5) + 1;
        MoneyText.text = "（必要資金: " + NeedMoney.ToString() + "両）";

        MinusButton.SetActive(true);
    }

    public void MinusButtonClick()
    {
        UpLevel -= 1;
        UpLevelText.text = "Lv" + (DefaultLevel + UpLevel).ToString();

        NeedMoney -= ((DefaultLevel + UpLevel + 1) / 5) + 1;
        MoneyText.text = "（必要資金: " + NeedMoney.ToString() + "両）";

        if (UpLevel == 1)
        {
            MinusButton.SetActive(false);
        }
    }

    public void CancelButtonClick()
    {
        this.gameObject.SetActive(false);
    }
}
