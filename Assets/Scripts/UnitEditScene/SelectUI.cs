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
        //���j�b�g�ҏW�V�[����
        Common.SelectUnitNum = EditManager.SelectUnitNum;
        SceneManager.LoadScene("UnitFormationScene", LoadSceneMode.Additive);
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
