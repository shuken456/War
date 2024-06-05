using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    //�eUI
    public GameObject StartDisplay;
    public GameObject StartHelpUI;
    public GameObject StartUI;
    public GameObject BattleStartDisplay;
    public GameObject BattleStartHelpUI;
    public GameObject ActionUI;
    public GameObject MoveUI;
    public GameObject SortieUI;
    public GameObject InstructionButton;
    public GameObject FighterStatusInfoUI;
    public LogUI LogUI;
    public UnitCountUI UnitCountUI;
    public GameObject TimeStopText;
    public GameObject ResultUI;
    public GameObject BattleInfoUI;
    public GameObject GameClearUI;

    //�I�𒆂̕��m
    public List<GameObject> SelectFighter = new List<GameObject>();
    public List<LineRenderer> SelectFighterLine = new List<LineRenderer>();

    //���j�b�gDB�ƕ��mDB
    public List<PlayerUnit> PlayerUnitDataBaseAllList;
    public List<PlayerFighter> PlayerFighterDataBaseAllList;

    //HP,�X�^�~�i�Q�[�W�����܂Ƃ߂��ǔ��I�u�W�F�N�g
    public GameObject ChaseObjectPrefab;

    //�Q�[�W�\���p�L�����o�X
    public GameObject CanvasWorldSpace;

    //����J�n���Ă��邩�ۂ�
    public bool StartFlg = false;

    //�o���l�f�B�N�V���i��
    public Dictionary<string, int> ExpDic = new Dictionary<string, int>();

    //�����t���O
    public bool WinFlg = true;

    //DB
    [SerializeField]
    PlayerFighterDB PlayerFighterTable;
    [SerializeField]
    PlayerUnitDB PlayerUnitTable;

    //�eBGM
    private AudioSource SettingBGM;
    private AudioSource VoiceBGM;
    private AudioSource BattleBGM;
    private AudioSource LastBattleBGM;
    private AudioSource HoragaiSE;
    public AudioSource ButtonSE;
    private AudioSource WinBGM;
    private AudioSource LoseBGM;
    private AudioSource GameClearBGM;

    // Start is called before the first frame update
    void Start()
    {
        //BGM�i�[
        SettingBGM = GameObject.Find("SettingBGM").GetComponent<AudioSource>();
        VoiceBGM = GameObject.Find("VoiceBGM").GetComponent<AudioSource>();
        BattleBGM = GameObject.Find("BattleBGM").GetComponent<AudioSource>();
        LastBattleBGM = GameObject.Find("LastBattleBGM").GetComponent<AudioSource>();
        HoragaiSE = GameObject.Find("HoragaiSE").GetComponent<AudioSource>();
        ButtonSE = GameObject.Find("ButtonSE").GetComponent<AudioSource>();
        WinBGM = GameObject.Find("WinBGM").GetComponent<AudioSource>();
        LoseBGM = GameObject.Find("LoseBGM").GetComponent<AudioSource>();
        GameClearBGM = GameObject.Find("GameClearBGM").GetComponent<AudioSource>();

        StartCoroutine(StartSetting());
    }

    //�V�[���N����
    private IEnumerator StartSetting()
    {
        Time.timeScale = 0;
        Common.BattleMode = true;

        //�f�[�^���[�h
        PlayerFighterTable.Load();
        PlayerUnitTable.Load();

        //DB�f�[�^�擾
        PlayerUnitDataBaseAllList = Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //���j�b�g�ԍ����ɕ��ёւ�
        PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList
            .OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ToList(); //�����ԍ����A����������ɗ���悤�ɕ��ёւ�

        /////////
        Common.Progress = 1;

        //�X�e�[�W���\��
        StartDisplay.transform.Find("Text (StageNum)").GetComponent<Text>().text = "�X�e�[�W" + Common.Progress.ToString();
        StartDisplay.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        StartDisplay.gameObject.SetActive(false);

        
        //�G�̃Q�[�W�𐶐�
        foreach (GameObject Enemy in GameObject.FindGameObjectsWithTag("EnemyFighter"))
        {
            CreateGaugeAndFlag(Enemy);
        }

        BattleInfoUI.SetActive(true);
        StartUI.SetActive(true);

        if(StartHelpUI)
        {
            StartHelpUI.SetActive(true);
        }
    }

    private void OnEnable()
    {
        //���j�b�g�ꗗ��ʂ���o�����镔����I�΂ꂽ�Ƃ��͏o��UI��\������
        if(Common.SelectUnitNum != 0)
        {
            StartUI.SetActive(false);
            ActionUI.SetActive(false);
            SortieUI.SetActive(true);
        }
    }

    private void OnDisable()
    {
        //�f�[�^�Z�[�u
        PlayerFighterTable.Save();
        PlayerUnitTable.Save();
    }


    //��J�n�{�^���N���b�N
    public void BattleStart()
    {
        StartCoroutine(BattleStartSetting());
    }

    private IEnumerator BattleStartSetting()
    {
        StartUI.GetComponent<StartUI>().DeleteMoveRoute();

        //���ꂼ��̕ϐ��Ɖ�ʏ�Ԃ����ɖ߂�
        foreach (GameObject Fighter in SelectFighter)
        {
            Fighter.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.clear;
        }
        SelectFighter.Clear();
        SelectFighterLine.Clear();
        StartFlg = true;
        
        Time.timeScale = 1;

        StartUI.gameObject.SetActive(false);

        //�z���L��炷
        SettingBGM.Stop();
        HoragaiSE.Play();

        //�퓬�J�n�z���L�\��
        BattleStartDisplay.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        BattleStartDisplay.gameObject.SetActive(false);

        //��p�w���{�^���\��
        InstructionButton.gameObject.SetActive(true);

        if (BattleStartHelpUI)
        {
            BattleStartHelpUI.SetActive(true);
        }

        //�퓬BGM��炷
        BattleMusic().Play();
        VoiceBGM.Play();
    }


    //�̗́A�X�^�~�i�Q�[�W���ǔ��I�u�W�F�N�g�쐬
    public void CreateGaugeAndFlag(GameObject targetObject)
    {
        var ChaseObject = Instantiate(ChaseObjectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        ChaseObject.transform.SetParent(CanvasWorldSpace.transform, false);
        ChaseObject.GetComponent<ChaseObject>().targetFighter = targetObject;
    }

    //����
    public void BattleWin()
    {
        StartFlg = false;
        BattleMusic().Stop();
        VoiceBGM.Stop();
        WinFlg = true;

        //�X�e�[�W20�ŃN���A
        if(Common.Progress < 20)
        {
            WinBGM.Play();
            ResultUI.SetActive(true);
        }
        else
        {
            GameClearBGM.Play();
            GameClearUI.SetActive(true);
        }
    }

    //�s�k
    public void BattleLose()
    {
        StartFlg = false;
        BattleMusic().Stop();
        VoiceBGM.Stop();
        LoseBGM.Play();
        WinFlg = false;
        ResultUI.SetActive(true);
    }

    private AudioSource BattleMusic()
    {
        if (Common.Progress < 20)
        {
            return BattleBGM;
        }
        else
        {
            return LastBattleBGM;
        }
    }
}
