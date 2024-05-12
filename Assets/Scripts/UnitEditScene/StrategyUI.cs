using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StrategyUI : MonoBehaviour
{
    public UnitEditManager EditManager;

    //DB�ۑ��@�J�����p
    private void OnDisable()
    {
        //EditorUtility.SetDirty(Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB"));
        //AssetDatabase.SaveAssets();
    }

    //�U���d���{�^��
    public void StrategyAtk()
    {
        EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].Strategy = 1;
        EditManager.DisplayUnitUI();
        EditManager.SelectUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //�ϋv�d���{�^��
    public void StrategyHp()
    {
        EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].Strategy = 2;
        EditManager.DisplayUnitUI();
        EditManager.SelectUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //�ړ��d���{�^��
    public void StrategyMove()
    {
        EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].Strategy = 3;
        EditManager.DisplayUnitUI();
        EditManager.SelectUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
