using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Button ContinueButton;
    public GameObject BeginWarningUI;

    //DB
    [SerializeField]
    PlayerFighterDB PlayerFighterTable;
    [SerializeField]
    PlayerUnitDB PlayerUnitTable;

    // Start is called before the first frame update
    void Start()
    {
        //�X�e�[�W1���N���A�����f�[�^�����݂��邩�`�F�b�N
        if (PlayerPrefs.GetInt("ContinueProgress") < 2)
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
        if (PlayerPrefs.GetInt("ContinueProgress") < 2)
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
        //�f�[�^���[�h
        PlayerFighterTable.Load();
        PlayerUnitTable.Load();

        //�o���t���O�����ɖ߂� ���ΐ풆�ɋ����I�������ƃt���O���߂�Ȃ�����
        List<PlayerUnit> SortieUnit = PlayerUnitTable.PlayerUnitDBList.FindAll((n) => n.SoriteFlg);
        foreach (PlayerUnit pu in SortieUnit)
        {
            pu.SoriteFlg = false;
        }

        //�f�[�^�Z�[�u
        PlayerFighterTable.Save();
        PlayerUnitTable.Save();

        SceneManager.LoadScene("SettingScene");
    }
}
