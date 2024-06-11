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
    private int[] PossibleSortieCountList = { 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2};

    //���X�̏o���\������
    public int PossibleSortieCountDefault;

    //���݂̏o���\������
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
