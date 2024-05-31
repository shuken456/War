using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DismissalUI : MonoBehaviour
{
    public FighterEditUI FeUI;

    public Text InfoText;
    public Text MoneyText;

    private FighterStatus fs;
    private int GetMoney;

    void OnEnable()
    {
        fs = FeUI.SelectFighterStatus;
        GetMoney = (fs.Level / 3) + 1;

        InfoText.text = fs.FighterName + "�����ق��܂��B\n��낵���ł����H";
        MoneyText.text = "�i�l������: " + GetMoney.ToString() + "���j";
    }

    public void YesButtonClick()
    {
        FeUI.PlayerFighterDataBaseAllList.Remove(Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList.Find((n) => n.Name == fs.FighterName));
        Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList.Remove(Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList.Find((n) => n.Name == fs.FighterName));

        Common.Money += GetMoney;

        FeUI.ButtonClickChange(false);
        FeUI.SeManager.MoUI.TextWrite();
        FeUI.FighterStatusInfo.Clear(fs.FighterName);
        FeUI.FighterViewDisplay();
        this.gameObject.SetActive(false);
    }

    public void NoButtonClick()
    {
        this.gameObject.SetActive(false);
    }
}
