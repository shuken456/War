using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//出撃可能部隊数表示UI 画面左上
public class UnitCountUI : MonoBehaviour
{
    public BattleManager BaManager;

    public Text CountText;

    //ステージごとの初期出撃可能部隊数
    //トータル　
    //ステージ1～4  →1
    //ステージ5～7  →2
    //ステージ8,9   →3
    //ステージ10,11 →4
    //ステージ12,13 →5
    //ステージ14    →6
    //ステージ15,16 →7
    //ステージ17    →8
    //ステージ18,19 →9
    //ステージ20    →10
    private int[] PossibleSortieCountList = { 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 5, 4, 5, 5, 5, 9, 6, 7};

    //元々の出撃可能部隊数
    public int PossibleSortieCountDefault;

    //現在の出撃可能部隊数
    public int PossibleSortieCountNow = 99;

    // Start is called before the first frame update
    void Start()
    {
        PossibleSortieCountDefault = PossibleSortieCountList[Common.Progress - 1];
        PossibleSortieCountNow = PossibleSortieCountDefault;
        TextDraw();
    }

    public void TextDraw()
    {
        CountText.text = PossibleSortieCountNow.ToString();
    }
}
