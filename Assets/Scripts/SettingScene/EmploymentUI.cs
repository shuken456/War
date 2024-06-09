using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//雇用所UI
public class EmploymentUI : MonoBehaviour
{
    public SettingManager SeManager;

    //説明UI
    public GameObject InfantryPage;
    public GameObject ArcherPage;
    public GameObject ShielderPage;
    public GameObject CavalryPage;

    //UI
    public GameObject EmployUI;
    public GameObject WarningUI;
    public Text WarningText;
    public Button InfantryButton;
    public Button ArcherButton;
    public Button ShielderButton;
    public Button CavalryButton;
    public GameObject ShielderHelpUI;
    public GameObject CavalryHelpUI;

    //選択している兵種
    public int SelectType;

    private void OnEnable()
    {
        PageClear();
        //歩兵ボタンを初期選択状態にする
        InfantryPage.SetActive(true);
        SelectType = 1;
        InfantryButton.GetComponent<Image>().color = Color.yellow;

        //盾兵はステージ5クリアで雇用できる
        if (Common.Progress > 5)
        {
            if(PlayerPrefs.GetInt("ShielderHelp", 0) == 0 && Common.Progress >= 6)
            {
                PlayerPrefs.SetInt("ShielderHelp", 1);
                ShielderHelpUI.SetActive(true);
            }
            ShielderButton.interactable = true;
        }
        else
        {
            ShielderHelpUI.SetActive(false);
            ShielderButton.interactable = false;
        }

        //騎兵はステージ10クリアで雇用できる
        if (Common.Progress > 10)
        {
            if (PlayerPrefs.GetInt("CavalryHelp", 0) == 0 && Common.Progress >= 11)
            {
                PlayerPrefs.SetInt("CavalryHelp", 1);
                CavalryHelpUI.SetActive(true);
            }
            CavalryButton.interactable = true;
        }
        else
        {
            CavalryHelpUI.SetActive(false);
            CavalryButton.interactable = false;
        }
    }

    //歩兵ボタン押下
    public void InfantryButtonClick()
    {
        PageClear();
        InfantryPage.SetActive(true);
        SelectType = 1;
        InfantryButton.GetComponent<Image>().color = Color.yellow;
    }

    //弓兵ボタン押下
    public void ArcherButtonClick()
    {
        PageClear();
        ArcherPage.SetActive(true);
        SelectType = 2;
        ArcherButton.GetComponent<Image>().color = Color.yellow;
    }

    //盾兵ボタン押下
    public void ShielderButtonClick()
    {
        PageClear();
        ShielderPage.SetActive(true);
        SelectType = 3;
        ShielderButton.GetComponent<Image>().color = Color.yellow;
    }

    //騎兵ボタン押下
    public void CavalryButtonClick()
    {
        PageClear();
        CavalryPage.SetActive(true);
        SelectType = 4;
        CavalryButton.GetComponent<Image>().color = Color.yellow;
    }

    private void PageClear()
    {
        InfantryPage.SetActive(false);
        ArcherPage.SetActive(false);
        ShielderPage.SetActive(false);
        CavalryPage.SetActive(false);

        InfantryButton.GetComponent<Image>().color = Color.white;
        ArcherButton.GetComponent<Image>().color = Color.white;
        ShielderButton.GetComponent<Image>().color = Color.white;
        CavalryButton.GetComponent<Image>().color = Color.white;
    }

    //兵士雇用
    public void EmployButtonClick()
    {
        //兵士数と所持金チェック
        if(SeManager.PlayerFighterTable.PlayerFighterDBList.Count == 120)
        {
            WarningUI.SetActive(true);
            WarningText.text = "最大人数を超えるため\n新たに雇用ができません。";
        }
        else if (Common.Money < FighterMoney(SelectType))
        {
            WarningUI.SetActive(true);
            WarningText.text = "資金が足りません。";
        }
        else
        {
            EmployUI.SetActive(true);
        }
    }

    public void BackButtonClick()
    {
        SeManager.HomeUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //兵種を雇う金額
    public int FighterMoney(int type)
    {
        switch (type)
        {
            case 1:
                return 2;
            case 2:
                return 2;
            case 3:
                return 3;
            case 4:
                return 4;
            default:
                return 0;
        }
    }
}
