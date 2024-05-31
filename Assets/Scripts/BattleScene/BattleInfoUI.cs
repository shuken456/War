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

    public void MenuButtonClick()
    {
        DefaultTime = Time.timeScale;
        Time.timeScale = 0;
        MenuUI.SetActive(true);
    }

    public void RevengeButtonClick()
    {
        //出撃フラグを元に戻す
        List<PlayerUnit> SortieUnit = BaManager.PlayerUnitDataBaseAllList.FindAll((n) => n.SoriteFlg);
        foreach (PlayerUnit pu in SortieUnit)
        {
           pu.SoriteFlg = false;
        }
        SceneManager.LoadScene("BattleScene");
    }

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

    public void MenuCloseButtonClick()
    {
        Time.timeScale = DefaultTime;
        MenuUI.SetActive(false);
    }
}
