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

    //�X�e�[�W�N���A
    private IEnumerator Win()
    {
        GameObject.Find("Virtual Camera").transform.position = this.gameObject.transform.position - new Vector3(0, 0, 1);//�ڕW�n�_����ʒ����ɗ���悤�ɂ���
        this.gameObject.GetComponent<AudioSource>().Play();//���ʉ���炷
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(GameObject.Find("BattleManager").GetComponent<BattleManager>().BattleWin());
    }
}
