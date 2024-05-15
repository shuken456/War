using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    //�^�[�Q�b�g�̃X�e�[�^�X
    public FighterStatus targetEnemyStatus;

    //�U����
    public float AtkPower;
    //��̑���
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

        //�^�[�Q�b�g�̏ꏊ�Ɉړ�
        var v = targetEnemyStatus.gameObject.transform.position - transform.position;
        transform.position += v.normalized * ArrowSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�U��
        if (collision.gameObject.tag == EnemyTag)
        {
            targetEnemyStatus = collision.gameObject.GetComponent<FighterStatus>();

            //�����ɓ��������ꍇ�_���[�W����
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
