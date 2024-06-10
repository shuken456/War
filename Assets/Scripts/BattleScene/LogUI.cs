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
    //スクロールバー
    public Scrollbar Scrollbar;

    //ログ記載
    public IEnumerator DrawLog(string log)
    {
        GameObject Text = Instantiate(LogText, LogView.transform);
        Text.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        Text.transform.Find("Text(Log)").GetComponent<Text>().text = log;
        Scrollbar.value = 0;
        yield return new WaitForSecondsRealtime(0.02f);

        //スクロールバーを一番下に ※コルーチンにしないと完全に一番下に行かない
        Scrollbar.value = 0;
    }
}
