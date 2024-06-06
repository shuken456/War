using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleHpGauge : MonoBehaviour
{
    //�Ώۂ̏�X�e�[�^�X
    public FighterStatus targetStatus;

    private Slider HpSlider;
    private Text HpText;

    // Start is called before the first frame update
    void Start()
    {
        HpSlider = this.gameObject.GetComponent<Slider>();
        HpText = this.transform.Find("Text (HP)").gameObject.GetComponent<Text>();
        this.transform.position = targetStatus.gameObject.transform.position + new Vector3(0, 1.55f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //�̗͂���ɕ\��
        HpSlider.value = (float)targetStatus.NowHp / (float)targetStatus.MaxHp;
        HpText.text = ((int)targetStatus.NowHp).ToString() + "/" + ((int)targetStatus.MaxHp).ToString();
    }
}
