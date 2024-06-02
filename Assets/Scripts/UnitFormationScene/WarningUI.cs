using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningUI : MonoBehaviour
{
    public Text InfoText;
    public UnitFormationManager ufManager;

    //Šù‚É‘¼‚Ì•”‘à‚ÉŠ‘®‚µ‚Ä‚¢‚é•ºm‚ÍŠm”F
    private void OnEnable()
    {
        FighterStatus SelectStatus = ufManager.SelectFighterButton.GetComponent<FighterStatus>();
        InfoText.text = SelectStatus.FighterName + "‚ÍŠù‚É \n" + ufManager.PlayerUnitDataBaseAllList.FindAll(n => n.Num == SelectStatus.UnitNum)[0].Name + "‚ÉŠ‘®‚µ‚Ä‚¢‚Ü‚·I";
    }

    public void OK()
    {
        this.gameObject.SetActive(false);
    }
}
