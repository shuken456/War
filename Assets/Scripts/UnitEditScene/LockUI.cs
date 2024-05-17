using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using System.Linq;

public class LockUI : MonoBehaviour
{
    public UnitEditManager EditManager;

    public PlayerUnitDB DB;

    private void OnEnable()
    {
        EditManager.SelectUI.transform.Find("Button (Name)").GetComponent<Button>().interactable = false;
    }

    private void OnDisable()
    {
        EditManager.SelectUI.transform.Find("Button (Name)").GetComponent<Button>().interactable = true;
        EditManager.LockButton.GetComponent<Button>().interactable = true;
    }

    public void Yes()
    {
        PlayerUnit NewPlayerUnit = new PlayerUnit();
        NewPlayerUnit.Num = EditManager.PlayerUnitDataBaseAllList.Count + 1;
        NewPlayerUnit.Name = "‘æ" + NewPlayerUnit.Num.ToString() + "•”‘à";
        NewPlayerUnit.UnitColor = Color.white;
        NewPlayerUnit.Strategy = 1;

        Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.Add(NewPlayerUnit); //‚È‚º‚©‚±‚±‚¾‚¯‚±‚¤‚¹‚ñ‚Æ‚¢‚©‚ñ
        Common.Save();
        EditManager.DisplayScreenStart();

        if (NewPlayerUnit.Num < 10)
        {
            EditManager.LockButton.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, EditManager.UnitOblect[NewPlayerUnit.Num].transform.position);
        }
        else
        {
            EditManager.LockButton.SetActive(false);
        }

        GameObject.Find("Unit" + EditManager.SelectUnitNum.ToString()).transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.green;

        this.gameObject.SetActive(false);
    }

    public void No()
    {
        this.gameObject.SetActive(false);
    }
}
