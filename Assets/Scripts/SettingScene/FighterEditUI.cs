using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

//兵士一覧UI
public class FighterEditUI : MonoBehaviour
{
    public SettingManager SeManager;
    [SerializeField] private EventSystem eventSystem;

    //各UI
    public GameObject NameUI;
    public GameObject NameWarningUI;
    public GameObject LevelUpUI;
    public GameObject DismissalUI;

    //名前入力フィールド
    public InputField NameField;

    //ユニットDBと兵士DB
    public List<PlayerUnit> PlayerUnitDataBaseAllList;
    public List<PlayerFighter> PlayerFighterDataBaseAllList;

    //兵士UI スクロールビュー
    public GameObject FighterView;

    //兵士UI表示用ボタンプレハブ
    public GameObject FighterButton;

    //兵士の絵
    public Sprite InfantryImage;
    public Sprite ArcherImage;

    //兵数テキスト
    public Text CountText;

    //兵士ステータスUI
    public FighterStatusInfoUI FighterStatusInfo;

    //並べ替えボタン
    public Toggle UnitOrderby;
    public Toggle NameOrderby;
    public Toggle LevelOrderby;

    //兵士を選択しないと押せないボタン
    public Button NameChangeButton;
    public Button LevelUpButton;
    public Button DismissalButton;

    //クリックされた兵士ボタンとステータス
    public GameObject SelectFighterButton;
    public FighterStatus SelectFighterStatus;

    //ボタン押下SE
    public AudioSource SE;

    void OnEnable()
    {
        //DBデータ取得
        PlayerUnitDataBaseAllList = Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //ユニット番号順に並び替え
        PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList
            .OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ThenBy((n) => n.Type).ToList(); //部隊番号順、部隊長が上に来るように、兵種順に並び替え

        FighterViewDisplay();
    }

    private void OnDisable()
    {
        //初期化
        //並べ替えを初期表示に
        NameOrderby.isOn = false;
        LevelOrderby.isOn = false;
        UnitOrderby.isOn = true;

        //各ボタンを押せないようにする
        ButtonClickChange(false);

        //ステータスUIを空白にする
        if(SelectFighterStatus)
        {
            FighterStatusInfo.Clear(SelectFighterStatus.FighterName);
        }

        SelectFighterStatus = null;
        SelectFighterButton = null;
    }

    //兵士ビュー表示
    public void FighterViewDisplay()
    {
        //兵士ビューリセット
        foreach (Transform f in FighterView.transform)
        {
            GameObject.Destroy(f.gameObject);
        }

        CountText.text = PlayerFighterDataBaseAllList.Count.ToString() + "/120";

        //スクロールバーが必要か否かで、ボタンの位置調整
        if (PlayerFighterDataBaseAllList.Count >= 10)
        {
            FighterView.GetComponent<VerticalLayoutGroup>().padding.right = 20;
        }
        else
        {
            FighterView.GetComponent<VerticalLayoutGroup>().padding.right = 40;
        }

        //兵士の数分、ボタンを作成
        foreach (PlayerFighter Fighter in PlayerFighterDataBaseAllList)
        {
            GameObject button = Instantiate(FighterButton, FighterView.transform);
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

            button.GetComponent<Button>().onClick.AddListener(FighterButtonClick);

            //選択されている兵士のボタンは押せないようにする
            if(Fighter.Name == FighterStatusInfo.transform.Find("StatusTexts/Text (Name)").GetComponent<Text>().text)
            {
                button.GetComponent<Button>().interactable = false;
                SelectFighterButton = button;
            }
        }
    }

    //兵士を選択してる間だけ押せるボタンの状態管理
    public void ButtonClickChange(bool b)
    {
        NameChangeButton.interactable = b;
        LevelUpButton.interactable = b;
        DismissalButton.interactable = b;
    }

    //兵士のボタンクリックイベント
    public void FighterButtonClick()
    {
        SE.Play();

        //各ボタンを押せるようにする
        ButtonClickChange(true);

        //ボタンで選択されている兵士がいる場合、再度選択できるようにボタン状態を元に戻す
        if (SelectFighterButton != null)
        {
            SelectFighterButton.GetComponent<Button>().interactable = true;
        }

        SelectFighterButton = eventSystem.currentSelectedGameObject;
        SelectFighterButton.GetComponent<Button>().interactable = false;
        SelectFighterStatus = SelectFighterButton.GetComponent<FighterStatus>();

        //ステータスUI表示
        FighterStatusInfo.TextWrite(SelectFighterStatus);
        //兵士の色
        Color UnitC = Color.white;
        if(SelectFighterStatus.UnitNum != 0)
        {
            UnitC = PlayerUnitDataBaseAllList[SelectFighterStatus.UnitNum - 1].UnitColor;
        }

        switch (SelectFighterStatus.Type)
        {
            case 1:
                FighterStatusInfo.ImageWrite(InfantryImage, UnitC);
                break;
            case 2:
                FighterStatusInfo.ImageWrite(ArcherImage, UnitC);
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }
    }

    //各並べ替えボタン処理
    public void UnitOrderbyChange(bool on)
    {
        if (on)
        {
            PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList
            .OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ThenBy((n) => n.Type).ToList(); //部隊番号順、部隊長が上に来るように、兵種順に並び替え

            FighterViewDisplay();
        }
    }
    public void NameOrderbyChange(bool on)
    {
        if (on)
        {
            PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList
            .OrderBy((n) => n.Name).ToList(); //名前順に並び替え

            FighterViewDisplay();
        }
    }
    public void LevelOrderbyChange(bool on)
    {
        if (on)
        {
            PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList
            .OrderBy((n) => n.Level).ToList(); //Lv順に並び替え

            FighterViewDisplay();
        }
    }

    public void NameChangeButtonClick()
    {
        NameUI.SetActive(true);
        NameField.text = SelectFighterStatus.FighterName;
        NameField.ActivateInputField();
    }

    //決定ボタン押下で兵士名変更
    public void DecisionName()
    {
        if (NameField.text != SelectFighterStatus.FighterName && PlayerFighterDataBaseAllList.FindAll((n) => n.Name == NameField.text).Count > 0)
        {
            NameWarningUI.SetActive(true);
        }
        else
        {
            PlayerFighterDataBaseAllList.Find((n) => n.Name == SelectFighterStatus.FighterName).Name = NameField.text;
            SelectFighterStatus.FighterName = NameField.text;
            NameUI.SetActive(false);
            FighterStatusInfo.TextWrite(SelectFighterStatus);
            FighterViewDisplay();
        }
    }

    //レベルアップボタンクリック
    public void LevelUpButtonClick()
    {
        LevelUpUI.SetActive(true);
    }

    //解雇ボタンクリック
    public void DismissalButtonClick()
    {
        DismissalUI.SetActive(true);
    }

    //戻るボタンクリック
    public void BackButtonClick()
    {
        SeManager.HomeUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
