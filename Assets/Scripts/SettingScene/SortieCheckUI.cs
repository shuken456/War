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

    public void YesButtonClick()
    {
        //SceneManager.LoadScene("BattleScene" + Common.Progress.ToString());
        SceneManager.LoadScene("BattleScene1");
    }

    public void NoButtonClick()
    {
        this.gameObject.SetActive(false);
    }
}
