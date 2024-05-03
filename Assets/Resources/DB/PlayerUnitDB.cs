using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerUnitDB : ScriptableObject
{
    public List<PlayerUnit> PlayerUnitDBList;
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
}
