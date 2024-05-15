using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAction : MonoBehaviour
{
    // アニメーション
    private Animator anim;

    //設定された目標移動地点
    public List<Vector3> targetPlace = new List<Vector3>();

    //設定された目標兵士
    public Transform targetFighter = null;
    public Transform targetFighterSave = null;

    //現在の目標地点
    private Vector3 NowTargetPlace = new Vector3();

    //自分のステータス
    private FighterStatus MyStatus;

    //アニメーションのアクション管理
    private int StandAction = 0;
    private int RunAction = 1;
    private int AttackAction = 2;

    //敵のタグ
    private string EnemyTag;

    //攻撃中フラグ
    private bool Atk = false;

    //攻撃相手のステータス
    public FighterStatus EnemyStatus;

    //矢のプレハブ
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

        //弓兵の場合、射程内に敵がいるか確認する
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

    //敵の状態を確認
    bool CheckEnemy()
    {
        //弓兵の場合、攻撃中か否かで判断
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

    //移動
    private void Move()
    {
        //移動目標を設定
        if (this.gameObject.tag == "EnemyFighter" && targetFighter == null)
        {
            //敵を探す
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
        //ターゲットの距離がある程度離れたら、再度追わせる
        else if (targetFighterSave && (Mathf.Abs(transform.position.x - targetFighterSave.position.x) > 1.5f || Mathf.Abs(transform.position.y - targetFighterSave.position.y) > 1.5f))
        {
            targetFighter = targetFighterSave;
            targetFighterSave = null;
        }
        else
        {
            NowTargetPlace = Vector3.zero;
        }

        //移動目標がある場合、移動
        if (NowTargetPlace != Vector3.zero)
        {
            anim.SetInteger("Action", RunAction);
            
            float moveSpeed;

            //スタミナ減少
            if (MyStatus.NowStamina > 0)
            {
                MyStatus.NowStamina -= Time.deltaTime;
                moveSpeed = MyStatus.MoveSpeed + MyStatus.MoveSpeedBuff;
            }
            else
            {
                moveSpeed = (MyStatus.MoveSpeed + MyStatus.MoveSpeedBuff) / 2;
            }

            //移動
            var v = NowTargetPlace - transform.position;
            transform.position += v.normalized * (moveSpeed / 10) * Time.deltaTime;
            anim.SetFloat("RunSpeed", moveSpeed / 10); //アニメーションスピード設定

            ChangeDirection(NowTargetPlace);

            //第一移動目標に着いた場合、リストから削除
            if (targetPlace.Count > 0 && Mathf.Abs(transform.position.x - NowTargetPlace.x) < 0.1f && Mathf.Abs(transform.position.y - NowTargetPlace.y) < 0.1f)
            {
                targetPlace.RemoveAt(0);
            }
        }
        else
        {
            //待機
            anim.SetInteger("Action", StandAction);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            //スタミナ回復
            if (MyStatus.NowStamina < MyStatus.MaxStamina)
            {
                MyStatus.NowStamina += (MyStatus.MaxStamina / 10) * Time.deltaTime;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //障害物or味方兵士に触れた場合、ある程度目的地に近ければ着いたことにする　※つっかえ防止
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerFighter")) && 
            targetPlace.Count > 0 && Mathf.Abs(transform.position.x - targetPlace[0].x) < 1.5f && Mathf.Abs(transform.position.y - targetPlace[0].y) < 1.5f)
        {
            targetPlace.RemoveAt(0);
        }

        //攻撃対象と接触した場合攻撃！
        if (MyStatus.Type != 2 && !Atk && EnemyStatus == null && collision.gameObject.tag == EnemyTag)
        {
            EnemyStatus = collision.gameObject.GetComponent<FighterStatus>();
            StartCoroutine(Attack());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //味方ターゲットに接触した場合、押し続けないように一度削除
        if (collision.gameObject.transform == targetFighter && collision.gameObject.layer == this.gameObject.layer)
        {
            targetFighterSave = targetFighter;
            targetFighter = null;
        }

        //攻撃対象と接触した場合攻撃！
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
        anim.SetFloat("AtkSpeed", (float)MyStatus.AtkSpeed / 10); //アニメーションスピード設定

        while (EnemyStatus != null)
        {
            ChangeDirection(EnemyStatus.gameObject.transform.position);

            yield return new WaitForSeconds(speed);

            //スタミナ減少
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

    //弓兵用の攻撃メソッド
    private IEnumerator SearchAndShot()
    {
        int power;
        float speed = 10 / (float)MyStatus.AtkSpeed;

        while (true)
        {
            yield return new WaitForSeconds(speed);

            //TODO ここで敵を探して、矢を撃つ githubtest
            var collider = Physics2D.OverlapCircle(transform.position, 5f, LayerMask.GetMask(EnemyTag));

            if (collider != null && collider.tag == EnemyTag)
            {
                Atk = true;
                anim.SetInteger("Action", AttackAction);

                EnemyStatus = collider.gameObject.GetComponent<FighterStatus>();

                //方向転換
                ChangeDirection(EnemyStatus.gameObject.transform.position);

                //スタミナ減少
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

    //方向転換
    private void ChangeDirection(Vector3 TargetDirection)
    {
        //方向転換
        var v = TargetDirection - transform.position;
        //右を向く
        if (v.x >= 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        //左を向く
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
