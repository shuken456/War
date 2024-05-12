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
        //�����l��0�̕����͏o���ł��Ȃ�
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
        //�o�����j�b�g��I�ѕԂ�
        Common.SelectUnitNum = EditManager.SelectUnitNum;

        Scene Bscene = SceneManager.GetSceneByName("BattleScene");

        foreach (var root in Bscene.GetRootGameObjects())
        {
            root.SetActive(true);
        }

        SceneManager.UnloadSceneAsync("UnitEditScene");
    }

    //���j�ύX�{�^��
    public void ChangeStrategy()
    {
        EditManager.StrategyUI.transform.position = this.gameObject.transform.position;
        EditManager.StrategyUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //�F�ύX�{�^��
    public void ChangeColor()
    {
        Vector2 UIPosition = this.gameObject.transform.position;
        UIPosition.x += 50;
        EditManager.ColorUI.transform.position = UIPosition;
        EditManager.ColorUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //���O�ύX�{�^��
    public void ChangeName()
    {
        EditManager.NameUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
