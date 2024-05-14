using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    //”ÍˆÍ‰æ‘œ
    public SpriteRenderer rangeRenderer;

    //”ÍˆÍ
    public float Range;

    //‰ñ•œüŠú
    public float HealTime;

    //”ÍˆÍ“à‚Ì•ºm
    private Collider2D[] colliderPlayer;
    private Collider2D[] colliderEnemy;

    // Start is called before the first frame update
    void Start()
    {
        rangeRenderer.transform.localScale = new Vector3(2, 2) * Range; //”¼Œa‚È‚Ì‚Å“ñ”{

        //‰ñ•œ‚ÍüŠú“I‚És‚¢‚½‚¢‚Ì‚Å‚±‚±‚ÅŒÄ‚Ô
        StartCoroutine(Heal());
    }

    // Update is called once per frame
    void Update()
    {
        colliderPlayer = Physics2D.OverlapCircleAll(transform.position, Range, LayerMask.GetMask("PlayerFighter"));
        colliderEnemy = Physics2D.OverlapCircleAll(transform.position, Range, LayerMask.GetMask("EnemyFighter"));
    }

    //‰ñ•œˆ—
    private IEnumerator Heal()
    {
        while (true)
        {
            yield return new WaitForSeconds(HealTime);

            //–¡•û§ˆ³ó‘Ô‚Ìê‡A”ÍˆÍ“à‚Ì–¡•û•ºm‚ğ‰ñ•œ
            if (this.tag == "PlayerBase")
            {
                foreach (Collider2D Fighter in colliderPlayer)
                {
                    FighterStatus fighterStatus = Fighter.gameObject.GetComponent<FighterStatus>();

                    if (fighterStatus.NowHp < (fighterStatus.MaxHp + fighterStatus.MaxHpBuff))
                    {
                        fighterStatus.NowHp += Mathf.Round((fighterStatus.MaxHp + fighterStatus.MaxHpBuff) / 10);
                    }
                }
            }

            //“G§ˆ³ó‘Ô‚Ìê‡A”ÍˆÍ“à‚Ì“G•ºm‚ğ‰ñ•œ
            else if (this.tag == "EnemyBase")
            {
                foreach (Collider2D Fighter in colliderEnemy)
                {
                    FighterStatus fighterStatus = Fighter.gameObject.GetComponent<FighterStatus>();

                    if (fighterStatus.NowHp < (fighterStatus.MaxHp + fighterStatus.MaxHpBuff))
                    {
                        fighterStatus.NowHp += Mathf.Round((fighterStatus.MaxHp + fighterStatus.MaxHpBuff) / 10);
                    }
                }
            }
        }
    }
}
