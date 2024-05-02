using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterStatus : MonoBehaviour
{
    //ステータス

    //名前
    public string FighterName;
    //レベル
    public int Level;
    //HP
    public float MaxHp;
    //現在のHP
    public float NowHp;
    //スタミナ
    public float MaxStamina;
    //現在のスタミナ
    public float NowStamina;
    //攻撃力
    public int AtkPower;
    //移動速度
    public int MoveSpeed;
    //攻撃速度
    public int AtkSpeed;
    //所属部隊ナンバー(0は無所属)
    public int UnitNum;
    //部隊長フラグ
    public bool UnitLeader;
}
