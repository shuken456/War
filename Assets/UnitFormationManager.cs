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

    public GameObject SaveUI;
    public GameObject EndUI;
    public GameObject WarningUI;

    //���j�b�g�\���I�u�W�F�N�g
    public GameObject UnitObjectBack;

    //���m�X�e�[�^�XUI
    public FighterStatusInfoUI FighterStatusInfo;

    //�e���m�̃v���n�u
    public GameObject InfantryPrefab;
    public GameObject ArcherPrefab;

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

    // Start is called before the first frame update
    void Start()
    {
        //DB�f�[�^�擾
        PlayerUnitDataBaseAllList = Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //���j�b�g�ԍ����ɕ��ёւ�
        PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList
            .OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ToList(); //�����ԍ����A����������ɗ���悤�ɕ��ёւ�

        //�X�N���[���o�[���K�v�ȏꍇ�A�{�^���̈ʒu����
        if(PlayerFighterDataBaseAllList.Count >= 10)
        {
            ReserveFighterView.GetComponent<VerticalLayoutGroup>().padding.right = 10;
        }

        //�T�����m�̐����A�{�^�����쐬
        foreach(PlayerFighter Fighter in PlayerFighterDataBaseAllList)
        {
            GameObject button = Instantiate(ReserveFighterButton, ReserveFighterView.transform);
            button.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-9f, 80);
            button.transform.Find("Text (Name)").GetComponent<Text>().text = Fighter.Name;
            button.transform.Find("Text (Type)").GetComponent<Text>().text = Common.FighterType(Fighter.Type);
            button.transform.Find("Text (Level)").GetComponent<Text>().text = Fighter.Level.ToString();

            //�쐬�����{�^���ɕ��m�X�e�[�^�X������
            Common.GetFighterStatusFromDB(button.GetComponent<FighterStatus>(), Fighter);

            //���ɂ��̃��j�b�g���ɂ��镺�m�̃{�^���͉����Ȃ��悤�ɂ���
            if (Fighter.UnitNum == Common.SelectUnitNum)
            { 
                button.GetComponent<Button>().interactable = false;
            }
            else if (Fighter.UnitNum != 0)
            {
                //�ǂ����̕����ɏ����ς݂̕��m�̓{�^���̐F��ς���
                button.GetComponent<Image>().color = Color.yellow;
            }
            button.GetComponent<Button>().onClick.AddListener(FighterButtonClick);
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
        SelectUnitInfoUI.transform.Find("Text (Strategy)").GetComponent<Text>().text = Common.FighterStrategy(PlayerUnitDataBaseSelectList[0].Strategy);
       

        //�������Ă��镺�m����ʂɕ\������
        for (int i = 0; i < PlayerFighterDataBaseSelectList.Count; i++)
        {
            GameObject Fighter = null;
            PlayerFighter FighterStatusList = PlayerFighterDataBaseSelectList[i];

            //����ɂ���ăI�u�W�F�N�g���쐬
            if (FighterStatusList.Type == 1)
            {
                Fighter = Instantiate(InfantryPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else if (FighterStatusList.Type == 2)
            {
                Fighter = Instantiate(ArcherPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            }
            Fighter.GetComponent<SpriteRenderer>().color = PlayerUnitDataBaseSelectList[0].UnitColor;

            //�쐬�������m�I�u�W�F�N�g�ɃX�e�[�^�X������
            Common.GetFighterStatusFromDB(Fighter.GetComponent<FighterStatus>(), FighterStatusList);

            //��ʍ��̃I�u�W�F�N�g�ƕR�Â���
            Fighter.transform.parent = UnitObjectBack.transform;
            Fighter.transform.localPosition = FighterStatusList.Position;

            //���m�̖��O��\������
            UnitFighterNameWrite(Fighter);
           
            //��ʉE�����j�b�g�����o�[UI�L��
            UnitMemberInfoWrite(i, FighterStatusList.Name, FighterStatusList.Type, FighterStatusList.Level);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

        //���N���b�N�Ń��j�b�g�̑I�����ǉ��A�h���b�O�ňړ��A�_�u���N���b�N�ŕ������ύX
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter", "SelectFighter"));
            if (col != null)
            {
                //�I������Ă��镺�m�����ɂ���ꍇ�A���̕��m�̌����ڂ����ɖ߂�
                if(StatusShowFighter != null)
                {
                    StatusShowFighter.layer = LayerMask.NameToLayer("PlayerFighter");
                }

                //�{�^���őI������Ă��镺�m������ꍇ�A�ēx�I���ł���悤�Ƀ{�^����Ԃ����ɖ߂�
                if(SelectFighterButton != null)
                {
                    SelectFighterButton.GetComponent<Button>().interactable = true;
                }
                
                SelectFighter = col.gameObject;
                StatusShowFighter = col.gameObject;
                StatusShowFighterName = StatusShowFighter.GetComponent<FighterStatus>().FighterName;

                //�����ڂ�ύX
                StatusShowFighter.layer = LayerMask.NameToLayer("SelectFighter");

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

                //�{�^���őI�����ꂽ���m���쐬���ăN���b�N�ʒu�ɕ\������
                switch (SelectStatus.Type)
                {
                    case 1:
                        Fighter = Instantiate(InfantryPrefab, CursorPosition, Quaternion.identity);
                        break;
                    case 2:
                        Fighter = Instantiate(ArcherPrefab, CursorPosition, Quaternion.identity);
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    default:
                        break;
                }

                Fighter.transform.parent = UnitObjectBack.transform;

                //�{�^�����ɂ���X�e�[�^�X���t�@�C�^�[�I�u�W�F�N�g�ɃR�s�[
                FighterStatus fs = Fighter.GetComponent<FighterStatus>();
                Common.FighterStatusCopy(fs, SelectStatus);
                Fighter.GetComponent<SpriteRenderer>().color = PlayerUnitDataBaseSelectList[0].UnitColor;

                int FighterCount = GameObject.FindGameObjectsWithTag("PlayerFighter").Length;

                //��l�̏ꍇ�A����������
                if(FighterCount == 1)
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
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter", "SelectFighter"));
            if (col != null)
            {
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

        yield return new WaitForSeconds(0.1f);

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
                }
            }
        }
        ClickCount = 0;
    }

    //���m�̃{�^���N���b�N�C�x���g
    public void FighterButtonClick()
    {
        //�{�^���őI������Ă��镺�m������ꍇ�A�ēx�I���ł���悤�Ƀ{�^����Ԃ����ɖ߂�
        if (SelectFighterButton != null)
        {
            SelectFighterButton.GetComponent<Button>().interactable = true;
        }

        SelectFighterButton = eventSystem.currentSelectedGameObject;
        SelectFighterButton.GetComponent<Button>().interactable = false;
        FighterStatus SelectStatus = SelectFighterButton.GetComponent<FighterStatus>();

        //�X�e�[�^�XUI�\��
        FighterStatusInfo.TextWrite(SelectStatus);
        switch (SelectStatus.Type)
        {
            case 1:
                FighterStatusInfo.ImageWrite(InfantryPrefab.GetComponent<SpriteRenderer>().sprite, PlayerUnitDataBaseSelectList[0].UnitColor);
                break;
            case 2:
                FighterStatusInfo.ImageWrite(ArcherPrefab.GetComponent<SpriteRenderer>().sprite, PlayerUnitDataBaseSelectList[0].UnitColor);
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }

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
            pfl[0].Position = Fighter.transform.localPosition;
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

        EditorUtility.SetDirty(Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB"));
        AssetDatabase.SaveAssets();
        SaveUI.SetActive(true);
    }
}
