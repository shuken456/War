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
    //àio[
    public int Num;
    //à¼
    public string Name;
    //àJ[
    public Color UnitColor;
    //àûj(1UdA2ÏvdA3Ú®d)
    public int Strategy;
    //otO(oÍtrue)
    public bool SoriteFlg;
}
