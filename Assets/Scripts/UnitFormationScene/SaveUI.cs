using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveUI : MonoBehaviour
{
    public void OK()
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
}
