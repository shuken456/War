using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using System.Linq;

public class LockUI : MonoBehaviour
{
    public UnitEditManager EditManager;

    //資金UI
    public MoneyUI MoUI;

    //表示テキストとボタン
    public Text InfoText;
    public Button YesButton;

    //必要資金
    private int NeedMoney;

    private void OnEnable()
    {
        NeedMoney = EditManager.PlayerUnitDataBaseAllList.Count * 5;
        InfoText.text = "新たな部隊を解放しますか？\n（必要資金: " + NeedMoney.ToString() + "両）";

        //資金が足りなければボタンを押せないように
        if(Common.Money < NeedMoney)
        {
            YesButton.interactable = false;
        }
        else
        {
            YesButton.interactable = true;
        }
    }
        
    //部隊開放
    public void Yes()
    {
        PlayerUnit NewPlayerUnit = new PlayerUnit();
        NewPlayerUnit.Num = EditManager.PlayerUnitDataBaseAllList.Count + 1;
        NewPlayerUnit.Name = "第" + NewPlayerUnit.Num.ToString() + "部隊";
        NewPlayerUnit.UnitColor = Color.yellow;
        NewPlayerUnit.Strategy = 1;

        Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.Add(NewPlayerUnit);
        EditManager.DisplayScreenStart();

        //ロックボタンの位置を次の部隊の位置へ
        if (NewPlayerUnit.Num < 10)
        {
            EditManager.LockButton.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, EditManager.UnitOblect[NewPlayerUnit.Num].transform.position);
        }
        else
        {
            EditManager.LockButton.SetActive(false);
        }

        GameObject.Find("Unit" + EditManager.SelectUnitNum.ToString()).transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.green;

        //資金減少
        Common.Money -= NeedMoney;
        MoUI.TextWrite();

        this.gameObject.SetActive(false);
    }

    public void No()
    {
        this.gameObject.SetActive(false);
    }
}
