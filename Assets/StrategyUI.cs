using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyUI : MonoBehaviour
{
    public FormationListManager FoManager;

    //攻撃重視ボタン
    public void StrategyAtk()
    {
        FoManager.PlayerUnitDataBaseAllList[FoManager.SelectUnitNum - 1].Strategy = 1;
        FoManager.DisplayUnitUI();
        FoManager.SelectUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //耐久重視ボタン
    public void StrategyHp()
    {
        FoManager.PlayerUnitDataBaseAllList[FoManager.SelectUnitNum - 1].Strategy = 2;
        FoManager.DisplayUnitUI();
        FoManager.SelectUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //移動重視ボタン
    public void StrategyMove()
    {
        FoManager.PlayerUnitDataBaseAllList[FoManager.SelectUnitNum - 1].Strategy = 3;
        FoManager.DisplayUnitUI();
        FoManager.SelectUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
