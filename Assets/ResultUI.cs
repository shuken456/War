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

    // Start is called before the first frame update
    void Start()
    {
        //�o���l���X�g���쐬
        GameObject[] tagObjects;
        tagObjects = GameObject.FindGameObjectsWithTag("PlayerFighter");
        foreach (GameObject Fighter in tagObjects)
        {
            FighterStatus fs = Fighter.GetComponent<FighterStatus>();
            if(!BaManager.ExpDic.ContainsKey(fs.FighterName))
            {
                BaManager.ExpDic.Add(fs.FighterName, fs.Exp + 30);�@//�����c��o���l�{�[�i�X30
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
            
            //���x���A�b�v����
            int AddExp = Mathf.CeilToInt(BaManager.ExpDic[pf.Name] * ((float)Common.SelectStageNum / (float)pf.Level)); //�X�e�[�W�ƌ��݂̃��x���ɂ���Čo���l�ɕ␳��������
            int SumExp = AddExp + pf.EXP;
            pf.EXP = SumExp % 100;
            button.transform.Find("FighterResultInfo/StatusTexts/Text (AddExp)").GetComponent<Text>().text = "+" + AddExp.ToString();
            button.transform.Find("FighterResultInfo/StatusTexts/Text (NextExp)").GetComponent<Text>().text = (100 - pf.EXP).ToString();

            //�o���l��100���܂�����1���x���A�b�v
            if (SumExp >= 100)
            {
                //�オ�郌�x����
                int UpLevel = Mathf.FloorToInt(SumExp / 100);
                //�オ��p�����[�^���X�g
                Dictionary<string, int> UpParameter = LevelUpParameter(pf.Type,UpLevel);

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

        Common.Save();
    }

    //���x���A�b�v���̃p�����[�^����
    private Dictionary<string,int> LevelUpParameter(int Type,int UpLevel)
    {
        Dictionary<string, int> UpParameter = new Dictionary<string, int>();
        UpParameter.Add("Hp", 0);
        UpParameter.Add("Stamina", 0);
        UpParameter.Add("AtkPower", 0);
        UpParameter.Add("AtkSpeed", 0);
        UpParameter.Add("MoveSpeed", 0);

        for(int i = 0; i < UpLevel; i++)
        {
            UpParameter["Hp"] += Random.Range(0, 3);
            UpParameter["Stamina"] += Random.Range(0, 3);
            UpParameter["AtkPower"] += Random.Range(0, 3);
            UpParameter["AtkSpeed"] += Random.Range(0, 3);
            UpParameter["MoveSpeed"] += Random.Range(0, 3);
        }
        return UpParameter;
    }
}
