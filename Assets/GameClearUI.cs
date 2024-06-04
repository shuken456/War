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
        //oŒ‚ƒtƒ‰ƒO‚ğŒ³‚É–ß‚·
        List<PlayerUnit> SortieUnit = BaManager.PlayerUnitDataBaseAllList.FindAll((n) => n.SoriteFlg);
        foreach (PlayerUnit pu in SortieUnit)
        {
            pu.SoriteFlg = false;
        }

        Common.BattleMode = false;

        //DontDestoy‚É“ü‚Á‚Ä‚éBGM‚ğíœ
        Destroy(GameObject.Find("SettingBGM"));
        Destroy(GameObject.Find("BattleBGM"));
        Destroy(GameObject.Find("VoiceBGM"));

        SceneManager.LoadScene("SettingScene");
    }
}
