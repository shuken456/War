using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectUI : MonoBehaviour
{
    public UnitEditManager EditManager;

    public void ChangeFormation()
    {
        //ユニット編集シーンへ
        Common.SelectUnitNum = EditManager.SelectUnitNum;
        SceneManager.LoadScene("UnitFormationScene", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("UnitEditScene");
    }

    //方針変更ボタン
    public void ChangeStrategy()
    {
        EditManager.StrategyUI.transform.position = this.gameObject.transform.position;
        EditManager.StrategyUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //色変更ボタン
    public void ChangeColor()
    {
        Vector2 UIPosition = this.gameObject.transform.position;
        UIPosition.x += 50;
        EditManager.ColorUI.transform.position = UIPosition;
        EditManager.ColorUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //名前変更ボタン
    public void ChangeName()
    {
        EditManager.NameUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
