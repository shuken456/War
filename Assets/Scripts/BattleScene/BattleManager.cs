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
    public Button InstructionButton;
    public GameObject SelectFighterInfoUI;

    //�I�𒆂̕��m
    public List<GameObject> SelectFighter = new List<GameObject>();
    public List<LineRenderer> SelectFighterLine = new List<LineRenderer>();

    //���j�b�gDB�ƕ��mDB
    public List<PlayerUnit> PlayerUnitDataBaseAllList;
    public List<PlayerFighter> PlayerFighterDataBaseAllList;

    // Start is called before the first frame update
    void Start()
    {
        Common.BattleMode = true;
        InstructionButton.gameObject.SetActive(true);

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
}
