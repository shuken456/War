using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private BattleManager BaManager;

    //�^�[�Q�b�g�̃X�e�[�^�X
    public FighterStatus targetEnemyStatus;

    //�|��ł������m�̖��O
    public string ArcherName;
    //�U����
    public float AtkPower;
    //��̑���
    public float ArrowSpeed;

    private string EnemyTag;
    private string EnemyBaseTag;

    void Start()
    {
        BaManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();

        //�����̃^�O�œG�̃^�O�𔻒f
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

        //�^�[�Q�b�g�̏ꏊ�ɔ��
        var v = targetEnemyStatus.gameObject.transform.position - transform.position;
        transform.position += v.normalized * ArrowSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�U��
        if (collision.gameObject.tag == EnemyTag || collision.gameObject.tag == EnemyBaseTag)
        {
            targetEnemyStatus = collision.gameObject.GetComponent<FighterStatus>();
            targetEnemyStatus.Exp += 2;

            //�����ɓ��������ꍇ�_���[�W����,�R���͑���
            if (targetEnemyStatus.Type == 3)
            {
                targetEnemyStatus.NowHp -= AtkPower * 0.2f;
            }
            else if (targetEnemyStatus.Type == 4)
            {
                targetEnemyStatus.NowHp -= AtkPower * 1.5f;
            }
            else
            {
                targetEnemyStatus.NowHp -= AtkPower;
            }

            if (targetEnemyStatus.NowHp <= 0 && targetEnemyStatus.gameObject.tag == EnemyTag)
            {
                //���O�\��
                if (this.tag == "PlayerArrow")
                {
                    StartCoroutine(BaManager.LogUI.DrawLog("<size=30>" + ArcherName + "</size>\n" + targetEnemyStatus.FighterName + "��|�����I"));
                }
                else
                {
                    if (targetEnemyStatus.UnitLeader)
                    {
                        StartCoroutine(BaManager.LogUI.DrawLog("<size=30><color=red>" + targetEnemyStatus.FighterName + "(������)</color></size>\n" + ArcherName + "�ɓ|���ꂽ�I"));
                    }
                    else
                    {
                        StartCoroutine(BaManager.LogUI.DrawLog("<size=30>" + targetEnemyStatus.FighterName + "</size>\n" + ArcherName + "�ɓ|���ꂽ�I"));
                    }
                }

                Destroy(targetEnemyStatus.gameObject);
            }
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
