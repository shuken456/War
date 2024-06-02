using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogUI : MonoBehaviour
{
    //�X�N���[���r���[
    public GameObject LogView;
    //���O�p�e�L�X�g
    public GameObject LogText;

    //���O�L��
    public void DrawLog(string log)
    {
        GameObject Text = Instantiate(LogText, LogView.transform);
        Text.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        Text.transform.Find("Text(Log)").GetComponent<Text>().text = log;
    }
}
