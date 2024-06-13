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

    //ユニットDBと兵士DB
    public List<PlayerUnit> PlayerUnitDataBaseAllList;
    public List<PlayerUnit> PlayerUnitDataBaseSelectList;
    public List<PlayerFighter> PlayerFighterDataBaseAllList;
    public List<PlayerFighter> PlayerFighterDataBaseSelectList;

    //ユニット詳細UI（画面下）
    public GameObject SelectUnitInfoUI;
    public GameObject[] UnitMemberInfoUI;

    public GameObject HelpUI;
    public GameObject SaveUI;
    public GameObject EndUI;
    public GameObject WarningUI;
    public GameObject WarningUIMax;

    //ユニット表示オブジェクト
    public GameObject UnitObjectBack;

    //兵士ステータスUI
    public FighterStatusInfoUI FighterStatusInfo;

    //各兵士のプレハブ
    public GameObject InfantryPrefab;
    public GameObject ArcherPrefab;
    public GameObject ShielderPrefab;
    public GameObject CavalryPrefab;

    //直接クリックして選択した兵士
    private GameObject SelectFighter = null;
    private GameObject StatusShowFighter = null;

    //直接クリックして選択した兵士の名前
    private string StatusShowFighterName;

    //追尾テキストゲーム表示用キャンパス
    public GameObject CanvasWorldSpace;
    //兵士名追尾テキストプレハブ
    public GameObject ChaseFighterNameText;

    //控え兵士UI スクロールビュー
    public GameObject ReserveFighterView;
    //控え兵士UI表示用ボタンプレハブ
    public GameObject ReserveFighterButton;
    
    //クリックされた控え兵士ボタン
    public GameObject SelectFighterButton;

    //ダブルクリック判定用　カウント変数
    private int ClickCount = 0;

    //部隊方針保持
    private int UnitStrategy = 0;

    //ボタン押下SE
    public AudioSource SE;

    //DB
    public PlayerFighterDB PlayerFighterTable;
    public PlayerUnitDB PlayerUnitTable;

    // Start is called before the first frame update
    void Start()
    {
        //ダブルクリック判定を行うため、この画面では時間を動かす
        Time.timeScale = 1;
        //ボタン押下で遷移される画面のため音を鳴らす
        SE.Play();
        //データロード
        PlayerFighterTable.Load();
        PlayerUnitTable.Load();

        //DBデータ取得
        PlayerUnitDataBaseAllList = PlayerUnitTable.PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //ユニット番号順に並び替え
        PlayerFighterDataBaseAllList = PlayerFighterTable.PlayerFighterDBList
            .OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ThenBy((n) => n.Type).ToList(); //部隊番号順、部隊長が上に来るように、兵種順に並び替え

        //スクロールバーが必要か否かで、ボタンの位置調整
        if(PlayerFighterDataBaseAllList.Count >= 10)
        {
            ReserveFighterView.GetComponent<VerticalLayoutGroup>().padding.right = 20;
        }
        else
        {
            ReserveFighterView.GetComponent<VerticalLayoutGroup>().padding.right = 40;
        }

        //対象ユニットのデータ検索
        PlayerUnitDataBaseSelectList = PlayerUnitDataBaseAllList.FindAll(n => n.Num == Common.SelectUnitNum);
        PlayerFighterDataBaseSelectList = PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == Common.SelectUnitNum);

        //画面左下　ユニット情報UI表記
        SelectUnitInfoUI.transform.Find("Text (UnitNum)").GetComponent<Text>().text = Common.SelectUnitNum.ToString();
        SelectUnitInfoUI.transform.Find("Text (UnitName)").GetComponent<Text>().text = PlayerUnitDataBaseSelectList[0].Name;
        //部隊長名
        if (PlayerFighterDataBaseSelectList.Count > 0)
        {
            SelectUnitInfoUI.transform.Find("Text (UnitLeader)").GetComponent<Text>().text = PlayerFighterDataBaseSelectList[0].Name;
        }
        //部隊人数
        SelectUnitInfoUI.transform.Find("Text (MemberCount)").GetComponent<Text>().text = PlayerFighterDataBaseSelectList.Count.ToString() + "人";
        //部隊方針
        UnitStrategy = PlayerUnitDataBaseSelectList[0].Strategy;
        SelectUnitInfoUI.transform.Find("Text (Strategy)").GetComponent<Text>().text = Common.FighterStrategy(UnitStrategy);
       

        //所属している兵士を画面に表示する
        for (int i = 0; i < PlayerFighterDataBaseSelectList.Count; i++)
        {
            GameObject Fighter = null;
            PlayerFighter FighterStatusList = PlayerFighterDataBaseSelectList[i];

            //兵種によってオブジェクトを作成
            Fighter = Instantiate(FighterPrefab(FighterStatusList.Type), new Vector3(0, 0, 0), Quaternion.identity);
            Fighter.GetComponent<SpriteRenderer>().color = PlayerUnitDataBaseSelectList[0].UnitColor;

            //作成した兵士オブジェクトにステータスをつける
            Common.GetFighterStatusFromDB(Fighter.GetComponent<FighterStatus>(), FighterStatusList);

            //画面左のオブジェクトと紐づける
            Fighter.transform.parent = UnitObjectBack.transform;
            Fighter.transform.localPosition = FighterStatusList.Position;

            //画面右下ユニットメンバーUI記載
            UnitMemberInfoWrite(i, FighterStatusList.Name, FighterStatusList.Type, FighterStatusList.Level);

            //兵士の名前を表示する
            UnitFighterNameWrite(Fighter);
        }

        ReserveFighterViewDisplay();

        if (PlayerPrefs.GetInt("FormationHelp",0) == 0)
        {
            PlayerPrefs.SetInt("FormationHelp", 1);
            HelpUI.SetActive(true);
        }
    }

    
    private void OnDisable()
    {
        Time.timeScale = 0;
    }

    //控え兵士ビュー更新
    private void ReserveFighterViewDisplay()
    {
        //選択されている兵士の名前をdestroy前に保存
        string SelectN = string.Empty;
        if (SelectFighterButton)
        {
            SelectN = SelectFighterButton.GetComponent<FighterStatus>().FighterName;
        }

        //兵士ビューリセット
            foreach (Transform f in ReserveFighterView.transform)
        {
            GameObject.Destroy(f.gameObject);
        }

        //今フィールドにいる兵士の名前をNameListに保持
        GameObject[] Fighters = GameObject.FindGameObjectsWithTag("PlayerFighter");
        List<string> NameList = new List<string>();
        foreach (GameObject Fighter in Fighters)
        {
            FighterStatus fs = Fighter.GetComponent<FighterStatus>();
            NameList.Add(fs.FighterName);
        }

        //控え兵士の数分、ボタンを作成
        foreach (PlayerFighter Fighter in PlayerFighterDataBaseAllList)
        {
            GameObject button = Instantiate(ReserveFighterButton, ReserveFighterView.transform);
            button.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-9f, 80);
            button.transform.Find("Text (Name)").GetComponent<Text>().text = Fighter.Name;

            if (Fighter.UnitNum == 0)
            {
                button.transform.Find("Text (UnitName)").GetComponent<Text>().text = "なし";
            }
            else
            {
                button.transform.Find("Text (UnitName)").GetComponent<Text>().text = PlayerUnitDataBaseAllList[Fighter.UnitNum - 1].Name;
            }

            button.transform.Find("Text (Type)").GetComponent<Text>().text = Common.FighterType(Fighter.Type);
            button.transform.Find("Text (Level)").GetComponent<Text>().text = Fighter.Level.ToString();

            //作成したボタンに兵士ステータスをつける
            Common.GetFighterStatusFromDB(button.GetComponent<FighterStatus>(), Fighter);

            //既にこのユニット内にいる兵士のボタンは押せないようにする
            if(NameList.Contains(Fighter.Name))
            {
                button.GetComponent<Button>().interactable = false;
            }
            else if (Fighter.UnitNum != 0 && Fighter.UnitNum != Common.SelectUnitNum)
            {
                //どこかの部隊に所属済みの兵士はボタンの色を変える
                button.GetComponent<Image>().color = Color.yellow;
            }
            button.GetComponent<Button>().onClick.AddListener(FighterButtonClick);

            //ビュー更新を行っても選択されているボタンを保持する
            if (SelectN == button.GetComponent<FighterStatus>().FighterName)
            {
                SelectFighterButton = button;
                button.GetComponent<Button>().interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

        //左クリックでユニットの選択か追加、ドラッグで移動、ダブルクリックで部隊長変更
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter"));
            if (col != null)
            {
                SE.Play();

                //選択されている兵士が既にいる場合、その兵士の見た目を元に戻す
                if(StatusShowFighter != null)
                {
                    StatusShowFighter.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.clear;
                }

                //ボタンで選択されている兵士がいる場合、再度選択できるようにボタン状態を元に戻す
                if(SelectFighterButton != null)
                {
                    SelectFighterButton.GetComponent<Button>().interactable = true;
                }
                SelectFighter = col.gameObject;
                StatusShowFighter = col.gameObject;

                FighterStatus fs = StatusShowFighter.GetComponent<FighterStatus>();
                StatusShowFighterName = fs.FighterName;

                //見た目を変更
                StatusShowFighter.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.yellow;

                //バフ設定
                Common.FighterBuff(fs, UnitStrategy, false);

                //ステータスUI表示
                FighterStatusInfo.TextWrite(StatusShowFighter.GetComponent<FighterStatus>());
                FighterStatusInfo.ImageWrite(StatusShowFighter.GetComponent<SpriteRenderer>().sprite, StatusShowFighter.GetComponent<SpriteRenderer>().color);

                //ダブルクリックか判定
                ClickCount++;
                Invoke("OnDoubleClick", 0.3f);
            }
            else if(SelectFighterButton != null && !SelectFighterButton.GetComponent<Button>().interactable)
            {
                //左のユニット編成用オブジェクト内でない場合return
                if (CursorPosition.x > UnitObjectBack.transform.position.x + 2.5 || CursorPosition.x < UnitObjectBack.transform.position.x - 2.5 ||
                    CursorPosition.y > UnitObjectBack.transform.position.y + 2 || CursorPosition.y < UnitObjectBack.transform.position.y - 1.3)
                {
                    return;
                }

                GameObject Fighter = null;
                FighterStatus SelectStatus = SelectFighterButton.GetComponent<FighterStatus>();

                //MAX10人まで
                int FighterCount = GameObject.FindGameObjectsWithTag("PlayerFighter").Length;

                if(FighterCount == 10)
                {
                    WarningUIMax.SetActive(true);
                }
                else
                {
                    SE.Play();
                    //ボタンで選択された兵士を作成してクリック位置に表示する
                    Fighter = Instantiate(FighterPrefab(SelectStatus.Type), CursorPosition, Quaternion.identity);
                    Fighter.transform.parent = UnitObjectBack.transform;

                    //ボタン内にあるステータスをファイターオブジェクトにコピー
                    FighterStatus fs = Fighter.GetComponent<FighterStatus>();
                    Common.FighterStatusCopy(fs, SelectStatus);
                    Fighter.GetComponent<SpriteRenderer>().color = PlayerUnitDataBaseSelectList[0].UnitColor;

                    FighterCount = GameObject.FindGameObjectsWithTag("PlayerFighter").Length;
                    //一人の場合、強制部隊長
                    if (FighterCount == 1)
                    {
                        fs.UnitLeader = true;
                        SelectUnitInfoUI.transform.Find("Text (UnitLeader)").GetComponent<Text>().text = fs.FighterName;
                    }
                    else
                    {
                        fs.UnitLeader = false; //部隊長が二人以上になることを防ぐため
                    }

                    //兵士の名前を表示する
                    UnitFighterNameWrite(Fighter);

                    SelectFighterButton = null;

                    //画面左下ユニットUI記載変更
                    SelectUnitInfoUI.transform.Find("Text (MemberCount)").GetComponent<Text>().text = FighterCount.ToString() + "人";

                    //画面下ユニットメンバーUI追記
                    UnitMemberInfoWrite(FighterCount - 1, SelectStatus.FighterName, SelectStatus.Type, SelectStatus.Level);
                }
            }
        }
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (SelectFighter != null)
            {
                //左のユニット編成用オブジェクト内でない場合無効
                if (CursorPosition.x > UnitObjectBack.transform.position.x + 2.5 || CursorPosition.x < UnitObjectBack.transform.position.x - 2.5 ||
                    CursorPosition.y > UnitObjectBack.transform.position.y + 2 || CursorPosition.y < UnitObjectBack.transform.position.y - 1.3)
                {
                    return;
                }
                SelectFighter.transform.position = CursorPosition;　//兵士移動
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            SelectFighter = null;
        }

        //右クリックで兵士削除　
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter"));
            if (col != null)
            {
                SE.Play();
                StartCoroutine(DestroyFighter(col));
            }
        }
    }

    //兵士削除　Destroy判定にタイムラグがあるのでコルーチンにしてなんとかする
    IEnumerator DestroyFighter(Collider2D col)
    {
        string DestroyName = col.gameObject.GetComponent<FighterStatus>().FighterName;
        bool LeaderDestroy = col.gameObject.GetComponent<FighterStatus>().UnitLeader;
        Destroy(col.gameObject);
        UnitMemberInfoClear();

        yield return new WaitForSecondsRealtime(0.1f);

        //選択中の兵士を削除した場合、ステータスUIを空白で表示する
        FighterStatusInfo.Clear(DestroyName);

        GameObject[] Fighters = GameObject.FindGameObjectsWithTag("PlayerFighter");

        //画面左下ユニットUI記載変更
        int FighterCount = Fighters.Length;
        SelectUnitInfoUI.transform.Find("Text (MemberCount)").GetComponent<Text>().text = FighterCount.ToString() + "人";

        //部隊長が削除されたら別の兵士に自動で割り当てる
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
                //画面下ユニットメンバーUI記載
                UnitMemberInfoWrite(i, fs.FighterName, fs.Type, fs.Level);
            }
        }
        else
        {
            //画面右下ユニットメンバーUI記載変更
            foreach (GameObject Fighter in Fighters)
            {
                FighterStatus fs = Fighter.GetComponent<FighterStatus>();

                if (fs.UnitLeader)
                {
                    //画面下ユニットメンバーUI記載
                    UnitMemberInfoWrite(0, fs.FighterName, fs.Type, fs.Level);
                }
                else
                {
                    NoLeaderCount++;

                    //画面下ユニットメンバーUI記載
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

    //左ダブルクリックで部隊長変更
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

                    //画面下ユニットメンバーUI記載
                    UnitMemberInfoWrite(NoLeaderCount, fs.FighterName, fs.Type, fs.Level);
                }
                else
                {
                    fs.UnitLeader = true;
                    SelectUnitInfoUI.transform.Find("Text (UnitLeader)").GetComponent<Text>().text = fs.FighterName;

                    //画面下ユニットメンバーUI記載
                    UnitMemberInfoWrite(0, fs.FighterName, fs.Type, fs.Level);

                    //ステータスUI更新(ステータスバフ変更のため)
                    Common.FighterBuff(fs, UnitStrategy, false);
                    FighterStatusInfo.TextWrite(fs);
                }
            }
        }
        ClickCount = 0;
    }

    //兵士のボタンクリックイベント
    public void FighterButtonClick()
    {
        SE.Play();

        //ボタンで選択されている兵士がいる場合、再度選択できるようにボタン状態を元に戻す
        if (SelectFighterButton != null)
        {
            SelectFighterButton.GetComponent<Button>().interactable = true;
        }

        SelectFighterButton = eventSystem.currentSelectedGameObject;
        SelectFighterButton.GetComponent<Button>().interactable = false;
        FighterStatus SelectStatus = SelectFighterButton.GetComponent<FighterStatus>();

        //バフ設定 ※控え兵士が別の部隊のリーダーであってもそのバフを表示させるとややこしいため　true
        Common.FighterBuff(SelectStatus, UnitStrategy, true);

        //ステータスUI表示
        FighterStatusInfo.TextWrite(SelectStatus);
        //兵士画像表示
        FighterStatusInfo.ImageWrite(FighterPrefab(SelectStatus.Type).GetComponent<SpriteRenderer>().sprite, PlayerUnitDataBaseSelectList[0].UnitColor);

        if(SelectStatus.UnitNum != 0 && SelectStatus.UnitNum != Common.SelectUnitNum)
        {
            WarningUI.SetActive(true);
        }
    }

    //画面下　ユニットメンバーUI描画
    private void UnitMemberInfoWrite(int position, string name, int type, int level)
    {
        //名前
        UnitMemberInfoUI[position].transform.Find("Text (MemberName)").GetComponent<Text>().text = name;
        //兵種
        UnitMemberInfoUI[position].transform.Find("Text (MemberType)").GetComponent<Text>().text = Common.FighterType(type);
        //レベル
        UnitMemberInfoUI[position].transform.Find("Text (MemberLevel)").GetComponent<Text>().text = "Lv" + level.ToString();
    }

    //画面下　ユニットメンバーUIクリア
    private void UnitMemberInfoClear()
    {
        foreach(GameObject MemberInfo in UnitMemberInfoUI)
        {
            //名前
            MemberInfo.transform.Find("Text (MemberName)").GetComponent<Text>().text = string.Empty;
            //兵種
            MemberInfo.transform.Find("Text (MemberType)").GetComponent<Text>().text = string.Empty;
            //レベル
            MemberInfo.transform.Find("Text (MemberLevel)").GetComponent<Text>().text = string.Empty;
        }
    }

    //兵士の名前を表示する
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

    //保存
    public void SaveButtonClick()
    {
        GameObject[] Fighters = GameObject.FindGameObjectsWithTag("PlayerFighter");
        List<string> NameList = new List<string>();

        foreach (GameObject Fighter in Fighters)
        {
            FighterStatus fs = Fighter.GetComponent<FighterStatus>();
            List<PlayerFighter> pfl = PlayerFighterDataBaseAllList.FindAll(n => n.Name == fs.FighterName);

            //変更する前の情報を保持しておく
            int originNum = pfl[0].UnitNum;
            bool originLeader = pfl[0].UnitLeader;

            //データ変更
            pfl[0].Position = new Vector3(float.Parse(Fighter.transform.localPosition.x.ToString("F1")), float.Parse(Fighter.transform.localPosition.y.ToString("F1")), 0); //桁数が多くならないよう小数点二位で切る
            pfl[0].UnitNum = Common.SelectUnitNum;
            pfl[0].UnitLeader = fs.UnitLeader;

            NameList.Add(fs.FighterName);

            //他の部隊の部隊長を編成していた場合、元の部隊の部隊長を自動的に編成する
            if (originNum != 0 && originNum != Common.SelectUnitNum && originLeader && PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == originNum).Count > 0)
            {
                PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == originNum)[0].UnitLeader = true;
            }
        }

        //削除された兵士は部隊ナンバーを0とする
        List<PlayerFighter> pfl2 = PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == Common.SelectUnitNum);
        foreach (PlayerFighter pf in pfl2)
        {
            if(!NameList.Contains(pf.Name))
            {
                PlayerFighterDataBaseAllList.Find(n => n.Name == pf.Name).UnitNum = 0;
            }
        }

        //データセーブ
        PlayerFighterTable.Save();
        PlayerUnitTable.Save();

        SaveUI.SetActive(true);
    }

    //最大人数オーバー警告　OKボタン押下時
    public void WarningMaxOK()
    {
        WarningUIMax.SetActive(false);
    }

    //各並べ替えボタン処理
    public void UnitOrderbyChange(bool on)
    {
        if (on)
        {
            SE.Play();
            PlayerFighterDataBaseAllList = PlayerFighterTable.PlayerFighterDBList
            .OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ThenBy((n) => n.Type).ToList(); //部隊番号順、部隊長が上に来るように、兵種順に並び替え

            ReserveFighterViewDisplay();
        }
    }
    public void NameOrderbyChange(bool on)
    {
        if (on)
        {
            SE.Play();
            PlayerFighterDataBaseAllList = PlayerFighterTable.PlayerFighterDBList
            .OrderBy((n) => n.Name).ToList(); //名前順に並び替え

            ReserveFighterViewDisplay();
        }
    }
    public void LevelOrderbyChange(bool on)
    {
        if (on)
        {
            SE.Play();
            PlayerFighterDataBaseAllList = PlayerFighterTable.PlayerFighterDBList
            .OrderBy((n) => n.Level).ToList(); //Lv順に並び替え

            ReserveFighterViewDisplay();
        }
    }

    private GameObject FighterPrefab(int type)
    {
        switch (type)
        {
            case 1:
                return InfantryPrefab;
            case 2:
                return ArcherPrefab;
            case 3:
                return ShielderPrefab;
            case 4:
                return CavalryPrefab;
            default:
                return null;
        }
    }
}
