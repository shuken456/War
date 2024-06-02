using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//兵士一覧からのレベルアップ後のパラメータ表示UI
public class LevelUpAfterUI : MonoBehaviour
{
    public LevelUpInfoUI LuUI;

    public Text InfoText;
    public Text ParameterText;
    public Image FighterImage;

    private FighterStatus fs;

    private void OnEnable()
    {
        //レベルアップ後の兵士パラメータを表示
        FighterImage.sprite = LuUI.FeUI.FighterStatusInfo.transform.Find("FighterBack/FighterImage").GetComponent<Image>().sprite;
        FighterImage.color = LuUI.FeUI.FighterStatusInfo.transform.Find("FighterBack/FighterImage").GetComponent<Image>().color;

        fs = LuUI.FeUI.SelectFighterStatus;
        InfoText.text = fs.FighterName + "のレベルが上がった！";

        //上がるパラメータリスト
        Dictionary<string, int> UpParameter = Common.LevelUpParameter(LuUI.FeUI.SelectFighterStatus.Type, LuUI.UpLevel);

        ParameterText.text = fs.Level.ToString() + "→" + (fs.Level + LuUI.UpLevel).ToString() + "\n"
            + fs.MaxHp.ToString() + "→" + (fs.MaxHp + UpParameter["Hp"]).ToString() + "\n"
            + fs.MaxStamina.ToString() + "→" + (fs.MaxStamina + UpParameter["Stamina"]).ToString() + "\n"
            + fs.AtkPower.ToString() + "→" + (fs.AtkPower + UpParameter["AtkPower"]).ToString() + "\n"
            + fs.AtkSpeed.ToString() + "→" + (fs.AtkSpeed + UpParameter["AtkSpeed"]).ToString() + "\n"
            + fs.MoveSpeed.ToString() + "→" + (fs.MoveSpeed + UpParameter["MoveSpeed"]).ToString();

        PlayerFighter pf = LuUI.FeUI.PlayerFighterDataBaseAllList.Find((n) => n.Name == fs.FighterName);
        pf.Level += LuUI.UpLevel;
        pf.Hp += UpParameter["Hp"];
        pf.Stamina += UpParameter["Stamina"];
        pf.AtkPower += UpParameter["AtkPower"];
        pf.AtkSpeed += UpParameter["AtkSpeed"];
        pf.MoveSpeed += UpParameter["MoveSpeed"];

        fs.Level += LuUI.UpLevel;
        fs.MaxHp += UpParameter["Hp"];
        fs.MaxStamina += UpParameter["Stamina"];
        fs.AtkPower += UpParameter["AtkPower"];
        fs.AtkSpeed += UpParameter["AtkSpeed"];
        fs.MoveSpeed += UpParameter["MoveSpeed"];
    }

    public void OkButtonClick()
    {
        LuUI.FeUI.FighterStatusInfo.TextWrite(fs);
        LuUI.FeUI.FighterViewDisplay();
        this.gameObject.SetActive(false);
    }
}
