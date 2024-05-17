using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class ColorUI : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    public UnitEditManager EditManager;


    //DB保存　開発環境用
    private void OnDisable()
    {
        Common.Save();
    }

    //押下したボタンの色にユニットカラーを変更
    public void ColorButton()
    {
        EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].UnitColor = eventSystem.currentSelectedGameObject.GetComponent<Image>().color;
        EditManager.DisplayScreenStart();
        EditManager.SelectUI.SetActive(true);
        GameObject.Find("Unit" + EditManager.SelectUnitNum.ToString()).transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.green;
        this.gameObject.SetActive(false);
    }
}
