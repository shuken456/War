using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleInfoUI : MonoBehaviour
{
    public BattleManager BaManager;
    public GameObject MenuUI;

    private float DefaultTime;

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
        //SceneManager.LoadScene("BattleScene" + Common.Progress.ToString());
        SceneManager.LoadScene("BattleScene1");
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
        SceneManager.LoadScene("SettingScene");
    }

    //���j���[�����
    public void MenuCloseButtonClick()
    {
        Time.timeScale = DefaultTime;
        MenuUI.SetActive(false);
    }
}
