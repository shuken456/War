using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpGauge : MonoBehaviour
{
    private Slider HpSlider;

    //�Ώە��m
    public GameObject targetFighter;

    public Vector3 offset;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        HpSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //����HP/MaxHP��\���@��Ń^�[�Q�b�g��ǔ�
        if (targetFighter != null)
        {
            Transform targetTransform = targetFighter.GetComponent<Transform>();
            FighterStatus targetStatus = targetFighter.GetComponent<FighterStatus>();
            HpSlider.value = (float)targetStatus.NowHp / (float)(targetStatus.MaxHp + targetStatus.MaxHpBuff);
            rectTransform.position = targetTransform.position + offset;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
