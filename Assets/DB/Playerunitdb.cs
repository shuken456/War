using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Playerunitdb : ScriptableObject
{
    public List<Playerunit> Playerunitdblist;
}

[System.Serializable]
public class Playerunit
{
    public int Unitnum;
    public string Unitname;
}
