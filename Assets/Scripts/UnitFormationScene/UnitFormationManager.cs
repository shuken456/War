using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEditor;

public class UnitFormationManager : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;

    //���j�b�gDB�ƕ��mDB
    public List<PlayerUnit> PlayerUnitDataBaseAllList;
    public List<PlayerUnit> PlayerUnitDataBaseSelectList;
    public List<PlayerFighter> PlayerFighterDataBaseAllList;
    public List<PlayerFighter> PlayerFighterDataBaseSelectList;

    //���j�b�g�ڍ�UI�i��ʉ��j
    public GameObject SelectUnitInfoUI;
    public GameObject[] UnitMemberInfoUI;

    public GameObject HelpUI;
    public GameObject SaveUI;
    public GameObject EndUI;
    public GameObject WarningUI;
    public GameObject WarningUIMax;

    //���j�b�g�\���I�u�W�F�N�g
    public GameObject UnitObjectBack;

    //���m�X�e�[�^�XUI
    public FighterStatusInfoUI FighterStatusInfo;

    //�e���m�̃v���n�u
    public GameObject InfantryPrefab;
    public GameObject ArcherPrefab;
    public GameObject ShielderPrefab;
    public GameObject CavalryPrefab;

    //���ڃN���b�N���đI���������m
    private GameObject SelectFighter = null;
    private GameObject StatusShowFighter = null;

    //���ڃN���b�N���đI���������m�̖��O
    private string StatusShowFighterName;

    //�ǔ��e�L�X�g�Q�[���\���p�L�����p�X
    public GameObject CanvasWorldSpace;
    //���m���ǔ��e�L�X�g�v���n�u
    public GameObject ChaseFighterNameText;

    //�T�����mUI �X�N���[���r���[
    public GameObject ReserveFighterView;
    //�T�����mUI�\���p�{�^���v���n�u
    public GameObject ReserveFighterButton;
    
    //�N���b�N���ꂽ�T�����m�{�^��
    public GameObject SelectFighterButton;

    //�_�u���N���b�N����p�@�J�E���g�ϐ�
    private int ClickCount = 0;

    //�������j�ێ�
    private int UnitStrategy = 0;

    //�{�^������SE
    public AudioSource SE;

    //DB
    public PlayerFighterDB PlayerFighterTable;
    public PlayerUnitDB PlayerUnitTable;

    // Start is called before the first frame update
    void Start()
    {
        //�_�u���N���b�N������s�����߁A���̉�ʂł͎��Ԃ𓮂���
        Time.timeScale = 1;
        //�{�^�������őJ�ڂ�����ʂ̂��߉���炷
        SE.Play();
        //�f�[�^���[�h
        PlayerFighterTable.Load();
        PlayerUnitTable.Load();

        //DB�f�[�^�擾
        PlayerUnitDataBaseAllList = PlayerUnitTable.PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //���j�b�g�ԍ����ɕ��ёւ�
        PlayerFighterDataBaseAllList = PlayerFighterTable.PlayerFighterDBList
            .OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ThenBy((n) => n.Type).ToList(); //�����ԍ����A����������ɗ���悤�ɁA���폇�ɕ��ёւ�

        //�X�N���[���o�[���K�v���ۂ��ŁA�{�^���̈ʒu����
        if(PlayerFighterDataBaseAllList.Count >= 10)
        {
            ReserveFighterView.GetComponent<VerticalLayoutGroup>().padding.right = 20;
        }
        else
        {
            ReserveFighterView.GetComponent<VerticalLayoutGroup>().padding.right = 40;
        }

        //�Ώۃ��j�b�g�̃f�[�^����
        PlayerUnitDataBaseSelectList = PlayerUnitDataBaseAllList.FindAll(n => n.Num == Common.SelectUnitNum);
        PlayerFighterDataBaseSelectList = PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == Common.SelectUnitNum);

        //��ʍ����@���j�b�g���UI�\�L
        SelectUnitInfoUI.transform.Find("Text (UnitNum)").GetComponent<Text>().text = Common.SelectUnitNum.ToString();
        SelectUnitInfoUI.transform.Find("Text (UnitName)").GetComponent<Text>().text = PlayerUnitDataBaseSelectList[0].Name;
        //��������
        if (PlayerFighterDataBaseSelectList.Count > 0)
        {
            SelectUnitInfoUI.transform.Find("Text (UnitLeader)").GetComponent<Text>().text = PlayerFighterDataBaseSelectList[0].Name;
        }
        //�����l��
        SelectUnitInfoUI.transform.Find("Text (MemberCount)").GetComponent<Text>().text = PlayerFighterDataBaseSelectList.Count.ToString() + "�l";
        //�������j
        UnitStrategy = PlayerUnitDataBaseSelectList[0].Strategy;
        SelectUnitInfoUI.transform.Find("Text (Strategy)").GetComponent<Text>().text = Common.FighterStrategy(UnitStrategy);
       

        //�������Ă��镺�m����ʂɕ\������
        for (int i = 0; i < PlayerFighterDataBaseSelectList.Count; i++)
        {
            GameObject Fighter = null;
            PlayerFighter FighterStatusList = PlayerFighterDataBaseSelectList[i];

            //����ɂ���ăI�u�W�F�N�g���쐬
            Fighter = Instantiate(FighterPrefab(FighterStatusList.Type), new Vector3(0, 0, 0), Quaternion.identity);
            Fighter.GetComponent<SpriteRenderer>().color = PlayerUnitDataBaseSelectList[0].UnitColor;

            //�쐬�������m�I�u�W�F�N�g�ɃX�e�[�^�X������
            Common.GetFighterStatusFromDB(Fighter.GetComponent<FighterStatus>(), FighterStatusList);

            //��ʍ��̃I�u�W�F�N�g�ƕR�Â���
            Fighter.transform.parent = UnitObjectBack.transform;
            Fighter.transform.localPosition = FighterStatusList.Position;

            //��ʉE�����j�b�g�����o�[UI�L��
            UnitMemberInfoWrite(i, FighterStatusList.Name, FighterStatusList.Type, FighterStatusList.Level);

            //���m�̖��O��\������
            UnitFighterNameWrite(Fighter);
        }

        ReserveFighterViewDisplay();

        if (PlayerPrefs.GetInt("FormationHelp",0) == 0)
        {
            PlayerPrefs.SetInt("FormationHelp", 1);
            HelpUI.SetActive(true);
        }
    }

    
    private void OnDisable()
    {
        Time.timeScale = 0;
    }

    //�T�����m�r���[�X�V
    private void ReserveFighterViewDisplay()
    {
        //�I������Ă��镺�m�̖��O��destroy�O�ɕۑ�
        string SelectN = string.Empty;
        if (SelectFighterButton)
        {
            SelectN = SelectFighterButton.GetComponent<FighterStatus>().FighterName;
        }

        //���m�r���[���Z�b�g
            foreach (Transform f in ReserveFighterView.transform)
        {
            GameObject.Destroy(f.gameObject);
        }

        //���t�B�[���h�ɂ��镺�m�̖��O��NameList�ɕێ�
        GameObject[] Fighters = GameObject.FindGameObjectsWithTag("PlayerFighter");
        List<string> NameList = new List<string>();
        foreach (GameObject Fighter in Fighters)
        {
            FighterStatus fs = Fighter.GetComponent<FighterStatus>();
            NameList.Add(fs.FighterName);
        }

        //�T�����m�̐����A�{�^�����쐬
        foreach (PlayerFighter Fighter in PlayerFighterDataBaseAllList)
        {
            GameObject button = Instantiate(ReserveFighterButton, ReserveFighterView.transform);
            button.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-9f, 80);
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

            //���ɂ��̃��j�b�g���ɂ��镺�m�̃{�^���͉����Ȃ��悤�ɂ���
            if(NameList.Contains(Fighter.Name))
            {
                button.GetComponent<Button>().interactable = false;
            }
            else if (Fighter.UnitNum != 0 && Fighter.UnitNum != Common.SelectUnitNum)
            {
                //�ǂ����̕����ɏ����ς݂̕��m�̓{�^���̐F��ς���
                button.GetComponent<Image>().color = Color.yellow;
            }
            button.GetComponent<Button>().onClick.AddListener(FighterButtonClick);

            //�r���[�X�V���s���Ă��I������Ă���{�^����ێ�����
            if (SelectN == button.GetComponent<FighterStatus>().FighterName)
            {
                SelectFighterButton = button;
                button.GetComponent<Button>().interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

        //���N���b�N�Ń��j�b�g�̑I�����ǉ��A�h���b�O�ňړ��A�_�u���N���b�N�ŕ������ύX
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter"));
            if (col != null)
            {
                SE.Play();

                //�I������Ă��镺�m�����ɂ���ꍇ�A���̕��m�̌����ڂ����ɖ߂�
                if(StatusShowFighter != null)
                {
                    StatusShowFighter.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.clear;
                }

                //�{�^���őI������Ă��镺�m������ꍇ�A�ēx�I���ł���悤�Ƀ{�^����Ԃ����ɖ߂�
                if(SelectFighterButton != null)
                {
                    SelectFighterButton.GetComponent<Button>().interactable = true;
                }
                SelectFighter = col.gameObject;
                StatusShowFighter = col.gameObject;

                FighterStatus fs = StatusShowFighter.GetComponent<FighterStatus>();
                StatusShowFighterName = fs.FighterName;

                //�����ڂ�ύX
                StatusShowFighter.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.yellow;

                //�o�t�ݒ�
                Common.FighterBuff(fs, UnitStrategy, false);

                //�X�e�[�^�XUI�\��
                FighterStatusInfo.TextWrite(StatusShowFighter.GetComponent<FighterStatus>());
                FighterStatusInfo.ImageWrite(StatusShowFighter.GetComponent<SpriteRenderer>().sprite, StatusShowFighter.GetComponent<SpriteRenderer>().color);

                //�_�u���N���b�N������
                ClickCount++;
                Invoke("OnDoubleClick", 0.3f);
            }
            else if(SelectFighterButton != null && !SelectFighterButton.GetComponent<Button>().interactable)
            {
                //���̃��j�b�g�Ґ��p�I�u�W�F�N�g���łȂ��ꍇreturn
                if (CursorPosition.x > UnitObjectBack.transform.position.x + 2.5 || CursorPosition.x < UnitObjectBack.transform.position.x - 2.5 ||
                    CursorPosition.y > UnitObjectBack.transform.position.y + 2 || CursorPosition.y < UnitObjectBack.transform.position.y - 1.3)
                {
                    return;
                }

                GameObject Fighter = null;
                FighterStatus SelectStatus = SelectFighterButton.GetComponent<FighterStatus>();

                //MAX10�l�܂�
                int FighterCount = GameObject.FindGameObjectsWithTag("PlayerFighter").Length;

                if(FighterCount == 10)
                {
                    WarningUIMax.SetActive(true);
                }
                else
                {
                    SE.Play();
                    //�{�^���őI�����ꂽ���m���쐬���ăN���b�N�ʒu�ɕ\������
                    Fighter = Instantiate(FighterPrefab(SelectStatus.Type), CursorPosition, Quaternion.identity);
                    Fighter.transform.parent = UnitObjectBack.transform;

                    //�{�^�����ɂ���X�e�[�^�X���t�@�C�^�[�I�u�W�F�N�g�ɃR�s�[
                    FighterStatus fs = Fighter.GetComponent<FighterStatus>();
                    Common.FighterStatusCopy(fs, SelectStatus);
                    Fighter.GetComponent<SpriteRenderer>().color = PlayerUnitDataBaseSelectList[0].UnitColor;

                    FighterCount = GameObject.FindGameObjectsWithTag("PlayerFighter").Length;
                    //��l�̏ꍇ�A����������
                    if (FighterCount == 1)
                    {
                        fs.UnitLeader = true;
                        SelectUnitInfoUI.transform.Find("Text (UnitLeader)").GetComponent<Text>().text = fs.FighterName;
                    }
                    else
                    {
                        fs.UnitLeader = false; //����������l�ȏ�ɂȂ邱�Ƃ�h������
                    }

                    //���m�̖��O��\������
                    UnitFighterNameWrite(Fighter);

                    SelectFighterButton = null;

                    //��ʍ������j�b�gUI�L�ڕύX
                    SelectUnitInfoUI.transform.Find("Text (MemberCount)").GetComponent<Text>().text = FighterCount.ToString() + "�l";

                    //��ʉ����j�b�g�����o�[UI�ǋL
                    UnitMemberInfoWrite(FighterCount - 1, SelectStatus.FighterName, SelectStatus.Type, SelectStatus.Level);
                }
            }
        }
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (SelectFighter != null)
            {
                //���̃��j�b�g�Ґ��p�I�u�W�F�N�g���łȂ��ꍇ����
                if (CursorPosition.x > UnitObjectBack.transform.position.x + 2.5 || CursorPosition.x < UnitObjectBack.transform.position.x - 2.5 ||
                    CursorPosition.y > UnitObjectBack.transform.position.y + 2 || CursorPosition.y < UnitObjectBack.transform.position.y - 1.3)
                {
                    return;
                }
                SelectFighter.transform.position = CursorPosition;�@//���m�ړ�
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            SelectFighter = null;
        }

        //�E�N���b�N�ŕ��m�폜�@
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter"));
            if (col != null)
            {
                SE.Play();
                StartCoroutine(DestroyFighter(col));
            }
        }
    }

    //���m�폜�@Destroy����Ƀ^�C�����O������̂ŃR���[�`���ɂ��ĂȂ�Ƃ�����
    IEnumerator DestroyFighter(Collider2D col)
    {
        string DestroyName = col.gameObject.GetComponent<FighterStatus>().FighterName;
        bool LeaderDestroy = col.gameObject.GetComponent<FighterStatus>().UnitLeader;
        Destroy(col.gameObject);
        UnitMemberInfoClear();

        yield return new WaitForSecondsRealtime(0.1f);

        //�I�𒆂̕��m���폜�����ꍇ�A�X�e�[�^�XUI���󔒂ŕ\������
        FighterStatusInfo.Clear(DestroyName);

        GameObject[] Fighters = GameObject.FindGameObjectsWithTag("PlayerFighter");

        //��ʍ������j�b�gUI�L�ڕύX
        int FighterCount = Fighters.Length;
        SelectUnitInfoUI.transform.Find("Text (MemberCount)").GetComponent<Text>().text = FighterCount.ToString() + "�l";

        //���������폜���ꂽ��ʂ̕��m�Ɏ����Ŋ��蓖�Ă�
        int NoLeaderCount = 0;
        if (LeaderDestroy)
        {
            if(Fighters.Length == 0)
            {
                SelectUnitInfoUI.transform.Find("Text (UnitLeader)").GetComponent<Text>().text = string.Empty;
            }
            for (int i = 0; i < Fighters.Length; i++)
            {
                FighterStatus fs = Fighters[i].GetComponent<FighterStatus>();
                if (i == 0)
                {
                    fs.UnitLeader = true;
                    SelectUnitInfoUI.transform.Find("Text (UnitLeader)").GetComponent<Text>().text = fs.FighterName;
                }
                //��ʉ����j�b�g�����o�[UI�L��
                UnitMemberInfoWrite(i, fs.FighterName, fs.Type, fs.Level);
            }
        }
        else
        {
            //��ʉE�����j�b�g�����o�[UI�L�ڕύX
            foreach (GameObject Fighter in Fighters)
            {
                FighterStatus fs = Fighter.GetComponent<FighterStatus>();

                if (fs.UnitLeader)
                {
                    //��ʉ����j�b�g�����o�[UI�L��
                    UnitMemberInfoWrite(0, fs.FighterName, fs.Type, fs.Level);
                }
                else
                {
                    NoLeaderCount++;

                    //��ʉ����j�b�g�����o�[UI�L��
                    UnitMemberInfoWrite(NoLeaderCount, fs.FighterName, fs.Type, fs.Level);
                }
            }
        }

        for (int i = 0; i < ReserveFighterView.transform.childCount; i++)
        {
            FighterStatus fs = ReserveFighterView.transform.GetChild(i).GetComponent<FighterStatus>();
            if (DestroyName == fs.FighterName)
            {
                ReserveFighterView.transform.GetChild(i).GetComponent<Button>().interactable = true;
                break;
            }
        }
    }

    //���_�u���N���b�N�ŕ������ύX
    private void OnDoubleClick()
    {
        if (ClickCount == 2) 
        {
            GameObject[] Fighters = GameObject.FindGameObjectsWithTag("PlayerFighter");

            int NoLeaderCount = 0;

            foreach (GameObject Fighter in Fighters)
            {
                FighterStatus fs = Fighter.GetComponent<FighterStatus>();

                if (fs.FighterName != StatusShowFighterName)
                {
                    fs.UnitLeader = false;

                    NoLeaderCount++;

                    //��ʉ����j�b�g�����o�[UI�L��
                    UnitMemberInfoWrite(NoLeaderCount, fs.FighterName, fs.Type, fs.Level);
                }
                else
                {
                    fs.UnitLeader = true;
                    SelectUnitInfoUI.transform.Find("Text (UnitLeader)").GetComponent<Text>().text = fs.FighterName;

                    //��ʉ����j�b�g�����o�[UI�L��
                    UnitMemberInfoWrite(0, fs.FighterName, fs.Type, fs.Level);

                    //�X�e�[�^�XUI�X�V(�X�e�[�^�X�o�t�ύX�̂���)
                    Common.FighterBuff(fs, UnitStrategy, false);
                    FighterStatusInfo.TextWrite(fs);
                }
            }
        }
        ClickCount = 0;
    }

    //���m�̃{�^���N���b�N�C�x���g
    public void FighterButtonClick()
    {
        SE.Play();

        //�{�^���őI������Ă��镺�m������ꍇ�A�ēx�I���ł���悤�Ƀ{�^����Ԃ����ɖ߂�
        if (SelectFighterButton != null)
        {
            SelectFighterButton.GetComponent<Button>().interactable = true;
        }

        SelectFighterButton = eventSystem.currentSelectedGameObject;
        SelectFighterButton.GetComponent<Button>().interactable = false;
        FighterStatus SelectStatus = SelectFighterButton.GetComponent<FighterStatus>();

        //�o�t�ݒ� ���T�����m���ʂ̕����̃��[�_�[�ł����Ă����̃o�t��\��������Ƃ�₱�������߁@true
        Common.FighterBuff(SelectStatus, UnitStrategy, true);

        //�X�e�[�^�XUI�\��
        FighterStatusInfo.TextWrite(SelectStatus);
        //���m�摜�\��
        FighterStatusInfo.ImageWrite(FighterPrefab(SelectStatus.Type).GetComponent<SpriteRenderer>().sprite, PlayerUnitDataBaseSelectList[0].UnitColor);

        if(SelectStatus.UnitNum != 0 && SelectStatus.UnitNum != Common.SelectUnitNum)
        {
            WarningUI.SetActive(true);
        }
    }

    //��ʉ��@���j�b�g�����o�[UI�`��
    private void UnitMemberInfoWrite(int position, string name, int type, int level)
    {
        //���O
        UnitMemberInfoUI[position].transform.Find("Text (MemberName)").GetComponent<Text>().text = name;
        //����
        UnitMemberInfoUI[position].transform.Find("Text (MemberType)").GetComponent<Text>().text = Common.FighterType(type);
        //���x��
        UnitMemberInfoUI[position].transform.Find("Text (MemberLevel)").GetComponent<Text>().text = "Lv" + level.ToString();
    }

    //��ʉ��@���j�b�g�����o�[UI�N���A
    private void UnitMemberInfoClear()
    {
        foreach(GameObject MemberInfo in UnitMemberInfoUI)
        {
            //���O
            MemberInfo.transform.Find("Text (MemberName)").GetComponent<Text>().text = string.Empty;
            //����
            MemberInfo.transform.Find("Text (MemberType)").GetComponent<Text>().text = string.Empty;
            //���x��
            MemberInfo.transform.Find("Text (MemberLevel)").GetComponent<Text>().text = string.Empty;
        }
    }

    //���m�̖��O��\������
    private void UnitFighterNameWrite(GameObject target)
    {
        FighterNameChaseText NameText = Instantiate(ChaseFighterNameText, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<FighterNameChaseText>();
        NameText.transform.SetParent(CanvasWorldSpace.transform, false);
        NameText.targetFighter = target;
        NameText.offset = new Vector3(0, 0.5f, 0);
    }

    public void EndButtonClick()
    {
        EndUI.SetActive(true);
    }

    //�ۑ�
    public void SaveButtonClick()
    {
        GameObject[] Fighters = GameObject.FindGameObjectsWithTag("PlayerFighter");
        List<string> NameList = new List<string>();

        foreach (GameObject Fighter in Fighters)
        {
            FighterStatus fs = Fighter.GetComponent<FighterStatus>();
            List<PlayerFighter> pfl = PlayerFighterDataBaseAllList.FindAll(n => n.Name == fs.FighterName);

            //�ύX����O�̏���ێ����Ă���
            int originNum = pfl[0].UnitNum;
            bool originLeader = pfl[0].UnitLeader;

            //�f�[�^�ύX
            pfl[0].Position = new Vector3(float.Parse(Fighter.transform.localPosition.x.ToString("F1")), float.Parse(Fighter.transform.localPosition.y.ToString("F1")), 0); //�����������Ȃ�Ȃ��悤�����_��ʂŐ؂�
            pfl[0].UnitNum = Common.SelectUnitNum;
            pfl[0].UnitLeader = fs.UnitLeader;

            NameList.Add(fs.FighterName);

            //���̕����̕�������Ґ����Ă����ꍇ�A���̕����̕������������I�ɕҐ�����
            if (originNum != 0 && originNum != Common.SelectUnitNum && originLeader && PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == originNum).Count > 0)
            {
                PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == originNum)[0].UnitLeader = true;
            }
        }

        //�폜���ꂽ���m�͕����i���o�[��0�Ƃ���
        List<PlayerFighter> pfl2 = PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == Common.SelectUnitNum);
        foreach (PlayerFighter pf in pfl2)
        {
            if(!NameList.Contains(pf.Name))
            {
                PlayerFighterDataBaseAllList.Find(n => n.Name == pf.Name).UnitNum = 0;
            }
        }

        //�f�[�^�Z�[�u
        PlayerFighterTable.Save();
        PlayerUnitTable.Save();

        SaveUI.SetActive(true);
    }

    //�ő�l���I�[�o�[�x���@OK�{�^��������
    public void WarningMaxOK()
    {
        WarningUIMax.SetActive(false);
    }

    //�e���בւ��{�^������
    public void UnitOrderbyChange(bool on)
    {
        if (on)
        {
            SE.Play();
            PlayerFighterDataBaseAllList = PlayerFighterTable.PlayerFighterDBList
            .OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ThenBy((n) => n.Type).ToList(); //�����ԍ����A����������ɗ���悤�ɁA���폇�ɕ��ёւ�

            ReserveFighterViewDisplay();
        }
    }
    public void NameOrderbyChange(bool on)
    {
        if (on)
        {
            SE.Play();
            PlayerFighterDataBaseAllList = PlayerFighterTable.PlayerFighterDBList
            .OrderBy((n) => n.Name).ToList(); //���O���ɕ��ёւ�

            ReserveFighterViewDisplay();
        }
    }
    public void LevelOrderbyChange(bool on)
    {
        if (on)
        {
            SE.Play();
            PlayerFighterDataBaseAllList = PlayerFighterTable.PlayerFighterDBList
            .OrderBy((n) => n.Level).ToList(); //Lv���ɕ��ёւ�

            ReserveFighterViewDisplay();
        }
    }

    private GameObject FighterPrefab(int type)
    {
        switch (type)
        {
            case 1:
                return InfantryPrefab;
            case 2:
                return ArcherPrefab;
            case 3:
                return ShielderPrefab;
            case 4:
                return CavalryPrefab;
            default:
                return null;
        }
    }
}
