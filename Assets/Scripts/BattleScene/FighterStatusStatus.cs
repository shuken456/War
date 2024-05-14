using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterStatus : MonoBehaviour
{
    //�X�e�[�^�X

    //���m�̎��(1�����A2�|���A3�����A4�R��)
    public int Type;
    //���O
    public string FighterName;
    //���x��
    public int Level;
    //HP
    public float MaxHp;
    //���݂�HP
    public float NowHp;
    //�X�^�~�i
    public float MaxStamina;
    //���݂̃X�^�~�i
    public float NowStamina;
    //�U����
    public int AtkPower;
    //�U�����x
    public int AtkSpeed;
    //�ړ����x
    public int MoveSpeed;
    //���������i���o�[(0�͖�����)
    public int UnitNum;
    //�������t���O
    public bool UnitLeader;

    //HP�o�t
    public float MaxHpBuff;
    //�U���̓o�t
    public int AtkPowerBuff;
    //�ړ����x�o�t
    public int MoveSpeedBuff;
}
