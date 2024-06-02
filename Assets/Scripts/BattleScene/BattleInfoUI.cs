using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleInfoUI : MonoBehaviour
{
    public BattleManager BaManager;
    public GameObject MenuUI;

    private float DefaultTime;

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
        //SceneManager.LoadScene("BattleScene" + Common.Progress.ToString());
        SceneManager.LoadScene("BattleScene1");
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
        SceneManager.LoadScene("SettingScene");
    }

    //メニューを閉じる
    public void MenuCloseButtonClick()
    {
        Time.timeScale = DefaultTime;
        MenuUI.SetActive(false);
    }
}
