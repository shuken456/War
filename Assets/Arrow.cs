using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    //ターゲットのステータス
    public FighterStatus targetEnemyStatus;

    //攻撃力
    public float AtkPower;
    //矢の速さ
    public float ArrowSpeed;

    private string EnemyTag;

    void Start()
    {
        if (this.tag == "PlayerArrow")
        {
            this.gameObject.layer = LayerMask.NameToLayer("PlayerArrow");
            EnemyTag = "EnemyFighter";
        }
        else
        {
            this.gameObject.layer = LayerMask.NameToLayer("EnemyArrow");
            EnemyTag = "PlayerFighter";
        }
    }

    void Update()
    {
        if (targetEnemyStatus == null)
        {
            Destroy(this.gameObject);
            return;
        }

        //ターゲットの場所に移動
        var v = targetEnemyStatus.gameObject.transform.position - transform.position;
        transform.position += v.normalized * ArrowSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //攻撃
        if (collision.gameObject.tag == EnemyTag)
        {
            targetEnemyStatus = collision.gameObject.GetComponent<FighterStatus>();

            //盾兵に当たった場合ダメージ減少
            if (targetEnemyStatus.Type == 3)
            {
                targetEnemyStatus.NowHp -= AtkPower * 0.2f;
            }
            else
            {
                targetEnemyStatus.NowHp -= AtkPower;
            }

            if (targetEnemyStatus.NowHp <= 0)
            {
                Destroy(targetEnemyStatus.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
