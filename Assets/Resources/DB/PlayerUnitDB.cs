using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerUnitDB : ScriptableObject
{
    public List<PlayerUnit> PlayerUnitDBList;

    public void Save()
    {
        var data = JsonUtility.ToJson(this, true);

        PlayerPrefs.SetString("PlayerUnitData", data);
    }

    public void Load()
    {
        var data = PlayerPrefs.GetString("PlayerUnitData");

        JsonUtility.FromJsonOverwrite(data, this);
    }

    public void InitialSave()
    {
        var data = JsonUtility.ToJson(this, true);

        PlayerPrefs.SetString("InitialPlayerUnitData", data);
    }

    public void InitialLoad()
    {
        var data = PlayerPrefs.GetString("InitialPlayerUnitData");

        JsonUtility.FromJsonOverwrite(data, this);
    }
}

[System.Serializable]
public class PlayerUnit
{
    //部隊ナンバー
    public int Num;
    //部隊名
    public string Name;
    //部隊カラー
    public Color UnitColor;
    //部隊方針(1攻撃重視、2耐久重視、3移動重視)
    public int Strategy;
    //出撃フラグ(出撃中はtrue)
    public bool SoriteFlg;
}
