using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpGauge : MonoBehaviour
{
    private Slider HpSlider;

    //対象兵士
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
        //今のHP/MaxHPを表示　んでターゲットを追尾
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
