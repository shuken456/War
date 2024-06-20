using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UnitEditManager : MonoBehaviour
{
    public GameObject canvas;

    //���j�b�gUI�ƃQ�[���I�u�W�F�N�g
    public GameObject[] UnitUI;
    public GameObject[] UnitOblect;

    //�I�𒆂̃��j�b�g�ڍ�UI�i��ʉ��j
    public GameObject SelectUnitInfoUI;
    public GameObject[] UnitMemberInfoUI;

    //���j�b�g�I����A�\������UI
    public GameObject SelectUI;
    public GameObject StrategyUI;
    public GameObject ColorUI;
    public GameObject NameUI;
    public GameObject SortieButton;

    //�o�����\��
    public GameObject SortieNow;

    //�����J���p���b�N�{�^��
    public GameObject LockButton;
    public GameObject LockUI;

    //�e���m�̃v���n�u�i�摜�̂݁j
    public GameObject EmptyInfantry;
    public GameObject EmptyArcher;
    public GameObject EmptyShielder;
    public GameObject EmptyCavalry;

    //���j�b�gDB�ƕ��mDB
    public List<PlayerUnit> PlayerUnitDataBaseAllList;
    public List<PlayerFighter> PlayerFighterDataBaseAllList;

    //�I�𒆂̃��j�b�g�i���o�[
    public int SelectUnitNum = 1;

    //�{�^������SE
    public AudioSource SE;

    //DB
    public PlayerFighterDB PlayerFighterTable;
    public PlayerUnitDB PlayerUnitTable;

    // Start is called before the first frame update
    void Start()
    {
        //�{�^�������őJ�ڂ�����ʂ̂��߉���炷
        SE.Play();

        //�f�[�^���[�h
        PlayerFighterTable.Load();
        PlayerUnitTable.Load();

        //��ʕ\������
        DisplayScreenStart();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

        //���N���b�N�Ń��j�b�g�I��
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerUnit"));
            if (col != null)
            {
                SE.Play();

                //�I�����Ă������j�b�g�̔w�i�𔒁A�V���ɑI���������j�b�g�w�i��΂�
                if(!PlayerUnitDataBaseAllList[SelectUnitNum - 1].SoriteFlg)
                {
                    GameObject.Find("Unit" + SelectUnitNum.ToString()).transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.white;
                }
                SelectUnitNum = int.Parse(col.gameObject.name.Replace("Unit", ""));
                col.gameObject.transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.green;

                //��ʉ�UI���N���A
                ClearUI();

                DisplayUnitUI();

                //�I����UI�\���ʒu����
                Vector2 UIPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, col.gameObject.transform.position);
                
                //�o�����[�h�̏ꍇ�͏o���{�^���A�����łȂ��ꍇ�͕Ґ�UI���o��
                if (Common.SortieMode)
                {
                    UIPosition.y -= 80;
                    SortieButton.GetComponent<RectTransform>().position = UIPosition;
                    SortieButton.SetActive(true);
                }
                else
                {
                    UIPosition.x -= 130;
                    UIPosition.y -= 150;
                    SelectUI.GetComponent<RectTransform>().position = UIPosition;
                    SelectUI.SetActive(true);
                }
                
                ColorUI.SetActive(false);
                StrategyUI.SetActive(false);
                NameUI.SetActive(false);
            }
        }
    }

    //��ʉ�UI���\��
    public void DisplayUnitUI()
    {
        List<PlayerFighter> SelectUnitFighterList = PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == SelectUnitNum);
        SelectUnitInfoUI.transform.Find("Text (UnitNum)").GetComponent<Text>().text = SelectUnitNum.ToString();
        SelectUnitInfoUI.transform.Find("Text (UnitName)").GetComponent<Text>().text = PlayerUnitDataBaseAllList[SelectUnitNum - 1].Name;

        //��������
        if (SelectUnitFighterList.Count > 0)
        {
            SelectUnitInfoUI.transform.Find("Text (UnitLeader)").GetComponent<Text>().text = SelectUnitFighterList[0].Name;
        }
        //�����l��
        SelectUnitInfoUI.transform.Find("Text (MemberCount)").GetComponent<Text>().text = SelectUnitFighterList.Count.ToString() + "�l";
        //�������j
        SelectUnitInfoUI.transform.Find("Text (Strategy)").GetComponent<Text>().text = Common.FighterStrategy(PlayerUnitDataBaseAllList[SelectUnitNum - 1].Strategy);

        //���������o�[���\��
        for (int i = 0; i < SelectUnitFighterList.Count; i++)
        {
            //���O
            UnitMemberInfoUI[i].transform.Find("Text (MemberName)").GetComponent<Text>().text = SelectUnitFighterList[i].Name;
            //����
            UnitMemberInfoUI[i].transform.Find("Text (MemberType)").GetComponent<Text>().text = Common.FighterType(SelectUnitFighterList[i].Type);
            //���x��
            UnitMemberInfoUI[i].transform.Find("Text (MemberLevel)").GetComponent<Text>().text = "Lv" + SelectUnitFighterList[i].Level.ToString();
        }
    }

    //��ʕ\������
    public void DisplayScreenStart()
    {
        //DB�f�[�^�擾
        PlayerUnitDataBaseAllList = PlayerUnitTable.PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //���j�b�g�ԍ����ɕ��ёւ�
        PlayerFighterDataBaseAllList = PlayerFighterTable.PlayerFighterDBList.OrderByDescending((n) => n.UnitLeader).ToList(); //���j�b�g���[�_�[�����ɗ���悤�ɂɕ��ёւ�
        
        //�������j�b�g��
        int UnitCount = PlayerUnitDataBaseAllList.Count;

        //��ʕ\������
        for (int i = 0; i < UnitCount; i++)
        {
            UnitUI[i].transform.Find("Text (UnitName)").GetComponent<Text>().text = PlayerUnitDataBaseAllList[i].Name;

            //�������j�b�g��UI�w�i�𔒂�
            UnitOblect[i].transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.white;
            //���C���[�ŏ������Ă邩�𔻒f���邽�ߒǉ�
            UnitOblect[i].layer = LayerMask.NameToLayer("PlayerUnit");

            //���j�b�g���̕��m�����i�[
            List<PlayerFighter> PlayerFighterDataBaseUnitList = PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == i + 1);

            //���j�b�g���Ƃɕ��m��\��
            for (int j = 0; j < PlayerFighterDataBaseUnitList.Count; j++)
            {
                GameObject Fighter = null;

                switch (PlayerFighterDataBaseUnitList[j].Type)
                {
                    case 1:
                        Fighter = Instantiate(EmptyInfantry, new Vector3(0, 0, 0), Quaternion.identity);
                        break;
                    case 2:
                        Fighter = Instantiate(EmptyArcher, new Vector3(0, 0, 0), Quaternion.identity);
                        break;
                    case 3:
                        Fighter = Instantiate(EmptyShielder, new Vector3(0, 0, 0), Quaternion.identity);
                        break;
                    case 4:
                        Fighter = Instantiate(EmptyCavalry, new Vector3(0, 0, 0), Quaternion.identity);
                        break;
                    default:
                        break;
                }

                Fighter.GetComponent<SpriteRenderer>().color = PlayerUnitDataBaseAllList[i].UnitColor;
                Fighter.transform.parent = UnitOblect[i].transform;
                Fighter.transform.localPosition = PlayerFighterDataBaseUnitList[j].Position / 2;
            }

            //�o�����[�h�̏ꍇ
            if(PlayerUnitDataBaseAllList[i].SoriteFlg)
            {
                UnitOblect[i].layer = LayerMask.NameToLayer("Default");
                UnitOblect[i].transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.gray;
                GameObject s = Instantiate(SortieNow, RectTransformUtility.WorldToScreenPoint(Camera.main, UnitOblect[i].transform.position), Quaternion.identity);
                s.transform.SetParent(canvas.transform, true);
            }
        }

        //����������������ĂȂ��ꍇ�A���b�N�{�^����\��
        if (UnitCount < 10 && !Common.SortieMode)
        {
            LockButton.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, UnitOblect[UnitCount].transform.position);
            LockButton.SetActive(true);
        }
    }

    //��ʉ���UI���N���A
    private void ClearUI()
    {
        SelectUnitInfoUI.transform.Find("Text (UnitLeader)").GetComponent<Text>().text = string.Empty;

        foreach (GameObject MemberUI in UnitMemberInfoUI)
        {
            MemberUI.transform.Find("Text (MemberName)").GetComponent<Text>().text = string.Empty;
            MemberUI.transform.Find("Text (MemberType)").GetComponent<Text>().text = string.Empty;
            MemberUI.transform.Find("Text (MemberLevel)").GetComponent<Text>().text = string.Empty;
        }
    }

    //���b�N�{�^������
    public void LockButtonClick()
    {
        LockUI.SetActive(true);
    }

    //�I���{�^��
    public void EndButtonClick()
    {
        //�f�[�^�Z�[�u
        PlayerFighterTable.Save();
        PlayerUnitTable.Save();

        Common.SelectUnitNum = 0;

        //�o�g���V�[�����A�N�e�B�u������
        if(Common.BattleMode)
        {
            Scene Bscene = SceneManager.GetSceneByName("BattleScene" + Common.Progress.ToString());

            foreach (var root in Bscene.GetRootGameObjects())
            {
                root.SetActive(true);
            }

            SceneManager.UnloadSceneAsync("UnitEditScene");
        }
        else
        {
            SceneManager.LoadScene("SettingScene");
        }
    }
}
