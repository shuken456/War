using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundKeepScript : MonoBehaviour
{
    public int Num;
    public static bool[] isLoad = { false, false, false };// 自身がすでにロードされているかを判定するフラグ

    private void Awake()
    {
        if (isLoad[Num])
        { // すでにロードされていたら
            Destroy(this.gameObject); // 自分自身を破棄して終了
            return;
        }
        isLoad[Num] = true; // ロードされていなかったら、フラグをロード済みに設定する
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDestroy()
    {
        isLoad[Num] = false;
    }
}
