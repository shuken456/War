using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSituationUI : MonoBehaviour
{
    public Text PlayerPower;
    public Text EnemyPower;

    public BattleManager BaManager;

    // Update is called once per frame
    void Update()
    {
        UpdatePower();
    }

    //ƒŒƒxƒ‹‚Ì‡Œv‚ğí—Í‚Æ‚µ‚Ä•\¦‚·‚é
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
            //•‰‚¯
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
            //Ÿ‚¿
            BaManager.BattleLose();
        }
    }
}
