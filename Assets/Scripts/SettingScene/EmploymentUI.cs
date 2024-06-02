using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//�ٗp��UI
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
    public Button InfantryButton;
    public Button ArcherButton;
    public Button ShielderButton;
    public Button CavalryButton;

    //�I�����Ă��镺��
    public int SelectType;

    private void OnEnable()
    {
        PageClear();
        //�����{�^���������I����Ԃɂ���
        InfantryPage.SetActive(true);
        SelectType = 1;
        InfantryButton.GetComponent<Image>().color = Color.yellow;

        //�����̓X�e�[�W5�N���A�Ōٗp�ł���
        if (Common.Progress > 5)
        {
            ShielderButton.interactable = true;
        }
        else
        {
            ShielderButton.interactable = false;
        }

        //�R���̓X�e�[�W10�N���A�Ōٗp�ł���
        if (Common.Progress > 10)
        {
            CavalryButton.interactable = true;
        }
        else
        {
            CavalryButton.interactable = false;
        }
    }

    //�����{�^������
    public void InfantryButtonClick()
    {
        PageClear();
        InfantryPage.SetActive(true);
        SelectType = 1;
        InfantryButton.GetComponent<Image>().color = Color.yellow;
    }

    //�|���{�^������
    public void ArcherButtonClick()
    {
        PageClear();
        ArcherPage.SetActive(true);
        SelectType = 2;
        ArcherButton.GetComponent<Image>().color = Color.yellow;
    }

    //�����{�^������
    public void ShielderButtonClick()
    {
        PageClear();
        ShielderPage.SetActive(true);
        SelectType = 3;
        ShielderButton.GetComponent<Image>().color = Color.yellow;
    }

    //�R���{�^������
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

    //���m�ٗp
    public void EmployButtonClick()
    {
        //���m���Ə������`�F�b�N
        if(Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList.Count == 120)
        {
            WarningUI.SetActive(true);
            WarningText.text = "�ő�l���𒴂��邽��\n�V���Ɍٗp���ł��܂���B";
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
                return 4;
            default:
                return 0;
        }
    }
}
