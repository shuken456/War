using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyUI : MonoBehaviour
{
    public FormationListManager FoManager;

    //�U���d���{�^��
    public void StrategyAtk()
    {
        FoManager.PlayerUnitDataBaseAllList[FoManager.SelectUnitNum - 1].Strategy = 1;
        FoManager.DisplayUnitUI();
        FoManager.SelectUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //�ϋv�d���{�^��
    public void StrategyHp()
    {
        FoManager.PlayerUnitDataBaseAllList[FoManager.SelectUnitNum - 1].Strategy = 2;
        FoManager.DisplayUnitUI();
        FoManager.SelectUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //�ړ��d���{�^��
    public void StrategyMove()
    {
        FoManager.PlayerUnitDataBaseAllList[FoManager.SelectUnitNum - 1].Strategy = 3;
        FoManager.DisplayUnitUI();
        FoManager.SelectUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
