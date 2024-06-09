using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class City : MonoBehaviour
{
    public BattleManager BaManager;

    //�R���g���[���Q�[���\���p�L�����p�X
    public GameObject CanvasWorldSpace;

    //�R���g���[���Q�[�W�v���n�u
    public GameObject ControlSlider;

    //�X�͈͉摜
    public SpriteRenderer rangeRenderer;

    //�X�𐧈�����̂ɂ����鎞��
    public float ControlTime;

    //�X�͈�
    public float Range;

    //�R���g���[���Q�[�W
    private Slider slider;
    private Image sliderImage;

    //�񕜕\���v���n�u
    public GameObject HealPrefab;

    //�X�̏��
    public enum CityStatus
    {
        Default, //�f�t�H���g
        PlayerBecoming, //�����̂��̂ɂȂ������
        Player, //�����̂���
        EnemyBecoming, //�G�̂��̂ɂȂ������
        Enemy //�G�̂���
    }

    //���݂̊X���
    private CityStatus currentStatus = CityStatus.Default;

    //�͈͓��̕��m
    private Collider2D[] colliderPlayer;
    private Collider2D[] colliderEnemy;

    //�͈͓��̕��m�����@�����̏ꍇ�͓G���̕�������
    private int FighterCount;

    //�񕜎���
    public float HealTime;

    //�����������̉�
    private AudioSource ControlSE;

    //���������ɏo���\���������v���X1���邽��
    private bool FirstControl = true;

    // Start is called before the first frame update
    void Start()
    {
        //�e�ݒ�
        slider = Instantiate(ControlSlider, this.transform.position + new Vector3(0, 1f, 0), Quaternion.identity).GetComponent<Slider>();
        slider.transform.SetParent(CanvasWorldSpace.transform, true);
        slider.maxValue = ControlTime;
        rangeRenderer.transform.localScale = new Vector3(2, 2) * Range; //���a�Ȃ̂œ�{

        ControlSE = this.gameObject.GetComponent<AudioSource>();
        //�X���C�_�[�̐F�ύX�p�Ɏ擾
        sliderImage = slider.gameObject.transform.Find("Fill Area/Fill").gameObject.GetComponent<Image>();
    }

    private void OnEnable()
    {
        //�񕜂͎����I�ɍs�������̂ł����ŌĂ�
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

    //��������
    private void Control()
    {
        //�������m�̕����͈͓��ɑ����ꍇ
        if (FighterCount > 0)
        {
            //�Q�[�W���G�o�[�W�����̏ꍇ�A�Q�[�W������������
            if (currentStatus == CityStatus.EnemyBecoming || currentStatus == CityStatus.Enemy)
            {
                slider.value -= (Time.deltaTime * FighterCount);

                //�Q�[�W0�Ńf�t�H���g��Ԃ�
                if (slider.value <= 0)
                {
                    this.transform.Find("CityRange").gameObject.layer = LayerMask.NameToLayer("Default");
                    currentStatus = CityStatus.Default;
                    rangeRenderer.color = new Color(1, 1, 1, 0.5f);
                }
            }
            else if (currentStatus == CityStatus.Default || currentStatus == CityStatus.PlayerBecoming)
            {
                //�Q�[�W����
                currentStatus = CityStatus.PlayerBecoming;
                slider.value += (Time.deltaTime * FighterCount);
                sliderImage.color = Color.yellow;

                //�Q�[�WMax�Ő���
                if (slider.value >= ControlTime)
                {
                    this.transform.Find("CityRange").gameObject.layer = LayerMask.NameToLayer("PlayerBase");
                    currentStatus = CityStatus.Player;
                    rangeRenderer.color = Color.yellow - new Color(0, 0, 0, 0.75f); //�������F

                    ControlSE.Play();
                    
                    if(FirstControl)
                    {
                        //�o���\��������+1
                        BaManager.UnitCountUI.PossibleSortieCountNow += 1;
                        BaManager.UnitCountUI.TextDraw();
                        FirstControl = false;
                        StartCoroutine(BaManager.LogUI.DrawLog("<color=red><size=30>���_�𐧈������I</size>\n�o���\������+�P</color>"));

                        //���Ԃ��~�߂Ă�����
                        BaManager.ActionUI.SetActive(true);
                        BaManager.InstructionButton.SetActive(false);
                    }
                    else
                    {
                        StartCoroutine(BaManager.LogUI.DrawLog("<color=red><size=30>���_�𐧈������I</size></color>"));
                    }
                }
            }
        }
        //�G���m�̕����͈͓��ɑ����ꍇ
        else if (FighterCount < 0)
        {
            if (currentStatus == CityStatus.PlayerBecoming || currentStatus == CityStatus.Player)
            {
                //�Q�[�W����
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
                //�Q�[�W����
                currentStatus = CityStatus.EnemyBecoming;
                slider.value += (Time.deltaTime * Mathf.Abs(FighterCount));
                sliderImage.color = Color.magenta;

                //�Q�[�WMax�Ő���
                if (slider.value >= ControlTime)
                {
                    this.transform.Find("CityRange").gameObject.layer = LayerMask.NameToLayer("EnemyBase");
                    currentStatus = CityStatus.Enemy;
                    rangeRenderer.color = Color.magenta - new Color(0, 0, 0, 0.75f);//������

                    ControlSE.Play();
                    StartCoroutine(BaManager.LogUI.DrawLog("<size=30><color=red>���_�𐧈����ꂽ�I</color></size>"));
                }
            }
        }
    }

    //�񕜏���
    private IEnumerator Heal()
    {
        while(true)
        {
            yield return new WaitForSeconds(HealTime);

            //����������Ԃ̏ꍇ�A�͈͓��̖������m����
            if (currentStatus == CityStatus.Player)
            {
                foreach (Collider2D Fighter in colliderPlayer)
                {
                    FighterStatus fighterStatus = Fighter.gameObject.GetComponent<FighterStatus>();

                    if (fighterStatus.NowHp < (fighterStatus.MaxHp + fighterStatus.MaxHpBuff))
                    {
                        fighterStatus.NowHp += Mathf.Round((fighterStatus.MaxHp + fighterStatus.MaxHpBuff) / 10);
                        Instantiate(HealPrefab, Fighter.gameObject.transform.position + new Vector3(0f, -0.2f, 0), Quaternion.identity);
                    }
                }
            }

            //�G������Ԃ̏ꍇ�A�͈͓��̓G���m����
            else if (currentStatus == CityStatus.Enemy)
            {
                foreach (Collider2D Fighter in colliderEnemy)
                {
                    FighterStatus fighterStatus = Fighter.gameObject.GetComponent<FighterStatus>();

                    if (fighterStatus.NowHp < (fighterStatus.MaxHp + fighterStatus.MaxHpBuff))
                    {
                        fighterStatus.NowHp += Mathf.Round((fighterStatus.MaxHp + fighterStatus.MaxHpBuff) / 10);
                        Instantiate(HealPrefab, Fighter.gameObject.transform.position + new Vector3(0f, -0.2f, 0), Quaternion.identity);
                    }
                }
            }
        }
    }
}
