using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveUI : MonoBehaviour
{
    public void OK()
    {
        this.gameObject.SetActive(false);
        SceneManager.LoadScene("UnitFormationScene");
    }
}
