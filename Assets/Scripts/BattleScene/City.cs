using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class City : MonoBehaviour
{
    //コントロールゲーム表示用キャンパス
    public GameObject CanvasWorldSpace;

    //コントロールゲージプレハブ
    public GameObject ControlSlider;

    //街範囲画像
    public SpriteRenderer rangeRenderer;

    //街を制圧するのにかかる時間
    public float ControlTime;

    //街範囲
    public float Range;

    //コントロールゲージ
    private Slider slider;
    private Image sliderImage;
    
    //街の状態
    public enum CityStatus
    {
        Default, //デフォルト
        PlayerBecoming, //味方のものになりつつある状態
        Player, //味方のもの
        EnemyBecoming, //敵のものになりつつある状態
        Enemy //敵のもの
    }

    //現在の街状態
    private CityStatus currentStatus = CityStatus.Default;

    //範囲内の兵士
    private Collider2D[] colliderPlayer;
    private Collider2D[] colliderEnemy;

    //範囲内の兵士数差　負数の場合は敵数の方が多い
    private int FighterCount;

    //回復周期
    public float HealTime;

    // Start is called before the first frame update
    void Start()
    {
        //各設定
        slider = Instantiate(ControlSlider, this.transform.position + new Vector3(0, 1f, 0), Quaternion.identity).GetComponent<Slider>();
        slider.transform.SetParent(CanvasWorldSpace.transform, true);
        slider.maxValue = ControlTime;
        rangeRenderer.transform.localScale = new Vector3(2, 2) * Range; //半径なので二倍

        //スライダーの色変更用に取得
        sliderImage = slider.gameObject.transform.Find("Fill Area/Fill").gameObject.GetComponent<Image>();

        //回復は周期的に行いたいのでここで呼ぶ
        StartCoroutine(Heal());
    }

    // Update is called once per frame
    void Update()
    {
        colliderPlayer = Physics2D.OverlapCircleAll(transform.position, Range, LayerMask.GetMask("PlayerFighter"));
        colliderEnemy = Physics2D.OverlapCircleAll(transform.position, Range, LayerMask.GetMask("EnemyFighter"));

        FighterCount = colliderPlayer.Length - colliderEnemy.Length;

        Control();
    }

    //制圧処理
    private void Control()
    {
        //味方兵士の方が範囲内に多い場合
        if (FighterCount > 0)
        {
            //ゲージが敵バージョンの場合、ゲージを減少させる
            if (currentStatus == CityStatus.EnemyBecoming || currentStatus == CityStatus.Enemy)
            {
                slider.value -= (Time.deltaTime * FighterCount);

                //ゲージ0でデフォルト状態へ
                if (slider.value <= 0)
                {
                    this.transform.Find("CityRange").gameObject.layer = LayerMask.NameToLayer("Default");
                    currentStatus = CityStatus.Default;
                    rangeRenderer.color = new Color(1, 1, 1, 0.5f);
                }
            }
            else if (currentStatus == CityStatus.Default || currentStatus == CityStatus.PlayerBecoming)
            {
                //ゲージ増加
                currentStatus = CityStatus.PlayerBecoming;
                slider.value += (Time.deltaTime * FighterCount);
                sliderImage.color = Color.yellow;

                //ゲージMaxかつ範囲内に敵がいない場合、制圧
                if (slider.value >= ControlTime && colliderEnemy.Length == 0)
                {
                    this.transform.Find("CityRange").gameObject.layer = LayerMask.NameToLayer("PlayerBase");
                    currentStatus = CityStatus.Player;
                    rangeRenderer.color = Color.yellow - new Color(0, 0, 0, 0.75f); //薄い黄色
                }
            }
        }
        //敵兵士の方が範囲内に多い場合
        else if (FighterCount < 0)
        {
            if (currentStatus == CityStatus.PlayerBecoming || currentStatus == CityStatus.Player)
            {
                //ゲージ減少
                slider.value -= (Time.deltaTime * Mathf.Abs(FighterCount));

                if (slider.value <= 0)
                {
                    this.transform.Find("CityRange").gameObject.layer = LayerMask.NameToLayer("Default");
                    currentStatus = CityStatus.Default;
                    rangeRenderer.color = new Color(1, 1, 1, 0.5f);
                }
            }
            else if (currentStatus == CityStatus.Default || currentStatus == CityStatus.EnemyBecoming)
            {
                //ゲージ増加
                currentStatus = CityStatus.EnemyBecoming;
                slider.value += (Time.deltaTime * Mathf.Abs(FighterCount));
                sliderImage.color = Color.magenta;

                //ゲージMaxかつ範囲内に敵がいない場合、制圧
                if (slider.value >= ControlTime && colliderPlayer.Length == 0)
                {
                    this.transform.Find("CityRange").gameObject.layer = LayerMask.NameToLayer("EnemyBase");
                    currentStatus = CityStatus.Enemy;
                    rangeRenderer.color = Color.magenta - new Color(0, 0, 0, 0.75f);//薄い紫
                }
            }
        }
    }

    //回復処理
    private IEnumerator Heal()
    {
        while(true)
        {
            yield return new WaitForSeconds(HealTime);

            //味方制圧状態の場合、範囲内の味方兵士を回復
            if (currentStatus == CityStatus.Player)
            {
                foreach (Collider2D Fighter in colliderPlayer)
                {
                    FighterStatus fighterStatus = Fighter.gameObject.GetComponent<FighterStatus>();

                    if (fighterStatus.NowHp < (fighterStatus.MaxHp + fighterStatus.MaxHpBuff))
                    {
                        fighterStatus.NowHp += Mathf.Round((fighterStatus.MaxHp + fighterStatus.MaxHpBuff) / 10);
                    }
                }
            }

            //敵制圧状態の場合、範囲内の敵兵士を回復
            else if (currentStatus == CityStatus.Enemy)
            {
                foreach (Collider2D Fighter in colliderEnemy)
                {
                    FighterStatus fighterStatus = Fighter.gameObject.GetComponent<FighterStatus>();

                    if (fighterStatus.NowHp < (fighterStatus.MaxHp + fighterStatus.MaxHpBuff))
                    {
                        fighterStatus.NowHp += Mathf.Round((fighterStatus.MaxHp + fighterStatus.MaxHpBuff) / 10);
                    }
                }
            }
        }
    }
}
