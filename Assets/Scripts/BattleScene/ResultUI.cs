using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    public BattleManager BaManager;

    //���m���U���g �X�N���[���r���[
    public GameObject FighterResultView;
    //���m���U���g�v���n�u
    public GameObject FighterResultUI;

    //�o���������m���X�g
    public List<PlayerFighter> SortieFighterList;

    //���m�v���n�u
    public GameObject EmptyInfantry;
    public GameObject EmptyArcher;

    //�������������\������e�L�X�g
    public Text ResultText;
    //�l��������\������e�L�X�g
    public Text MoneyText;

    //�e�{�^��
    public GameObject OkButton;
    public GameObject RevengeButton;
    public GameObject SettingButton;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        int GetMoney;

        if (BaManager.WinFlg)
        {
            ResultText.text = "�����I";
            ResultText.color = Color.red;
            GetMoney = (Common.Progress * 2) + 3;
            OkButton.SetActive(true);
        }
        else
        {
            ResultText.text = "�s�k�c";
            ResultText.color = Color.blue;
            GetMoney = Common.Progress;
            RevengeButton.SetActive(true);
            SettingButton.SetActive(true);
        }

        //�������v���X
        MoneyText.text = GetMoney.ToString() + "��"; 
        Common.Money += GetMoney;

        //�o���l���X�g���쐬
        GameObject[] tagObjects;
        tagObjects = GameObject.FindGameObjectsWithTag("PlayerFighter");
        foreach (GameObject Fighter in tagObjects)
        {
            FighterStatus fs = Fighter.GetComponent<FighterStatus>();
            if(!BaManager.ExpDic.ContainsKey(fs.FighterName))
            {
                BaManager.ExpDic.Add(fs.FighterName, fs.Exp);
                if (BaManager.WinFlg)
                {
                    BaManager.ExpDic[fs.FighterName] += 30; //�����c��o���l�{�[�i�X30
                }
            }
        }

        if (BaManager.WinFlg)
        {
            List<string> nameList = BaManager.ExpDic.Keys.ToList();
            foreach (string name in nameList)
            {
                BaManager.ExpDic[name] += 30; //�����{�[�i�X30
            }

        }

        //�o���������m���擾
        List<PlayerUnit> SortieUnit = BaManager.PlayerUnitDataBaseAllList.FindAll((n) => n.SoriteFlg);
        foreach (PlayerUnit pu in SortieUnit)
        {
            List<PlayerFighter> SortieFighter = BaManager.PlayerFighterDataBaseAllList.FindAll((n) => n.UnitNum == pu.Num);
            foreach (PlayerFighter pf in SortieFighter)
            {
                SortieFighterList.Add(pf);
            }
            pu.SoriteFlg = false;
        }

        //�o���������m�����U���g���쐬
        foreach (PlayerFighter pf in SortieFighterList)
        {
            GameObject button = Instantiate(FighterResultUI, FighterResultView.transform);
            switch (pf.Type)
            {
                case 1:
                    button.transform.Find("FighterResultInfo/FighterBack/FighterImage").GetComponent<Image>().sprite = EmptyInfantry.GetComponent<SpriteRenderer>().sprite;
                    break;
                case 2:
                    button.transform.Find("FighterResultInfo/FighterBack/FighterImage").GetComponent<Image>().sprite = EmptyArcher.GetComponent<SpriteRenderer>().sprite;
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    break;
            }
            button.transform.Find("FighterResultInfo/FighterBack/FighterImage").GetComponent<Image>().color = BaManager.PlayerUnitDataBaseAllList.Find((n) => n.Num == pf.UnitNum).UnitColor;
            button.transform.Find("FighterResultInfo/StatusTexts/Text (Name)").GetComponent<Text>().text = pf.Name;
            
            //���x���A�b�v���� ���X�̌o���l���������Ƃŉ��Z�����o���l�����߂�
            int AddExp = Mathf.CeilToInt((BaManager.ExpDic[pf.Name] - pf.EXP) * ((float)Common.Progress / (float)pf.Level)); //�X�e�[�W�ƌ��݂̃��x���ɂ���Čo���l�ɕ␳��������
            int SumExp = AddExp + pf.EXP;

            //�オ�郌�x���� ���o���l��100���܂�����1���x���A�b�v
            int UpLevel = Mathf.FloorToInt(SumExp / 100);

            pf.EXP = SumExp - (100 * UpLevel);
            button.transform.Find("FighterResultInfo/StatusTexts/Text (AddExp)").GetComponent<Text>().text = "+" + AddExp.ToString();
            button.transform.Find("FighterResultInfo/StatusTexts/Text (NextExp)").GetComponent<Text>().text = (100 - pf.EXP).ToString();

            //�p�����[�^�A�b�v
            if (UpLevel > 0)
            {
                //�オ��p�����[�^���X�g
                Dictionary<string, int> UpParameter = Common.LevelUpParameter(pf.Type,UpLevel);
                button.transform.Find("FighterResultInfo/StatusTexts/Text (LevelUp)").gameObject.SetActive(true);
                button.transform.Find("FighterResultInfo/StatusTexts/Text (Level)").GetComponent<Text>().text = pf.Level.ToString() + "��" + (pf.Level + UpLevel).ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (Hp)").GetComponent<Text>().text = pf.Hp + "��" + (pf.Hp + UpParameter["Hp"]).ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (Stamina)").GetComponent<Text>().text = pf.Stamina + "��" + (pf.Stamina + UpParameter["Stamina"]).ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (AtkPower)").GetComponent<Text>().text = pf.AtkPower + "��" + (pf.AtkPower + UpParameter["AtkPower"]).ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (AtkSpeed)").GetComponent<Text>().text = pf.AtkSpeed + "��" + (pf.AtkSpeed + UpParameter["AtkSpeed"]).ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (MoveSpeed)").GetComponent<Text>().text = pf.MoveSpeed + "��" + (pf.MoveSpeed + UpParameter["MoveSpeed"]).ToString();
                pf.Level += UpLevel;
                pf.Hp += UpParameter["Hp"];
                pf.Stamina += UpParameter["Stamina"];
                pf.AtkPower += UpParameter["AtkPower"];
                pf.AtkSpeed += UpParameter["AtkSpeed"];
                pf.MoveSpeed += UpParameter["MoveSpeed"];
            }
            else
            {
                button.transform.Find("FighterResultInfo/StatusTexts/Text (LevelUp)").gameObject.SetActive(false);
                button.transform.Find("FighterResultInfo/StatusTexts/Text (Level)").GetComponent<Text>().text = pf.Level.ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (Hp)").GetComponent<Text>().text = pf.Hp.ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (Stamina)").GetComponent<Text>().text = pf.Stamina.ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (AtkPower)").GetComponent<Text>().text = pf.AtkPower.ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (AtkSpeed)").GetComponent<Text>().text = pf.AtkSpeed.ToString();
                button.transform.Find("FighterResultInfo/StatusTexts/Text (MoveSpeed)").GetComponent<Text>().text = pf.MoveSpeed.ToString();
            }
        }

        if (BaManager.WinFlg)
        {
            //�i�s�x�X�V
            Common.Progress += 1;
        }
    }

    //OK�{�^���N���b�N�ŏ�����ʂ�
    public void OkButtonClick()
    {
        Common.BattleMode = false;
        Common.SortieMode = false;

        //DontDestoy�ɓ����Ă�BGM���폜
        Common.MusicReset();

        SceneManager.LoadScene("SettingScene");
    }

    //�Ē���{�^���N���b�N�Ő퓬�V�[���ēǂݍ���
    public void RevengeButtonClick()
    {
        Common.BattleMode = false;
        Common.SortieMode = false;

        //DontDestoy�ɓ����Ă�BGM���폜
        Common.MusicReset();

        SceneManager.LoadScene("BattleScene" + Common.Progress.ToString());
    }
}
