using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using UnityEngine.UI;

public class MoveUI : MonoBehaviour
{
    public BattleManager BaManager;

    //�ړ����[�g
    private List<Vector3> MovePositions = new List<Vector3>();

    //�ړ�����UI
    public GameObject MoveUIAfter;

    //�ǂ��{�^��
    public Button ChaseButton;

    //�ǂ��Ώ�
    private GameObject ChaseTarget;

    //�ړ�����UI
    public Canvas canvas;

    //���C���J����
    public CinemachineVirtualCamera mainCam;

    //�ŏI�����_
    private Vector3 LastPosition;

    //�N���b�N���������ԃJ�E���g�p
    private float ClickTime = 0;

    private void OnEnable()
    {
        //�I�𒆂̃��j�b�g�̈ړ�����V���ɍ쐬����
        foreach (GameObject Unit in BaManager.SelectUnit)
        {
            if (Unit.transform.Find("Line(Clone)"))
            {
                Destroy(Unit.transform.Find("Line(Clone)").gameObject);

                if (Unit.transform.Find("MovePoint(Clone)"))
                {
                    Destroy(Unit.transform.Find("MovePoint(Clone)").gameObject);
                }
            }

            //�ړ��������q�v�f�ɂ���
            LineRenderer moveline = Instantiate(BaManager.ActionUI.GetComponent<ActionUI>().MoveLine, Unit.transform.position, Quaternion.identity).GetComponent<LineRenderer>();
            moveline.gameObject.transform.parent = Unit.transform;
            moveline.SetPosition(0, Unit.transform.position);
            moveline.SetPosition(1, Unit.transform.position);
            BaManager.SelectUnitLine.Add(moveline);
        }
    }

    private void OnDisable()
    {
        //�A�N�V����UI���J����邽�߁A�ړ������ꊇ�폜����
        BaManager.ActionUI.GetComponent<ActionUI>().DeleteMoveRoute();

        //���ꂼ��̕ϐ��Ɖ�ʏ�Ԃ����ɖ߂�
        MoveUIAfter.SetActive(false);
        MovePositions.Clear();
        BaManager.SelectUnitLine.Clear();
        BaManager.currentMode = BattleManager.Mode.Instruction;
        BaManager.ActionUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //���[�h�ɂ���ď�����ς���
        switch (BaManager.currentMode)
        {
            case BattleManager.Mode.MoveDecisionBefore:
                UpdateMoveDecisionBeforeMode();
                break;

            case BattleManager.Mode.MoveDecisionAfter:
                break;
        }
    }

    //�ړ����[�g�w�蒆
    void UpdateMoveDecisionBeforeMode()
    {
        //�ړ��\���ǂ���
        bool MoveFlg = true;

        //�ړ����[�g�`�F�b�N�pray
        Ray2D ray;
        RaycastHit2D hit;

        Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

        //�J�[�\����UI��ɂ���ꍇ�A�ړ�����\��
        if (EventSystem.current.IsPointerOverGameObject())
        {
            foreach (LineRenderer UnitLine in BaManager.SelectUnitLine)
            {
                UnitLine.gameObject.SetActive(false);
            }
        }
        else
        {
            //�ړ�����\��
            foreach (LineRenderer UnitLine in BaManager.SelectUnitLine)
            {
                UnitLine.gameObject.SetActive(true);
            }

            //��ڂ̃��[�g�w�莞
            if (MovePositions.Count == 0)
            {
                //���ꂼ��̑I�𒆃��j�b�g����̃��[�g�ɏ�Q�����Ȃ����m�F
                for (int i = 0; i <= BaManager.SelectUnit.Count - 1; i++)
                {
                    Vector3 SelectUnitPosition = BaManager.SelectUnit[i].transform.position;
                    ray = new Ray2D(SelectUnitPosition, CursorPosition - SelectUnitPosition);
                    hit = Physics2D.Raycast(ray.origin, ray.direction, Vector2.Distance(SelectUnitPosition, CursorPosition), LayerMask.GetMask("Obstacle"));

                    if (hit.collider)
                    {
                        //��ł��ړ��s�̃��[�g������Έړ������s�Ƃ���
                        MoveFlg = false;
                        DrawMoveLine(BaManager.SelectUnitLine[i], hit.point, Color.red);
                    }
                    else
                    {
                        DrawMoveLine(BaManager.SelectUnitLine[i], CursorPosition, Color.yellow);
                    }
                }
            }
            else
            {
                //���p�_����̃��[�g�ɏ�Q�����Ȃ����m�F
                Vector3 BeforePosition = MovePositions[MovePositions.Count - 1];
                ray = new Ray2D(BeforePosition, CursorPosition - BeforePosition);
                hit = Physics2D.Raycast(ray.origin, ray.direction, Vector2.Distance(BeforePosition, CursorPosition), LayerMask.GetMask("Obstacle"));

                if (hit.collider)
                {
                    MoveFlg = false;
                    foreach (LineRenderer UnitLine in BaManager.SelectUnitLine)
                    {
                        DrawMoveLine(UnitLine, hit.point, Color.red);
                    }
                }
                else
                {
                    foreach (LineRenderer UnitLine in BaManager.SelectUnitLine)
                    {
                        DrawMoveLine(UnitLine, CursorPosition, Color.yellow);
                    }
                }
            }

            //��Q�����Ȃ��ꍇ�A�ړ����[�g�w���
            if (MoveFlg)
            {
                //���N���b�N�ňړ����[�g�ꎞ����A����UI��\��
                if (Input.GetMouseButtonUp(0))
                {
                    //�N���b�N�ʒu��ێ����Ă���
                    LastPosition = CursorPosition;
                    
                    //�ŏI�ړ��n�_�Ƀ��j�b�g������ꍇ�A�ړ��Ώۂ����̃��j�b�g���I��������
                    var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerUnit", "EnemyUnit"));
                    if (col != null && !BaManager.SelectUnit.Contains(col.gameObject))
                    {
                        ChaseTarget = col.gameObject;
                        ChaseButton.interactable = true;

                        //�ړ��Ώۃ��j�b�g�̌����ڂ�ύX
                        ChaseTarget.GetComponent<SpriteRenderer>().material = BaManager.SelectMaterial2;
                    }
                    else
                    {
                        ChaseTarget = null;
                        ChaseButton.interactable = false;

                        foreach (GameObject Unit in BaManager.SelectUnit)
                        {
                            //�ړ��n�_��\��
                            GameObject movepoint = Instantiate(BaManager.ActionUI.GetComponent<ActionUI>().MovePoint, CursorPosition, Quaternion.identity);
                            movepoint.transform.parent = Unit.transform;
                        }
                    }

                    //�ړ��ŏI����pUI�\���ʒu����
                    Vector2 UIPosition = RectTransformUtility.WorldToScreenPoint(CinemachineCore.Instance.FindPotentialTargetBrain(mainCam).OutputCamera, CursorPosition);
                   
                    if (UIPosition.x > 1600)
                    {
                        UIPosition.x -= 200;
                    }
                    if (UIPosition.y > 900)
                    {
                        UIPosition.y -= 100;
                    }
                    if (UIPosition.y < 100)
                    {
                        UIPosition.y += 100;
                    }

                    //�f�t�H���g�͈ړ��n�_�̏����E�ɕ\��������
                    MoveUIAfter.GetComponent<RectTransform>().position = UIPosition + new Vector2(100, 0);

                    //�ړ��ŏI����pUI��\��
                    BaManager.currentMode = BattleManager.Mode.MoveDecisionAfter;
                    MoveUIAfter.SetActive(true);
                }

                //�E�N���b�N���������łȂ��ꍇ�A���p�_�w��@���J�����ʒu�ύX�Ƃ̍��ʉ�
                if (Input.GetMouseButton(1))
                {
                    ClickTime += Time.unscaledDeltaTime;
                }
                if (Input.GetMouseButtonUp(1))
                {
                    if(ClickTime < 0.2)
                    {
                        foreach (LineRenderer UnitLine in BaManager.SelectUnitLine)
                        {
                            UnitLine.positionCount++;
                            UnitLine.SetPosition(UnitLine.positionCount - 1, CursorPosition);
                        }

                        MovePositions.Add(CursorPosition);
                    }
                    ClickTime = 0;
                }
            }
        }
    }

    //�ړ����[�g�`��
    void DrawMoveLine(LineRenderer line, Vector3 LinePosition, Color LineColor)
    {
        line.SetPosition(line.positionCount - 1, LinePosition);
        line.material.color = LineColor;
    }

    //�ړ�����
    public void DecisionMove()
    {
        MovePositions.Add(LastPosition);

        //�e���j�b�g��targetPlace��MovePositions(�w�肵�����[�g)������
        foreach (GameObject Unit in BaManager.SelectUnit)
        {
            UnitAction UnitA = Unit.GetComponent<UnitAction>();

            //�����O�̃N���A
            UnitA.targetPlace.Clear();
            UnitA.targetUnit = null;

            foreach (Vector3 pos in MovePositions)
            {
                UnitA.targetPlace.Add(pos);
            }

            //�I����ԉ���
            Unit.GetComponent<SpriteRenderer>().material = BaManager.NoSelectMaterial;
        }

        //�ړ����[�g�w���A�I��
        BaManager.SelectUnit.Clear();
        this.gameObject.SetActive(false);
    }

    //�ǂ�
    public void DecisionChase()
    {
        //�e���j�b�g��targetPlace��MovePositions(�w�肵�����[�g)������
        foreach (GameObject Unit in BaManager.SelectUnit)
        {
            UnitAction UnitA = Unit.GetComponent<UnitAction>();

            //�����O�̃N���A
            UnitA.targetPlace.Clear();

            foreach (Vector3 pos in MovePositions)
            {
                UnitA.targetPlace.Add(pos);
            }

            //�ړ��ڕW�̃��j�b�g������
            UnitA.targetUnit = ChaseTarget.transform;

            //�I����ԉ���
            Unit.GetComponent<SpriteRenderer>().material = BaManager.NoSelectMaterial;
            ChaseTarget.GetComponent<SpriteRenderer>().material = BaManager.NoSelectMaterial;
        }

        //�ړ����[�g�w���A�I��
        BaManager.SelectUnit.Clear();
        this.gameObject.SetActive(false);
    }


    //�ړ��L�����Z��
    public void Cancel()
    {
        this.gameObject.SetActive(false);
    }
}
