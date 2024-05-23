using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCountUI : MonoBehaviour
{
    public Text CountText;

    //�X�e�[�W���Ƃ̏����o���\������
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
