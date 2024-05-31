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

        PlayerPrefs.SetString("ContinueFighterData", data);

        //所持金と進行度セーブ
        Common.ProgressMoneySave();
    }

    public void Load()
    {
        var data = PlayerPrefs.GetString("ContinueFighterData");

        JsonUtility.FromJsonOverwrite(data, this);

        //所持金と進行度ロード
        Common.ProgressMoneyLoad();
    }

    public void InitialSave()
    {
        var data = JsonUtility.ToJson(this, true);

        PlayerPrefs.SetString("InitialFighterData", data);
    }

    public void InitialLoad()
    {
        var data = PlayerPrefs.GetString("InitialFighterData");
        PlayerPrefs.SetString("ContinueFighterData", data);

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
