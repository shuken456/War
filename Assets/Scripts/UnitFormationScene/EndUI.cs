using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndUI : MonoBehaviour
{
    public void Yes()
    {
        SceneManager.LoadScene("UnitEditScene");
    }

    public void No()
    {
        this.gameObject.SetActive(false);
    }
}
