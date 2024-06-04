using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

//���m�ꗗUI
public class FighterEditUI : MonoBehaviour
{
    public SettingManager SeManager;
    [SerializeField] private EventSystem eventSystem;

    //�eUI
    public GameObject NameUI;
    public GameObject NameWarningUI;
    public GameObject LevelUpUI;
    public GameObject DismissalUI;

    //���O���̓t�B�[���h
    public InputField NameField;

    //���j�b�gDB�ƕ��mDB
    public List<PlayerUnit> PlayerUnitDataBaseAllList;
    public List<PlayerFighter> PlayerFighterDataBaseAllList;

    //���mUI �X�N���[���r���[
    public GameObject FighterView;

    //���mUI�\���p�{�^���v���n�u
    public GameObject FighterButton;

    //���m�̊G
    public Sprite InfantryImage;
    public Sprite ArcherImage;

    //�����e�L�X�g
    public Text CountText;

    //���m�X�e�[�^�XUI
    public FighterStatusInfoUI FighterStatusInfo;

    //���בւ��{�^��
    public Toggle UnitOrderby;
    public Toggle NameOrderby;
    public Toggle LevelOrderby;

    //���m��I�����Ȃ��Ɖ����Ȃ��{�^��
    public Button NameChangeButton;
    public Button LevelUpButton;
    public Button DismissalButton;

    //�N���b�N���ꂽ���m�{�^���ƃX�e�[�^�X
    public GameObject SelectFighterButton;
    public FighterStatus SelectFighterStatus;

    //�{�^������SE
    public AudioSource SE;

    void OnEnable()
    {
        //DB�f�[�^�擾
        PlayerUnitDataBaseAllList = Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //���j�b�g�ԍ����ɕ��ёւ�
        PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList
            .OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ThenBy((n) => n.Type).ToList(); //�����ԍ����A����������ɗ���悤�ɁA���폇�ɕ��ёւ�

        FighterViewDisplay();
    }

    private void OnDisable()
    {
        //������
        //���בւ��������\����
        NameOrderby.isOn = false;
        LevelOrderby.isOn = false;
        UnitOrderby.isOn = true;

        //�e�{�^���������Ȃ��悤�ɂ���
        ButtonClickChange(false);

        //�X�e�[�^�XUI���󔒂ɂ���
        if(SelectFighterStatus)
        {
            FighterStatusInfo.Clear(SelectFighterStatus.FighterName);
        }

        SelectFighterStatus = null;
        SelectFighterButton = null;
    }

    //���m�r���[�\��
    public void FighterViewDisplay()
    {
        //���m�r���[���Z�b�g
        foreach (Transform f in FighterView.transform)
        {
            GameObject.Destroy(f.gameObject);
        }

        CountText.text = PlayerFighterDataBaseAllList.Count.ToString() + "/120";

        //�X�N���[���o�[���K�v���ۂ��ŁA�{�^���̈ʒu����
        if (PlayerFighterDataBaseAllList.Count >= 10)
        {
            FighterView.GetComponent<VerticalLayoutGroup>().padding.right = 20;
        }
        else
        {
            FighterView.GetComponent<VerticalLayoutGroup>().padding.right = 40;
        }

        //���m�̐����A�{�^�����쐬
        foreach (PlayerFighter Fighter in PlayerFighterDataBaseAllList)
        {
            GameObject button = Instantiate(FighterButton, FighterView.transform);
            button.transform.Find("Text (Name)").GetComponent<Text>().text = Fighter.Name;

            if (Fighter.UnitNum == 0)
            {
                button.transform.Find("Text (UnitName)").GetComponent<Text>().text = "�Ȃ�";
            }
            else
            {
                button.transform.Find("Text (UnitName)").GetComponent<Text>().text = PlayerUnitDataBaseAllList[Fighter.UnitNum - 1].Name;
            }

            button.transform.Find("Text (Type)").GetComponent<Text>().text = Common.FighterType(Fighter.Type);
            button.transform.Find("Text (Level)").GetComponent<Text>().text = Fighter.Level.ToString();

            //�쐬�����{�^���ɕ��m�X�e�[�^�X������
            Common.GetFighterStatusFromDB(button.GetComponent<FighterStatus>(), Fighter);

            button.GetComponent<Button>().onClick.AddListener(FighterButtonClick);

            //�I������Ă��镺�m�̃{�^���͉����Ȃ��悤�ɂ���
            if(Fighter.Name == FighterStatusInfo.transform.Find("StatusTexts/Text (Name)").GetComponent<Text>().text)
            {
                button.GetComponent<Button>().interactable = false;
                SelectFighterButton = button;
            }
        }
    }

    //���m��I�����Ă�Ԃ���������{�^���̏�ԊǗ�
    public void ButtonClickChange(bool b)
    {
        NameChangeButton.interactable = b;
        LevelUpButton.interactable = b;
        DismissalButton.interactable = b;
    }

    //���m�̃{�^���N���b�N�C�x���g
    public void FighterButtonClick()
    {
        SE.Play();

        //�e�{�^����������悤�ɂ���
        ButtonClickChange(true);

        //�{�^���őI������Ă��镺�m������ꍇ�A�ēx�I���ł���悤�Ƀ{�^����Ԃ����ɖ߂�
        if (SelectFighterButton != null)
        {
            SelectFighterButton.GetComponent<Button>().interactable = true;
        }

        SelectFighterButton = eventSystem.currentSelectedGameObject;
        SelectFighterButton.GetComponent<Button>().interactable = false;
        SelectFighterStatus = SelectFighterButton.GetComponent<FighterStatus>();

        //�X�e�[�^�XUI�\��
        FighterStatusInfo.TextWrite(SelectFighterStatus);
        //���m�̐F
        Color UnitC = Color.white;
        if(SelectFighterStatus.UnitNum != 0)
        {
            UnitC = PlayerUnitDataBaseAllList[SelectFighterStatus.UnitNum - 1].UnitColor;
        }

        switch (SelectFighterStatus.Type)
        {
            case 1:
                FighterStatusInfo.ImageWrite(InfantryImage, UnitC);
                break;
            case 2:
                FighterStatusInfo.ImageWrite(ArcherImage, UnitC);
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }
    }

    //�e���בւ��{�^������
    public void UnitOrderbyChange(bool on)
    {
        if (on)
        {
            PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList
            .OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ThenBy((n) => n.Type).ToList(); //�����ԍ����A����������ɗ���悤�ɁA���폇�ɕ��ёւ�

            FighterViewDisplay();
        }
    }
    public void NameOrderbyChange(bool on)
    {
        if (on)
        {
            PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList
            .OrderBy((n) => n.Name).ToList(); //���O���ɕ��ёւ�

            FighterViewDisplay();
        }
    }
    public void LevelOrderbyChange(bool on)
    {
        if (on)
        {
            PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList
            .OrderBy((n) => n.Level).ToList(); //Lv���ɕ��ёւ�

            FighterViewDisplay();
        }
    }

    public void NameChangeButtonClick()
    {
        NameUI.SetActive(true);
        NameField.text = SelectFighterStatus.FighterName;
        NameField.ActivateInputField();
    }

    //����{�^�������ŕ��m���ύX
    public void DecisionName()
    {
        if (NameField.text != SelectFighterStatus.FighterName && PlayerFighterDataBaseAllList.FindAll((n) => n.Name == NameField.text).Count > 0)
        {
            NameWarningUI.SetActive(true);
        }
        else
        {
            PlayerFighterDataBaseAllList.Find((n) => n.Name == SelectFighterStatus.FighterName).Name = NameField.text;
            SelectFighterStatus.FighterName = NameField.text;
            NameUI.SetActive(false);
            FighterStatusInfo.TextWrite(SelectFighterStatus);
            FighterViewDisplay();
        }
    }

    //���x���A�b�v�{�^���N���b�N
    public void LevelUpButtonClick()
    {
        LevelUpUI.SetActive(true);
    }

    //���ك{�^���N���b�N
    public void DismissalButtonClick()
    {
        DismissalUI.SetActive(true);
    }

    //�߂�{�^���N���b�N
    public void BackButtonClick()
    {
        SeManager.HomeUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
