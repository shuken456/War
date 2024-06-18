using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�o���\�������\��UI ��ʍ���
public class UnitCountUI : MonoBehaviour
{
    public BattleManager BaManager;

    public Text CountText;

    //�X�e�[�W���Ƃ̏����o���\������
    //�g�[�^���@
    //�X�e�[�W1�`4  ��1
    //�X�e�[�W5�`7  ��2
    //�X�e�[�W8,9   ��3
    //�X�e�[�W10,11 ��4
    //�X�e�[�W12,13 ��5
    //�X�e�[�W14    ��6
    //�X�e�[�W15,16 ��7
    //�X�e�[�W17    ��8
    //�X�e�[�W18,19 ��9
    //�X�e�[�W20    ��10
    private int[] PossibleSortieCountList = { 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 5, 4, 5, 5, 5, 9, 6, 7};

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
