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

    //�Ώە��m
    public GameObject targetFighter;
    private FighterStatus targetStatus;
    private Transform targetTransform;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        //�e�q�I�u�W�F�N�g���擾
        HpSlider = this.transform.Find("HpGauge").gameObject.GetComponent<Slider>();
        StaminaSlider = this.transform.Find("StaminaGauge").gameObject.GetComponent<Slider>();
        LeaderFlag = this.transform.Find("LeaderFlag").gameObject;
        LeaderFlag.GetComponent<Image>().color = targetFighter.GetComponent<SpriteRenderer>().color;
        DeadLeaderFlag = this.transform.Find("DeadLeaderFlag").gameObject;
        DeadLeaderFlag.GetComponent<Image>().color = targetFighter.GetComponent<SpriteRenderer>().color;

        //�^�[�Q�b�g���
        targetStatus = targetFighter.GetComponent<FighterStatus>();
        targetTransform = targetFighter.GetComponent<Transform>();

        rectTransform = GetComponent<RectTransform>();

        //���������|���ꂽ�ꍇ�}�[�N�\��
        if(!DeadLeaderFlag.activeSelf && targetStatus.UnitLeader)
        {
            LeaderFlag.SetActive(true);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //����HP�A�X�^�~�i����\���@�^�[�Q�b�g��ǔ�
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
