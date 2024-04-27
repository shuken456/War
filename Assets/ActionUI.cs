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

    //���̃N���b�N���ɂ��̃��j�b�g���I�����ꂽ���ۂ����Ǘ�
    private List<GameObject> OneClickSelectUnit = new List<GameObject>();
    private List<GameObject> OneClickNoSelectUnit = new List<GameObject>();


    // Start is called before the first frame update
    private void OnEnable()
    {
        ChangeButton();

        //�S�������j�b�g�̈ړ����[�g��\��
        GameObject[] tagObjects = GameObject.FindGameObjectsWithTag("PlayerUnit");

        foreach (GameObject Unit in tagObjects)
        {
            var targetList = Unit.GetComponent<UnitAction>().targetPlace;
            var targetUnit = Unit.GetComponent<UnitAction>().targetUnit;

            if (targetList.Count > 0 || targetUnit)
            {
                LineRenderer moveline = Instantiate(MoveLine, Unit.transform.position, Quaternion.identity).GetComponent<LineRenderer>();
                moveline.gameObject.transform.parent = Unit.transform;
                moveline.positionCount = 1;
                moveline.SetPosition(0, Unit.transform.position);

                //�ړ��ڕW�����A�܂���ɂ���
                for (int i = 0; i <= targetList.Count - 1; i++)
                {
                    moveline.positionCount++;
                    moveline.SetPosition(i + 1, targetList[i]);
                }

                //�ړ��Ώۃ��j�b�g������ꍇ�A�ǉ�
                if (targetUnit)
                {
                    moveline.positionCount++;
                    moveline.SetPosition(moveline.positionCount - 1, targetUnit.position);
                    moveline.material.color = Color.green;
                }
                else
                {
                    //�ŏI�ړ��n�_�\��
                    GameObject movepoint = Instantiate(MovePoint, moveline.GetPosition(moveline.positionCount - 1), Quaternion.identity);
                    movepoint.transform.parent = Unit.transform;
                    moveline.material.color = Color.yellow;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

        //���N���b�N�Ń��j�b�g�I���A����
        if (Input.GetMouseButton(0))
        {
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerUnit"));
            if (col != null && !BaManager.SelectUnit.Contains(col.gameObject) && !OneClickNoSelectUnit.Contains(col.gameObject))
            {
                BaManager.SelectUnit.Add(col.gameObject);
                OneClickSelectUnit.Add(col.gameObject);

                //�I�𒆂̌����ڂ�ύX
                col.gameObject.GetComponent<SpriteRenderer>().material = BaManager.SelectMaterial;
            }

            if (col != null && BaManager.SelectUnit.Contains(col.gameObject) && !OneClickSelectUnit.Contains(col.gameObject))
            {
                BaManager.SelectUnit.Remove(col.gameObject);
                OneClickNoSelectUnit.Add(col.gameObject);

                //�I�𒆂̌����ڂ�ύX
                col.gameObject.GetComponent<SpriteRenderer>().material = BaManager.NoSelectMaterial;
            }
            ChangeButton();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OneClickSelectUnit.Clear();
            OneClickNoSelectUnit.Clear();
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

    //�I�𒆂̃��j�b�g�����̏�őҋ@
    public void Wait()
    {
        foreach (GameObject Unit in BaManager.SelectUnit)
        {
            UnitAction UnitA = Unit.GetComponent<UnitAction>();
            //�N���A
            UnitA.targetPlace.Clear();
            UnitA.targetUnit = null;
        }

        DeleteMoveRoute();
        OnEnable();
    }

    private void ChangeButton()
    {
        //�I�𒆂̃��j�b�g�����邩�ۂ��Ŏg�p�\�ȋ@�\��ς���
        if (BaManager.SelectUnit.Count == 0)
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
        //�S�������j�b�g�̈ړ����[�g���폜
        GameObject[] tagObjects = GameObject.FindGameObjectsWithTag("PlayerUnit");

        foreach (GameObject Unit in tagObjects)
        {
            if (Unit.transform.Find("Line(Clone)"))
            {
                Destroy(Unit.transform.Find("Line(Clone)").gameObject);

                if (Unit.transform.Find("MovePoint(Clone)"))
                {
                    Destroy(Unit.transform.Find("MovePoint(Clone)").gameObject);
                }
            }
        }
    }

    //�ĊJ
    public void ReStart()
    {
        DeleteMoveRoute();

        //���ꂼ��̕ϐ��Ɖ�ʏ�Ԃ����ɖ߂�
        foreach (GameObject Unit in BaManager.SelectUnit)
        {
            Unit.GetComponent<SpriteRenderer>().material = BaManager.NoSelectMaterial;
        }
        BaManager.SelectUnit.Clear();
        BaManager.SelectUnitLine.Clear();
        BaManager.currentMode = BattleManager.Mode.Default;
        BaManager.InstructionButton.gameObject.SetActive(true);
        Time.timeScale = 1;

        this.gameObject.SetActive(false);
    }
}
