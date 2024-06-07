using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    public BattleManager BaManager;

    //兵士リザルト スクロールビュー
    public GameObject FighterResultView;
    //兵士リザルトプレハブ
    public GameObject FighterResultUI;

    //出撃した兵士リスト
    public List<PlayerFighter> SortieFighterList;

    //兵士プレハブ
    public GameObject EmptyInfantry;
    public GameObject EmptyArcher;

    //勝ちか負けか表示するテキスト
    public Text ResultText;
    //獲得資金を表示するテキスト
    public Text MoneyText;

    //各ボタン
    public GameObject OkButton;
    public GameObject RevengeButton;
    public GameObject SettingButton;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        int GetMoney;

        if (BaManager.WinFlg)
        {
            ResultText.text = "勝利！";
            ResultText.color = Color.red;
            GetMoney = (Common.Progress * 2) + 3;
            OkButton.SetActive(true);
        }
        else
        {
            ResultText.text = "敗北…";
            ResultText.color = Color.blue;
            GetMoney = Common.Progress;
            RevengeButton.SetActive(true);
            SettingButton.SetActive(true);
        }

        //所持金プラス
        MoneyText.text = GetMoney.ToString() + "両"; 
        Common.Money += GetMoney;

        //経験値リストを作成
        GameObject[] tagObjects;
        tagObjects = GameObject.FindGameObjectsWithTag("PlayerFighter");
        foreach (GameObject Fighter in tagObjects)
        {
            FighterStatus fs = Fighter.GetComponent<FighterStatus>();
            if(!BaManager.ExpDic.ContainsKey(fs.FighterName))
            {
                BaManager.ExpDic.Add(fs.FighterName, fs.Exp);
                if (BaManager.WinFlg)
                {
                    BaManager.ExpDic[fs.FighterName] += 30; //生き残り経験値ボーナス30
                }
            }
        }

        if (BaManager.WinFlg)
        {
            List<string> nameList = BaManager.ExpDic.Keys.ToList();
            foreach (string name in nameList)
            {
                BaManager.ExpDic[name] += 30; //勝利ボーナス30
            }

        }

        //出撃した兵士を取得
        List<PlayerUnit> SortieUnit = BaManager.PlayerUnitDataBaseAllList.FindAll((n) => n.SoriteFlg);
        foreach (PlayerUnit pu in SortieUnit)
        {
            List<PlayerFighter> SortieFighter = BaManager.PlayerFighterDataBaseAllList.FindAll((n) => n.UnitNum == pu.Num);
            foreach (PlayerFighter pf in SortieFighter)
            {
                SortieFighterList.Add(pf);
            }
            pu.SoriteFlg = false;
        }

        //出撃した兵士分リザルトを作成
        foreach (PlayerFighter pf in SortieFighterList)
        {
            GameObject button = Instantiate(FighterResultUI, FighterResultView.transform);
            switch (pf.Type)
            {
                case 1:
                    button.transform.Find("FighterResultInfo/FighterBack/FighterImage").GetComponent<Image>().sprite = EmptyInfantry.GetComponent<SpriteRenderer>().sprite;
                    break;
                case 2:
                    button.transform.Find("FighterResultInfo/FighterBack/FighterImage").GetComponent<Image>().sprite = EmptyArcher.GetComponent<SpriteRenderer>().sprite;
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    break;
            }
            button.transform.Find("FighterResultInfo/FighterBack/FighterImage").GetComponent<Image>().color = BaManager.PlayerUnitDataBaseAllList.Find((n) => n.Num == pf.UnitNum).UnitColor;
            button.transform.Find("FighterResultInfo/StatusTexts/Text (Name)").GetComponent<Text>().text = pf.Name;
            
            //レベルアップ判定 元々の経験値を引くことで加算される経験値を求める
            int AddExp = Mathf.CeilToInt((BaManager.ExpDic[pf.Name] - pf.EXP) * ((float)Common.Progress / (float)pf.Level)); //ステージと現在のレベルによって経験値に補正をかける
            int SumExp = AddExp + pf.EXP;

            //上がるレベル数 ※経験値が100たまったら1レベルアップ
            int UpLevel = Mathf.FloorToInt(SumExp / 100);

            pf.EXP = SumExp - (100 * UpLevel);
            button.transform.Find("FighterResultInfo/StatusTexts/Text (AddExp)").GetComponent<Text>().text = "+" + AddExp.ToString();
            button.transform.Find("FighterResultInfo/StatusTexts/Text (NextExp)").GetComponent<Text>().text = (100 - pf.EXP).ToString();

            //パラメータアップ
            if (UpLevel > 0)
            {
                //上がるパラメータリスト
                Dictionary<string, int> UpParameter = Common.LevelUpParameter(pf.Type,UpLevel);
                button.transform.Find("FighterResultInfo/StatusTexts/Text (LevelUp)").gameObject.SetActive(true);
                button.transform.Find("FighterResultInfo/StatusTexts/Text (Level)").GetComponent<Text>().text = pf.Level.ToString() + "→" + (pf.Level + UpLevel).ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (Hp)").GetComponent<Text>().text = pf.Hp + "→" + (pf.Hp + UpParameter["Hp"]).ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (Stamina)").GetComponent<Text>().text = pf.Stamina + "→" + (pf.Stamina + UpParameter["Stamina"]).ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (AtkPower)").GetComponent<Text>().text = pf.AtkPower + "→" + (pf.AtkPower + UpParameter["AtkPower"]).ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (AtkSpeed)").GetComponent<Text>().text = pf.AtkSpeed + "→" + (pf.AtkSpeed + UpParameter["AtkSpeed"]).ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (MoveSpeed)").GetComponent<Text>().text = pf.MoveSpeed + "→" + (pf.MoveSpeed + UpParameter["MoveSpeed"]).ToString();
                pf.Level += UpLevel;
                pf.Hp += UpParameter["Hp"];
                pf.Stamina += UpParameter["Stamina"];
                pf.AtkPower += UpParameter["AtkPower"];
                pf.AtkSpeed += UpParameter["AtkSpeed"];
                pf.MoveSpeed += UpParameter["MoveSpeed"];
            }
            else
            {
                button.transform.Find("FighterResultInfo/StatusTexts/Text (LevelUp)").gameObject.SetActive(false);
                button.transform.Find("FighterResultInfo/StatusTexts/Text (Level)").GetComponent<Text>().text = pf.Level.ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (Hp)").GetComponent<Text>().text = pf.Hp.ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (Stamina)").GetComponent<Text>().text = pf.Stamina.ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (AtkPower)").GetComponent<Text>().text = pf.AtkPower.ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (AtkSpeed)").GetComponent<Text>().text = pf.AtkSpeed.ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (MoveSpeed)").GetComponent<Text>().text = pf.MoveSpeed.ToString();
            }
        }

        if (BaManager.WinFlg)
        {
            //進行度更新
            Common.Progress += 1;
        }
    }

    //OKボタンクリックで準備画面へ
    public void OkButtonClick()
    {
        Common.BattleMode = false;
        Common.SortieMode = false;

        //DontDestoyに入ってるBGMを削除
        Common.MusicReset();

        SceneManager.LoadScene("SettingScene");
    }

    //再挑戦ボタンクリックで戦闘シーン再読み込み
    public void RevengeButtonClick()
    {
        Common.BattleMode = false;
        Common.SortieMode = false;

        //DontDestoyに入ってるBGMを削除
        Common.MusicReset();

        SceneManager.LoadScene("BattleScene" + Common.Progress.ToString());
    }
}
