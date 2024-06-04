using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//兵士右クリック時のステータス表示UI
public class FighterStatusInfo : MonoBehaviour
{
    public BattleManager BaManager;
    private FighterStatus BeforeFs; //一回前に開いた兵士
    private float DefaultTime;

    private void OnEnable()
    {
        DefaultTime = Time.timeScale;
        Time.timeScale = 0;
    }

    //表示テキスト記載
    public void TextWrite(FighterStatus fs)
    {
        //前に開いた兵士の選択状態の見た目を解除
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

        //バフ表示
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

    //兵士画像表示
    public void ImageWrite(Sprite sprite, Color color)
    {
        this.transform.Find("FighterStatusInfoImage/FighterBack/FighterImage").GetComponent<Image>().sprite = sprite;
        this.transform.Find("FighterStatusInfoImage/FighterBack/FighterImage").GetComponent<Image>().color = color;
    }

    //閉じるボタン押下
    public void Close()
    {
        if (BaManager.InstructionButton.activeSelf)
        {
            BaManager.TimeStopText.SetActive(false);
            Time.timeScale = 1;
        }

        //普通に選択状態の兵士だった場合、見た目を戻す
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
        Time.timeScale = DefaultTime;
    }
}
