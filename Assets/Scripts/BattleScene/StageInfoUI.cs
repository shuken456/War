using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoUI : MonoBehaviour
{
    public Text StageNumText;

    public Text WinConditionText;

    public Text LoseConditionText;

    //勝利、敗北条件リスト
    private string[] WinConditionList = {"敵の全滅", "敵の全滅or\n敵拠点撃破", "敵の全滅or\n目標地点到達", "敵将の撃破or\n敵拠点撃破" };
    private string[] LoseConditionList = { "味方の全滅or\n味方拠点撃破", "味方の全滅or\n防衛地点到達"};
    private int[] StageWinList = { 0, 0, 0, 0, 1, 1, 2, 1, 1, 3 };
    private int[] StageLoseList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        StageNumText.text = "ステージ" + Common.Progress.ToString();
        WinConditionText.text = WinConditionList[StageWinList[Common.Progress - 1]];
        LoseConditionText.text = LoseConditionList[StageLoseList[Common.Progress - 1]];
    }
}
