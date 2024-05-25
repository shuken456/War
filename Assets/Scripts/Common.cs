using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Common
{
    //���j�b�g�ꗗ�@�őI�𒆂��ꂽ���j�b�g�i���o�[
    public static int SelectUnitNum = 2;

    //���j�b�g�ꗗ���o�����[�h�ŊJ�����ۂ�
    public static bool SortieMode = false;

    //�o�g���V�[�����ۂ�
    public static bool BattleMode = false;

    //�I�𒆂��ꂽ�X�e�[�W�i���o�[
    public static int SelectStageNum = 1;

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

    public static void Save()
    {
        EditorUtility.SetDirty(Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB"));
        EditorUtility.SetDirty(Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB"));
        AssetDatabase.SaveAssets();
    }
}