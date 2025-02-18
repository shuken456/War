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
        //出撃フラグを元に戻す
        List<PlayerUnit> SortieUnit = BaManager.PlayerUnitDataBaseAllList.FindAll((n) => n.SoriteFlg);
        foreach (PlayerUnit pu in SortieUnit)
        {
            pu.SoriteFlg = false;
        }

        Common.BattleMode = false;

        //DontDestoyに入ってるBGMを削除
        Common.MusicReset();

        SceneManager.LoadScene("TitleScene");
    }
}
