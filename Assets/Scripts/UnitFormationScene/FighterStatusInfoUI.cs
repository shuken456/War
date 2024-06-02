using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FighterStatusInfoUI : MonoBehaviour
{
    //���j�b�gDB
    private List<PlayerUnit> PlayerUnitDataBaseAllList;

    private void OnEnable()
    {
        //DB�f�[�^�擾
        PlayerUnitDataBaseAllList = Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //���j�b�g�ԍ����ɕ��ёւ�
    }

    //���m�̃X�e�[�^�X��\��
    public void TextWrite(FighterStatus fs)
    {
        this.transform.Find("StatusTexts/Text (Name)").GetComponent<Text>().text = fs.FighterName;
        this.transform.Find("StatusTexts/Text (Exp)").GetComponent<Text>().text = (100 - fs.Exp).ToString();

        if(fs.UnitNum == 0)
        {
            this.transform.Find("StatusTexts/Text (Unit)").GetComponent<Text>().text = "�Ȃ�";
        }
        else
        {
            this.transform.Find("StatusTexts/Text (Unit)").GetComponent<Text>().text = PlayerUnitDataBaseAllList.FindAll(n => n.Num == fs.UnitNum)[0].Name;
        }
        
        this.transform.Find("StatusTexts/Text (Type)").GetComponent<Text>().text = Common.FighterType(fs.Type);
        this.transform.Find("StatusTexts/Text (Level)").GetComponent<Text>().text = fs.Level.ToString();
        this.transform.Find("StatusTexts/Text (Hp)").GetComponent<Text>().text = fs.MaxHp.ToString();
        this.transform.Find("StatusTexts/Text (Stamina)").GetComponent<Text>().text = fs.MaxStamina.ToString();
        this.transform.Find("StatusTexts/Text (AtkPower)").GetComponent<Text>().text = fs.AtkPower.ToString();
        this.transform.Find("StatusTexts/Text (AtkSpeed)").GetComponent<Text>().text = fs.AtkSpeed.ToString();
        this.transform.Find("StatusTexts/Text (MoveSpeed)").GetComponent<Text>().text = fs.MoveSpeed.ToString();

        //�o�t�\��
        if (fs.AtkPowerBuff > 0)
        {
            this.transform.Find("StatusTexts/Text (AtkPower)").GetComponent<Text>().text += " <color=green>(+" + fs.AtkPowerBuff.ToString() +")</color>";
        }
        if (fs.MaxHpBuff > 0)
        {
            this.transform.Find("StatusTexts/Text (Hp)").GetComponent<Text>().text += " <color=green>(+" + fs.MaxHpBuff.ToString() + ")</color>";
        }
        if (fs.MoveSpeedBuff > 0)
        {
            this.transform.Find("StatusTexts/Text (MoveSpeed)").GetComponent<Text>().text += " <color=green>(+" + fs.MoveSpeedBuff.ToString() + ")</color>";
        }
    }

    //���m�̉摜��\��
    public void ImageWrite(Sprite sprite, Color color)
    {
        this.transform.Find("FighterBack/FighterImage").GetComponent<Image>().sprite = sprite;
        this.transform.Find("FighterBack/FighterImage").GetComponent<Image>().color = color;
    }

    public void Clear(string FighterName)
    {
        if(FighterName == this.transform.Find("StatusTexts/Text (Name)").GetComponent<Text>().text)
        {
            this.transform.Find("StatusTexts/Text (Name)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("StatusTexts/Text (Exp)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("StatusTexts/Text (Unit)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("StatusTexts/Text (Type)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("StatusTexts/Text (Level)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("StatusTexts/Text (Hp)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("StatusTexts/Text (Stamina)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("StatusTexts/Text (AtkPower)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("StatusTexts/Text (AtkSpeed)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("StatusTexts/Text (MoveSpeed)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("FighterBack/FighterImage").GetComponent<Image>().sprite = null;
            this.transform.Find("FighterBack/FighterImage").GetComponent<Image>().color = Color.clear;
        }
    }

}
