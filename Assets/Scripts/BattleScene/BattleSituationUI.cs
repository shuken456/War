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

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale >= 1)
        {
            UpdatePower();
        }
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
        if (power == 0 && BaManager.StartFlg)
        {
            //負け
            BaManager.BattleLose();
        }

        power = 0;
        tagObjects = GameObject.FindGameObjectsWithTag("EnemyFighter");
        foreach (GameObject Fighter in tagObjects)
        {
            power += Fighter.GetComponent<FighterStatus>().Level;
        }
        EnemyPower.text = power.ToString();
        if (power == 0 && BaManager.StartFlg)
        {
            //勝ち
            BaManager.BattleWin();
        }
    }
}
