using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterStatusInfo : MonoBehaviour
{
    public BattleManager BaManager;
    private FighterStatus BeforeFs;

    public void TextWrite(FighterStatus fs)
    {
        if(BeforeFs)
        {
            if(BaManager.SelectFighter.Contains(BeforeFs.gameObject))
            {
                BeforeFs.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else
            {
                BeforeFs.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.clear;
            }
        }

        BeforeFs = fs;
        fs.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.red;
        this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (Name)").GetComponent<Text>().text = fs.FighterName;
        this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (Type)").GetComponent<Text>().text = Common.FighterType(fs.Type);
        this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (Level)").GetComponent<Text>().text = fs.Level.ToString();
        this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (Hp)").GetComponent<Text>().text = Mathf.Round(fs.NowHp).ToString() + "/" + fs.MaxHp.ToString();
        this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (Stamina)").GetComponent<Text>().text = Mathf.Round(fs.NowStamina).ToString() + "/" + fs.MaxStamina.ToString();
        this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (AtkPower)").GetComponent<Text>().text = fs.AtkPower.ToString();
        this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (AtkSpeed)").GetComponent<Text>().text = fs.AtkSpeed.ToString();
        this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (MoveSpeed)").GetComponent<Text>().text = fs.MoveSpeed.ToString();

        //ƒoƒt•\Ž¦
        if (fs.AtkPowerBuff > 0)
        {
            this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (AtkPower)").GetComponent<Text>().text += " <color=green>(+" + fs.AtkPowerBuff.ToString() + ")</color>";
        }
        if (fs.MaxHpBuff > 0)
        {
            this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (Hp)").GetComponent<Text>().text += " <color=green>(+" + fs.MaxHpBuff.ToString() + ")</color>";
        }
        if (fs.MoveSpeedBuff > 0)
        {
            this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (MoveSpeed)").GetComponent<Text>().text += " <color=green>(+" + fs.MoveSpeedBuff.ToString() + ")</color>";
        }
    }

    public void ImageWrite(Sprite sprite, Color color)
    {
        this.transform.Find("FighterStatusInfoImage/FighterBack/FighterImage").GetComponent<Image>().sprite = sprite;
        this.transform.Find("FighterStatusInfoImage/FighterBack/FighterImage").GetComponent<Image>().color = color;
    }

    public void Clear(string FighterName)
    {
        if (FighterName == this.transform.Find("StatusTexts/Text (Name)").GetComponent<Text>().text)
        {
            this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (Name)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (Type)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (Level)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (Hp)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (Stamina)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (AtkPower)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (AtkSpeed)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("FighterStatusInfoImage/StatusTexts/Text (MoveSpeed)").GetComponent<Text>().text = string.Empty;
            this.transform.Find("FighterStatusInfoImage/FighterBack/FighterImage").GetComponent<Image>().sprite = null;
            this.transform.Find("FighterStatusInfoImage/FighterBack/FighterImage").GetComponent<Image>().color = Color.clear;
        }
    }

    public void Close()
    {
        if (BaManager.InstructionButton.activeSelf)
        {
            BaManager.TimeStopText.SetActive(false);
            Time.timeScale = 1;
        }

        if (BaManager.SelectFighter.Contains(BeforeFs.gameObject))
        {
            BeforeFs.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            BeforeFs.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.clear;
        }

        BeforeFs = null;
        this.gameObject.SetActive(false);
    }
}
