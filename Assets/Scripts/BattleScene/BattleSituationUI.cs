using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//画面中央上　戦力表示UI
public class BattleSituationUI : MonoBehaviour
{
    public Text PlayerPower;
    public Text EnemyPower;

    public BattleManager BaManager;


    void Update()
    {
        UpdatePower();
    }

    //レベルの合計を戦力として表示する
    public void UpdatePower()
    {
        GameObject[] tagObjects;
        int power = 0;

        tagObjects = GameObject.FindGameObjectsWithTag("PlayerFighter");
        foreach(GameObject Fighter in tagObjects)
        {
            power += Fighter.GetComponent<FighterStatus>().Level;
        }
        PlayerPower.text = power.ToString();
        if (power == 0 && BaManager.StartFlg && Time.timeScale >= 1)
        {
            //味方戦力0で負け
            StartCoroutine(BaManager.BattleLose());
        }

        power = 0;
        tagObjects = GameObject.FindGameObjectsWithTag("EnemyFighter");
        foreach (GameObject Fighter in tagObjects)
        {
            power += Fighter.GetComponent<FighterStatus>().Level;
        }
        EnemyPower.text = power.ToString();
        if (power == 0 && BaManager.StartFlg && Time.timeScale >= 1)
        {
            //敵戦力0で勝ち
            StartCoroutine(BaManager.BattleWin());
        }
    }
}
