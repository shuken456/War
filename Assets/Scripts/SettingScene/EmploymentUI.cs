using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmploymentUI : MonoBehaviour
{
    public SettingManager SeManager;

    //����UI
    public GameObject InfantryPage;
    public GameObject ArcherPage;
    public GameObject ShielderPage;
    public GameObject CavalryPage;

    //UI
    public GameObject EmployUI;
    public GameObject WarningUI;
    public Text WarningText;

    //�I�����Ă��镺��
    public int SelectType = 1;

    public void InfantryButtonClick()
    {
        PageClear();
        InfantryPage.SetActive(true);
        SelectType = 1;
    }

    public void ArcherButtonClick()
    {
        PageClear();
        ArcherPage.SetActive(true);
        SelectType = 2;
    }

    public void ShielderButtonClick()
    {
        PageClear();
        ShielderPage.SetActive(true);
        SelectType = 3;
    }

    public void CavalryButtonClick()
    {
        PageClear();
        CavalryPage.SetActive(true);
        SelectType = 4;
    }

    private void PageClear()
    {
        InfantryPage.SetActive(false);
        ArcherPage.SetActive(false);
        ShielderPage.SetActive(false);
        CavalryPage.SetActive(false);
    }

    public void EmployButtonClick()
    {
        if(Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList.Count == 120)
        {
            WarningUI.SetActive(true);
            WarningText.text = "�ő�l���𒴂��邽��\n���炽�ɂ��悤���ł��܂���B";
        }
        else if (Common.Money < FighterMoney(SelectType))
        {
            WarningUI.SetActive(true);
            WarningText.text = "����������܂���B";
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

    public void WarningOkButtonClick()
    {
        WarningUI.SetActive(false);
    }

    //������ق����z
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
                return 3;
            default:
                return 0;
        }
    }
}
