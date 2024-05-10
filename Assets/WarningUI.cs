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
        InfoText.text = SelectStatus.FighterName + "ÇÕä˘Ç… \n" + ufManager.PlayerUnitDataBaseAllList.FindAll(n => n.Num == SelectStatus.UnitNum)[0].Name + "Ç…èäëÆÇµÇƒÇ¢Ç‹Ç∑ÅI";
    }

    public void OK()
    {
        this.gameObject.SetActive(false);
    }
}
