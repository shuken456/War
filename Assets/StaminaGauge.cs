using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaGauge : MonoBehaviour
{
    private Slider StaminaSlider;

    //ëŒè€ÉÜÉjÉbÉg
    public GameObject targetUnit;
    
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
        if (targetUnit != null)
        {
            Transform targetTransform = targetUnit.GetComponent<Transform>();
            Status targetStatus = targetUnit.GetComponent<Status>();
            StaminaSlider.value = (float)targetStatus.NowStamina / (float)targetStatus.MaxStamina;
            rectTransform.position = targetTransform.position + offset;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
