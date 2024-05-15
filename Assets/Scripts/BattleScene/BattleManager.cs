using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    //�eUI
    public GameObject ActionUI;
    public GameObject MoveUI;
    public GameObject SortieUI;
    public GameObject InstructionButton;
    public GameObject FighterStatusInfoUI;

    //�I�𒆂̕��m
    public List<GameObject> SelectFighter = new List<GameObject>();
    public List<LineRenderer> SelectFighterLine = new List<LineRenderer>();

    //���j�b�gDB�ƕ��mDB
    public List<PlayerUnit> PlayerUnitDataBaseAllList;
    public List<PlayerFighter> PlayerFighterDataBaseAllList;

    //�e�X�e�[�W�̓G
    public GameObject[] StageEnemy;

    //HP,�X�^�~�i�Q�[�W
    public GameObject HpGaugePrefab;
    public GameObject StaminaGaugePrefab;

    //�Q�[�W�\���p�L�����o�X
    public GameObject CanvasWorldSpace;

    // Start is called before the first frame update
    void Start()
    {
        Common.BattleMode = true;
        
        foreach(Transform Enemy in StageEnemy[Common.SelectStageNum - 1].transform)
        {
            var obj = Instantiate(Enemy.gameObject);
            CreateGauge(obj);
        }

        InstructionButton.SetActive(true);

        //DB�f�[�^�擾
        PlayerUnitDataBaseAllList = Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //���j�b�g�ԍ����ɕ��ёւ�
        PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList
            .OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ToList(); //�����ԍ����A����������ɗ���悤�ɕ��ёւ�
    }

    private void OnEnable()
    {
        if(Common.SelectUnitNum != 0)
        {
            ActionUI.SetActive(false);
            SortieUI.SetActive(true);
        }
    }

    public void CreateGauge(GameObject targetObject)
    {
        var Hpgauge = Instantiate(HpGaugePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Hpgauge.transform.SetParent(CanvasWorldSpace.transform, false);
        Hpgauge.GetComponent<HpGauge>().targetFighter = targetObject;

        var Staminagauge = Instantiate(StaminaGaugePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Staminagauge.transform.SetParent(CanvasWorldSpace.transform, false);
        Staminagauge.GetComponent<StaminaGauge>().targetFighter = targetObject;
    }
}
