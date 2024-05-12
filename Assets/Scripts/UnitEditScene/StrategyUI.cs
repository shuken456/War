using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StrategyUI : MonoBehaviour
{
    public UnitEditManager EditManager;

    //DB保存　開発環境用
    private void OnDisable()
    {
        //EditorUtility.SetDirty(Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB"));
        //AssetDatabase.SaveAssets();
    }

    //攻撃重視ボタン
    public void StrategyAtk()
    {
        EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].Strategy = 1;
        EditManager.DisplayUnitUI();
        EditManager.SelectUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //耐久重視ボタン
    public void StrategyHp()
    {
        EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].Strategy = 2;
        EditManager.DisplayUnitUI();
        EditManager.SelectUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //移動重視ボタン
    public void StrategyMove()
    {
        EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].Strategy = 3;
        EditManager.DisplayUnitUI();
        EditManager.SelectUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
