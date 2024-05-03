using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    public FormationListManager FoManager;

    public void ChangeFormation()
    {
        //ユニット編集シーンへ
    }

    //方針変更ボタン
    public void ChangeStrategy()
    {
        FoManager.StrategyUI.transform.position = this.gameObject.transform.position;
        FoManager.StrategyUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //色変更ボタン
    public void ChangeColor()
    {
        Vector2 UIPosition = this.gameObject.transform.position;
        UIPosition.x += 50;
        FoManager.ColorUI.transform.position = UIPosition;
        FoManager.ColorUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //名前変更ボタン
    public void ChangeName()
    {
        FoManager.NameUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
