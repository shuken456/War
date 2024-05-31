using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    public Text PointText;

    // Start is called before the first frame update
    void Start()
    {
        TextWrite();
    }

    public void TextWrite()
    {
        PointText.text = Common.Money.ToString() + "—¼";
    }
}
