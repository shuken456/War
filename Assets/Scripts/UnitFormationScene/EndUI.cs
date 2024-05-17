using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndUI : MonoBehaviour
{
    public void Yes()
    {
        Scene Bscene = SceneManager.GetSceneByName("UnitEditScene");

        SceneManager.LoadScene("UnitEditScene", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("UnitFormationScene");
    }

    public void No()
    {
        this.gameObject.SetActive(false);
    }
}
