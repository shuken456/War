using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeUI : MonoBehaviour
{
    public SettingManager SeManager;
    public Text NextText;

    // Start is called before the first frame update
    void Start()
    {
        NextText.text = "Next…ステージ" + Common.Progress.ToString();
    }

    public void SortieButtonClick()
    {
        SeManager.SortieCheckUIUI.SetActive(true);
    }

    public void EmploymentButtonClick()
    {
        SeManager.EmploymentUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void UnitEditButtonClick()
    {
        SceneManager.LoadScene("UnitEditScene");
    }

    public void FighterEditButtonClick()
    {
        SeManager.FighterEditUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void SaveButtonClick()
    {
        SeManager.SaveQuestionUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
