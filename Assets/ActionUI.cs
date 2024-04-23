using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUI : MonoBehaviour
{

    public BattleManager BaManager;

    //�ړ��{�^��
    public Button MoveButton;

    //�ړ����[�g��\�������
    public GameObject MoveLine;

    // Start is called before the first frame update
    private void OnEnable()
    {
        //���Ɉړ����[�g���ݒ肳��Ă��郆�j�b�g���I�����ꂽ�ꍇ�A���̃��[�g��\��
        for (int i = 0; i <= BaManager.SelectUnit.Count - 1; i++)
        {
            Transform LineObject = BaManager.SelectUnit[i].transform.Find("Line(Clone)");
            if (LineObject)
            {
                LineObject.gameObject.SetActive(true);
                LineObject.gameObject.GetComponent<LineRenderer>().SetPosition(0, BaManager.SelectUnit[i].transform.position);
            }
        }

        if(BaManager.SelectUnit.Count == 0)
        {
            MoveButton.interactable = false;
        }
        else
        {
            MoveButton.interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�ړ����[�h��
    public void MoveMode()
    {
        for (int i = 0; i <= BaManager.SelectUnit.Count - 1; i++)
        {
            Transform LineObject = BaManager.SelectUnit[i].transform.Find("Line(Clone)");

            if (!LineObject)
            {
                //�ړ����[�g���ݒ肳��Ă��Ȃ��ꍇ�A�ړ��������q�v�f�ɂ���
                GameObject moveline = Instantiate(MoveLine, new Vector3(0, 0, 0), Quaternion.identity);
                moveline.transform.parent = BaManager.SelectUnit[i].transform;
                moveline.GetComponent<LineRenderer>().SetPosition(0, BaManager.SelectUnit[i].transform.position);
            }
            else
            {
                //���Ɉړ����[�g���ݒ肳��Ă���ꍇ�A���[�g���̌����ڂ�����������
                LineObject.gameObject.GetComponent<LineRenderer>().SetPosition(0, BaManager.SelectUnit[i].transform.position);
                LineObject.gameObject.GetComponent<LineRenderer>().positionCount = 2;
            }

            BaManager.SelectUnitLine.Add(BaManager.SelectUnit[i].transform.Find("Line(Clone)").gameObject.GetComponent<LineRenderer>());
        }

        //�ړ����[�h�ɕύX
        BaManager.currentMode = BattleManager.Mode.Move;
        this.gameObject.SetActive(false);
        BaManager.MoveUI.SetActive(true);
    }

    //�ĊJ
    public void ReStart()
    {
        //�I������
        for (int i = 0; i <= BaManager.SelectUnit.Count - 1; i++)
        {
            BaManager.SelectUnit[i].GetComponent<SpriteRenderer>().material = BaManager.NoSelectMaterial;

            //�ړ����[�g���\��
            Transform LineObject = BaManager.SelectUnit[i].transform.Find("Line(Clone)");
            if (LineObject)
            {
                LineObject.gameObject.SetActive(false);
            }
        }

        BaManager.SelectUnit.Clear();
        BaManager.SelectUnitLine.Clear();
        Time.timeScale = 1;
        BaManager.currentMode = BattleManager.Mode.Default;
        this.gameObject.SetActive(false);
        BaManager.InstructionButton.gameObject.SetActive(true);
    }
}
