using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//解雇UI
public class DismissalUI : MonoBehaviour
{
    public FighterEditUI FeUI;

    public Text InfoText;
    public Text MoneyText;

    private FighterStatus fs;
    private int GetMoney; //解雇したときに手に入るお金

    void OnEnable()
    {
        fs = FeUI.SelectFighterStatus;
        GetMoney = (fs.Level / 5) + 1;

        InfoText.text = fs.FighterName + "を解雇します。\nよろしいですか？";
        MoneyText.text = "（獲得資金: " + GetMoney.ToString() + "両）";
    }

    public void YesButtonClick()
    {
        //兵士データを削除
        FeUI.PlayerFighterDataBaseAllList.Remove(FeUI.SeManager.PlayerFighterTable.PlayerFighterDBList.Find((n) => n.Name == fs.FighterName));
        FeUI.SeManager.PlayerFighterTable.PlayerFighterDBList.Remove(FeUI.SeManager.PlayerFighterTable.PlayerFighterDBList.Find((n) => n.Name == fs.FighterName));

        //所持金プラス
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
