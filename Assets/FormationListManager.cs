using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class FormationListManager : MonoBehaviour
{
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

    //部隊開放用ロックボタン
    public GameObject LockButton;

    //各兵士のプレハブ（画像のみ）
    public GameObject EmptyInfantry;
    public GameObject EmptyArcher;

    //ユニットDBと兵士DB
    public List<PlayerUnit> PlayerUnitDataBaseAllList;
    public List<PlayerFighter> PlayerFighterDataBaseAllList;

    //選択中のユニットナンバー
    public int SelectUnitNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        //DBデータ取得
        PlayerUnitDataBaseAllList = Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //ユニット番号順に並び替え
        PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList.OrderByDescending((n) => n.UnitLeader).ToList(); //ユニットリーダーが頭に来るようにに並び替え

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
                //選択していたユニットの背景を白、新たに選択したユニット背景を緑に
                GameObject.Find("Unit" + SelectUnitNum.ToString()).transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.white;
                SelectUnitNum = int.Parse(col.gameObject.name.Replace("Unit", ""));
                col.gameObject.transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.green;

                //画面下UI情報クリア
                ClearUI();

                DisplayUnitUI();

                //選択語UI表示位置調整
                Vector2 UIPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, col.gameObject.transform.position);
                UIPosition.x -= 130;
                UIPosition.y -= 150;
                SelectUI.GetComponent<RectTransform>().position = UIPosition;
                SelectUI.SetActive(true);
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

        //ユニット名
        if (SelectUnitFighterList.Count > 0)
        {
            SelectUnitInfoUI.transform.Find("Text (UnitLeader)").GetComponent<Text>().text = SelectUnitFighterList[0].Name;
        }
        //部隊人数
        SelectUnitInfoUI.transform.Find("Text (MemberCount)").GetComponent<Text>().text = SelectUnitFighterList.Count.ToString() + "人";
        //部隊方針
        switch (PlayerUnitDataBaseAllList[SelectUnitNum - 1].Strategy)
        {
            case 1:
                SelectUnitInfoUI.transform.Find("Text (Strategy)").GetComponent<Text>().text = "攻撃重視";
                break;
            case 2:
                SelectUnitInfoUI.transform.Find("Text (Strategy)").GetComponent<Text>().text = "耐久重視";
                break;
            case 3:
                SelectUnitInfoUI.transform.Find("Text (Strategy)").GetComponent<Text>().text = "移動重視";
                break;
        }

        //部隊メンバー情報表示
        for (int i = 0; i < SelectUnitFighterList.Count; i++)
        {
            //名前
            UnitMemberInfoUI[i].transform.Find("Text (MemberName)").GetComponent<Text>().text = SelectUnitFighterList[i].Name;
            //兵種
            switch (SelectUnitFighterList[i].type)
            {
                case 1:
                    UnitMemberInfoUI[i].transform.Find("Text (MemberType)").GetComponent<Text>().text = "歩兵";
                    break;
                case 2:
                    UnitMemberInfoUI[i].transform.Find("Text (MemberType)").GetComponent<Text>().text = "弓兵";
                    break;
                case 3:
                    UnitMemberInfoUI[i].transform.Find("Text (MemberType)").GetComponent<Text>().text = "盾兵";
                    break;
                case 4:
                    UnitMemberInfoUI[i].transform.Find("Text (MemberType)").GetComponent<Text>().text = "騎兵";
                    break;
            }
            //レベル
            UnitMemberInfoUI[i].transform.Find("Text (MemberLevel)").GetComponent<Text>().text = "Lv" + SelectUnitFighterList[i].Level.ToString();
        }
    }

    //画面表示処理
    public void DisplayScreenStart()
    {
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

                if (PlayerFighterDataBaseUnitList[j].type == 1)
                {
                    Fighter = Instantiate(EmptyInfantry, new Vector3(0, 0, 0), Quaternion.identity);
                }
                else if (PlayerFighterDataBaseUnitList[j].type == 2)
                {
                    Fighter = Instantiate(EmptyArcher, new Vector3(0, 0, 0), Quaternion.identity);
                }

                Fighter.GetComponent<SpriteRenderer>().color = PlayerUnitDataBaseAllList[i].UnitColor;
                Fighter.transform.parent = UnitOblect[i].transform;
                Fighter.transform.localPosition = PlayerFighterDataBaseUnitList[j].Position;
            }
        }

        //部隊を解放しきってない場合、ロックボタンを表示
        if (UnitCount < 10)
        {
            LockButton.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, UnitOblect[UnitCount].transform.position);
            LockButton.SetActive(true);
        }
    }

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
}
