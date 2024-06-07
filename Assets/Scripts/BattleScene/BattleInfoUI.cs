using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleInfoUI : MonoBehaviour
{
    public BattleManager BaManager;

    public GameObject MenuUI;
    public Button SettingButton;

    private float DefaultTime;

    private void OnEnable()
    {
        //最初のステージは準備画面へ戻れないように
        if(Common.Progress == 1)
        {
            SettingButton.interactable = false;
        }
        else
        {
            SettingButton.interactable = true;
        }
    }

    //画面右下　メニューボタンクリック時
    public void MenuButtonClick()
    {
        DefaultTime = Time.timeScale;
        Time.timeScale = 0;
        MenuUI.SetActive(true);
    }

    //ステージやり直し
    public void RevengeButtonClick()
    {
        //出撃フラグを元に戻す
        List<PlayerUnit> SortieUnit = BaManager.PlayerUnitDataBaseAllList.FindAll((n) => n.SoriteFlg);
        foreach (PlayerUnit pu in SortieUnit)
        {
           pu.SoriteFlg = false;
        }

        //DontDestoyに入ってるBGMを削除
        Common.MusicReset();

        SceneManager.LoadScene("BattleScene" + Common.Progress.ToString());
    }

    //準備画面へ戻る
    public void SettingButtonClick()
    {
        //出撃フラグを元に戻す
        List<PlayerUnit> SortieUnit = BaManager.PlayerUnitDataBaseAllList.FindAll((n) => n.SoriteFlg);
        foreach (PlayerUnit pu in SortieUnit)
        {
            pu.SoriteFlg = false;
        }

        Common.BattleMode = false;

        //DontDestoyに入ってるBGMを削除
        Common.MusicReset();

        SceneManager.LoadScene("SettingScene");
    }

    //メニューを閉じる
    public void MenuCloseButtonClick()
    {
        Time.timeScale = DefaultTime;
        MenuUI.SetActive(false);
    }
}
