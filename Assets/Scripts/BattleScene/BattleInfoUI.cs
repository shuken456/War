using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleInfoUI : MonoBehaviour
{
    public BattleManager BaManager;

    public GameObject MenuUI;
    public Button SettingButton;

    private float DefaultTime;

    private void OnEnable()
    {
        //�ŏ��̃X�e�[�W�͏�����ʂ֖߂�Ȃ��悤��
        if(Common.Progress == 1)
        {
            SettingButton.interactable = false;
        }
        else
        {
            SettingButton.interactable = true;
        }
    }

    //��ʉE���@���j���[�{�^���N���b�N��
    public void MenuButtonClick()
    {
        DefaultTime = Time.timeScale;
        Time.timeScale = 0;
        MenuUI.SetActive(true);
    }

    //�X�e�[�W��蒼��
    public void RevengeButtonClick()
    {
        //�o���t���O�����ɖ߂�
        List<PlayerUnit> SortieUnit = BaManager.PlayerUnitDataBaseAllList.FindAll((n) => n.SoriteFlg);
        foreach (PlayerUnit pu in SortieUnit)
        {
           pu.SoriteFlg = false;
        }

        //DontDestoy�ɓ����Ă�BGM���폜
        Common.MusicReset();

        SceneManager.LoadScene("BattleScene" + Common.Progress.ToString());
    }

    //������ʂ֖߂�
    public void SettingButtonClick()
    {
        //�o���t���O�����ɖ߂�
        List<PlayerUnit> SortieUnit = BaManager.PlayerUnitDataBaseAllList.FindAll((n) => n.SoriteFlg);
        foreach (PlayerUnit pu in SortieUnit)
        {
            pu.SoriteFlg = false;
        }

        Common.BattleMode = false;

        //DontDestoy�ɓ����Ă�BGM���폜
        Common.MusicReset();

        SceneManager.LoadScene("SettingScene");
    }

    //���j���[�����
    public void MenuCloseButtonClick()
    {
        Time.timeScale = DefaultTime;
        MenuUI.SetActive(false);
    }
}
