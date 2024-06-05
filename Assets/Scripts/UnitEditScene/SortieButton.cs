using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SortieButton : MonoBehaviour
{
    public UnitEditManager EditManager;

    private void OnEnable()
    {
        //部隊人数0の部隊は出撃できない
        if(EditManager.PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == EditManager.SelectUnitNum).Count == 0)
        {
            this.GetComponent<Button>().interactable = false;
        }
        else
        {
            this.GetComponent<Button>().interactable = true;
        }
    }

    public void Sortie() 
    {
        //出撃ユニットを選び返す
        Common.SelectUnitNum = EditManager.SelectUnitNum;

        Scene Bscene = SceneManager.GetSceneByName("BattleScene" + Common.Progress.ToString());

        foreach (var root in Bscene.GetRootGameObjects())
        {
            root.SetActive(true);
        }

        SceneManager.UnloadSceneAsync("UnitEditScene");
    }
}
