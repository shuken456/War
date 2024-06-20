using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpInfoUI : MonoBehaviour
{
    public FighterEditUI FeUI;

    //���x���A�b�v��\��UI
    public GameObject LevelUpAfterUI;

    //Level��
    private int DefaultLevel;
    public int UpLevel;

    //�K�v����
    private int NeedMoney;

    //�e�\���e�L�X�g
    public Text NameText;
    public Text DefaultLevelText;
    public Text UpLevelText;
    public Text MoneyText;

    //���x���A�b�v�{�^��
    public Button LvUpButton;

    //���x�������{�^��
    public GameObject MinusButton;

    void OnEnable()
    {
        //�������
        MinusButton.SetActive(false);
        UpLevel = 1;
        DefaultLevel = FeUI.SelectFighterStatus.Level;
        NeedMoney = ((DefaultLevel + UpLevel) / 5) + 1;

        //�e�L�X�g�\��
        NameText.text = FeUI.SelectFighterStatus.FighterName;
        DefaultLevelText.text = "Lv" + DefaultLevel.ToString();
        UpLevelText.text = "Lv" + (DefaultLevel + UpLevel).ToString();
        MoneyText.text = "�i�K�v����: " + NeedMoney.ToString() + "���j";

        //�K�v�����ɑ���Ȃ���΃��x���A�b�v�{�^���������Ȃ��悤��
        if (NeedMoney > Common.Money)
        {
            LvUpButton.interactable = false;
        }
    }

    //���{�^���N���b�N
    public void PlusButtonClick()
    {
        //���x������
        UpLevel += 1;
        UpLevelText.text = "Lv" + (DefaultLevel + UpLevel).ToString();

        NeedMoney += ((DefaultLevel + UpLevel) / 5) + 1;
        MoneyText.text = "�i�K�v����: " + NeedMoney.ToString() + "���j";

        MinusButton.SetActive(true);

        //�K�v�����ɑ���Ȃ���΃��x���A�b�v�{�^���������Ȃ��悤��
        if (NeedMoney > Common.Money)
        {
            LvUpButton.interactable = false;
        }
    }

    //���{�^���N���b�N
    public void MinusButtonClick()
    {
        //���x������
        UpLevel -= 1;
        UpLevelText.text = "Lv" + (DefaultLevel + UpLevel).ToString();

        NeedMoney -= ((DefaultLevel + UpLevel + 1) / 5) + 1; 
        MoneyText.text = "�i�K�v����: " + NeedMoney.ToString() + "���j";

        if (UpLevel == 1)
        {
            MinusButton.SetActive(false);
        }

        //�K�v����������΃��x���A�b�v�{�^����������悤��
        if (NeedMoney <= Common.Money)
        {
            LvUpButton.interactable = true;
        }
    }

    //���x���A�b�v�{�^���N���b�N
    public void LvUpButtonClick()
    {
        Common.Money -= NeedMoney;
        FeUI.SeManager.MoUI.TextWrite();

        LevelUpAfterUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //�L�����Z��
    public void CancelButtonClick()
    {
        this.gameObject.SetActive(false);
    }
}
