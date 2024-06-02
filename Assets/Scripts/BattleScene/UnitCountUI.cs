using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�o���\�������\��UI ��ʍ���
public class UnitCountUI : MonoBehaviour
{
    public BattleManager BaManager;

    public Text CountText;

    //�X�e�[�W���Ƃ̏o���\������
    public int[] PossibleSortieCountList = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
    //���݂̏o���\������
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
