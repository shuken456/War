using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private BattleManager BaManager;

    //ターゲットのステータス
    public FighterStatus targetEnemyStatus;

    //弓を打った兵士の名前
    public string ArcherName;
    //攻撃力
    public float AtkPower;
    //矢の速さ
    public float ArrowSpeed;

    private string EnemyTag;
    private string EnemyBaseTag;

    void Start()
    {
        BaManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();

        //自分のタグで敵のタグを判断
        if (this.tag == "PlayerArrow")
        {
            this.gameObject.layer = LayerMask.NameToLayer("PlayerArrow");
            EnemyTag = "EnemyFighter";
            EnemyBaseTag = "EnemyBase";
        }
        else
        {
            this.gameObject.layer = LayerMask.NameToLayer("EnemyArrow");
            EnemyTag = "PlayerFighter";
            EnemyBaseTag = "PlayerBase";
        }
    }

    void Update()
    {
        if (targetEnemyStatus == null)
        {
            Destroy(this.gameObject);
            return;
        }

        //ターゲットの場所に飛ぶ
        var v = targetEnemyStatus.gameObject.transform.position - transform.position;
        transform.position += v.normalized * ArrowSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //攻撃
        if (collision.gameObject.tag == EnemyTag || collision.gameObject.tag == EnemyBaseTag)
        {
            targetEnemyStatus = collision.gameObject.GetComponent<FighterStatus>();
            targetEnemyStatus.Exp += 2;

            //盾兵に当たった場合ダメージ減少
            if (targetEnemyStatus.Type == 3)
            {
                targetEnemyStatus.NowHp -= AtkPower * 0.2f;
            }
            else
            {
                targetEnemyStatus.NowHp -= AtkPower;
            }

            if (targetEnemyStatus.NowHp <= 0 && targetEnemyStatus.gameObject.tag == EnemyTag)
            {
                //ログ表示
                if (this.tag == "PlayerArrow")
                {
                    BaManager.LogUI.DrawLog("<size=30>" + ArcherName + "</size>\n" + targetEnemyStatus.FighterName + "を倒した！");
                }
                else
                {
                    if (targetEnemyStatus.UnitLeader)
                    {
                        BaManager.LogUI.DrawLog("<size=30><color=red>" + targetEnemyStatus.FighterName + "</color></size>\n" + ArcherName + "に倒された！");
                    }
                    else
                    {
                        BaManager.LogUI.DrawLog("<size=30>" + targetEnemyStatus.FighterName + "</size>\n" + ArcherName + "に倒された！");
                    }
                }

                Destroy(targetEnemyStatus.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
