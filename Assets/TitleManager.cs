using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Button ContinueButton;
    public GameObject BeginWarningUI;
    private string ContinueData;

    //DB
    [SerializeField]
    PlayerFighterDB PlayerFighterTable;
    [SerializeField]
    PlayerUnitDB PlayerUnitTable;

    // Start is called before the first frame update
    void Start()
    {
        ContinueData = PlayerPrefs.GetString("ContinueFighterData", string.Empty);

        //�f�[�^�����݂��邩�`�F�b�N
        if (ContinueData == string.Empty)
        {
            ContinueButton.interactable = false;
        }
        else
        {
            ContinueButton.interactable = true;
        }
    }

    public void Begin()
    {
        if (ContinueData == string.Empty)
        {
            //�����f�[�^�Z�[�u
            PlayerFighterTable.InitialSave();
            PlayerUnitTable.InitialSave();
            //�������Ɛi�s�x�����Z�[�u
            Common.InitialSave();

            SceneManager.LoadScene("SettingScene");
        }
        else
        {
            BeginWarningUI.SetActive(true);
        }
    }

    public void BeginWarningYes()
    {
        //�����f�[�^���[�h
        PlayerFighterTable.InitialLoad();
        PlayerUnitTable.InitialLoad();
        //�������Ɛi�s�x�����Z�[�u
        Common.InitialSave();

        SceneManager.LoadScene("SettingScene");
    }

    public void BeginWarningNo()
    {
        BeginWarningUI.SetActive(false);
    }

    public void Continue()
    {
        SceneManager.LoadScene("SettingScene");
    }
}
