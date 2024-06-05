using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    //各UI
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

    //選択中の兵士
    public List<GameObject> SelectFighter = new List<GameObject>();
    public List<LineRenderer> SelectFighterLine = new List<LineRenderer>();

    //ユニットDBと兵士DB
    public List<PlayerUnit> PlayerUnitDataBaseAllList;
    public List<PlayerFighter> PlayerFighterDataBaseAllList;

    //HP,スタミナゲージ等をまとめた追尾オブジェクト
    public GameObject ChaseObjectPrefab;

    //ゲージ表示用キャンバス
    public GameObject CanvasWorldSpace;

    //戦を開始しているか否か
    public bool StartFlg = false;

    //経験値ディクショナリ
    public Dictionary<string, int> ExpDic = new Dictionary<string, int>();

    //勝ちフラグ
    public bool WinFlg = true;

    //DB
    [SerializeField]
    PlayerFighterDB PlayerFighterTable;
    [SerializeField]
    PlayerUnitDB PlayerUnitTable;

    //各BGM
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
        //BGM格納
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

    //シーン起動時
    private IEnumerator StartSetting()
    {
        Time.timeScale = 0;
        Common.BattleMode = true;

        //データロード
        PlayerFighterTable.Load();
        PlayerUnitTable.Load();

        //DBデータ取得
        PlayerUnitDataBaseAllList = Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //ユニット番号順に並び替え
        PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList
            .OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ToList(); //部隊番号順、部隊長が上に来るように並び替え

        /////////
        Common.Progress = 1;

        //ステージ名表示
        StartDisplay.transform.Find("Text (StageNum)").GetComponent<Text>().text = "ステージ" + Common.Progress.ToString();
        StartDisplay.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        StartDisplay.gameObject.SetActive(false);

        
        //敵のゲージを生成
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
        //ユニット一覧画面から出撃する部隊を選ばれたときは出撃UIを表示する
        if(Common.SelectUnitNum != 0)
        {
            StartUI.SetActive(false);
            ActionUI.SetActive(false);
            SortieUI.SetActive(true);
        }
    }

    private void OnDisable()
    {
        //データセーブ
        PlayerFighterTable.Save();
        PlayerUnitTable.Save();
    }


    //戦開始ボタンクリック
    public void BattleStart()
    {
        StartCoroutine(BattleStartSetting());
    }

    private IEnumerator BattleStartSetting()
    {
        StartUI.GetComponent<StartUI>().DeleteMoveRoute();

        //それぞれの変数と画面状態を元に戻す
        foreach (GameObject Fighter in SelectFighter)
        {
            Fighter.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.clear;
        }
        SelectFighter.Clear();
        SelectFighterLine.Clear();
        StartFlg = true;
        
        Time.timeScale = 1;

        StartUI.gameObject.SetActive(false);

        //ホラ貝を鳴らす
        SettingBGM.Stop();
        HoragaiSE.Play();

        //戦闘開始ホラ貝表示
        BattleStartDisplay.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        BattleStartDisplay.gameObject.SetActive(false);

        //戦術指示ボタン表示
        InstructionButton.gameObject.SetActive(true);

        if (BattleStartHelpUI)
        {
            BattleStartHelpUI.SetActive(true);
        }

        //戦闘BGMを鳴らす
        BattleMusic().Play();
        VoiceBGM.Play();
    }


    //体力、スタミナゲージ等追尾オブジェクト作成
    public void CreateGaugeAndFlag(GameObject targetObject)
    {
        var ChaseObject = Instantiate(ChaseObjectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        ChaseObject.transform.SetParent(CanvasWorldSpace.transform, false);
        ChaseObject.GetComponent<ChaseObject>().targetFighter = targetObject;
    }

    //勝利
    public void BattleWin()
    {
        StartFlg = false;
        BattleMusic().Stop();
        VoiceBGM.Stop();
        WinFlg = true;

        //ステージ20でクリア
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

    //敗北
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
