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
        //�����l��0�̕����͏o���ł��Ȃ�
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
        //�o�����j�b�g��I�ѕԂ�
        Common.SelectUnitNum = EditManager.SelectUnitNum;

        Scene Bscene = SceneManager.GetSceneByName("BattleScene" + Common.Progress.ToString());

        foreach (var root in Bscene.GetRootGameObjects())
        {
            root.SetActive(true);
        }

        SceneManager.UnloadSceneAsync("UnitEditScene");
    }
}
