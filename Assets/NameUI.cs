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


    //�����\����I�������������ɂ���
    private void OnEnable()
    {
        NameField.text = EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].Name;
        NameField.ActivateInputField();
        EditManager.LockButton.GetComponent<Button>().interactable = false;
    }

    //DB�ۑ��@�J�����p
    private void OnDisable()
    {
        EditorUtility.SetDirty(Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB"));
        AssetDatabase.SaveAssets();
        EditManager.LockButton.GetComponent<Button>().interactable = true;
    }

    //����{�^�������ŕ������ύX
    public void DecisionName()
    {
        EditManager.PlayerUnitDataBaseAllList[EditManager.SelectUnitNum - 1].Name = NameField.text;
        EditManager.DisplayScreenStart();
        EditManager.DisplayUnitUI();
        GameObject.Find("Unit" + EditManager.SelectUnitNum.ToString()).transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.green;
        EditManager.SelectUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
