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
    //�����i���o�[
    public int Num;
    //������
    public string Name;
    //�����J���[
    public Color UnitColor;
    //�������j(1�U���d���A2�ϋv�d���A3�ړ��d��)
    public int Strategy;
}
