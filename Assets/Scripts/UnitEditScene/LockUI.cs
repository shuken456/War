using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using System.Linq;

public class LockUI : MonoBehaviour
{
    public UnitEditManager EditManager;

    //����UI
    public MoneyUI MoUI;

    //�\���e�L�X�g�ƃ{�^��
    public Text InfoText;
    public Button YesButton;

    //�K�v����
    private int NeedMoney;

    private void OnEnable()
    {
        NeedMoney = EditManager.PlayerUnitDataBaseAllList.Count * 5;
        InfoText.text = "�V���ȕ�����������܂����H\n�i�K�v����: " + NeedMoney.ToString() + "���j";

        //����������Ȃ���΃{�^���������Ȃ��悤��
        if(Common.Money < NeedMoney)
        {
            YesButton.interactable = false;
        }
        else
        {
            YesButton.interactable = true;
        }
    }
        
    //�����J��
    public void Yes()
    {
        PlayerUnit NewPlayerUnit = new PlayerUnit();
        NewPlayerUnit.Num = EditManager.PlayerUnitDataBaseAllList.Count + 1;
        NewPlayerUnit.Name = "��" + NewPlayerUnit.Num.ToString() + "����";
        NewPlayerUnit.UnitColor = Color.yellow;
        NewPlayerUnit.Strategy = 1;

        Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.Add(NewPlayerUnit);
        EditManager.DisplayScreenStart();

        //���b�N�{�^���̈ʒu�����̕����̈ʒu��
        if (NewPlayerUnit.Num < 10)
        {
            EditManager.LockButton.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, EditManager.UnitOblect[NewPlayerUnit.Num].transform.position);
        }
        else
        {
            EditManager.LockButton.SetActive(false);
        }

        GameObject.Find("Unit" + EditManager.SelectUnitNum.ToString()).transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.green;

        //��������
        Common.Money -= NeedMoney;
        MoUI.TextWrite();

        this.gameObject.SetActive(false);
    }

    public void No()
    {
        this.gameObject.SetActive(false);
    }
}
