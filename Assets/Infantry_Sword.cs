using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infantry_Sword : MonoBehaviour
{
    // アニメーション
    private Animator anim;

    //設定された目標移動地点
    public List<Vector3> targetPlace = new List<Vector3>();

    //現在の目標地点
    private Vector3 NowTargetPlace;

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
        Move();
    }

    //移動
    private void Move()
    {
        //移動目標がある場合、移動
        if (targetPlace.Count > 0 && Time.timeScale >= 1)
        {
            anim.SetInteger("Action", RunAction);
            Transform LineObject = this.transform.Find("Line(Clone)");

            //第一移動目標に着いた場合
            if (Mathf.Abs(transform.position.x - NowTargetPlace.x) < 0.1f && Mathf.Abs(transform.position.y - NowTargetPlace.y) < 0.1f)
            {
                //第一移動目標をクリア
                NowTargetPlace = new Vector3(0,0,0);
                targetPlace.RemoveAt(0);

                //移動目標が0になった場合、移動線を消去
                if (targetPlace.Count == 0)
                {
                    Destroy(LineObject.gameObject);
                    anim.SetInteger("Action", StandAction);
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    //次の目標を設定
                    for (int i = 0; i <= targetPlace.Count; i++)
                    {
                        LineObject.gameObject.GetComponent<LineRenderer>().SetPosition(i, LineObject.gameObject.GetComponent<LineRenderer>().GetPosition(i + 1));
                    }
                    LineObject.gameObject.GetComponent<LineRenderer>().positionCount--;
                }
            }
            else
            {
                //移動
                NowTargetPlace = targetPlace[0];
                var v = NowTargetPlace - transform.position;
                transform.position += v.normalized * MyStatus.MoveSpeed * Time.deltaTime;

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
    }
}
