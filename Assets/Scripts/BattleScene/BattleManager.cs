using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    //各UI
    public GameObject ActionUI;
    public GameObject MoveUI;
    public GameObject SortieUI;
    public GameObject InstructionButton;
    public GameObject FighterStatusInfoUI;

    //選択中の兵士
    public List<GameObject> SelectFighter = new List<GameObject>();
    public List<LineRenderer> SelectFighterLine = new List<LineRenderer>();

    //ユニットDBと兵士DB
    public List<PlayerUnit> PlayerUnitDataBaseAllList;
    public List<PlayerFighter> PlayerFighterDataBaseAllList;

    //各ステージの敵
    public GameObject[] StageEnemy;

    //HP,スタミナゲージ
    public GameObject HpGaugePrefab;
    public GameObject StaminaGaugePrefab;

    //ゲージ表示用キャンバス
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

        //DBデータ取得
        PlayerUnitDataBaseAllList = Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //ユニット番号順に並び替え
        PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList
            .OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ToList(); //部隊番号順、部隊長が上に来るように並び替え
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
