using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SortieCheckUI : MonoBehaviour
{
    public Text InfoText;

    // Start is called before the first frame update
    void Start()
    {
        InfoText.text = "ステージ" + Common.Progress.ToString() + "に出陣します。\nよろしいですか？";
    }

    //出陣　次のステージを開始
    public void YesButtonClick()
    {
        //DontDestoyに入ってるBGMを削除
        Common.MusicReset();

        SceneManager.LoadScene("BattleScene" + Common.Progress.ToString());
    }

    public void NoButtonClick()
    {
        this.gameObject.SetActive(false);
    }
}
