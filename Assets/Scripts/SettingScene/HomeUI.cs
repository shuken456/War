using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//準備画面　ホームUI
public class HomeUI : MonoBehaviour
{
    public SettingManager SeManager;
    public Text NextText;

    // Start is called before the first frame update
    void Start()
    {
        //次のステージを画面上に表示
        NextText.text = "Next…ステージ" + Common.Progress.ToString();
    }

    //出撃ボタン押下
    public void SortieButtonClick()
    {
        SeManager.SortieCheckUIUI.SetActive(true);
    }

    //雇用所ボタン押下
    public void EmploymentButtonClick()
    {
        SeManager.EmploymentUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //部隊編成ボタン押下
    public void UnitEditButtonClick()
    {
        SceneManager.LoadScene("UnitEditScene");
    }

    //兵士一覧ボタン押下
    public void FighterEditButtonClick()
    {
        SeManager.FighterEditUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //タイトル画面に戻すボタン押下
    public void TitleButtonClick()
    {
        SeManager.TitleUI.SetActive(true);
    }

    public void TitleYes()
    {
        //DontDestoyに入ってるBGMを削除
        Common.MusicReset();
        SceneManager.LoadScene("TitleScene");
    }

    public void TitleNo()
    {
        SeManager.TitleUI.SetActive(false);
    }
}
