using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterStatusInfoUI : MonoBehaviour
{
    public void TextWrite(FighterStatus fs)
    {
        this.transform.Find("StatusTexts/Text (Name)").GetComponent<Text>().text = fs.FighterName;
        this.transform.Find("StatusTexts/Text (Type)").GetComponent<Text>().text = Common.FighterType(fs.Type);
        this.transform.Find("StatusTexts/Text (Level)").GetComponent<Text>().text = fs.Level.ToString();
        this.transform.Find("StatusTexts/Text (Hp)").GetComponent<Text>().text = fs.MaxHp.ToString();
        this.transform.Find("StatusTexts/Text (Stamina)").GetComponent<Text>().text = fs.MaxStamina.ToString();
        this.transform.Find("StatusTexts/Text (AtkPower)").GetComponent<Text>().text = fs.AtkPower.ToString();
        this.transform.Find("StatusTexts/Text (AtkSpeed)").GetComponent<Text>().text = fs.AtkSpeed.ToString();
        this.transform.Find("StatusTexts/Text (MoveSpeed)").GetComponent<Text>().text = fs.MoveSpeed.ToString();

        //ƒoƒt•\Ž¦
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
