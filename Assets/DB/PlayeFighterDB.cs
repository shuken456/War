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

        //�������Ɛi�s�x�Z�[�u
        Common.ProgressMoneySave();
    }

    public void Load()
    {
        var data = PlayerPrefs.GetString("ContinueFighterData");

        JsonUtility.FromJsonOverwrite(data, this);

        //�������Ɛi�s�x���[�h
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
    //���O
    public string Name;
    //���m�̎��(1�����A2�|���A3�����A4�R��)
    public int Type;
    //���x��
    public int Level;
    //HP
    public float Hp;
    //�X�^�~�i
    public float Stamina;
    //�U����
    public int AtkPower;
    //�ړ����x
    public int MoveSpeed;
    //�U�����x
    public int AtkSpeed;
    //�o���l
    public int EXP;
    //���������i���o�[(0�͖�����)
    public int UnitNum;
    //�z�u
    public Vector3 Position;
    //�������t���O
    public bool UnitLeader;
}
