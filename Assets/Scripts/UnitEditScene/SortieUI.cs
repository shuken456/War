using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SortieUI : MonoBehaviour
{
    public UnitEditManager EditManager;

    private void OnEnable()
    {
        //部隊人数0の部隊は出撃できない
        if(EditManager.PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == EditManager.SelectUnitNum).Count == 0)
        {
            this.transform.Find("Button (Sortie)").GetComponent<Button>().interactable = false;
        }
        else
        {
            this.transform.Find("Button (Sortie)").GetComponent<Button>().interactable = true;
        }
    }

    public void Sortie() 
    {
        //出撃ユニットを選び返す
        Common.SelectUnitNum = EditManager.SelectUnitNum;

        //Scene Bscene = SceneManager.GetSceneByName("BattleScene" + Common.Progress.ToString());
        Scene Bscene = SceneManager.GetSceneByName("BattleScene1");

        foreach (var root in Bscene.GetRootGameObjects())
        {
            root.SetActive(true);
        }

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
