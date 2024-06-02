using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StrategyUI : MonoBehaviour
{
    public UnitEditManager EditManager;

    //攻撃重視ボタン
    public void StrategyAtk()
    {
        EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].Strategy = 1;
        EditManager.DisplayUnitUI();
        this.gameObject.SetActive(false);
    }

    //耐久重視ボタン
    public void StrategyHp()
    {
        EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].Strategy = 2;
        EditManager.DisplayUnitUI();
        this.gameObject.SetActive(false);
    }

    //移動重視ボタン
    public void StrategyMove()
    {
        EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].Strategy = 3;
        EditManager.DisplayUnitUI();
        this.gameObject.SetActive(false);
    }
}
