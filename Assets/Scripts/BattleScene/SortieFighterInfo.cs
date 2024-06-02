using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using Cinemachine;

public class SortieFighterInfo : MonoBehaviour
{
    public BattleManager BaManager;

    [SerializeField] private EventSystem eventSystem;

    //兵士一覧
    public GameObject SortieFighterView;

    //兵士一覧用ボタンプレハブ
    public GameObject SortieFighterButton;

    //カメラ
    public CinemachineVirtualCamera mainCam;

    private List <FighterStatus> PlayerFighters = new List<FighterStatus>();

    private void OnEnable()
    {
        PlayerFighters = new List<FighterStatus>();
        GameObject[] Fighters = GameObject.FindGameObjectsWithTag("PlayerFighter");

        foreach (GameObject Fighter in Fighters)
        {
            PlayerFighters.Add(Fighter.GetComponent<FighterStatus>());
        }

        //スクロールバーが必要か否かで、ボタンの位置調整
        if (PlayerFighters.Count >= 10)
        {
            SortieFighterView.GetComponent<VerticalLayoutGroup>().padding.right = 20;
        }
        else
        {
            SortieFighterView.GetComponent<VerticalLayoutGroup>().padding.right = 40;
        }

        SortieFighterViewDisplay();
    }

    //出撃中兵士ビュー更新
    private void SortieFighterViewDisplay()
    {
        //兵士ビューリセット
        foreach (Transform f in SortieFighterView.transform)
        {
            GameObject.Destroy(f.gameObject);
        }

        //今フィールドにいる兵士の数分、ボタンを作成
        foreach (FighterStatus fs in PlayerFighters)
        {
            GameObject button = Instantiate(SortieFighterButton, SortieFighterView.transform);
            button.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-9f, 80);

            button.transform.Find("Text (Name)").GetComponent<Text>().text = fs.FighterName;
            button.transform.Find("Text (UnitName)").GetComponent<Text>().text = BaManager.PlayerUnitDataBaseAllList[fs.UnitNum - 1].Name;
            button.transform.Find("Text (Type)").GetComponent<Text>().text = Common.FighterType(fs.Type);
            button.transform.Find("Text (Level)").GetComponent<Text>().text = fs.Level.ToString();

            //作成したボタンに兵士ステータスをつける
            Common.FighterStatusCopy(button.GetComponent<FighterStatus>(), fs);
            button.GetComponent<Button>().onClick.AddListener(FighterButtonClick);
        }
    }

    //兵士のボタンクリックイベント
    public void FighterButtonClick()
    {
        FighterStatus fs = PlayerFighters.Find((n) => n.FighterName == eventSystem.currentSelectedGameObject.GetComponent<FighterStatus>().FighterName);
        mainCam.transform.position = fs.gameObject.transform.position - new Vector3(0, 0, 1);//その兵士が画面中央に来るようにする

        //その兵士を選択中とする
        if (!BaManager.SelectFighter.Contains(fs.gameObject))
        {
            BaManager.SelectFighter.Add(fs.gameObject);

            //選択兵士ビュー更新
            BaManager.ActionUI.transform.Find("SelectFighterInfoUI").GetComponent<SelectFighterInfoUI>().UpdateView();

            //選択中の見た目を変更
            fs.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        BaManager.ActionUI.GetComponent<ActionUI>().ChangeButton();

        this.gameObject.SetActive(false);
    }

    //各並べ替えボタン処理
    public void UnitOrderbyChange(bool on)
    {
        if (on)
        {
            PlayerFighters = PlayerFighters.OrderBy((n) => n.UnitNum).ThenByDescending((n) => n.UnitLeader).ThenBy((n) => n.Type).ToList(); //部隊番号順、部隊長が上に来るように、兵種順に並び替え
            SortieFighterViewDisplay();
        }
    }
    public void NameOrderbyChange(bool on)
    {
        if (on)
        {
            PlayerFighters = PlayerFighters.OrderBy((n) => n.FighterName).ToList(); //名前順に並び替え
            SortieFighterViewDisplay();
        }
    }
    public void LevelOrderbyChange(bool on)
    {
        if (on)
        {
            PlayerFighters = PlayerFighters.OrderBy((n) => n.Level).ToList(); //Lv順に並び替え
            SortieFighterViewDisplay();
        }
    }

    //閉じるボタン押下
    public void SearchClose()
    {
        this.gameObject.SetActive(false);
    }
}
