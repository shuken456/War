using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class NameUI : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;

    public UnitEditManager EditManager;

    public InputField NameField;


    //初期表示を選択した部隊名にする
    private void OnEnable()
    {
        NameField.text = EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].Name;
        NameField.ActivateInputField();
    }

    //決定ボタン押下で部隊名変更
    public void DecisionName()
    {
        EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].Name = NameField.text;
        EditManager.DisplayScreenStart();
        EditManager.DisplayUnitUI();
        GameObject.Find("Unit" + EditManager.SelectUnitNum.ToString()).transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.green;
        this.gameObject.SetActive(false);
    }
}
