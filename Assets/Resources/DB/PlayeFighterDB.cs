using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerFighterDB : ScriptableObject
{
    public List<PlayerFighter> PlayerFighterDBList;
}

[System.Serializable]
public class PlayerFighter
{
    //名前
    public string Name;
    //兵士の種類(1歩兵、2弓兵、3盾兵、4騎兵)
    public int type;
    //レベル
    public int Level;
    //HP
    public float Hp;
    //スタミナ
    public float Stamina;
    //攻撃力
    public int AtkPower;
    //移動速度
    public int MoveSpeed;
    //攻撃速度
    public int AtkSpeed;
    //経験値
    public int EXP;
    //所属部隊ナンバー(0は無所属)
    public int UnitNum;
    //配置
    public Vector3 Position;
    //部隊長フラグ
    public bool UnitLeader;
}
