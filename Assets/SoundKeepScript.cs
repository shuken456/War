using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundKeepScript : MonoBehaviour
{
    public int Num;
    public static bool[] isLoad = { false, false, false };// ���g�����łɃ��[�h����Ă��邩�𔻒肷��t���O

    private void Awake()
    {
        if (isLoad[Num])
        { // ���łɃ��[�h����Ă�����
            Destroy(this.gameObject); // �������g��j�����ďI��
            return;
        }
        isLoad[Num] = true; // ���[�h����Ă��Ȃ�������A�t���O�����[�h�ς݂ɐݒ肷��
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDestroy()
    {
        isLoad[Num] = false;
    }
}
