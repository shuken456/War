using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCountUI : MonoBehaviour
{
    public Text CountText;

    //ステージごとの初期出撃可能部隊数
    public int[] CountList = { 2, 2, 3, 4 };
    public int Count = 1;

    // Start is called before the first frame update
    void Start()
    {
        Count = CountList[Common.SelectStageNum - 1];
        TextDraw();
    }

    public void TextDraw()
    {
        CountText.text = Count.ToString();
    }
}
