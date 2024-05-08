using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Common
{

    //ユニット一覧→ユニット編成　で選択中されたユニットナンバー
    public static int SelectUnitNum = 1;

    public static string FighterType(int type)
    {
        switch (type)
        {
            case 1:
                return "歩兵";
            case 2:
                return "弓兵";
            case 3:
                return "盾兵";
            case 4:
                return "騎兵";
            default:
                return string.Empty;
        }
    }

    public static string FighterStrategy(int strategy)
    {
        switch (strategy)
        {
            case 1:
                return "攻撃重視";
            case 2:
                return "耐久重視";
            case 3:
                return "移動重視";
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