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
    }

    public void ImageWrite(Sprite sprite, Color color)
    {
        this.transform.Find("FighterBack/FighterImage").GetComponent<Image>().sprite = sprite;
        this.transform.Find("FighterBack/FighterImage").GetComponent<Image>().color = color;
    }
}
