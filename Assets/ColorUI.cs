using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorUI : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    public FormationListManager FoManager;

    //���������{�^���̐F�Ƀ��j�b�g�J���[��ύX
    public void ColorButton()
    {
        FoManager.PlayerUnitDataBaseAllList[FoManager.SelectUnitNum - 1].UnitColor = eventSystem.currentSelectedGameObject.GetComponent<Image>().color;
        FoManager.DisplayScreenStart();
        FoManager.SelectUI.SetActive(true);
        GameObject.Find("Unit" + FoManager.SelectUnitNum.ToString()).transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.green;
        this.gameObject.SetActive(false);
    }
}
