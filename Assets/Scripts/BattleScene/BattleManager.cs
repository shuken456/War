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
    public PlayerFighterDB PlayerFighterTable;
    public PlayerUnitDB PlayerUnitTable;

    //�eBGM
    private AudioSource SettingBGM;
    private AudioSource VoiceBGM;
    private AudioSource BattleBGM;
    private AudioSource HoragaiSE;
    public AudioSource ButtonSE;
    private AudioSource WinBGM;
    private AudioSource LoseBGM;

    //�ڕW�n�_
    public GameObject StageTargetPlace;

    // Start is called before the first frame update
    void Start()
    {
        //BGM�i�[
        SettingBGM = GameObject.Find("SettingBGM").GetComponent<AudioSource>();
        VoiceBGM = GameObject.Find("VoiceBGM").GetComponent<AudioSource>();
        BattleBGM = GameObject.Find("BattleBGM").GetComponent<AudioSource>();
        HoragaiSE = GameObject.Find("HoragaiSE").GetComponent<AudioSource>();
        ButtonSE = GameObject.Find("ButtonSE").GetComponent<AudioSource>();
        WinBGM = GameObject.Find("WinBGM").GetComponent<AudioSource>();
        LoseBGM = GameObject.Find("LoseBGM").GetComponent<AudioSource>();

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
        PlayerUnitDataBaseAllList = PlayerUnitTable.PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //���j�b�g�ԍ����ɕ��ёւ�
        PlayerFighterDataBaseAllList = PlayerFighterTable.PlayerFighterDBList
            .OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ToList(); //�����ԍ����A����������ɗ���悤�ɕ��ёւ�

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

        //�ڕW�n�_������Ε\��
        if (StageTargetPlace)
        {
            StageTargetPlace.SetActive(true);
            StartCoroutine(ShowPlace());
        }

        StartUI.SetActive(true);

        //���[�U�[�Ɍ�����w���v������Ε\��
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
            ButtonSE.Play();
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

        //���[�U�[�Ɍ�����w���v������Ε\��
        if (BattleStartHelpUI)
        {
            BattleStartHelpUI.SetActive(true);
        }

        //�퓬BGM��炷
        BattleBGM.Play();
        VoiceBGM.Play();
    }


    //�̗́A�X�^�~�i�Q�[�W���ǔ��I�u�W�F�N�g�쐬
    public void CreateGaugeAndFlag(GameObject targetObject)
    {
        var ChaseObject = Instantiate(ChaseObjectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        ChaseObject.transform.SetParent(CanvasWorldSpace.transform, false);
        ChaseObject.GetComponent<ChaseObject>().targetFighter = targetObject;

        //�ړ�UI���O�ɗ��Ȃ��悤�ɂ���
        ChaseObject.transform.SetAsFirstSibling();
    }

    //���R���j�p�����Ăяo���iondestroy�ŃR���[�`�����ĂׂȂ����߁j
    public void Win()
    {
        StartCoroutine(BattleWin());
    }

    //����
    public IEnumerator BattleWin()
    {
        StartFlg = false;
        WinFlg = true;
        Time.timeScale = 0;
        
        yield return new WaitForSecondsRealtime(1.5f);

        BattleBGM.Stop();
        VoiceBGM.Stop();
        WinBGM.Play();

        //�X�e�[�W20�ŃN���A
        if (Common.Progress < 20)
        {
            ResultUI.SetActive(true);
        }
        else
        {
            GameClearUI.SetActive(true);
        }
    }

    //�s�k
    public IEnumerator BattleLose()
    {
        Time.timeScale = 0;
        StartFlg = false;
        WinFlg = false;

        yield return new WaitForSecondsRealtime(1.5f);

        BattleBGM.Stop();
        VoiceBGM.Stop();
        LoseBGM.Play();
        ResultUI.SetActive(true);
    }


    //�ڕW�n�_���ǂ���������
    private IEnumerator ShowPlace()
    {
        Vector3 pos = GameObject.Find("Virtual Camera").transform.position;
        GameObject.Find("Virtual Camera").transform.position = StageTargetPlace.transform.position - new Vector3(0, 0, 1);
        yield return new WaitForSecondsRealtime(2f);
        GameObject.Find("Virtual Camera").transform.position = pos;
    }

}
