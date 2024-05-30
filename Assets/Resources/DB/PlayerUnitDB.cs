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
    //�����i���o�[
    public int Num;
    //������
    public string Name;
    //�����J���[
    public Color UnitColor;
    //�������j(1�U���d���A2�ϋv�d���A3�ړ��d��)
    public int Strategy;
    //�o���t���O(�o������true)
    public bool SoriteFlg;
}
