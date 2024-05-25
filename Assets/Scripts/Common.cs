using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Common
{
    //ユニット一覧　で選択中されたユニットナンバー
    public static int SelectUnitNum = 2;

    //ユニット一覧を出撃モードで開くか否か
    public static bool SortieMode = false;

    //バトルシーンか否か
    public static bool BattleMode = false;

    //選択中されたステージナンバー
    public static int SelectStageNum = 1;

    //兵種
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

    //部隊方針
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

    //兵士のステータス受け渡し
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

    //バフ設定
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