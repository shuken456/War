using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterStatus : MonoBehaviour
{
    //ステータス

    //兵士の種類(1歩兵、2弓兵、3盾兵、4騎兵)
    public int Type;
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
    //攻撃速度
    public int AtkSpeed;
    //移動速度
    public int MoveSpeed;
    //所属部隊ナンバー(0は無所属)
    public int UnitNum;
    //部隊長フラグ
    public bool UnitLeader;

    //HPバフ
    public float MaxHpBuff;
    //攻撃力バフ
    public int AtkPowerBuff;
    //移動速度バフ
    public int MoveSpeedBuff;
}
