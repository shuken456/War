using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Common
{

    //���j�b�g�ꗗ�����j�b�g�Ґ��@�őI�𒆂��ꂽ���j�b�g�i���o�[
    public static int SelectUnitNum = 1;

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
    }
}