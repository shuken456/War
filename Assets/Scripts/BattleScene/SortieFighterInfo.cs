using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using Cinemachine;

public class SortieFighterInfo : MonoBehaviour
{
    public BattleManager BaManager;

    [SerializeField] private EventSystem eventSystem;

    //���m�ꗗ
    public GameObject SortieFighterView;

    //���m�ꗗ�p�{�^���v���n�u
    public GameObject SortieFighterButton;

    //�J����
    public CinemachineVirtualCamera mainCam;

    private List <FighterStatus> PlayerFighters = new List<FighterStatus>();

    private void OnEnable()
    {
        PlayerFighters = new List<FighterStatus>();
        GameObject[] Fighters = GameObject.FindGameObjectsWithTag("PlayerFighter");

        foreach (GameObject Fighter in Fighters)
        {
            PlayerFighters.Add(Fighter.GetComponent<FighterStatus>());
        }

        //�X�N���[���o�[���K�v���ۂ��ŁA�{�^���̈ʒu����
        if (PlayerFighters.Count >= 10)
        {
            SortieFighterView.GetComponent<VerticalLayoutGroup>().padding.right = 20;
        }
        else
        {
            SortieFighterView.GetComponent<VerticalLayoutGroup>().padding.right = 40;
        }

        SortieFighterViewDisplay();
    }

    //�o�������m�r���[�X�V
    private void SortieFighterViewDisplay()
    {
        //���m�r���[���Z�b�g
        foreach (Transform f in SortieFighterView.transform)
        {
            GameObject.Destroy(f.gameObject);
        }

        //���t�B�[���h�ɂ��镺�m�̐����A�{�^�����쐬
        foreach (FighterStatus fs in PlayerFighters)
        {
            GameObject button = Instantiate(SortieFighterButton, SortieFighterView.transform);
            button.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-9f, 80);

            button.transform.Find("Text (Name)").GetComponent<Text>().text = fs.FighterName;
            button.transform.Find("Text (UnitName)").GetComponent<Text>().text = BaManager.PlayerUnitDataBaseAllList[fs.UnitNum - 1].Name;
            button.transform.Find("Text (Type)").GetComponent<Text>().text = Common.FighterType(fs.Type);
            button.transform.Find("Text (Level)").GetComponent<Text>().text = fs.Level.ToString();

            //�쐬�����{�^���ɕ��m�X�e�[�^�X������
            Common.FighterStatusCopy(button.GetComponent<FighterStatus>(), fs);
            button.GetComponent<Button>().onClick.AddListener(FighterButtonClick);
        }
    }

    //���m�̃{�^���N���b�N�C�x���g
    public void FighterButtonClick()
    {
        FighterStatus fs = PlayerFighters.Find((n) => n.FighterName == eventSystem.currentSelectedGameObject.GetComponent<FighterStatus>().FighterName);
        mainCam.transform.position = fs.gameObject.transform.position - new Vector3(0, 0, 1);//���̕��m����ʒ����ɗ���悤�ɂ���

        //���̕��m��I�𒆂Ƃ���
        if (!BaManager.SelectFighter.Contains(fs.gameObject))
        {
            BaManager.SelectFighter.Add(fs.gameObject);

            //�I�𕺎m�r���[�X�V
            BaManager.ActionUI.transform.Find("SelectFighterInfoUI").GetComponent<SelectFighterInfoUI>().UpdateView();

            //�I�𒆂̌����ڂ�ύX
            fs.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        BaManager.ActionUI.GetComponent<ActionUI>().ChangeButton();

        this.gameObject.SetActive(false);
    }

    //�e���בւ��{�^������
    public void UnitOrderbyChange(bool on)
    {
        if (on)
        {
            PlayerFighters = PlayerFighters.OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ThenBy((n) => n.Type).ToList(); //�����ԍ����A����������ɗ���悤�ɁA���폇�ɕ��ёւ�
            SortieFighterViewDisplay();
        }
    }
    public void NameOrderbyChange(bool on)
    {
        if (on)
        {
            PlayerFighters = PlayerFighters.OrderBy((n) => n.FighterName).ToList(); //���O���ɕ��ёւ�
            SortieFighterViewDisplay();
        }
    }
    public void LevelOrderbyChange(bool on)
    {
        if (on)
        {
            PlayerFighters = PlayerFighters.OrderBy((n) => n.Level).ToList(); //Lv���ɕ��ёւ�
            SortieFighterViewDisplay();
        }
    }

    //����{�^������
    public void SearchClose()
    {
        this.gameObject.SetActive(false);
    }
}
