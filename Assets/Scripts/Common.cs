using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Common
{
    //������
    public static int Money = 10;

    //�i�s�x(�X�e�[�W�N���A�x ���ɒ��ރX�e�[�W��)
    public static int Progress = 1;

    //���j�b�g�ꗗ�@�őI�𒆂��ꂽ���j�b�g�i���o�[
    public static int SelectUnitNum = 0;

    //���j�b�g�ꗗ���o�����[�h�ŊJ�����ۂ�
    public static bool SortieMode = false;

    //�o�g���V�[�����ۂ�
    public static bool BattleMode = false;

    //����
    public static string FighterType(int type)
    {
        switch (type)
        {
            case 1:
                return "����";
            case 2:
                return "�|��";
            case 3:
                return "����";
            case 4:
                return "�R��";
            default:
                return string.Empty;
        }
    }

    //�������j
    public static string FighterStrategy(int strategy)
    {
        switch (strategy)
        {
            case 1:
                return "�U���d��";
            case 2:
                return "�ϋv�d��";
            case 3:
                return "�ړ��d��";
            default:
                return string.Empty;
        }
    }

    //���m�̃X�e�[�^�X�󂯓n��
    public static void FighterStatusCopy(FighterStatus fsTo, FighterStatus fsFrom)
    {
        fsTo.Type = fsFrom.Type;
        fsTo.FighterName = fsFrom.FighterName;
        fsTo.Level = fsFrom.Level;
        fsTo.MaxHp = fsFrom.MaxHp;
        fsTo.NowHp = fsFrom.NowHp;
        fsTo.MaxStamina = fsFrom.MaxStamina;
        fsTo.NowStamina = fsFrom.NowStamina;
        fsTo.AtkPower = fsFrom.AtkPower;
        fsTo.AtkSpeed = fsFrom.AtkSpeed;
        fsTo.MoveSpeed = fsFrom.MoveSpeed;
        fsTo.UnitNum = fsFrom.UnitNum;
        fsTo.UnitLeader = fsFrom.UnitLeader;
        fsTo.Exp = fsFrom.Exp;
    }

    public static void GetFighterStatusFromDB(FighterStatus fs, PlayerFighter pf)
    {
        fs.Type = pf.Type;
        fs.FighterName = pf.Name;
        fs.Level = pf.Level;
        fs.MaxHp = pf.Hp;
        fs.NowHp = pf.Hp;
        fs.MaxStamina = pf.Stamina;
        fs.NowStamina = pf.Stamina;
        fs.AtkPower = pf.AtkPower;
        fs.AtkSpeed = pf.AtkSpeed;
        fs.MoveSpeed = pf.MoveSpeed;
        fs.UnitNum = pf.UnitNum;
        fs.UnitLeader = pf.UnitLeader;
        fs.Exp = pf.EXP;
    }

    //�o�t�ݒ�
    public static void FighterBuff(FighterStatus fs, int UnitStrategy, bool LeaderIgnore)
    {
        if(LeaderIgnore)
        {
            if (UnitStrategy == 1)
            {
                fs.AtkPowerBuff = (int)Mathf.Round(fs.AtkPower * 0.3f);
            }
            if (UnitStrategy == 2)
            {
                fs.MaxHpBuff = Mathf.Round(fs.MaxHp * 0.3f);
                fs.NowHp += fs.MaxHpBuff;
            }
            if (UnitStrategy == 3)
            {
                fs.MoveSpeedBuff = (int)Mathf.Round(fs.MoveSpeed * 0.3f);
            }
        }
        else
        {
            if (fs.UnitLeader || UnitStrategy == 1)
            {
                fs.AtkPowerBuff = (int)Mathf.Round(fs.AtkPower * 0.3f);
            }
            if (fs.UnitLeader || UnitStrategy == 2)
            {
                fs.MaxHpBuff = Mathf.Round(fs.MaxHp * 0.3f);
                fs.NowHp += fs.MaxHpBuff;
            }
            if (fs.UnitLeader || UnitStrategy == 3)
            {
                fs.MoveSpeedBuff = (int)Mathf.Round(fs.MoveSpeed * 0.3f);
            }
        }
    }

    //���x���A�b�v���̃p�����[�^����
    public static Dictionary<string, int> LevelUpParameter(int type, int UpLevel)
    {
        Dictionary<string, int> UpParameter = new Dictionary<string, int>();
        UpParameter.Add("Hp", 0);
        UpParameter.Add("Stamina", 0);
        UpParameter.Add("AtkPower", 0);
        UpParameter.Add("AtkSpeed", 0);
        UpParameter.Add("MoveSpeed", 0);

        switch (type)
        {
            case 1:
                for (int i = 0; i < UpLevel; i++)
                {
                    UpParameter["Hp"] += Random.Range(3, 6);
                    UpParameter["Stamina"] += Random.Range(2, 5);
                    UpParameter["AtkPower"] += Random.Range(0, 3);
                    UpParameter["AtkSpeed"] += Random.Range(0, 3);
                    UpParameter["MoveSpeed"] += Random.Range(0, 2);
                }
                break;
            case 2:
                for (int i = 0; i < UpLevel; i++)
                {
                    UpParameter["Hp"] += Random.Range(1, 4);
                    UpParameter["Stamina"] += Random.Range(0, 3);
                    UpParameter["AtkPower"] += Random.Range(0, 3);
                    UpParameter["AtkSpeed"] += Random.Range(0, 2);
                    UpParameter["MoveSpeed"] += Random.Range(0, 2);
                }
                break;
            case 3:
                for (int i = 0; i < UpLevel; i++)
                {
                    UpParameter["Hp"] += Random.Range(6, 9);
                    UpParameter["Stamina"] += Random.Range(0, 3);
                    UpParameter["AtkPower"] += Random.Range(0, 2);
                    UpParameter["AtkSpeed"] += Random.Range(0, 2);
                    UpParameter["MoveSpeed"] += Random.Range(0, 2);
                }
                break;
            case 4:
                for(int i = 0; i < UpLevel; i++)
                {
                    UpParameter["Hp"] += Random.Range(3, 6);
                    UpParameter["Stamina"] += Random.Range(0, 3);
                    UpParameter["AtkPower"] += Random.Range(0, 3);
                    UpParameter["AtkSpeed"] += Random.Range(0, 3);
                    UpParameter["MoveSpeed"] += Random.Range(1, 3);
                }
                break;
        }
        
        return UpParameter;
    }

    //�������Ɛi�s�x�Z�[�u���[�h
    public static void ProgressMoneySave()
    {
        PlayerPrefs.SetInt("ContinueProgress", Progress);
        PlayerPrefs.SetInt("ContinueMoney", Money);
    }

    public static void ProgressMoneyLoad()
    {
        Progress = PlayerPrefs.GetInt("ContinueProgress");
        Money = PlayerPrefs.GetInt("ContinueMoney");
        Progress = 1;
    }

    public static void InitialSave()
    {
        PlayerPrefs.SetInt("ContinueProgress", 1);
        PlayerPrefs.SetInt("ContinueMoney", 10);
        Progress = 1;
        Money = 10;
    }
}