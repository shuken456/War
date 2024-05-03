using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    public FormationListManager FoManager;

    public void ChangeFormation()
    {
        //���j�b�g�ҏW�V�[����
    }

    //���j�ύX�{�^��
    public void ChangeStrategy()
    {
        FoManager.StrategyUI.transform.position = this.gameObject.transform.position;
        FoManager.StrategyUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //�F�ύX�{�^��
    public void ChangeColor()
    {
        Vector2 UIPosition = this.gameObject.transform.position;
        UIPosition.x += 50;
        FoManager.ColorUI.transform.position = UIPosition;
        FoManager.ColorUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //���O�ύX�{�^��
    public void ChangeName()
    {
        FoManager.NameUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
