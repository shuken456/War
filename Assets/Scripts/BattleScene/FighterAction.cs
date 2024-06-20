using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAction : MonoBehaviour
{
    private BattleManager BaManager;

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
    private string EnemyBaseTag;
    //味方拠点のタグ
    private string PlayerBaseTag;

    //攻撃中フラグ
    private bool AtkNow = false;

    //攻撃相手のステータス
    public FighterStatus EnemyStatus;

    //矢のプレハブ
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

        //自分のタグで敵のタグを判断
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

        //弓兵の場合、射程内に敵がいるか確認する
        if (MyStatus && MyStatus.Type == 2)
        {
            StartCoroutine(SearchAndShot());
        }
    }

    private void OnEnable()
    {
        EnemyStatus = null;
        AtkNow = false;

        //弓兵の場合、射程内に敵がいるか確認する
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
            //出撃準備中の位置調整　（障害物内に出撃できないように）
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
            //自分が部隊長だった場合、部隊メンバーのバフを無くす
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

            //経験値を格納
            if (this.tag == "PlayerFighter" && BaManager && !BaManager.ExpDic.ContainsKey(MyStatus.FighterName))
            {
                BaManager.ExpDic.Add(MyStatus.FighterName, MyStatus.Exp);
            }

            //自分が将軍の場合ゲームセット
            if (MyStatus.Type == 5 && this.tag == "EnemyFighter" && BaManager.StartFlg)
            {
                //勝ち
                BaManager.Win();
            }
        }
    }
   
    //敵の状態を確認(true = 攻撃相手なし false = 攻撃相手あり)
    bool CheckEnemy()
    {
        //弓兵の場合、攻撃中か否かで判断
        if (MyStatus.Type == 2)
        {
            return !AtkNow;
        }

        //近接攻撃中は、敵がある程度離れたらターゲットから削除
        if (EnemyStatus != null)
        {
            Vector2 direction = EnemyStatus.transform.position - this.transform.position;
            float MaxDirection;
            //敵兵士か敵城かによって距離を変える(敵城は大きいため、兵士と同じ距離で管理できない)
            if (EnemyStatus.gameObject.tag == EnemyTag)
            {
                //騎兵かどうかによって距離を変える
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

    //移動
    private void Move()
    {
        //移動目標を設定
        if (targetFighter == null)
        {
            float range = 0;

            //敵兵士は広い範囲で、味方兵士は狭い範囲で敵を探す
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
                    //障害物が間になければターゲットにする
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
            
            float moveSpeed = MyStatus.MoveSpeed + MyStatus.MoveSpeedBuff;

            //スタミナ減少
            if (MyStatus.NowStamina > 0)
            {
                MyStatus.NowStamina -= Time.deltaTime;
            }
            else
            {
                //スタミナがなければ移動速度減少
                moveSpeed /= 2;
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
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") || collision.gameObject.layer == this.gameObject.layer) && 
            targetPlace.Count > 0 && Mathf.Abs(transform.position.x - targetPlace[0].x) < 1.3f && Mathf.Abs(transform.position.y - targetPlace[0].y) < 1.3f)
        {
            targetPlace.RemoveAt(0);
        }

        //攻撃対象と接触した場合攻撃！
        if ((collision.gameObject.tag == EnemyTag || collision.gameObject.tag == EnemyBaseTag) && MyStatus.Type != 2 && !AtkNow && EnemyStatus == null)
        {
            EnemyStatus = collision.gameObject.GetComponent<FighterStatus>();
            StartCoroutine(Attack());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //味方ターゲットに接触した場合、押し続けないように一度削除
        if (collision.gameObject.transform == targetFighter && collision.gameObject.layer == this.gameObject.layer)
        {
            targetFighterSave = targetFighter;
            targetFighter = null;
        }

        //攻撃対象と接触した場合攻撃！
        if ((collision.gameObject.tag == EnemyTag || collision.gameObject.tag == EnemyBaseTag) && MyStatus.Type != 2 && !AtkNow && EnemyStatus == null)
        {
            EnemyStatus = collision.gameObject.GetComponent<FighterStatus>();
            StartCoroutine(Attack());
        }
    }

    //攻撃(弓兵以外)
    private IEnumerator Attack()
    {
        AtkNow = true;
        float speed = 15 / (float)MyStatus.AtkSpeed; //攻撃スピード

        anim.SetInteger("Action", AttackAction);
        anim.SetFloat("AtkSpeed", (float)MyStatus.AtkSpeed / 15); //アニメーションスピード設定

        while (EnemyStatus != null)
        {
            float power = MyStatus.AtkPower + MyStatus.AtkPowerBuff; //攻撃力

            //方向転換
            ChangeDirection(EnemyStatus.gameObject.transform.position);

            yield return new WaitForSeconds(speed);

            //スタミナ減少
            if (MyStatus.NowStamina > 0)
            {
                MyStatus.NowStamina -= speed;
            }
            else
            {
                //スタミナがなければ攻撃力減少
                power /= 2;
            }

            //敵にダメージを与える
            if (EnemyStatus != null)
            {
                if(AtkSE)
                {
                    AtkSE.Play();
                }
                
                //自分が騎兵で相手が歩兵なら攻撃力アップ
                if (MyStatus.Type == 4 && EnemyStatus.Type == 1)
                {
                    power *= 1.5f;
                }

                //経験値
                MyStatus.Exp += 2;
                EnemyStatus.Exp += 2;

                EnemyStatus.NowHp -= power;
                if (EnemyStatus.NowHp <= 0 && EnemyStatus.gameObject.tag == EnemyTag)
                {
                    //ログ表示
                    if (this.tag == "PlayerFighter")
                    {
                        StartCoroutine(BaManager.LogUI.DrawLog("<size=30>" + MyStatus.FighterName + "</size>\n" + EnemyStatus.FighterName + "を倒した！"));
                    }
                    else if (this.tag == "EnemyFighter")
                    {
                        if(EnemyStatus.UnitLeader)
                        {
                            StartCoroutine(BaManager.LogUI.DrawLog("<size=30><color=red>" + EnemyStatus.FighterName + "(部隊長)</color></size>\n" + MyStatus.FighterName + "に倒された！"));
                        }
                        else
                        {
                            StartCoroutine(BaManager.LogUI.DrawLog("<size=30>" + EnemyStatus.FighterName + "</size>\n" + MyStatus.FighterName + "に倒された！"));
                        }
                    }
                    Destroy(EnemyStatus.gameObject);
                    EnemyStatus = null;
                }
            }
        }
        AtkNow = false;
    }

    //弓兵用の攻撃メソッド
    private IEnumerator SearchAndShot()
    {
        float speed = 15 / (float)MyStatus.AtkSpeed;//攻撃スピード

        while (true)
        {
            float power = MyStatus.AtkPower + MyStatus.AtkPowerBuff; //攻撃力

            yield return new WaitForSeconds(speed);

            //TODO ここで敵を探して、矢を撃つ
            var collider = Physics2D.OverlapCircle(transform.position, 6f, LayerMask.GetMask(EnemyTag, EnemyBaseTag));

            if (collider != null && (collider.tag == EnemyTag || collider.tag == EnemyBaseTag))
            {
                Ray2D ray = new Ray2D(this.gameObject.transform.position, collider.gameObject.transform.position - this.gameObject.transform.position);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Vector2.Distance(this.gameObject.transform.position, collider.gameObject.transform.position), LayerMask.GetMask("Obstacle", PlayerBaseTag));

                if (!hit.collider)
                {
                    //障害物が間になければターゲットにする
                    AtkNow = true;
                    anim.SetInteger("Action", AttackAction);

                    EnemyStatus = collider.gameObject.GetComponent<FighterStatus>();

                    //たまに落ちるのでif書いてます
                    if(EnemyStatus)
                    {
                        //方向転換
                        ChangeDirection(EnemyStatus.gameObject.transform.position);
                    }

                    //攻撃経験値
                    MyStatus.Exp += 2;

                    //スタミナ減少
                    if (MyStatus.NowStamina > 0)
                    {
                        MyStatus.NowStamina -= speed;
                    }
                    else
                    {
                        //スタミナがなければ攻撃力減少
                        power /= 2;
                    }

                    if (AtkSE)
                    {
                        AtkSE.Play();
                    }

                    //矢を生成する
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

    //方向転換
    private void ChangeDirection(Vector3 TargetDirection)
    {
        //方向転換
        var v = TargetDirection - transform.position;
        //右を向く
        if (v.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        //左を向く
        else if(v.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
