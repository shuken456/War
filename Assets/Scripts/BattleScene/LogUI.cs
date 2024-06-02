using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogUI : MonoBehaviour
{
    //スクロールビュー
    public GameObject LogView;
    //ログ用テキスト
    public GameObject LogText;

    //ログ記載
    public void DrawLog(string log)
    {
        GameObject Text = Instantiate(LogText, LogView.transform);
        Text.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        Text.transform.Find("Text(Log)").GetComponent<Text>().text = log;
    }
}
