using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StrategyUI : MonoBehaviour
{
    public UnitEditManager EditManager;

    //DBÛ¶@J­Â«p
    private void OnDisable()
    {
        Common.Save();
    }

    //Ud{^
    public void StrategyAtk()
    {
        EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].Strategy = 1;
        EditManager.DisplayUnitUI();
        EditManager.SelectUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //Ïvd{^
    public void StrategyHp()
    {
        EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].Strategy = 2;
        EditManager.DisplayUnitUI();
        EditManager.SelectUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //Ú®d{^
    public void StrategyMove()
    {
        EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].Strategy = 3;
        EditManager.DisplayUnitUI();
        EditManager.SelectUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
