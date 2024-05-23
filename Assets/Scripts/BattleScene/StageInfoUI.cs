using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoUI : MonoBehaviour
{
    public Text StageNumText;

    public Text WinConditionText;

    public Text LoseConditionText;

    // Start is called before the first frame update
    void Start()
    {
        StageNumText.text = "ステージ" + Common.SelectStageNum.ToString();
        WinConditionText.text = "敵の全滅";
        LoseConditionText.text = "味方の全滅";
    }
}
