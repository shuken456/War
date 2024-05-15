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

    //�G�̃^�O
    private string EnemyTag;

    //�U�����t���O
    private bool Atk = false;

    //�U������̃X�e�[�^�X
    public FighterStatus EnemyStatus;

    //��̃v���n�u
    public Arrow arrowPrefab;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        MyStatus = GetComponent<FighterStatus>();

        if (this.tag == "PlayerFighter")
        {
            EnemyTag = "EnemyFighter";
        }
        else
        {
            EnemyTag = "PlayerFighter";
        }

        //�|���̏ꍇ�A�˒����ɓG�����邩�m�F����
        if(MyStatus.Type == 2)
        {
            StartCoroutine(SearchAndShot());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale >= 1)
        {
            if(CheckEnemy())
            {
                Move();
            }
        }
    }

    //�G�̏�Ԃ��m�F
    bool CheckEnemy()
    {
        //�|���̏ꍇ�A�U�������ۂ��Ŕ��f
        if (MyStatus.Type == 2)
        {
            return !Atk;
        }

        if (EnemyStatus != null)
        {
            Vector2 direction = EnemyStatus.transform.position - this.transform.position;
            if ((Mathf.Abs(direction.x) > 2f || Mathf.Abs(direction.y) > 2f))
            {
                EnemyStatus = null;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    //�ړ�
    private void Move()
    {
        //�ړ��ڕW��ݒ�
        if (this.gameObject.tag == "EnemyFighter" && targetFighter == null)
        {
            //�G��T��
            var collider = Physics2D.OverlapCircle(transform.position, 10f, LayerMask.GetMask(EnemyTag));
            if (collider != null)
            {
                targetFighter = collider.gameObject.transform;
            }
        }
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
                moveSpeed = (MyStatus.MoveSpeed + MyStatus.MoveSpeedBuff) / 2;
            }

            //�ړ�
            var v = NowTargetPlace - transform.position;
            transform.position += v.normalized * (moveSpeed / 10) * Time.deltaTime;
            anim.SetFloat("RunSpeed", moveSpeed / 10); //�A�j���[�V�����X�s�[�h�ݒ�

            ChangeDirection(NowTargetPlace);

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

        //�U���ΏۂƐڐG�����ꍇ�U���I
        if (MyStatus.Type != 2 && !Atk && EnemyStatus == null && collision.gameObject.tag == EnemyTag)
        {
            EnemyStatus = collision.gameObject.GetComponent<FighterStatus>();
            StartCoroutine(Attack());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�����^�[�Q�b�g�ɐڐG�����ꍇ�A���������Ȃ��悤�Ɉ�x�폜
        if (collision.gameObject.transform == targetFighter && collision.gameObject.layer == this.gameObject.layer)
        {
            targetFighterSave = targetFighter;
            targetFighter = null;
        }

        //�U���ΏۂƐڐG�����ꍇ�U���I
        if (MyStatus.Type != 2 && !Atk && EnemyStatus == null && collision.gameObject.tag == EnemyTag)
        {
            EnemyStatus = collision.gameObject.GetComponent<FighterStatus>();
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        Atk = true;
        int power;
        float speed = 10 / (float)MyStatus.AtkSpeed;

        anim.SetInteger("Action", AttackAction);
        anim.SetFloat("AtkSpeed", (float)MyStatus.AtkSpeed / 10); //�A�j���[�V�����X�s�[�h�ݒ�

        while (EnemyStatus != null)
        {
            ChangeDirection(EnemyStatus.gameObject.transform.position);

            yield return new WaitForSeconds(speed);

            //�X�^�~�i����
            if (MyStatus.NowStamina > 0)
            {
                MyStatus.NowStamina -= speed;
                power = MyStatus.AtkPower + MyStatus.AtkPowerBuff;
            }
            else
            {
                power = (MyStatus.AtkPower + MyStatus.AtkPowerBuff) / 2;
            }

            if (EnemyStatus != null)
            {
                EnemyStatus.NowHp -= power;
                if (EnemyStatus.NowHp <= 0)
                {
                    Destroy(EnemyStatus.gameObject);
                    EnemyStatus = null;
                }
            }
        }
        Atk = false;
    }

    //�|���p�̍U�����\�b�h
    private IEnumerator SearchAndShot()
    {
        int power;
        float speed = 10 / (float)MyStatus.AtkSpeed;

        while (true)
        {
            yield return new WaitForSeconds(speed);

            //TODO �����œG��T���āA������� githubtest
            var collider = Physics2D.OverlapCircle(transform.position, 5f, LayerMask.GetMask(EnemyTag));

            if (collider != null && collider.tag == EnemyTag)
            {
                Atk = true;
                anim.SetInteger("Action", AttackAction);

                EnemyStatus = collider.gameObject.GetComponent<FighterStatus>();

                //�����]��
                ChangeDirection(EnemyStatus.gameObject.transform.position);

                //�X�^�~�i����
                if (MyStatus.NowStamina > 0)
                {
                    MyStatus.NowStamina -= speed;
                    power = MyStatus.AtkPower + MyStatus.AtkPowerBuff;
                }
                else
                {
                    power = (MyStatus.AtkPower + MyStatus.AtkPowerBuff) / 2;
                }

                var arrow = Instantiate(arrowPrefab, transform.position, Quaternion.FromToRotation(Vector3.right, collider.transform.position - transform.position));
                arrow.targetEnemyStatus = collider.GetComponent<FighterStatus>();
                arrow.AtkPower = power;
                arrow.ArrowSpeed = 2;
            }

            else
            {
                Atk = false;
            }
        }
    }

    //�����]��
    private void ChangeDirection(Vector3 TargetDirection)
    {
        //�����]��
        var v = TargetDirection - transform.position;
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
    }
}
