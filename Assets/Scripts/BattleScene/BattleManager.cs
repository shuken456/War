using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    //各UI
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

    //選択中の兵士
    public List<GameObject> SelectFighter = new List<GameObject>();
    public List<LineRenderer> SelectFighterLine = new List<LineRenderer>();

    //ユニットDBと兵士DB
    public List<PlayerUnit> PlayerUnitDataBaseAllList;
    public List<PlayerFighter> PlayerFighterDataBaseAllList;

    //各ステージの敵
    public GameObject[] StageEnemy;

    //HP,スタミナゲージ,リーダーマーク
    public GameObject HpGaugePrefab;
    public GameObject StaminaGaugePrefab;
    public GameObject LeaderFlagPrefab;

    //ゲージ表示用キャンバス
    public GameObject CanvasWorldSpace;

    //戦を開始しているか否か
    public bool StartFlg = false;

    //経験値ディクショナリ
    public Dictionary<string, int> ExpDic = new Dictionary<string, int>();

    //勝ちフラグ
    public bool WinFlg = true;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        Common.BattleMode = true;

        //DBデータ取得
        PlayerUnitDataBaseAllList = Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //ユニット番号順に並び替え
        PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList
            .OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ToList(); //部隊番号順、部隊長が上に来るように並び替え

        //敵を生成
        foreach (Transform Enemy in StageEnemy[Common.SelectStageNum - 1].transform)
        {
            var obj = Instantiate(Enemy.gameObject);
            CreateGaugeAndFlag(obj);
        }
        StartUI.SetActive(true);
    }

    private void OnEnable()
    {
        //ユニット一覧画面から出撃する部隊を選ばれたときは出撃UIを表示する
        if(Common.SelectUnitNum != 0)
        {
            StartUI.SetActive(false);
            ActionUI.SetActive(false);
            SortieUI.SetActive(true);
        }
    }

    //体力、スタミナゲージ、リーダーマーク作成
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

    //勝利
    public void BattleWin()
    {
        WinFlg = true;
        ResultUI.SetActive(true);
    }

    //敗北
    public void BattleLose()
    {
        WinFlg = false;
        ResultUI.SetActive(true);
    }
}
