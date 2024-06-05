using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    //�͈͉摜
    public SpriteRenderer rangeRenderer;

    //�͈�
    public float Range;

    //�񕜎���
    public float HealTime;

    //�񕜕\���v���n�u
    public GameObject HealPrefab;
    //��
    public GameObject Fire;

    //�͈͓��̕��m
    private Collider2D[] colliderPlayer;
    private Collider2D[] colliderEnemy;

    private FighterStatus MyStatus;

    // Start is called before the first frame update
    void Start()
    {
        MyStatus = this.gameObject.GetComponent<FighterStatus>();
        rangeRenderer.transform.localScale = new Vector3(2, 2) * Range; //���a�Ȃ̂œ�{

        //�񕜂͎����I�ɍs�������̂ł����ŌĂ�
        StartCoroutine(Heal());
    }

    // Update is called once per frame
    void Update()
    {
        //�邪�j�󂳂ꂽ��X�e�[�W�I��
        if(MyStatus.NowHp <= 0 && Time.timeScale == 1)
        {
            Time.timeScale = 0;
            StartCoroutine(GameSet());
        }

        colliderPlayer = Physics2D.OverlapCircleAll(transform.position, Range, LayerMask.GetMask("PlayerFighter"));
        colliderEnemy = Physics2D.OverlapCircleAll(transform.position, Range, LayerMask.GetMask("EnemyFighter"));
    }

    private IEnumerator GameSet()
    {
        GameObject.Find("Virtual Camera").transform.position = this.gameObject.transform.position - new Vector3(0, 0, 1);//�邪��ʒ����ɗ���悤�ɂ���

        yield return new WaitForSecondsRealtime(1f);

        this.gameObject.transform.Find("Fire").gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);

        if (this.gameObject.tag == "EnemyBase")
        {
            GameObject.Find("BattleManager").GetComponent<BattleManager>().BattleWin();
        }
        else
        {
            GameObject.Find("BattleManager").GetComponent<BattleManager>().BattleLose();
        }
    }

    //�񕜏���
    private IEnumerator Heal()
    {
        while (true)
        {
            yield return new WaitForSeconds(HealTime);

            //����������Ԃ̏ꍇ�A�͈͓��̖������m����
            if (this.tag == "PlayerBase")
            {
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

            //�G������Ԃ̏ꍇ�A�͈͓��̓G���m����
            else if (this.tag == "EnemyBase")
            {
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
