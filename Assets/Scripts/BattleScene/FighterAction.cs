using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAction : MonoBehaviour
{
    // �A�j���[�V����
    private Animator anim;

    //�ݒ肳�ꂽ�ڕW�ړ��n�_
    public List<Vector3> targetPlace = new List<Vector3>();

    //�ݒ肳�ꂽ�ڕW���m
    public Transform targetFighter = null;
    public Transform targetFighterSave = null;

    //���݂̖ڕW�n�_
    private Vector3 NowTargetPlace = new Vector3();

    //�����̃X�e�[�^�X
    private FighterStatus MyStatus;

    //�A�j���[�V�����̃A�N�V�����Ǘ�
    private int StandAction = 0;
    private int RunAction = 1;
    private int AttackAction = 2;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        MyStatus = GetComponent<FighterStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale >= 1)
        {
            Move();
        }
    }

    //�ړ�
    private void Move()
    {
        //�ړ��ڕW��ݒ�
        if (targetPlace.Count > 0)
        {
            NowTargetPlace = targetPlace[0];
        }
        else if (targetFighter)
        {
            NowTargetPlace = targetFighter.position;
        }
        //�^�[�Q�b�g�̋�����������x���ꂽ��A�ēx�ǂ킹��
        else if (targetFighterSave && (Mathf.Abs(transform.position.x - targetFighterSave.position.x) > 1.5f || Mathf.Abs(transform.position.y - targetFighterSave.position.y) > 1.5f))
        {
            targetFighter = targetFighterSave;
            targetFighterSave = null;
        }
        else
        {
            NowTargetPlace = Vector3.zero;
        }

        //�ړ��ڕW������ꍇ�A�ړ�
        if (NowTargetPlace != Vector3.zero)
        {
            anim.SetInteger("Action", RunAction);
            
            float moveSpeed;

            //�X�^�~�i����
            if (MyStatus.NowStamina > 0)
            {
                MyStatus.NowStamina -= Time.deltaTime;
                moveSpeed = MyStatus.MoveSpeed + MyStatus.MoveSpeedBuff;
            }
            else
            {
                moveSpeed = MyStatus.MoveSpeed / 2;
            }

            //�ړ�
            var v = NowTargetPlace - transform.position;
            transform.position += v.normalized * (moveSpeed / 10) * Time.deltaTime;
            anim.SetFloat("RunSpeed", moveSpeed / 10); //�A�j���[�V�����X�s�[�h�ݒ�

            //�E������
            if (v.x >= 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            //��������
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            //���ړ��ڕW�ɒ������ꍇ�A���X�g����폜
            if (targetPlace.Count > 0 && Mathf.Abs(transform.position.x - NowTargetPlace.x) < 0.1f && Mathf.Abs(transform.position.y - NowTargetPlace.y) < 0.1f)
            {
                targetPlace.RemoveAt(0);
            }
        }
        else
        {
            //�ҋ@
            anim.SetInteger("Action", StandAction);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            //�X�^�~�i��
            if (MyStatus.NowStamina < MyStatus.MaxStamina)
            {
                MyStatus.NowStamina += (MyStatus.MaxStamina / 10) * Time.deltaTime;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //��Q��or�������m�ɐG�ꂽ�ꍇ�A������x�ړI�n�ɋ߂���Β��������Ƃɂ���@���������h�~
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerFighter")) && 
            targetPlace.Count > 0 && Mathf.Abs(transform.position.x - targetPlace[0].x) < 1.5f && Mathf.Abs(transform.position.y - targetPlace[0].y) < 1.5f)
        {
            targetPlace.RemoveAt(0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�����^�[�Q�b�g�ɐڐG�����ꍇ�A���������Ȃ��悤�Ɉ�x�폜
        if (collision.gameObject.transform == targetFighter && collision.gameObject.layer == LayerMask.NameToLayer("PlayerFighter"))
        {
            targetFighterSave = targetFighter;
            targetFighter = null;
        }
    }
}
