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

    //移動
    private void Move()
    {
        //移動目標を設定
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
                moveSpeed = MyStatus.MoveSpeed / 2;
            }

            //移動
            var v = NowTargetPlace - transform.position;
            transform.position += v.normalized * (moveSpeed / 10) * Time.deltaTime;
            anim.SetFloat("RunSpeed", moveSpeed / 10); //アニメーションスピード設定

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        //味方ターゲットに接触した場合、押し続けないように一度削除
        if (collision.gameObject.transform == targetFighter && collision.gameObject.layer == LayerMask.NameToLayer("PlayerFighter"))
        {
            targetFighterSave = targetFighter;
            targetFighter = null;
        }
    }
}
