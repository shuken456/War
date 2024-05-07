using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUI : MonoBehaviour
{
    public BattleManager BaManager;

    //�ړ��{�^��
    public Button MoveButton;

    //�ҋ@�{�^��
    public Button WaitButton;

    //�ړ����[�g��\�������
    public GameObject MoveLine;

    //�����_
    public GameObject MovePoint;

    //���̃N���b�N���ɂ��̕��m���I�����ꂽ���ۂ����Ǘ�
    private List<GameObject> OneClickSelectFighter = new List<GameObject>();
    private List<GameObject> OneClickNoSelectFighter = new List<GameObject>();


    // Start is called before the first frame update
    private void OnEnable()
    {
        ChangeButton();

        //�S�������m�̈ړ����[�g��\��
        GameObject[] tagObjects = GameObject.FindGameObjectsWithTag("PlayerFighter");

        foreach (GameObject Fighter in tagObjects)
        {
            var targetList = Fighter.GetComponent<FighterAction>().targetPlace;
            var targetFighter = Fighter.GetComponent<FighterAction>().targetFighter;

            if (targetList.Count > 0 || targetFighter)
            {
                LineRenderer moveline = Instantiate(MoveLine, Fighter.transform.position, Quaternion.identity).GetComponent<LineRenderer>();
                moveline.gameObject.transform.parent = Fighter.transform;
                moveline.positionCount = 1;
                moveline.SetPosition(0, Fighter.transform.position);

                //�ړ��ڕW�����A�܂���ɂ���
                for (int i = 0; i <= targetList.Count - 1; i++)
                {
                    moveline.positionCount++;
                    moveline.SetPosition(i + 1, targetList[i]);
                }

                //�ړ��Ώە��m������ꍇ�A�ǉ�
                if (targetFighter)
                {
                    moveline.positionCount++;
                    moveline.SetPosition(moveline.positionCount - 1, targetFighter.position);
                    moveline.material.color = Color.green;
                }
                else
                {
                    //�ŏI�ړ��n�_�\��
                    GameObject movepoint = Instantiate(MovePoint, moveline.GetPosition(moveline.positionCount - 1), Quaternion.identity);
                    movepoint.transform.parent = Fighter.transform;
                    moveline.material.color = Color.yellow;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

        //���N���b�N�ŕ��m�I���A����
        if (Input.GetMouseButton(0))
        {
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter", "SelectFighter"));
            if (col != null && !BaManager.SelectFighter.Contains(col.gameObject) && !OneClickNoSelectFighter.Contains(col.gameObject))
            {
                BaManager.SelectFighter.Add(col.gameObject);
                OneClickSelectFighter.Add(col.gameObject);

                //�I�𒆂̌����ڂ�ύX
                col.gameObject.layer = LayerMask.NameToLayer("SelectFighter");
            }

            if (col != null && BaManager.SelectFighter.Contains(col.gameObject) && !OneClickSelectFighter.Contains(col.gameObject))
            {
                BaManager.SelectFighter.Remove(col.gameObject);
                OneClickNoSelectFighter.Add(col.gameObject);

                //�I�𒆂̌����ڂ�ύX
                col.gameObject.layer = LayerMask.NameToLayer("PlayerFighter");
            }
            ChangeButton();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OneClickSelectFighter.Clear();
            OneClickNoSelectFighter.Clear();
        }
    }

    //�ړ����[�h��
    public void MoveMode()
    {
        //�ړ����[�h�ɕύX
        BaManager.currentMode = BattleManager.Mode.MoveDecisionBefore;
        BaManager.MoveUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //�I�𒆂̕��m�����̏�őҋ@
    public void Wait()
    {
        foreach (GameObject Fighter in BaManager.SelectFighter)
        {
            FighterAction FighterA = Fighter.GetComponent<FighterAction>();
            //�N���A
            FighterA.targetPlace.Clear();
            FighterA.targetFighter = null;
        }

        DeleteMoveRoute();
        OnEnable();
    }

    private void ChangeButton()
    {
        //�I�𒆂̕��m�����邩�ۂ��Ŏg�p�\�ȋ@�\��ς���
        if (BaManager.SelectFighter.Count == 0)
        {
            MoveButton.interactable = false;
            WaitButton.interactable = false;
        }
        else
        {
            MoveButton.interactable = true;
            WaitButton.interactable = true;
        }
    }

    public void DeleteMoveRoute()
    {
        //�S�������m�̈ړ����[�g���폜
        GameObject[] tagObjects = GameObject.FindGameObjectsWithTag("PlayerFighter");

        foreach (GameObject Fighter in tagObjects)
        {
            if (Fighter.transform.Find("Line(Clone)"))
            {
                Destroy(Fighter.transform.Find("Line(Clone)").gameObject);

                if (Fighter.transform.Find("MovePoint(Clone)"))
                {
                    Destroy(Fighter.transform.Find("MovePoint(Clone)").gameObject);
                }
            }
        }
    }

    //�ĊJ
    public void ReStart()
    {
        DeleteMoveRoute();

        //���ꂼ��̕ϐ��Ɖ�ʏ�Ԃ����ɖ߂�
        foreach (GameObject Fighter in BaManager.SelectFighter)
        {
            Fighter.layer = LayerMask.NameToLayer("PlayerFighter");
        }
        BaManager.SelectFighter.Clear();
        BaManager.SelectFighterLine.Clear();
        BaManager.currentMode = BattleManager.Mode.Default;
        BaManager.InstructionButton.gameObject.SetActive(true);
        Time.timeScale = 1;

        this.gameObject.SetActive(false);
    }
}
