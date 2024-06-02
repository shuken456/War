using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaGauge : MonoBehaviour
{
    private Slider StaminaSlider;

    //対象兵士
    public GameObject targetFighter;
    
    public Vector3 offset;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StaminaSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //スタミナ表示
        if (targetFighter != null)
        {
            Transform targetTransform = targetFighter.GetComponent<Transform>();
            FighterStatus targetStatus = targetFighter.GetComponent<FighterStatus>();
            StaminaSlider.value = (float)targetStatus.NowStamina / (float)targetStatus.MaxStamina;
            rectTransform.position = targetTransform.position + offset;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
