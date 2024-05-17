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
    private bool AtkNow = false;

    //�U������̃X�e�[�^�X
    public FighterStatus EnemyStatus;

    //��̃v���n�u
    public Arrow arrowPrefab;

    //�o���l
    public int EXP;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        MyStatus = GetComponent<FighterStatus>();

        //�����̃^�O�œG�̃^�O�𔻒f
        if (this.tag == "PlayerFighter")
        {
            EnemyTag = "EnemyFighter";
        }
        else
        {
            EnemyTag = "PlayerFighter";
        }

        //�|���̏ꍇ�A�˒����ɓG�����邩�m�F����
        if (MyStatus && MyStatus.Type == 2)
        {
            StartCoroutine(SearchAndShot());
        }
    }

    private void OnEnable()
    {
        EnemyStatus = null;
        AtkNow = false;

        //�|���̏ꍇ�A�˒����ɓG�����邩�m�F����
        if (MyStatus && MyStatus.Type == 2)
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

    private void OnDestroy()
    {
        //�������������������ꍇ�A���������o�[�̃o�t�𖳂���
        if (MyStatus.UnitLeader)
        {
            GameObject[] tagObjects = GameObject.FindGameObjectsWithTag(this.gameObject.tag);

            foreach (GameObject Fighter in tagObjects)
            {
                FighterStatus fs = Fighter.GetComponent<FighterStatus>();
                if (fs.UnitNum == MyStatus.UnitNum)
                {
                    fs.AtkPowerBuff = 0;
                    fs.MaxHpBuff = 0;
                    fs.MoveSpeedBuff = 0;

                    if(fs.NowHp > fs.MaxHp)
                    {
                        fs.NowHp = fs.MaxHp;
                    }
                }
            }
        }
    }
   
    //�G�̏�Ԃ��m�F(true = �U������Ȃ� false = �U�����肠��)
    bool CheckEnemy()
    {
        //�|���̏ꍇ�A�U�������ۂ��Ŕ��f
        if (MyStatus.Type == 2)
        {
            return !AtkNow;
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
        if (collision.gameObject.tag == EnemyTag && MyStatus.Type != 2 && !AtkNow && EnemyStatus == null)
        {
            EnemyStatus = collision.gameObject.GetComponent<FighterStatus>();
            StartCoroutine(Attack());
        }

        //�o�����ɏ�Q���ɂ������Ȃ��悤��
        if(collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && anim.GetInteger("Action") == StandAction)
        {
            var v = collision.gameObject.transform.position - transform.position;
            if (v.x > 0)
            {
                transform.position += new Vector3(0.5f, 0, 0);
            }
            else if (v.x < 0)
            {
                transform.position -= new Vector3(0.5f, 0, 0);
            }

            if (v.y > 0)
            {
                transform.position += new Vector3(0, 0.5f, 0);
            }
            else if (v.y < 0)
            {
                transform.position -= new Vector3(0, 0.5f, 0);
            }
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
        if (collision.gameObject.tag == EnemyTag && MyStatus.Type != 2 && !AtkNow && EnemyStatus == null)
        {
            EnemyStatus = collision.gameObject.GetComponent<FighterStatus>();
            StartCoroutine(Attack());
        }
    }

    //�U��(�|���ȊO)
    private IEnumerator Attack()
    {
        AtkNow = true;
        int power;�@//�U����
        float speed = 10 / (float)MyStatus.AtkSpeed; //�U���X�s�[�h

        anim.SetInteger("Action", AttackAction);
        anim.SetFloat("AtkSpeed", (float)MyStatus.AtkSpeed / 10); //�A�j���[�V�����X�s�[�h�ݒ�

        while (EnemyStatus != null)
        {
            //�����]��
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

            //�G�Ƀ_���[�W��^����
            if (EnemyStatus != null)
            {
                EXP += power;
                EnemyStatus.NowHp -= power;
                if (EnemyStatus.NowHp <= 0)
                {
                    if (this.tag == "PlayerFighter")
                    {
                        GameObject.Find("BattleManager").GetComponent<BattleManager>().LogUI.DrawLog("<size=30>" + MyStatus.FighterName + "</size>\n" + EnemyStatus.FighterName + "��|�����I");
                    }
                    else if (this.tag == "EnemyFighter")
                    {
                        if(EnemyStatus.UnitLeader)
                        {
                            GameObject.Find("BattleManager").GetComponent<BattleManager>().LogUI.DrawLog("<size=30><color=red>" + EnemyStatus.FighterName + "(������)</color></size>\n" + MyStatus.FighterName + "�ɓ|���ꂽ�I");
                        }
                        else
                        {
                            GameObject.Find("BattleManager").GetComponent<BattleManager>().LogUI.DrawLog("<size=30>" + EnemyStatus.FighterName + "</size>\n" + MyStatus.FighterName + "�ɓ|���ꂽ�I");
                        }
                    }

                    Destroy(EnemyStatus.gameObject);
                    EnemyStatus = null;
                }
            }
        }
        AtkNow = false;
    }

    //�|���p�̍U�����\�b�h
    private IEnumerator SearchAndShot()
    {
        int power;//�U����
        float speed = 10 / (float)MyStatus.AtkSpeed;//�U���X�s�[�h

        while (true)
        {
            yield return new WaitForSeconds(speed);

            //TODO �����œG��T���āA������� githubtest
            var collider = Physics2D.OverlapCircle(transform.position, 5f, LayerMask.GetMask(EnemyTag));

            if (collider != null && collider.tag == EnemyTag)
            {
                AtkNow = true;
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

                //��𐶐�����
                var arrow = Instantiate(arrowPrefab, transform.position, Quaternion.FromToRotation(Vector3.right, collider.transform.position - transform.position));
                arrow.targetEnemyStatus = collider.GetComponent<FighterStatus>();
                arrow.ArcherName = MyStatus.FighterName;
                arrow.AtkPower = power;
                arrow.ArrowSpeed = 2;
                arrow.GetComponent<SpriteRenderer>().color = this.gameObject.GetComponent<SpriteRenderer>().color;
            }

            else
            {
                AtkNow = false;
            }
        }
    }

    //�����]��
    private void ChangeDirection(Vector3 TargetDirection)
    {
        //�����]��
        var v = TargetDirection - transform.position;
        //�E������
        if (v.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        //��������
        else if(v.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
