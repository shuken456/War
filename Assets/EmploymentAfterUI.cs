using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EmploymentAfterUI : MonoBehaviour
{
    //UI
    public EmploymentUI EmUI;
    public GameObject NameUI;
    public GameObject WarningUI;
    public MoneyUI MoUI;

    //���m�̊G
    public Image FighterImage;

    //���mDB
    public List<PlayerFighter> PlayerFighterDataBaseAllList;

    //�ǉ����镺�m
    PlayerFighter Fighter = new PlayerFighter();

    //�\������e�L�X�g
    public Text ParameterText;
    public Text InfoText;

    //���O���̓t�B�[���h
    public InputField NameField;

    void OnEnable()
    {
        //���m�̊G��ݒ�
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

        //DB�擾
        PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList;

        //���m�̔\�͐ݒ�
        Fighter = FighterParameter(EmUI.SelectType);

        //�\������e�L�X�g
        ParameterText.text = Fighter.Name + "\n" + Fighter.Level.ToString() + "\n" + Fighter.Hp.ToString() + "\n" + Fighter.Stamina.ToString() + "\n" + Fighter.AtkPower.ToString()
            + "\n" + Fighter.AtkSpeed.ToString() + "\n" + Fighter.MoveSpeed.ToString();
        InfoText.text = Fighter.Name + "���ق����I";
    }

    //�ق������m�̃p�����[�^�ݒ�
    private PlayerFighter FighterParameter(int type)
    {
        PlayerFighter NewFighter = new PlayerFighter();

        NewFighter.Type = type;
        NewFighter.Name = Common.FighterType(type) + (PlayerFighterDataBaseAllList.FindAll((n) => n.Type == type).Count + 1).ToString();

        //���O�����Ȃ��悤�ɂ���
        for (int i = 1; PlayerFighterDataBaseAllList.FindAll((n) => n.Name == NewFighter.Name).Count > 0; i++)
        {
            NewFighter.Name = Common.FighterType(type) + (PlayerFighterDataBaseAllList.FindAll((n) => n.Type == type).Count + 1 + i).ToString();
        }
            
        switch (type)
        {
            case 1:
                NewFighter.Hp = Random.Range(8, 13);
                NewFighter.Stamina = Random.Range(13, 18);
                NewFighter.AtkPower = Random.Range(2, 5);
                NewFighter.AtkSpeed = Random.Range(8, 13);
                NewFighter.MoveSpeed = Random.Range(8, 13);
                break;
            case 2:
                NewFighter.Hp = Random.Range(5, 9);
                NewFighter.Stamina = Random.Range(8, 13);
                NewFighter.AtkPower = Random.Range(2, 5);
                NewFighter.AtkSpeed = Random.Range(8, 13);
                NewFighter.MoveSpeed = Random.Range(8, 13);
                break;
            case 3:
                NewFighter.Hp = Random.Range(15, 20);
                NewFighter.Stamina = Random.Range(8, 13);
                NewFighter.AtkPower = Random.Range(1, 4);
                NewFighter.AtkSpeed = Random.Range(6, 10);
                NewFighter.MoveSpeed = Random.Range(6, 10);
                break;
            case 4:
                NewFighter.Hp = Random.Range(8, 13);
                NewFighter.Stamina = Random.Range(8, 13);
                NewFighter.AtkPower = Random.Range(2, 5);
                NewFighter.AtkSpeed = Random.Range(8, 13);
                NewFighter.MoveSpeed = Random.Range(13, 18);
                break;
        }
        NewFighter.Level = 1;
        NewFighter.EXP = 0;
        NewFighter.UnitNum = 0;
        NewFighter.UnitLeader = false;

        return NewFighter;
    }

    public void EndButtonClick()
    {
        //���m��ǉ�
        PlayerFighterDataBaseAllList.Add(Fighter);
        //��������
        Common.Money -= EmUI.FighterMoney(EmUI.SelectType);
        MoUI.TextWrite();

        this.gameObject.SetActive(false);
    }

    public void NameButtonClick()
    {
        NameUI.SetActive(true);

        NameField.text = Fighter.Name;
        NameField.ActivateInputField();
    }

    //����{�^�������ŕ��m���ύX
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
            //�\������e�L�X�g
            ParameterText.text = Fighter.Name + "\n" + Fighter.Level.ToString() + "\n" + Fighter.Hp.ToString() + "\n" + Fighter.Stamina.ToString() + "\n" + Fighter.AtkPower.ToString()
                + "\n" + Fighter.AtkSpeed.ToString() + "\n" + Fighter.MoveSpeed.ToString();
            InfoText.text = Fighter.Name + "���ق����I";
        }
    }
    public void WarningOkButtonClick()
    {
        WarningUI.SetActive(false);
    }
}
