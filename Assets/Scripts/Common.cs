using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Common
{
    //所持金
    public static int Money;

    //進行度(ステージクリア度 次に挑むステージ数または現在挑んでるステージ数)
    public static int Progress;

    //ユニット一覧　で選択中されたユニットナンバー
    public static int SelectUnitNum = 0;

    //ユニット一覧を出撃モードで開くか否か
    public static bool SortieMode = false;

    //バトルシーンか否か
    public static bool BattleMode = false;

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
            case 5:
                return "将軍";
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
            else
            {
                fs.AtkPowerBuff = 0;
            }
            if (UnitStrategy == 2)
            {
                fs.MaxHpBuff = Mathf.Round(fs.MaxHp * 0.3f);
                fs.NowHp += fs.MaxHpBuff;
            }
            else
            {
                fs.MaxHpBuff = 0;
            }
            if (UnitStrategy == 3)
            {
                fs.MoveSpeedBuff = (int)Mathf.Round(fs.MoveSpeed * 0.2f);
            }
            else
            {
                fs.MoveSpeedBuff = 0;
            }
        }
        else
        {
            if (fs.UnitLeader || UnitStrategy == 1)
            {
                fs.AtkPowerBuff = (int)Mathf.Round(fs.AtkPower * 0.3f);
            }
            else
            {
                fs.AtkPowerBuff = 0;
            }
            if (fs.UnitLeader || UnitStrategy == 2)
            {
                fs.MaxHpBuff = Mathf.Round(fs.MaxHp * 0.3f);
                fs.NowHp += fs.MaxHpBuff;
            }
            else
            {
                fs.MaxHpBuff = 0;
            }
            if (fs.UnitLeader || UnitStrategy == 3)
            {
                fs.MoveSpeedBuff = (int)Mathf.Round(fs.MoveSpeed * 0.2f);
            }
            else
            {
                fs.MoveSpeedBuff = 0;
            }
        }
    }

    //レベルアップ時のパラメータ処理
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
                    UpParameter["Stamina"] += Random.Range(2, 4);
                    UpParameter["AtkPower"] += Random.Range(0, 3);
                    UpParameter["AtkSpeed"] += Random.Range(0, 3);
                    UpParameter["MoveSpeed"] += Random.Range(0, 3);
                }
                break;
            case 2:
                for (int i = 0; i < UpLevel; i++)
                {
                    UpParameter["Hp"] += Random.Range(1, 4);
                    UpParameter["Stamina"] += Random.Range(1, 3);
                    UpParameter["AtkPower"] += Random.Range(0, 3);
                    UpParameter["AtkSpeed"] += Random.Range(0, 2);
                    UpParameter["MoveSpeed"] += Random.Range(0, 3);
                }
                break;
            case 3:
                for (int i = 0; i < UpLevel; i++)
                {
                    UpParameter["Hp"] += Random.Range(5, 8);
                    UpParameter["Stamina"] += Random.Range(1, 3);
                    UpParameter["AtkPower"] += Random.Range(0, 2);
                    UpParameter["AtkSpeed"] += Random.Range(0, 2);
                    UpParameter["MoveSpeed"] += Random.Range(0, 2);
                }
                break;
            case 4:
                for(int i = 0; i < UpLevel; i++)
                {
                    UpParameter["Hp"] += Random.Range(3, 6);
                    UpParameter["Stamina"] += Random.Range(1, 3);
                    UpParameter["AtkPower"] += Random.Range(0, 3);
                    UpParameter["AtkSpeed"] += Random.Range(0, 3);
                    UpParameter["MoveSpeed"] += Random.Range(2, 4);
                }
                break;
        }
        
        return UpParameter;
    }

    //所持金と進行度セーブロード
    public static void ProgressMoneySave()
    {
        PlayerPrefs.SetInt("ContinueProgress", Progress);
        PlayerPrefs.SetInt("ContinueMoney", Money);
    }

    public static void ProgressMoneyLoad()
    {
        Progress = PlayerPrefs.GetInt("ContinueProgress");
        Money = PlayerPrefs.GetInt("ContinueMoney");
    }

    public static void InitialSave()
    {
        //お金と進行度初期化
        PlayerPrefs.SetInt("ContinueProgress", 1);
        PlayerPrefs.SetInt("ContinueMoney", 0);
        Progress = 1;
        Money = 0;

        //各ヘルプ表示フラグもセーブ
        PlayerPrefs.SetInt("SettingHelp", 0);
        PlayerPrefs.SetInt("FormationHelp", 0);
        PlayerPrefs.SetInt("ShielderHelp", 0);
        PlayerPrefs.SetInt("CavalryHelp", 0);
    }

    public static void MusicReset()
    {
        //DontDestoyに入ってるBGMを削除
        SoundKeepScript.isLoad[0] = false;
        SoundKeepScript.isLoad[1] = false;
        SoundKeepScript.isLoad[2] = false;
        SoundKeepScript.isLoad[3] = false;
        Object.Destroy(GameObject.Find("BGM"));
        Object.Destroy(GameObject.Find("SettingBGM"));
        Object.Destroy(GameObject.Find("BattleBGM"));
        Object.Destroy(GameObject.Find("VoiceBGM"));
    }
}