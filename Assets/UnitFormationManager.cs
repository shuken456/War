using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class UnitFormationManager : MonoBehaviour
{
    //ユニットDBと兵士DB
    public List<PlayerUnit> PlayerUnitDataBaseAllList;
    public List<PlayerUnit> PlayerUnitDataBaseSelectList;
    public List<PlayerFighter> PlayerFighterDataBaseAllList;
    public List<PlayerFighter> PlayerFighterDataBaseSelectList;

    //ユニット詳細UI（画面下）
    public GameObject SelectUnitInfoUI;
    public GameObject[] UnitMemberInfoUI;

    public GameObject UnitObjectBack;

    public GameObject FighterStatusInfoText;
    public GameObject FighterStatusInfoImage;

    //選択中されたユニットナンバー
    public int SelectUnitNum = 1;

    //各兵士のプレハブ
    public GameObject InfantryPrefab;
    public GameObject ArcherPrefab;

    private GameObject SelectFighter = null;
    private GameObject StatusShowFighter = null;

    private string StatusShowFighterName;

    //追尾テキストゲーム表示用キャンパス
    public GameObject CanvasWorldSpace;
    //兵士名追尾テキストプレハブ
    public GameObject ChaseFighterNameText;

    int ClickCount = 0;
    bool DoubleClickflg = false;

    public GameObject ReserveFighterButton;
    public GameObject ReserveFighterView;


    // Start is called before the first frame update
    void Start()
    {
        //DBデータ取得
        PlayerUnitDataBaseAllList = Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //ユニット番号順に並び替え
        PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList.OrderByDescending((n) => n.UnitLeader).ToList(); //ユニットリーダーが頭に来るようにに並び替え


        ///////
        foreach(PlayerFighter Fighter in PlayerFighterDataBaseAllList)
        {
            GameObject button = Instantiate(ReserveFighterButton, ReserveFighterView.transform);
            button.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-9f, 80);
        }



        //対象ユニットのデータ検索
        PlayerUnitDataBaseSelectList = PlayerUnitDataBaseAllList.FindAll(n => n.Num == SelectUnitNum);
        PlayerFighterDataBaseSelectList = PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == SelectUnitNum);

        //ユニット所属兵士数
        int SelectUnitFighterCount = PlayerFighterDataBaseSelectList.Count;

        //画面下UI
        SelectUnitInfoUI.transform.Find("Text (UnitNum)").GetComponent<Text>().text = SelectUnitNum.ToString();
        SelectUnitInfoUI.transform.Find("Text (UnitName)").GetComponent<Text>().text = PlayerUnitDataBaseSelectList[0].Name;
        //部隊長名
        if (PlayerFighterDataBaseSelectList.Count > 0)
        {
            SelectUnitInfoUI.transform.Find("Text (UnitLeader)").GetComponent<Text>().text = PlayerFighterDataBaseSelectList[0].Name;
        }
        //部隊人数
        SelectUnitInfoUI.transform.Find("Text (MemberCount)").GetComponent<Text>().text = PlayerFighterDataBaseSelectList.Count.ToString() + "人";
        //部隊方針
        SelectUnitInfoUI.transform.Find("Text (Strategy)").GetComponent<Text>().text = Common.FighterStrategy(PlayerUnitDataBaseSelectList[0].Strategy);
       

        //画面表示処理
        for (int i = 0; i < SelectUnitFighterCount; i++)
        {
            GameObject Fighter = null;
            PlayerFighter FighterStatusList = PlayerFighterDataBaseSelectList[i];

            if (FighterStatusList.Type == 1)
            {
                Fighter = Instantiate(InfantryPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else if (FighterStatusList.Type == 2)
            {
                Fighter = Instantiate(ArcherPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            }

            Fighter.GetComponent<SpriteRenderer>().color = PlayerUnitDataBaseSelectList[0].UnitColor;
            Fighter.transform.parent = UnitObjectBack.transform;
            Fighter.transform.localPosition = FighterStatusList.Position;

            FighterStatus PrefabStatus = Fighter.GetComponent<FighterStatus>();
            PrefabStatus.Type = FighterStatusList.Type;
            PrefabStatus.FighterName = FighterStatusList.Name;
            PrefabStatus.Level = FighterStatusList.Level;
            PrefabStatus.MaxHp = FighterStatusList.Hp;
            PrefabStatus.NowHp = FighterStatusList.Hp;
            PrefabStatus.MaxStamina = FighterStatusList.Stamina;
            PrefabStatus.NowStamina = FighterStatusList.Stamina;
            PrefabStatus.AtkPower = FighterStatusList.AtkPower;
            PrefabStatus.AtkSpeed = FighterStatusList.AtkSpeed;
            PrefabStatus.MoveSpeed = FighterStatusList.MoveSpeed;
            PrefabStatus.UnitLeader = FighterStatusList.UnitLeader;

            FighterNameChaseText NameText = Instantiate(ChaseFighterNameText, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<FighterNameChaseText>();
            NameText.transform.SetParent(CanvasWorldSpace.transform, false);
            NameText.targetFighter = Fighter;
            NameText.offset = new Vector3(0, 0.5f, 0);




            //名前
            UnitMemberInfoUI[i].transform.Find("Text (MemberName)").GetComponent<Text>().text = FighterStatusList.Name;
            //兵種
            UnitMemberInfoUI[i].transform.Find("Text (MemberType)").GetComponent<Text>().text = Common.FighterType(FighterStatusList.Type);
            //レベル
            UnitMemberInfoUI[i].transform.Find("Text (MemberLevel)").GetComponent<Text>().text = "Lv" + FighterStatusList.Level.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

        
        //左クリックでユニットの選択、ドラッグで移動、ダブルクリックで部隊長変更
        if (Input.GetMouseButtonDown(0))
        {
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter", "SelectFighter"));
            if (col != null)
            {

                if(StatusShowFighter != null)
                {
                    StatusShowFighter.layer = LayerMask.NameToLayer("PlayerFighter");
                }
                
                SelectFighter = col.gameObject;
                StatusShowFighter = col.gameObject;

                

                //見た目を変更
                StatusShowFighter.layer = LayerMask.NameToLayer("SelectFighter");

                FighterStatus fs = StatusShowFighter.GetComponent<FighterStatus>();
                StatusShowFighterName = fs.FighterName;
                FighterStatusInfoText.transform.Find("Text (Name)").GetComponent<Text>().text = fs.FighterName;
                FighterStatusInfoText.transform.Find("Text (Type)").GetComponent<Text>().text = Common.FighterType(fs.Type);
                FighterStatusInfoText.transform.Find("Text (Level)").GetComponent<Text>().text = fs.Level.ToString();
                FighterStatusInfoText.transform.Find("Text (Hp)").GetComponent<Text>().text = fs.MaxHp.ToString();
                FighterStatusInfoText.transform.Find("Text (Stamina)").GetComponent<Text>().text = fs.MaxStamina.ToString();
                FighterStatusInfoText.transform.Find("Text (AtkPower)").GetComponent<Text>().text = fs.AtkPower.ToString();
                FighterStatusInfoText.transform.Find("Text (AtkSpeed)").GetComponent<Text>().text = fs.AtkSpeed.ToString();
                FighterStatusInfoText.transform.Find("Text (MoveSpeed)").GetComponent<Text>().text = fs.MoveSpeed.ToString();

                FighterStatusInfoImage.GetComponent<Image>().sprite = StatusShowFighter.GetComponent<SpriteRenderer>().sprite;
                FighterStatusInfoImage.GetComponent<Image>().color = StatusShowFighter.GetComponent<SpriteRenderer>().color;

                ClickCount++;
                Invoke("OnDoubleClick", 0.3f);
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (SelectFighter != null)
            {
                if (CursorPosition.x > UnitObjectBack.transform.position.x + 2.5 || CursorPosition.x < UnitObjectBack.transform.position.x - 2.5 ||
                    CursorPosition.y > UnitObjectBack.transform.position.y + 2 || CursorPosition.y < UnitObjectBack.transform.position.y - 1.3)
                {
                    return;
                }

                SelectFighter.transform.position = CursorPosition;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            SelectFighter = null;
        }

        //右クリックで兵士削除　
        if (Input.GetMouseButtonDown(1))
        {
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter", "SelectFighter"));
            if (col != null)
            {
                
            }
        }
    }

    private void OnDoubleClick()
    {
        if (ClickCount != 2) { ClickCount = 0; return; }
        else { ClickCount = 0; }

        if (DoubleClickflg == false)
        {
            GameObject[] Fighters = GameObject.FindGameObjectsWithTag("PlayerFighter");

            int NoLeaderCount = 0;

            foreach(GameObject Fighter in Fighters)
            {
                FighterStatus fs = Fighter.GetComponent<FighterStatus>();

                if(fs.FighterName != StatusShowFighterName)
                {
                    fs.UnitLeader = false;

                    NoLeaderCount++;
                    //名前
                    UnitMemberInfoUI[NoLeaderCount].transform.Find("Text (MemberName)").GetComponent<Text>().text = fs.FighterName;
                    //兵種
                    UnitMemberInfoUI[NoLeaderCount].transform.Find("Text (MemberType)").GetComponent<Text>().text = Common.FighterType(fs.Type);
                    //レベル
                    UnitMemberInfoUI[NoLeaderCount].transform.Find("Text (MemberLevel)").GetComponent<Text>().text = "Lv" + fs.Level.ToString();
                }
                else
                {
                    fs.UnitLeader = true;
                    SelectUnitInfoUI.transform.Find("Text (UnitLeader)").GetComponent<Text>().text = fs.FighterName;

                    //名前
                    UnitMemberInfoUI[0].transform.Find("Text (MemberName)").GetComponent<Text>().text = fs.FighterName;
                    //兵種
                    UnitMemberInfoUI[0].transform.Find("Text (MemberType)").GetComponent<Text>().text = Common.FighterType(fs.Type);
                    //レベル
                    UnitMemberInfoUI[0].transform.Find("Text (MemberLevel)").GetComponent<Text>().text = "Lv" + fs.Level.ToString();
                }
            }

            //DoubleClickflg = true;
        }
        else
        {
            DoubleClickflg = false;
        }
    }
}
