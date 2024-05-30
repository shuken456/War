using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerFighterDB : ScriptableObject
{
    public List<PlayerFighter> PlayerFighterDBList;

    public void Save()
    {
        var data = JsonUtility.ToJson(this, true);

        PlayerPrefs.SetString("PlayerFighterData", data);
    }

    public void Load()
    {
        var data = PlayerPrefs.GetString("PlayerFighterData");

        JsonUtility.FromJsonOverwrite(data, this);
    }

    public void InitialSave()
    {
        var data = JsonUtility.ToJson(this, true);

        PlayerPrefs.SetString("InitialPlayerFighterData", data);
    }

    public void InitialLoad()
    {
        var data = PlayerPrefs.GetString("InitialPlayerFighterData");

        JsonUtility.FromJsonOverwrite(data, this);
    }
}

[System.Serializable]
public class PlayerFighter
{
    //名前
    public string Name;
    //兵士の種類(1歩兵、2弓兵、3盾兵、4騎兵)
    public int Type;
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
