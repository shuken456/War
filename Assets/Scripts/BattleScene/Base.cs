using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    public BattleManager BaManager;

    //コントロールゲーム表示用キャンパス
    public GameObject CanvasWorldSpace;

    //コントロールゲージプレハブ
    public GameObject ControlSlider;

    //敵兵士プレハブ
    public GameObject EnemyFighter;
    private Dictionary<string, int> UpParameter;
    private int EnemyHealCount = 0;

    //街範囲画像
    public SpriteRenderer rangeRenderer;

    //街を制圧するのにかかる時間
    public float ControlTime;

    //街範囲
    public float Range;

    //コントロールゲージ
    private Slider slider;
    private Image sliderImage;

    //回復表示プレハブ
    public GameObject HealPrefab;

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

    //制圧した時の音
    private AudioSource ControlSE;

    //初制圧時に出撃可能部隊数をプラス1するため
    private bool FirstControl = true;

    // Start is called before the first frame update
    void Start()
    {
        //出現する敵兵士のステータス確定
        UpParameter = Common.LevelUpParameter(1, Common.Progress - 4);
        
        //各設定
        slider = Instantiate(ControlSlider, this.transform.position + new Vector3(0, 1f, 0), Quaternion.identity).GetComponent<Slider>();
        slider.transform.SetParent(CanvasWorldSpace.transform, true);
        slider.maxValue = ControlTime;
        rangeRenderer.transform.localScale = new Vector3(2, 2) * Range; //半径なので二倍

        ControlSE = this.gameObject.GetComponent<AudioSource>();
        //スライダーの色変更用に取得
        sliderImage = slider.gameObject.transform.Find("Fill Area/Fill").gameObject.GetComponent<Image>();
    }

    private void OnEnable()
    {
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
                    rangeRenderer.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                }
            }
            else if (currentStatus == CityStatus.Default || currentStatus == CityStatus.PlayerBecoming)
            {
                //ゲージ増加
                currentStatus = CityStatus.PlayerBecoming;
                slider.value += (Time.deltaTime * FighterCount);
                sliderImage.color = Color.yellow;

                //ゲージMaxで制圧
                if (slider.value >= ControlTime)
                {
                    this.transform.Find("CityRange").gameObject.layer = LayerMask.NameToLayer("PlayerHealRange");
                    currentStatus = CityStatus.Player;
                    rangeRenderer.color = Color.yellow - new Color(0, 0, 0, 0.75f); //薄い黄色

                    ControlSE.Play();
                    
                    if(FirstControl)
                    {
                        //出撃可能部隊数を+1
                        BaManager.UnitCountUI.PossibleSortieCountNow += 1;
                        BaManager.UnitCountUI.TextDraw();
                        FirstControl = false;
                        StartCoroutine(BaManager.LogUI.DrawLog("<color=red><size=30>拠点を制圧した！</size>\n出撃可能部隊数+１</color>"));

                        //時間を止めてあげる
                        BaManager.ActionUI.SetActive(true);
                        BaManager.InstructionButton.SetActive(false);
                    }
                    else
                    {
                        StartCoroutine(BaManager.LogUI.DrawLog("<color=red><size=30>拠点を制圧した！</size></color>"));
                    }
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
                    rangeRenderer.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                }
            }
            else if (currentStatus == CityStatus.Default || currentStatus == CityStatus.EnemyBecoming)
            {
                //ゲージ増加
                currentStatus = CityStatus.EnemyBecoming;
                slider.value += (Time.deltaTime * Mathf.Abs(FighterCount));
                sliderImage.color = Color.magenta;

                //ゲージMaxで制圧
                if (slider.value >= ControlTime)
                {
                    this.transform.Find("CityRange").gameObject.layer = LayerMask.NameToLayer("EnemyHealRange");
                    currentStatus = CityStatus.Enemy;
                    rangeRenderer.color = Color.magenta - new Color(0, 0, 0, 0.75f);//薄い紫

                    ControlSE.Play();
                    StartCoroutine(BaManager.LogUI.DrawLog("<size=30><color=red>拠点を制圧された！</color></size>"));
                    EnemyHealCount = 0;
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
                EnemyHealCount = 0;

                foreach (Collider2D Fighter in colliderPlayer)
                {
                    FighterStatus fighterStatus = Fighter.gameObject.GetComponent<FighterStatus>();

                    if (fighterStatus.NowHp < (fighterStatus.MaxHp + fighterStatus.MaxHpBuff))
                    {
                        fighterStatus.NowHp += Mathf.Round((fighterStatus.MaxHp + fighterStatus.MaxHpBuff) / 10);
                        var heal = Instantiate(HealPrefab, Fighter.gameObject.transform.position + new Vector3(0f, -0.2f, 0), Quaternion.identity);
                        heal.transform.parent = Fighter.gameObject.transform;
                    }
                }
            }

            //敵制圧状態の場合、範囲内の敵兵士を回復
            else if (currentStatus == CityStatus.Enemy)
            {
                EnemyHealCount++;

                //3回回復分時間が経過したら敵兵士を出現させる
                if (EnemyHealCount == 3)
                {
                    EnemyHealCount = 0;
                    GameObject Enemy = Instantiate(EnemyFighter, this.transform.position, Quaternion.identity);

                    //出現する敵兵士のステータス設定　※プレハブにステータスを代入するとおかしくなるので、作った兵士オブジェクトに代入する
                    FighterStatus fs = Enemy.GetComponent<FighterStatus>();
                    fs.Level = Common.Progress - 1;
                    fs.MaxHp += UpParameter["Hp"];
                    fs.NowHp += UpParameter["Hp"];
                    fs.MaxStamina += UpParameter["Stamina"];
                    fs.NowStamina += UpParameter["Stamina"];
                    fs.AtkPower += UpParameter["AtkPower"];
                    fs.AtkSpeed += UpParameter["AtkSpeed"];
                    fs.MoveSpeed += UpParameter["MoveSpeed"];
                }

                foreach (Collider2D Fighter in colliderEnemy)
                {
                    FighterStatus fighterStatus = Fighter.gameObject.GetComponent<FighterStatus>();

                    if (fighterStatus.NowHp < (fighterStatus.MaxHp + fighterStatus.MaxHpBuff))
                    {
                        fighterStatus.NowHp += Mathf.Round((fighterStatus.MaxHp + fighterStatus.MaxHpBuff) / 10);
                        var heal = Instantiate(HealPrefab, Fighter.gameObject.transform.position + new Vector3(0f, -0.2f, 0), Quaternion.identity);
                        heal.transform.parent = Fighter.gameObject.transform;
                    }
                }
            }
        }
    }
}
