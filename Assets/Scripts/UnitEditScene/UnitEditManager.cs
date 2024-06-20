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

    //ユニットUIとゲームオブジェクト
    public GameObject[] UnitUI;
    public GameObject[] UnitOblect;

    //選択中のユニット詳細UI（画面下）
    public GameObject SelectUnitInfoUI;
    public GameObject[] UnitMemberInfoUI;

    //ユニット選択後、表示するUI
    public GameObject SelectUI;
    public GameObject StrategyUI;
    public GameObject ColorUI;
    public GameObject NameUI;
    public GameObject SortieButton;

    //出撃中表示
    public GameObject SortieNow;

    //部隊開放用ロックボタン
    public GameObject LockButton;
    public GameObject LockUI;

    //各兵士のプレハブ（画像のみ）
    public GameObject EmptyInfantry;
    public GameObject EmptyArcher;
    public GameObject EmptyShielder;
    public GameObject EmptyCavalry;

    //ユニットDBと兵士DB
    public List<PlayerUnit> PlayerUnitDataBaseAllList;
    public List<PlayerFighter> PlayerFighterDataBaseAllList;

    //選択中のユニットナンバー
    public int SelectUnitNum = 1;

    //ボタン押下SE
    public AudioSource SE;

    //DB
    public PlayerFighterDB PlayerFighterTable;
    public PlayerUnitDB PlayerUnitTable;

    // Start is called before the first frame update
    void Start()
    {
        //ボタン押下で遷移される画面のため音を鳴らす
        SE.Play();

        //データロード
        PlayerFighterTable.Load();
        PlayerUnitTable.Load();

        //画面表示処理
        DisplayScreenStart();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

        //左クリックでユニット選択
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerUnit"));
            if (col != null)
            {
                SE.Play();

                //選択していたユニットの背景を白、新たに選択したユニット背景を緑に
                if(!PlayerUnitDataBaseAllList[SelectUnitNum - 1].SoriteFlg)
                {
                    GameObject.Find("Unit" + SelectUnitNum.ToString()).transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.white;
                }
                SelectUnitNum = int.Parse(col.gameObject.name.Replace("Unit", ""));
                col.gameObject.transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.green;

                //画面下UI情報クリア
                ClearUI();

                DisplayUnitUI();

                //選択語UI表示位置調整
                Vector2 UIPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, col.gameObject.transform.position);
                
                //出撃モードの場合は出撃ボタン、そうでない場合は編成UIを出す
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

    //画面下UI情報表示
    public void DisplayUnitUI()
    {
        List<PlayerFighter> SelectUnitFighterList = PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == SelectUnitNum);
        SelectUnitInfoUI.transform.Find("Text (UnitNum)").GetComponent<Text>().text = SelectUnitNum.ToString();
        SelectUnitInfoUI.transform.Find("Text (UnitName)").GetComponent<Text>().text = PlayerUnitDataBaseAllList[SelectUnitNum - 1].Name;

        //部隊長名
        if (SelectUnitFighterList.Count > 0)
        {
            SelectUnitInfoUI.transform.Find("Text (UnitLeader)").GetComponent<Text>().text = SelectUnitFighterList[0].Name;
        }
        //部隊人数
        SelectUnitInfoUI.transform.Find("Text (MemberCount)").GetComponent<Text>().text = SelectUnitFighterList.Count.ToString() + "人";
        //部隊方針
        SelectUnitInfoUI.transform.Find("Text (Strategy)").GetComponent<Text>().text = Common.FighterStrategy(PlayerUnitDataBaseAllList[SelectUnitNum - 1].Strategy);

        //部隊メンバー情報表示
        for (int i = 0; i < SelectUnitFighterList.Count; i++)
        {
            //名前
            UnitMemberInfoUI[i].transform.Find("Text (MemberName)").GetComponent<Text>().text = SelectUnitFighterList[i].Name;
            //兵種
            UnitMemberInfoUI[i].transform.Find("Text (MemberType)").GetComponent<Text>().text = Common.FighterType(SelectUnitFighterList[i].Type);
            //レベル
            UnitMemberInfoUI[i].transform.Find("Text (MemberLevel)").GetComponent<Text>().text = "Lv" + SelectUnitFighterList[i].Level.ToString();
        }
    }

    //画面表示処理
    public void DisplayScreenStart()
    {
        //DBデータ取得
        PlayerUnitDataBaseAllList = PlayerUnitTable.PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //ユニット番号順に並び替え
        PlayerFighterDataBaseAllList = PlayerFighterTable.PlayerFighterDBList.OrderByDescending((n) => n.UnitLeader).ToList(); //ユニットリーダーが頭に来るようにに並び替え
        
        //所持ユニット数
        int UnitCount = PlayerUnitDataBaseAllList.Count;

        //画面表示処理
        for (int i = 0; i < UnitCount; i++)
        {
            UnitUI[i].transform.Find("Text (UnitName)").GetComponent<Text>().text = PlayerUnitDataBaseAllList[i].Name;

            //所持ユニットのUI背景を白に
            UnitOblect[i].transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.white;
            //レイヤーで所持してるかを判断するため追加
            UnitOblect[i].layer = LayerMask.NameToLayer("PlayerUnit");

            //ユニット内の兵士情報を格納
            List<PlayerFighter> PlayerFighterDataBaseUnitList = PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == i + 1);

            //ユニットごとに兵士を表示
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

            //出撃モードの場合
            if(PlayerUnitDataBaseAllList[i].SoriteFlg)
            {
                UnitOblect[i].layer = LayerMask.NameToLayer("Default");
                UnitOblect[i].transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.gray;
                GameObject s = Instantiate(SortieNow, RectTransformUtility.WorldToScreenPoint(Camera.main, UnitOblect[i].transform.position), Quaternion.identity);
                s.transform.SetParent(canvas.transform, true);
            }
        }

        //部隊を解放しきってない場合、ロックボタンを表示
        if (UnitCount < 10 && !Common.SortieMode)
        {
            LockButton.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, UnitOblect[UnitCount].transform.position);
            LockButton.SetActive(true);
        }
    }

    //画面下のUI情報クリア
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

    //ロックボタン押下
    public void LockButtonClick()
    {
        LockUI.SetActive(true);
    }

    //終了ボタン
    public void EndButtonClick()
    {
        //データセーブ
        PlayerFighterTable.Save();
        PlayerUnitTable.Save();

        Common.SelectUnitNum = 0;

        //バトルシーンをアクティブ化する
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
