using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Button ContinueButton;
    public GameObject BeginWarningUI;
    private string ContinueData;

    //DB
    [SerializeField]
    PlayerFighterDB PlayerFighterTable;
    [SerializeField]
    PlayerUnitDB PlayerUnitTable;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
        ContinueData = PlayerPrefs.GetString("ContinueFighterData", string.Empty);

        //データが存在するかチェック
        if (ContinueData == string.Empty)
        {
            ContinueButton.interactable = false;
        }
        else
        {
            ContinueButton.interactable = true;
        }
    }

    //はじめから
    public void Begin()
    {
        if (ContinueData == string.Empty)
        {
            //初期データをここでセーブ　次にはじめからスタートする時に使う
            PlayerFighterTable.InitialSave();
            PlayerUnitTable.InitialSave();
            //所持金と進行度初期セーブ
            Common.InitialSave();

            SceneManager.LoadScene("BattleScene1");
        }
        else
        {
            //既にデータがある場合、そのデータが消えてもいいか確認する
            BeginWarningUI.SetActive(true);
        }
    }

    //データ消えてもOKなら
    public void BeginWarningYes()
    {
        //初期データロード
        PlayerFighterTable.InitialLoad();
        PlayerUnitTable.InitialLoad();
        //所持金と進行度初期セーブ
        Common.InitialSave();

        SceneManager.LoadScene("BattleScene1");
    }

    public void BeginWarningNo()
    {
        BeginWarningUI.SetActive(false);
    }

    //続きから
    public void Continue()
    {
        SceneManager.LoadScene("SettingScene");
    }
}
