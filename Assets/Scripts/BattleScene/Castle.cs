using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    //範囲画像
    public SpriteRenderer rangeRenderer;

    //範囲
    public float Range;

    //回復周期
    public float HealTime;

    //範囲内の兵士
    private Collider2D[] colliderPlayer;
    private Collider2D[] colliderEnemy;

    // Start is called before the first frame update
    void Start()
    {
        rangeRenderer.transform.localScale = new Vector3(2, 2) * Range; //半径なので二倍

        //回復は周期的に行いたいのでここで呼ぶ
        StartCoroutine(Heal());
    }

    // Update is called once per frame
    void Update()
    {
        colliderPlayer = Physics2D.OverlapCircleAll(transform.position, Range, LayerMask.GetMask("PlayerFighter"));
        colliderEnemy = Physics2D.OverlapCircleAll(transform.position, Range, LayerMask.GetMask("EnemyFighter"));
    }

    //回復処理
    private IEnumerator Heal()
    {
        while (true)
        {
            yield return new WaitForSeconds(HealTime);

            //味方制圧状態の場合、範囲内の味方兵士を回復
            if (this.tag == "PlayerBase")
            {
                foreach (Collider2D Fighter in colliderPlayer)
                {
                    FighterStatus fighterStatus = Fighter.gameObject.GetComponent<FighterStatus>();

                    if (fighterStatus.NowHp < fighterStatus.MaxHp)
                    {
                        fighterStatus.NowHp += Mathf.Round(fighterStatus.MaxHp / 10);
                    }
                }
            }

            //敵制圧状態の場合、範囲内の敵兵士を回復
            else if (this.tag == "EnemyBase")
            {
                foreach (Collider2D Fighter in colliderEnemy)
                {
                    FighterStatus fighterStatus = Fighter.gameObject.GetComponent<FighterStatus>();

                    if (fighterStatus.NowHp < fighterStatus.MaxHp)
                    {
                        fighterStatus.NowHp += Mathf.Round(fighterStatus.MaxHp / 10);
                    }
                }
            }
        }
    }
}
