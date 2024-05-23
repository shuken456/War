using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    //�eUI
    public GameObject StartUI;
    public GameObject ActionUI;
    public GameObject MoveUI;
    public GameObject SortieUI;
    public GameObject InstructionButton;
    public GameObject FighterStatusInfoUI;
    public LogUI LogUI;
    public UnitCountUI UnitCountUI;
    public GameObject TimeStopText;
    public GameObject ResultUI;

    //�I�𒆂̕��m
    public List<GameObject> SelectFighter = new List<GameObject>();
    public List<LineRenderer> SelectFighterLine = new List<LineRenderer>();

    //���j�b�gDB�ƕ��mDB
    public List<PlayerUnit> PlayerUnitDataBaseAllList;
    public List<PlayerFighter> PlayerFighterDataBaseAllList;

    //�e�X�e�[�W�̓G
    public GameObject[] StageEnemy;

    //HP,�X�^�~�i�Q�[�W,���[�_�[�}�[�N
    public GameObject HpGaugePrefab;
    public GameObject StaminaGaugePrefab;
    public GameObject LeaderFlagPrefab;

    //�Q�[�W�\���p�L�����o�X
    public GameObject CanvasWorldSpace;

    //����J�n���Ă��邩�ۂ�
    public bool StartFlg = false;

    //�o���l�f�B�N�V���i��
    public Dictionary<string, int> ExpDic = new Dictionary<string, int>();

    //�����t���O
    public bool WinFlg = true;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        Common.BattleMode = true;

        //DB�f�[�^�擾
        PlayerUnitDataBaseAllList = Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //���j�b�g�ԍ����ɕ��ёւ�
        PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList
            .OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ToList(); //�����ԍ����A����������ɗ���悤�ɕ��ёւ�

        //�G�𐶐�
        foreach (Transform Enemy in StageEnemy[Common.SelectStageNum - 1].transform)
        {
            var obj = Instantiate(Enemy.gameObject);
            CreateGaugeAndFlag(obj);
        }
        StartUI.SetActive(true);
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

    //�̗́A�X�^�~�i�Q�[�W�A���[�_�[�}�[�N�쐬
    public void CreateGaugeAndFlag(GameObject targetObject)
    {
        var Hpgauge = Instantiate(HpGaugePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Hpgauge.transform.SetParent(CanvasWorldSpace.transform, false);
        Hpgauge.GetComponent<HpGauge>().targetFighter = targetObject;

        var Staminagauge = Instantiate(StaminaGaugePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Staminagauge.transform.SetParent(CanvasWorldSpace.transform, false);
        Staminagauge.GetComponent<StaminaGauge>().targetFighter = targetObject;

        if(targetObject.GetComponent<FighterStatus>().UnitLeader)
        {
            var LeaderFlag = Instantiate(LeaderFlagPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            LeaderFlag.transform.SetParent(CanvasWorldSpace.transform, false);
            LeaderFlag.GetComponent<LeaderFlag>().targetFighter = targetObject;
        }
    }

    //����
    public void BattleWin()
    {
        WinFlg = true;
        ResultUI.SetActive(true);
    }

    //�s�k
    public void BattleLose()
    {
        WinFlg = false;
        ResultUI.SetActive(true);
    }
}
