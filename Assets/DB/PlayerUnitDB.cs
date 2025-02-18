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

        PlayerPrefs.SetString("ContinueUnitData", data);
    }

    public void Load()
    {
        var data = PlayerPrefs.GetString("ContinueUnitData");

        JsonUtility.FromJsonOverwrite(data, this);
    }

    public void InitialSave()
    {
        var data = JsonUtility.ToJson(this, true);

        PlayerPrefs.SetString("InitialUnitData", data);
    }

    public void InitialLoad()
    {
        var data = PlayerPrefs.GetString("InitialUnitData");
        PlayerPrefs.SetString("ContinueUnitData", data);

        JsonUtility.FromJsonOverwrite(data, this);
    }
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
