using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SettingManager : MonoBehaviour
{
    //各UI
    public GameObject HelpUI;
    public GameObject HomeUI;
    public GameObject SortieCheckUIUI;
    public GameObject EmploymentUI;
    public GameObject FighterEditUI;
    public GameObject TitleUI;
    public MoneyUI MoUI;

    //DB
    [SerializeField]
    PlayerFighterDB PlayerFighterTable;
    [SerializeField]
    PlayerUnitDB PlayerUnitTable;

    // Start is called before the first frame update
    void Start()
    {
        //データロード
        PlayerFighterTable.Load();
        PlayerUnitTable.Load();

        if (PlayerPrefs.GetInt("SettingHelp", 0) == 0)
        {
            PlayerPrefs.SetInt("SettingHelp", 1);
            HelpUI.SetActive(true);
        }
    }

    private void OnDisable()
    {
        //データセーブ
        PlayerFighterTable.Save();
        PlayerUnitTable.Save();
    }
}
