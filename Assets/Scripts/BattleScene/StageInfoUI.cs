using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoUI : MonoBehaviour
{
    public Text StageNumText;

    public Text WinConditionText;

    public Text LoseConditionText;

    //�����A�s�k�������X�g
    private string[] WinConditionList = {"�G�̑S��", "�G�̑S��or\n�G���_���j", "�G�̑S��or\n�ڕW�n�_���B", "�G���̌��jor\n�G���_���j" };
    private string[] LoseConditionList = { "�����̑S��or\n�������_���j", "�����̑S��or\n�h�q�n�_���B"};
    private int[] StageWinList = { 0, 0, 0, 0, 1, 1, 2, 1, 1, 3 };
    private int[] StageLoseList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        StageNumText.text = "�X�e�[�W" + Common.Progress.ToString();
        WinConditionText.text = WinConditionList[StageWinList[Common.Progress - 1]];
        LoseConditionText.text = LoseConditionList[StageLoseList[Common.Progress - 1]];
    }
}
