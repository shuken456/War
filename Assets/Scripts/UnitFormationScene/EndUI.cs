using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndUI : MonoBehaviour
{
    public void Yes()
    {
        StartCoroutine(LoadEditScene());
    }

    IEnumerator LoadEditScene()
    {
        var op = SceneManager.LoadSceneAsync("UnitEditScene", LoadSceneMode.Additive);
        yield return op;

        //ロード後、アンロード
        SceneManager.UnloadSceneAsync("UnitFormationScene");
    }

    public void No()
    {
        this.gameObject.SetActive(false);
    }
}
