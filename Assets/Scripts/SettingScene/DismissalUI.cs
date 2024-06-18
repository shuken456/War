using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//����UI
public class DismissalUI : MonoBehaviour
{
    public FighterEditUI FeUI;

    public Text InfoText;
    public Text MoneyText;

    private FighterStatus fs;
    private int GetMoney; //���ق����Ƃ��Ɏ�ɓ��邨��

    void OnEnable()
    {
        fs = FeUI.SelectFighterStatus;
        GetMoney = (fs.Level / 5) + 1;

        InfoText.text = fs.FighterName + "�����ق��܂��B\n��낵���ł����H";
        MoneyText.text = "�i�l������: " + GetMoney.ToString() + "���j";
    }

    public void YesButtonClick()
    {
        //���m�f�[�^���폜
        FeUI.PlayerFighterDataBaseAllList.Remove(FeUI.SeManager.PlayerFighterTable.PlayerFighterDBList.Find((n) => n.Name == fs.FighterName));
        FeUI.SeManager.PlayerFighterTable.PlayerFighterDBList.Remove(FeUI.SeManager.PlayerFighterTable.PlayerFighterDBList.Find((n) => n.Name == fs.FighterName));

        //�������v���X
        Common.Money += GetMoney;

        FeUI.ButtonClickChange(false);
        FeUI.SeManager.MoUI.TextWrite();
        FeUI.FighterStatusInfo.Clear(fs.FighterName);
        FeUI.FighterViewDisplay();
        this.gameObject.SetActive(false);
    }

    public void NoButtonClick()
    {
        this.gameObject.SetActive(false);
    }
}
