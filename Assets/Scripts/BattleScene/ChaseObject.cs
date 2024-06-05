using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaseObject : MonoBehaviour
{
    private Slider HpSlider;
    private Slider StaminaSlider;
    private GameObject LeaderFlag;
    private GameObject DeadLeaderFlag;

    //対象兵士
    public GameObject targetFighter;
    private FighterStatus targetStatus;
    private Transform targetTransform;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        //各子オブジェクトを取得
        HpSlider = this.transform.Find("HpGauge").gameObject.GetComponent<Slider>();
        StaminaSlider = this.transform.Find("StaminaGauge").gameObject.GetComponent<Slider>();
        LeaderFlag = this.transform.Find("LeaderFlag").gameObject;
        LeaderFlag.GetComponent<Image>().color = targetFighter.GetComponent<SpriteRenderer>().color;
        DeadLeaderFlag = this.transform.Find("DeadLeaderFlag").gameObject;
        DeadLeaderFlag.GetComponent<Image>().color = targetFighter.GetComponent<SpriteRenderer>().color;

        //ターゲット情報
        targetStatus = targetFighter.GetComponent<FighterStatus>();
        targetTransform = targetFighter.GetComponent<Transform>();

        rectTransform = GetComponent<RectTransform>();

        //部隊長が倒された場合マーク表示
        if(!DeadLeaderFlag.activeSelf && targetStatus.UnitLeader)
        {
            LeaderFlag.SetActive(true);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //今のHP、スタミナ等を表示　ターゲットを追尾
        if (targetFighter != null)
        {
            HpSlider.value = (float)targetStatus.NowHp / (float)(targetStatus.MaxHp + targetStatus.MaxHpBuff);
            StaminaSlider.value = (float)targetStatus.NowStamina / (float)targetStatus.MaxStamina;
            rectTransform.position = targetTransform.position;

            if(targetStatus.DeadUnitLeader)
            {
                DeadLeaderFlag.SetActive(true);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
