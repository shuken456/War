using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//出撃可能部隊数表示UI 画面左上
public class UnitCountUI : MonoBehaviour
{
    public BattleManager BaManager;

    public Text CountText;

    //ステージごとの出撃可能部隊数
    public int[] PossibleSortieCountList = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
    //現在の出撃可能部隊数
    public int PossibleSortieCountNow = 99;

    // Start is called before the first frame update
    void Start()
    {
        PossibleSortieCountNow = PossibleSortieCountList[Common.Progress - 1];
        TextDraw();
    }

    public void TextDraw()
    {
        CountText.text = PossibleSortieCountNow.ToString();
    }
}
