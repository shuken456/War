using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//��ʒ�����@��͕\��UI
public class BattleSituationUI : MonoBehaviour
{
    public Text PlayerPower;
    public Text EnemyPower;

    public BattleManager BaManager;


    void Update()
    {
        UpdatePower();
    }

    //���x���̍��v���͂Ƃ��ĕ\������
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
            //����
            BaManager.BattleLose();
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
            //����
            BaManager.BattleWin();
        }
    }
}
