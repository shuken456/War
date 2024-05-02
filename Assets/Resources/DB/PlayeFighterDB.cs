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
    //���O
    public string Name;
    //���m�̎��(1�����A2�|���A3�����A4�R��)
    public int type;
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
