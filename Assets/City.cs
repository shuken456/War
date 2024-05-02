using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class City : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        //�e�ݒ�
        slider = Instantiate(ControlSlider, this.transform.position + new Vector3(0, 1f, 0), Quaternion.identity).GetComponent<Slider>();
        slider.transform.SetParent(CanvasWorldSpace.transform, true);
        slider.maxValue = ControlTime;
        rangeRenderer.transform.localScale = new Vector3(2, 2) * Range; //���a�Ȃ̂œ�{

        //�X���C�_�[�̐F�ύX�p�Ɏ擾
        sliderImage = slider.gameObject.transform.Find("Fill Area/Fill").gameObject.GetComponent<Image>();

        //�񕜂͎����I�ɍs�������̂ł����ŌĂ�
        StartCoroutine(Heal());
    }

    // Update is called once per frame
    void Update()
    {
        colliderPlayer = Physics2D.OverlapCircleAll(transform.position, Range/1.5f, LayerMask.GetMask("PlayerFighter"));//�Ȃ���1.5�Ŋ���Ƃ��������ɂȂ�
        colliderEnemy = Physics2D.OverlapCircleAll(transform.position, Range/1.5f, LayerMask.GetMask("EnemyFighter"));

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
                    currentStatus = CityStatus.Default;
                    rangeRenderer.color = new Color(0, 0, 0, 0.5f);
                }
            }
            else if (currentStatus == CityStatus.Default || currentStatus == CityStatus.PlayerBecoming)
            {
                //�Q�[�W����
                currentStatus = CityStatus.PlayerBecoming;
                slider.value += (Time.deltaTime * FighterCount);
                sliderImage.color = Color.red;

                //�Q�[�WMax���͈͓��ɓG�����Ȃ��ꍇ�A����
                if (slider.value >= ControlTime && colliderEnemy.Length == 0)
                {
                    currentStatus = CityStatus.Player;
                    rangeRenderer.color = Color.red - new Color(0, 0, 0, 0.5f); //������
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
                    currentStatus = CityStatus.Default;
                    rangeRenderer.color = new Color(0, 0, 0, 0.5f);
                }
            }
            else if (currentStatus == CityStatus.Default || currentStatus == CityStatus.EnemyBecoming)
            {
                //�Q�[�W����
                currentStatus = CityStatus.EnemyBecoming;
                slider.value += (Time.deltaTime * Mathf.Abs(FighterCount));
                sliderImage.color = Color.magenta;

                //�Q�[�WMax���͈͓��ɓG�����Ȃ��ꍇ�A����
                if (slider.value >= ControlTime && colliderPlayer.Length == 0)
                {
                    currentStatus = CityStatus.Enemy;
                    rangeRenderer.color = Color.magenta - new Color(0, 0, 0, 0.5f);//������
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

                    if (fighterStatus.NowHp < fighterStatus.MaxHp)
                    {
                        fighterStatus.NowHp += Mathf.Round(fighterStatus.MaxHp / 10);
                    }
                }
            }

            //�G������Ԃ̏ꍇ�A�͈͓��̓G���m����
            else if (currentStatus == CityStatus.Enemy)
            {
                foreach (Collider2D Fighter in colliderEnemy)
                {
                    FighterStatus fighterStatus = Fighter.gameObject.GetComponent<FighterStatus>();

                    if (fighterStatus.NowHp < fighterStatus.MaxHp)
                    {
                        fighterStatus.NowHp += Mathf.Round(fighterStatus.MaxHp / 10);
                    }
                }
            }
        }
    }
}
