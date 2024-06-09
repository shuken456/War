using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    //範囲画像
    public GameObject rangeRenderer;

    //範囲
    public Vector2 Range;

    //回復周期
    public float HealTime;

    //回復表示プレハブ
    public GameObject HealPrefab;
    //炎
    public GameObject Fire;

    //範囲内の兵士
    private Collider2D[] colliderPlayer;
    private Collider2D[] colliderEnemy;

    private FighterStatus MyStatus;

    // Start is called before the first frame update
    void Start()
    {
        MyStatus = this.gameObject.GetComponent<FighterStatus>();
        rangeRenderer.GetComponent<SpriteRenderer>().transform.localScale = Range;
    }

    private void OnEnable()
    {
        //回復は周期的に行いたいのでここで呼ぶ
        StartCoroutine(Heal());
    }

    // Update is called once per frame
    void Update()
    {
        //城が破壊されたらステージ終了
        if(MyStatus.NowHp <= 0 && Time.timeScale == 1)
        {
            Time.timeScale = 0;
            StartCoroutine(GameSet());
        }
    }

    //ゲームセット　城が燃えるところを見せる
    private IEnumerator GameSet()
    {
        GameObject.Find("Virtual Camera").transform.position = this.gameObject.transform.position - new Vector3(0, 0, 1);//城が画面中央に来るようにする

        yield return new WaitForSecondsRealtime(1f);

        this.gameObject.transform.Find("Fire").gameObject.SetActive(true);

        if (this.gameObject.tag == "EnemyBase")
        {
            yield return StartCoroutine(GameObject.Find("BattleManager").GetComponent<BattleManager>().BattleWin());
        }
        else
        {
            yield return StartCoroutine(GameObject.Find("BattleManager").GetComponent<BattleManager>().BattleLose());
        }
    }

    //回復処理
    private IEnumerator Heal()
    {
        while (true)
        {
            yield return new WaitForSeconds(HealTime);

            //味方城の場合、範囲内の味方兵士を回復
            if (this.tag == "PlayerBase")
            {
                colliderPlayer = Physics2D.OverlapBoxAll (rangeRenderer.transform.position, Range, 0f,LayerMask.GetMask("PlayerFighter"));

                foreach (Collider2D Fighter in colliderPlayer)
                {
                    FighterStatus fighterStatus = Fighter.gameObject.GetComponent<FighterStatus>();

                    if (fighterStatus.NowHp < (fighterStatus.MaxHp + fighterStatus.MaxHpBuff))
                    {
                        fighterStatus.NowHp += Mathf.Round((fighterStatus.MaxHp + fighterStatus.MaxHpBuff) / 10);
                        Instantiate(HealPrefab, Fighter.gameObject.transform.position + new Vector3(0f, -0.2f, 0), Quaternion.identity);
                    }
                }
            }

            //敵城の場合、範囲内の敵兵士を回復
            else if (this.tag == "EnemyBase")
            {
                colliderEnemy = Physics2D.OverlapBoxAll(rangeRenderer.transform.position, Range, 0f, LayerMask.GetMask("EnemyFighter"));

                foreach (Collider2D Fighter in colliderEnemy)
                {
                    FighterStatus fighterStatus = Fighter.gameObject.GetComponent<FighterStatus>();

                    if (fighterStatus.NowHp < (fighterStatus.MaxHp + fighterStatus.MaxHpBuff))
                    {
                        fighterStatus.NowHp += Mathf.Round((fighterStatus.MaxHp + fighterStatus.MaxHpBuff) / 10);
                        Instantiate(HealPrefab, Fighter.gameObject.transform.position + new Vector3(0f, -0.2f, 0), Quaternion.identity);
                    }
                }
            }
        }
    }
}
