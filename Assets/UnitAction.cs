using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAction : MonoBehaviour
{
    // アニメーション
    private Animator anim;

    //設定された目標移動地点
    public List<Vector3> targetPlace = new List<Vector3>();

    //設定された目標ユニット
    public Transform targetUnit = null;

    //現在の目標地点
    private Vector3 NowTargetPlace = new Vector3();

    //自分のステータス
    private Status MyStatus;

    //アニメーションのアクション管理
    public int StandAction = 0;
    public int RunAction = 1;
    public int AttackAction = 2;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        MyStatus = GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale >= 1)
        {
            Move();
        }
    }

    //移動
    private void Move()
    {
        //移動目標を設定
        if (targetPlace.Count > 0)
        {
            NowTargetPlace = targetPlace[0];
        }
        else if (targetUnit)
        {
            NowTargetPlace = targetUnit.position;
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
                moveSpeed = MyStatus.MoveSpeed;
            }
            else
            {
                moveSpeed = MyStatus.MoveSpeed / 2;
            }

            //移動
            var v = NowTargetPlace - transform.position;
            transform.position += v.normalized * moveSpeed * Time.deltaTime;

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
            if (MyStatus.NowStamina < MyStatus.MaxHp)
            {
                MyStatus.NowStamina += Time.deltaTime;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //障害物or味方ユニットに触れた場合、ある程度目的地に近ければ着いたことにする　※つっかえ防止
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerUnit")) && 
            targetPlace.Count > 0 && Mathf.Abs(transform.position.x - NowTargetPlace.x) < 1.5f && Mathf.Abs(transform.position.y - NowTargetPlace.y) < 1.5f)
        {
            targetPlace.RemoveAt(0);
        }
    }
}
