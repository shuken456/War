using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//�ٗp�m���\������UI
public class EmploymentAfterUI : MonoBehaviour
{
    //UI
    public EmploymentUI EmUI;
    public GameObject NameUI;
    public GameObject WarningUI;
    public Button OneMoreButton;

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
        PlayerFighterDataBaseAllList = EmUI.SeManager.PlayerFighterTable.PlayerFighterDBList;

        //���m�̔\�͐ݒ�
        Fighter = FighterParameter(EmUI.SelectType);

        //�\������e�L�X�g
        ParameterText.text = Fighter.Name + "\n" + Fighter.Level.ToString() + "\n" + Fighter.Hp.ToString() + "\n" + Fighter.Stamina.ToString() + "\n" + Fighter.AtkPower.ToString()
            + "\n" + Fighter.AtkSpeed.ToString() + "\n" + Fighter.MoveSpeed.ToString();
        InfoText.text = Fighter.Name + "���ق����I";

        //���m���Ə������`�F�b�N ������l�ق��邩�`�F�b�N
        if (PlayerFighterDataBaseAllList.Count == 119 || Common.Money < EmUI.FighterMoney(EmUI.SelectType) * 2)
        {
            OneMoreButton.interactable = false;
        }
        else
        {
            OneMoreButton.interactable = true;
        }
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

        //���݂̐i�s�x�ɂ���ă��x���������_���ɐݒ�
        NewFighter.Level = Random.Range(1, Common.Progress);
        Dictionary<string, int> UpParameter = Common.LevelUpParameter(NewFighter.Type, NewFighter.Level - 1);
        NewFighter.Hp += UpParameter["Hp"];
        NewFighter.Stamina += UpParameter["Stamina"];
        NewFighter.AtkPower += UpParameter["AtkPower"];
        NewFighter.AtkSpeed += UpParameter["AtkSpeed"];
        NewFighter.MoveSpeed += UpParameter["MoveSpeed"];

        return NewFighter;
    }

    //����1�l�ق��{�^������
    public void OneMoreButtonClick()
    {
        //���m��ǉ�
        PlayerFighterDataBaseAllList.Add(Fighter);
        //��������
        Common.Money -= EmUI.FighterMoney(EmUI.SelectType);
        EmUI.SeManager.MoUI.TextWrite();
        OnEnable();
    }

    //�I���{�^������
    public void EndButtonClick()
    {
        //���m��ǉ�
        PlayerFighterDataBaseAllList.Add(Fighter);
        //��������
        Common.Money -= EmUI.FighterMoney(EmUI.SelectType);
        EmUI.SeManager.MoUI.TextWrite();

        this.gameObject.SetActive(false);
    }

    //���O�ύX�{�^������
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
}
