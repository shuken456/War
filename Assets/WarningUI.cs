using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningUI : MonoBehaviour
{
    public Text InfoText;
    public UnitFormationManager ufManager;

    private void OnEnable()
    {
        FighterStatus SelectStatus = ufManager.SelectFighterButton.GetComponent<FighterStatus>();
        InfoText.text = SelectStatus.FighterName + "は既に \n" + ufManager.PlayerUnitDataBaseAllList.FindAll(n => n.Num == SelectStatus.UnitNum)[0].Name + "に所属しています！";
    }

    public void OK()
    {
        this.gameObject.SetActive(false);
    }
}
