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
    //�X�N���[���o�[
    public Scrollbar Scrollbar;

    //���O�L��
    public IEnumerator DrawLog(string log)
    {
        GameObject Text = Instantiate(LogText, LogView.transform);
        Text.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        Text.transform.Find("Text(Log)").GetComponent<Text>().text = log;
        Scrollbar.value = 0;
        yield return new WaitForSecondsRealtime(0.02f);

        //�X�N���[���o�[����ԉ��� ���R���[�`���ɂ��Ȃ��Ɗ��S�Ɉ�ԉ��ɍs���Ȃ�
        Scrollbar.value = 0;
    }
}
