using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//雇用確定後表示するUI
public class EmploymentAfterUI : MonoBehaviour
{
    //UI
    public EmploymentUI EmUI;
    public GameObject NameUI;
    public GameObject WarningUI;
    public Button OneMoreButton;

    //兵士の絵
    public Image FighterImage;

    //兵士DB
    public List<PlayerFighter> PlayerFighterDataBaseAllList;

    //追加する兵士
    PlayerFighter Fighter = new PlayerFighter();

    //表示するテキスト
    public Text ParameterText;
    public Text InfoText;

    //名前入力フィールド
    public InputField NameField;

    void OnEnable()
    {
        //兵士の絵を設定
        switch (EmUI.SelectType)
        {
            case 1:
                FighterImage.overrideSprite = EmUI.InfantryPage.transform.Find("FighterBackImage/FighterImage").GetComponent<Image>().sprite;
                break;
            case 2:
                FighterImage.overrideSprite = EmUI.ArcherPage.transform.Find("FighterBackImage/FighterImage").GetComponent<Image>().sprite;
                break;
            case 3:
                FighterImage.overrideSprite = EmUI.ShielderPage.transform.Find("FighterBackImage/FighterImage").GetComponent<Image>().sprite;
                break;
            case 4:
                FighterImage.overrideSprite = EmUI.CavalryPage.transform.Find("FighterBackImage/FighterImage").GetComponent<Image>().sprite;
                break;
        }

        //DB取得
        PlayerFighterDataBaseAllList = EmUI.SeManager.PlayerFighterTable.PlayerFighterDBList;

        //兵士の能力設定
        Fighter = FighterParameter(EmUI.SelectType);

        //表示するテキスト
        ParameterText.text = Fighter.Name + "\n" + Fighter.Level.ToString() + "\n" + Fighter.Hp.ToString() + "\n" + Fighter.Stamina.ToString() + "\n" + Fighter.AtkPower.ToString()
            + "\n" + Fighter.AtkSpeed.ToString() + "\n" + Fighter.MoveSpeed.ToString();
        InfoText.text = Fighter.Name + "を雇った！";

        //兵士数と所持金チェック もう一人雇えるかチェック
        if (PlayerFighterDataBaseAllList.Count == 119 || Common.Money < EmUI.FighterMoney(EmUI.SelectType) * 2)
        {
            OneMoreButton.interactable = false;
        }
        else
        {
            OneMoreButton.interactable = true;
        }
    }

    //雇った兵士のパラメータ設定
    private PlayerFighter FighterParameter(int type)
    {
        PlayerFighter NewFighter = new PlayerFighter();

        NewFighter.Type = type;
        NewFighter.Name = Common.FighterType(type) + (PlayerFighterDataBaseAllList.FindAll((n) => n.Type == type).Count + 1).ToString();

        //名前が被らないようにする
        for (int i = 1; PlayerFighterDataBaseAllList.FindAll((n) => n.Name == NewFighter.Name).Count > 0; i++)
        {
            NewFighter.Name = Common.FighterType(type) + (PlayerFighterDataBaseAllList.FindAll((n) => n.Type == type).Count + 1 + i).ToString();
        }
            
        switch (type)
        {
            case 1:
                NewFighter.Hp = Random.Range(8, 13);
                NewFighter.Stamina = Random.Range(13, 18);
                NewFighter.AtkPower = Random.Range(2, 4);
                NewFighter.AtkSpeed = Random.Range(8, 13);
                NewFighter.MoveSpeed = Random.Range(8, 13);
                break;
            case 2:
                NewFighter.Hp = Random.Range(5, 9);
                NewFighter.Stamina = Random.Range(8, 13);
                NewFighter.AtkPower = Random.Range(2, 4);
                NewFighter.AtkSpeed = Random.Range(6, 10);
                NewFighter.MoveSpeed = Random.Range(8, 13);
                break;
            case 3:
                NewFighter.Hp = Random.Range(17, 22);
                NewFighter.Stamina = Random.Range(8, 13);
                NewFighter.AtkPower = Random.Range(1, 3);
                NewFighter.AtkSpeed = Random.Range(6, 10);
                NewFighter.MoveSpeed = Random.Range(6, 10);
                break;
            case 4:
                NewFighter.Hp = Random.Range(8, 13);
                NewFighter.Stamina = Random.Range(8, 13);
                NewFighter.AtkPower = Random.Range(2, 4);
                NewFighter.AtkSpeed = Random.Range(8, 13);
                NewFighter.MoveSpeed = Random.Range(13, 18);
                break;
        }
        NewFighter.EXP = 0;
        NewFighter.UnitNum = 0;
        NewFighter.UnitLeader = false;

        //現在の進行度によってレベルをランダムに設定
        NewFighter.Level = Random.Range(1, Common.Progress);
        Dictionary<string, int> UpParameter = Common.LevelUpParameter(NewFighter.Type, NewFighter.Level - 1);
        NewFighter.Hp += UpParameter["Hp"];
        NewFighter.Stamina += UpParameter["Stamina"];
        NewFighter.AtkPower += UpParameter["AtkPower"];
        NewFighter.AtkSpeed += UpParameter["AtkSpeed"];
        NewFighter.MoveSpeed += UpParameter["MoveSpeed"];

        return NewFighter;
    }

    //もう1人雇うボタン押下
    public void OneMoreButtonClick()
    {
        //兵士を追加
        PlayerFighterDataBaseAllList.Add(Fighter);
        //資金減少
        Common.Money -= EmUI.FighterMoney(EmUI.SelectType);
        EmUI.SeManager.MoUI.TextWrite();
        OnEnable();
    }

    //終了ボタン押下
    public void EndButtonClick()
    {
        //兵士を追加
        PlayerFighterDataBaseAllList.Add(Fighter);
        //資金減少
        Common.Money -= EmUI.FighterMoney(EmUI.SelectType);
        EmUI.SeManager.MoUI.TextWrite();

        this.gameObject.SetActive(false);
    }

    //名前変更ボタン押下
    public void NameButtonClick()
    {
        NameUI.SetActive(true);

        NameField.text = Fighter.Name;
        NameField.ActivateInputField();
    }

    //決定ボタン押下で兵士名変更
    public void DecisionName()
    {
        if(PlayerFighterDataBaseAllList.FindAll((n) => n.Name == NameField.text).Count > 0)
        {
            WarningUI.SetActive(true);
        }
        else
        {
            Fighter.Name = NameField.text;
            NameUI.SetActive(false);
            //表示するテキスト
            ParameterText.text = Fighter.Name + "\n" + Fighter.Level.ToString() + "\n" + Fighter.Hp.ToString() + "\n" + Fighter.Stamina.ToString() + "\n" + Fighter.AtkPower.ToString()
                + "\n" + Fighter.AtkSpeed.ToString() + "\n" + Fighter.MoveSpeed.ToString();
            InfoText.text = Fighter.Name + "を雇った！";
        }
    }
}
