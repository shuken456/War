using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    //�͈͉摜
    public GameObject rangeRenderer;

    //�͈�
    public Vector2 Range;

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
        rangeRenderer.GetComponent<SpriteRenderer>().transform.localScale = Range;
    }

    private void OnEnable()
    {
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
    }

    //�Q�[���Z�b�g�@�邪�R����Ƃ����������
    private IEnumerator GameSet()
    {
        GameObject.Find("Virtual Camera").transform.position = this.gameObject.transform.position - new Vector3(0, 0, 1);//�邪��ʒ����ɗ���悤�ɂ���

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

    //�񕜏���
    private IEnumerator Heal()
    {
        while (true)
        {
            yield return new WaitForSeconds(HealTime);

            //������̏ꍇ�A�͈͓��̖������m����
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

            //�G��̏ꍇ�A�͈͓��̓G���m����
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
