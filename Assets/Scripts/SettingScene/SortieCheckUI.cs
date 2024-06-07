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
        InfoText.text = "�X�e�[�W" + Common.Progress.ToString() + "�ɏo�w���܂��B\n��낵���ł����H";
    }

    //�o�w�@���̃X�e�[�W���J�n
    public void YesButtonClick()
    {
        //DontDestoy�ɓ����Ă�BGM���폜
        Common.MusicReset();

        SceneManager.LoadScene("BattleScene" + Common.Progress.ToString());
    }

    public void NoButtonClick()
    {
        this.gameObject.SetActive(false);
    }
}
