using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpUI : MonoBehaviour
{
    //�e�y�[�W�I�u�W�F�N�g
    public GameObject[] PageObject;

    //�ő�y�[�W���ƌ��݂̃y�[�W��
    private int MaxPage;
    private int NowPage;

    public GameObject NextButton;
    public GameObject BackButton;
    public GameObject CloseButton;

    private float DefaultTime;

    // Start is called before the first frame update
    void OnEnable()
    {
        DefaultTime = Time.timeScale;
        Time.timeScale = 0;

        MaxPage = PageObject.Length;
        NowPage = 1;

        if(NowPage < MaxPage)
        {
            NextButton.SetActive(true);
        }
        else
        {
            CloseButton.SetActive(true);
        }
    }

    private void OnDisable()
    {
        Time.timeScale = DefaultTime;
    }

    //���̃y�[�W��
    public void NextButtonClick()
    {
        PageObject[NowPage - 1].SetActive(false);
        NowPage += 1;
        PageObject[NowPage - 1].SetActive(true);

        BackButton.SetActive(true);
        if (NowPage == MaxPage)
        {
            NextButton.SetActive(false);
            CloseButton.SetActive(true);
        }
    }

    //�O�̃y�[�W��
    public void BackButtonClick()
    {
        PageObject[NowPage - 1].SetActive(false);
        NowPage -= 1;
        PageObject[NowPage - 1].SetActive(true);

        NextButton.SetActive(true);
        CloseButton.SetActive(false);
        if (NowPage == 1)
        {
            BackButton.SetActive(false);
        }
    }

    //����
    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
