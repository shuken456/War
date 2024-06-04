using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearUI : MonoBehaviour
{
    public BattleManager BaManager;

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    public void TitleButtonClick()
    {
        //�o���t���O�����ɖ߂�
        List<PlayerUnit> SortieUnit = BaManager.PlayerUnitDataBaseAllList.FindAll((n) => n.SoriteFlg);
        foreach (PlayerUnit pu in SortieUnit)
        {
            pu.SoriteFlg = false;
        }

        Common.BattleMode = false;

        //DontDestoy�ɓ����Ă�BGM���폜
        Destroy(GameObject.Find("SettingBGM"));
        Destroy(GameObject.Find("BattleBGM"));
        Destroy(GameObject.Find("VoiceBGM"));

        SceneManager.LoadScene("SettingScene");
    }
}
