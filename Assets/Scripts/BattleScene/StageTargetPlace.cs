using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTargetPlace : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerFighter" && Time.timeScale == 1)
        {
            Time.timeScale = 0;
            StartCoroutine(Win());
        }
    }

    //ステージクリア
    private IEnumerator Win()
    {
        GameObject.Find("Virtual Camera").transform.position = this.gameObject.transform.position - new Vector3(0, 0, 1);//目標地点が画面中央に来るようにする
        this.gameObject.GetComponent<AudioSource>().Play();//効果音を鳴らす
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(GameObject.Find("BattleManager").GetComponent<BattleManager>().BattleWin());
    }
}
