using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//������ʁ@�z�[��UI
public class HomeUI : MonoBehaviour
{
    public SettingManager SeManager;
    public Text NextText;

    // Start is called before the first frame update
    void Start()
    {
        //���̃X�e�[�W����ʏ�ɕ\��
        NextText.text = "Next�c�X�e�[�W" + Common.Progress.ToString();
    }

    //�o���{�^������
    public void SortieButtonClick()
    {
        SeManager.SortieCheckUIUI.SetActive(true);
    }

    //�ٗp���{�^������
    public void EmploymentButtonClick()
    {
        SeManager.EmploymentUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //�����Ґ��{�^������
    public void UnitEditButtonClick()
    {
        SceneManager.LoadScene("UnitEditScene");
    }

    //���m�ꗗ�{�^������
    public void FighterEditButtonClick()
    {
        SeManager.FighterEditUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //�^�C�g����ʂɖ߂��{�^������
    public void TitleButtonClick()
    {
        SeManager.TitleUI.SetActive(true);
    }

    public void TitleYes()
    {
        //DontDestoy�ɓ����Ă�BGM���폜
        Common.MusicReset();
        SceneManager.LoadScene("TitleScene");
    }

    public void TitleNo()
    {
        SeManager.TitleUI.SetActive(false);
    }
}
