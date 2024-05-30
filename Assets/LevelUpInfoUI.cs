using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpInfoUI : MonoBehaviour
{
    public FighterEditUI FeUI;

    //Level��
    private int DefaultLevel;
    private int UpLevel;

    //�K�v����
    private int NeedMoney;

    //�e�\���e�L�X�g
    public Text NameText;
    public Text DefaultLevelText;
    public Text UpLevelText;
    public Text MoneyText;

    //���x�������{�^��
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
        MoneyText.text = "�i�K�v����: " + NeedMoney.ToString() + "���j";
    }

    public void PlusButtonClick()
    {
        UpLevel += 1;
        UpLevelText.text = "Lv" + (DefaultLevel + UpLevel).ToString();

        NeedMoney += ((DefaultLevel + UpLevel) / 5) + 1;
        MoneyText.text = "�i�K�v����: " + NeedMoney.ToString() + "���j";

        MinusButton.SetActive(true);
    }

    public void MinusButtonClick()
    {
        UpLevel -= 1;
        UpLevelText.text = "Lv" + (DefaultLevel + UpLevel).ToString();

        NeedMoney -= ((DefaultLevel + UpLevel + 1) / 5) + 1;
        MoneyText.text = "�i�K�v����: " + NeedMoney.ToString() + "���j";

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
