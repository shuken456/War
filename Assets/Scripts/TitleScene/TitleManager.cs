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
        PlayerPrefs.DeleteAll();
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

    //�͂��߂���
    public void Begin()
    {
        if (ContinueData == string.Empty)
        {
            //�����f�[�^�������ŃZ�[�u�@���ɂ͂��߂���X�^�[�g���鎞�Ɏg��
            PlayerFighterTable.InitialSave();
            PlayerUnitTable.InitialSave();
            //�������Ɛi�s�x�����Z�[�u
            Common.InitialSave();

            SceneManager.LoadScene("BattleScene1");
        }
        else
        {
            //���Ƀf�[�^������ꍇ�A���̃f�[�^�������Ă��������m�F����
            BeginWarningUI.SetActive(true);
        }
    }

    //�f�[�^�����Ă�OK�Ȃ�
    public void BeginWarningYes()
    {
        //�����f�[�^���[�h
        PlayerFighterTable.InitialLoad();
        PlayerUnitTable.InitialLoad();
        //�������Ɛi�s�x�����Z�[�u
        Common.InitialSave();

        SceneManager.LoadScene("BattleScene1");
    }

    public void BeginWarningNo()
    {
        BeginWarningUI.SetActive(false);
    }

    //��������
    public void Continue()
    {
        SceneManager.LoadScene("SettingScene");
    }
}
