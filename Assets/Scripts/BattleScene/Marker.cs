using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //�X�e�[�W�̃T�C�Y�ɂ����marker�̃T�C�Y���ς���
        if(GameObject.Find("MiniMapCamera"))
        {
            float size = GameObject.Find("MiniMapCamera").GetComponent<Camera>().orthographicSize / 6;
            this.gameObject.transform.localScale = new Vector3(size, size, 1);
        }
        
    }

}
