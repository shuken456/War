using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAction : MonoBehaviour
{
    private BattleManager BaManager;

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
    private string EnemyBaseTag;
    //�������_�̃^�O
    private string PlayerBaseTag;

    //�U�����t���O
    private bool AtkNow = false;

    //�U������̃X�e�[�^�X
    public FighterStatus EnemyStatus;

    //��̃v���n�u
    public Arrow arrowPrefab;

    public Vector2 SettingPosition;

    private AudioSource AtkSE;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        MyStatus = GetComponent<FighterStatus>();
        AtkSE = GetComponent<AudioSource>();

        if (GameObject.Find("BattleManager"))
        {
            BaManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        }

        //�����̃^�O�œG�̃^�O�𔻒f
        if (this.tag == "PlayerFighter" || this.tag == "SortieSettingFighter")
        {
            EnemyTag = "EnemyFighter";
            EnemyBaseTag = "EnemyBase";
            PlayerBaseTag = "PlayerBase";
        }
        else
        {
            EnemyTag = "PlayerFighter";
            EnemyBaseTag = "PlayerBase";
            PlayerBaseTag = "EnemyBase";
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
        if (Common.BattleMode)
        {
            if (Time.timeScale >= 1 && CheckEnemy())
            {
                Move();
            }
            //�o���������̈ʒu�����@�i��Q�����ɏo���ł��Ȃ��悤�Ɂj
            else if (this.gameObject.layer == LayerMask.NameToLayer("SortieSettingFighter"))
            {
                Vector3 SettingPosition2 = Quaternion.Euler(0, 0, this.gameObject.transform.parent.gameObject.transform.transform.eulerAngles.z) * SettingPosition;
                var col = Physics2D.OverlapPoint(this.gameObject.transform.parent.gameObject.transform.position + SettingPosition2, LayerMask.GetMask("Obstacle","PlayerBase"));

                if (!col)
                {
                    this.gameObject.transform.localPosition = SettingPosition;
                }
            }
        }  
    }

    private void OnDestroy()
    {
        if(Common.BattleMode)
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
                        fs.DeadUnitLeader = true;
                        fs.AtkPowerBuff = 0;
                        fs.MaxHpBuff = 0;
                        fs.MoveSpeedBuff = 0;

                        if (fs.NowHp > fs.MaxHp)
                        {
                            fs.NowHp = fs.MaxHp;
                        }
                    }
                }
            }

            //�o���l���i�[
            if (this.tag == "PlayerFighter" && BaManager && !BaManager.ExpDic.ContainsKey(MyStatus.FighterName))
            {
                BaManager.ExpDic.Add(MyStatus.FighterName, MyStatus.Exp);
            }

            //���������R�̏ꍇ�Q�[���Z�b�g
            if (MyStatus.Type == 5 && this.tag == "EnemyFighter" && BaManager.StartFlg)
            {
                //����
                BaManager.Win();
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

        //�ߐڍU�����́A�G��������x���ꂽ��^�[�Q�b�g����폜
        if (EnemyStatus != null)
        {
            Vector2 direction = EnemyStatus.transform.position - this.transform.position;
            float MaxDirection;
            //�G���m���G�邩�ɂ���ċ�����ς���(�G��͑傫�����߁A���m�Ɠ��������ŊǗ��ł��Ȃ�)
            if (EnemyStatus.gameObject.tag == EnemyTag)
            {
                //�R�����ǂ����ɂ���ċ�����ς���
                if(EnemyStatus.Type <= 3)
                {
                    MaxDirection = 1.5f;
                }
                else
                {
                    MaxDirection = 2f;
                }
            }
            else
            {
                MaxDirection = 2.5f;
            }

            if (Mathf.Abs(direction.x) > MaxDirection || Mathf.Abs(direction.y) > MaxDirection)
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
        if (targetFighter == null)
        {
            float range = 0;

            //�G���m�͍L���͈͂ŁA�������m�͋����͈͂œG��T��
            if (this.gameObject.tag == "PlayerFighter")
            {
                range = 1.5f;
            }
            else if(MyStatus.NowStamina >= (MyStatus.MaxStamina / 2))
            {
                range = 7f;
            }

            var collider = Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask(EnemyTag));
            if (collider != null)
            {
                Ray2D ray = new Ray2D(this.gameObject.transform.position, collider.gameObject.transform.position - this.gameObject.transform.position);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Vector2.Distance(this.gameObject.transform.position, collider.gameObject.transform.position), LayerMask.GetMask("Obstacle",PlayerBaseTag));

                if (!hit.collider)
                {
                    //��Q�����ԂɂȂ���΃^�[�Q�b�g�ɂ���
                    targetFighter = collider.gameObject.transform;
                }
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
            
            float moveSpeed = MyStatus.MoveSpeed + MyStatus.MoveSpeedBuff;

            //�X�^�~�i����
            if (MyStatus.NowStamina > 0)
            {
                MyStatus.NowStamina -= Time.deltaTime;
            }
            else
            {
                //�X�^�~�i���Ȃ���Έړ����x����
                moveSpeed /= 2;
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
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") || collision.gameObject.layer == this.gameObject.layer) && 
            targetPlace.Count > 0 && Mathf.Abs(transform.position.x - targetPlace[0].x) < 1.3f && Mathf.Abs(transform.position.y - targetPlace[0].y) < 1.3f)
        {
            targetPlace.RemoveAt(0);
        }

        //�U���ΏۂƐڐG�����ꍇ�U���I
        if ((collision.gameObject.tag == EnemyTag || collision.gameObject.tag == EnemyBaseTag) && MyStatus.Type != 2 && !AtkNow && EnemyStatus == null)
        {
            EnemyStatus = collision.gameObject.GetComponent<FighterStatus>();
            StartCoroutine(Attack());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�����^�[�Q�b�g�ɐڐG�����ꍇ�A���������Ȃ��悤�Ɉ�x�폜
        if (collision.gameObject.transform == targetFighter && collision.gameObject.layer == this.gameObject.layer)
        {
            targetFighterSave = targetFighter;
            targetFighter = null;
        }

        //�U���ΏۂƐڐG�����ꍇ�U���I
        if ((collision.gameObject.tag == EnemyTag || collision.gameObject.tag == EnemyBaseTag) && MyStatus.Type != 2 && !AtkNow && EnemyStatus == null)
        {
            EnemyStatus = collision.gameObject.GetComponent<FighterStatus>();
            StartCoroutine(Attack());
        }
    }

    //�U��(�|���ȊO)
    private IEnumerator Attack()
    {
        AtkNow = true;
        float speed = 15 / (float)MyStatus.AtkSpeed; //�U���X�s�[�h

        anim.SetInteger("Action", AttackAction);
        anim.SetFloat("AtkSpeed", (float)MyStatus.AtkSpeed / 15); //�A�j���[�V�����X�s�[�h�ݒ�

        while (EnemyStatus != null)
        {
            float power = MyStatus.AtkPower + MyStatus.AtkPowerBuff; //�U����

            //�����]��
            ChangeDirection(EnemyStatus.gameObject.transform.position);

            yield return new WaitForSeconds(speed);

            //�X�^�~�i����
            if (MyStatus.NowStamina > 0)
            {
                MyStatus.NowStamina -= speed;
            }
            else
            {
                //�X�^�~�i���Ȃ���΍U���͌���
                power /= 2;
            }

            //�G�Ƀ_���[�W��^����
            if (EnemyStatus != null)
            {
                if(AtkSE)
                {
                    AtkSE.Play();
                }
                
                //�������R���ő��肪�����Ȃ�U���̓A�b�v
                if (MyStatus.Type == 4 && EnemyStatus.Type == 1)
                {
                    power *= 1.5f;
                }

                //�o���l
                MyStatus.Exp += 2;
                EnemyStatus.Exp += 2;

                EnemyStatus.NowHp -= power;
                if (EnemyStatus.NowHp <= 0 && EnemyStatus.gameObject.tag == EnemyTag)
                {
                    //���O�\��
                    if (this.tag == "PlayerFighter")
                    {
                        StartCoroutine(BaManager.LogUI.DrawLog("<size=30>" + MyStatus.FighterName + "</size>\n" + EnemyStatus.FighterName + "��|�����I"));
                    }
                    else if (this.tag == "EnemyFighter")
                    {
                        if(EnemyStatus.UnitLeader)
                        {
                            StartCoroutine(BaManager.LogUI.DrawLog("<size=30><color=red>" + EnemyStatus.FighterName + "(������)</color></size>\n" + MyStatus.FighterName + "�ɓ|���ꂽ�I"));
                        }
                        else
                        {
                            StartCoroutine(BaManager.LogUI.DrawLog("<size=30>" + EnemyStatus.FighterName + "</size>\n" + MyStatus.FighterName + "�ɓ|���ꂽ�I"));
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
        float speed = 15 / (float)MyStatus.AtkSpeed;//�U���X�s�[�h

        while (true)
        {
            float power = MyStatus.AtkPower + MyStatus.AtkPowerBuff; //�U����

            yield return new WaitForSeconds(speed);

            //TODO �����œG��T���āA�������
            var collider = Physics2D.OverlapCircle(transform.position, 6f, LayerMask.GetMask(EnemyTag, EnemyBaseTag));

            if (collider != null && (collider.tag == EnemyTag || collider.tag == EnemyBaseTag))
            {
                Ray2D ray = new Ray2D(this.gameObject.transform.position, collider.gameObject.transform.position - this.gameObject.transform.position);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Vector2.Distance(this.gameObject.transform.position, collider.gameObject.transform.position), LayerMask.GetMask("Obstacle", PlayerBaseTag));

                if (!hit.collider)
                {
                    //��Q�����ԂɂȂ���΃^�[�Q�b�g�ɂ���
                    AtkNow = true;
                    anim.SetInteger("Action", AttackAction);

                    EnemyStatus = collider.gameObject.GetComponent<FighterStatus>();

                    //���܂ɗ�����̂�if�����Ă܂�
                    if(EnemyStatus)
                    {
                        //�����]��
                        ChangeDirection(EnemyStatus.gameObject.transform.position);
                    }

                    //�U���o���l
                    MyStatus.Exp += 2;

                    //�X�^�~�i����
                    if (MyStatus.NowStamina > 0)
                    {
                        MyStatus.NowStamina -= speed;
                    }
                    else
                    {
                        //�X�^�~�i���Ȃ���΍U���͌���
                        power /= 2;
                    }

                    if (AtkSE)
                    {
                        AtkSE.Play();
                    }

                    //��𐶐�����
                    arrowPrefab.targetEnemyStatus = collider.GetComponent<FighterStatus>();
                    var arrow = Instantiate(arrowPrefab, transform.position, Quaternion.FromToRotation(Vector3.right, collider.transform.position - transform.position));
                    arrow.ArcherName = MyStatus.FighterName;
                    arrow.AtkPower = power;
                    arrow.GetComponent<SpriteRenderer>().color = this.gameObject.GetComponent<SpriteRenderer>().color;
                }
                else
                {
                    EnemyStatus = null;
                    AtkNow = false;
                }
            }
            else
            {
                EnemyStatus = null;
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
