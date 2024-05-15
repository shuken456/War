using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using UnityEngine.UI;

public class MoveUI : MonoBehaviour
{
    public BattleManager BaManager;

    public enum Mode
    {
        MoveDecisionBefore, //�ړ����[�g����O
        MoveDecisionAfter //�ړ����[�g�����
    }

    //���݂̃��[�h
    public Mode currentMode = Mode.MoveDecisionBefore;

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
        //�I�𒆂̕��m�̈ړ�����V���ɍ쐬����
        foreach (GameObject Fighter in BaManager.SelectFighter)
        {
            if (Fighter.transform.Find("Line(Clone)"))
            {
                Destroy(Fighter.transform.Find("Line(Clone)").gameObject);

                if (Fighter.transform.Find("MovePoint(Clone)"))
                {
                    Destroy(Fighter.transform.Find("MovePoint(Clone)").gameObject);
                }
            }

            //�ړ��������q�v�f�ɂ���
            LineRenderer moveline = Instantiate(BaManager.ActionUI.GetComponent<ActionUI>().MoveLine, Fighter.transform.position, Quaternion.identity).GetComponent<LineRenderer>();
            moveline.gameObject.transform.parent = Fighter.transform;
            moveline.SetPosition(0, Fighter.transform.position);
            moveline.SetPosition(1, Fighter.transform.position);
            BaManager.SelectFighterLine.Add(moveline);
        }
    }

    private void OnDisable()
    {
        //�ړ��^�[�Q�b�g���w�肵�Ă����ꍇ�����ڂ�߂�
        if (ChaseTarget != null)
        {
            ChaseTarget.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.clear;
        }

        MoveUIAfter.SetActive(false);
        MovePositions.Clear();
        BaManager.SelectFighterLine.Clear();
        currentMode = Mode.MoveDecisionBefore;
        BaManager.ActionUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //���[�h�ɂ���ď�����ς���
        switch (currentMode)
        {
            case Mode.MoveDecisionBefore:
                UpdateMoveDecisionBeforeMode();
                break;

            case Mode.MoveDecisionAfter:
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
            foreach (LineRenderer FighterLine in BaManager.SelectFighterLine)
            {
                FighterLine.gameObject.SetActive(false);
            }
        }
        else
        {
            //�ړ�����\��
            foreach (LineRenderer FighterLine in BaManager.SelectFighterLine)
            {
                FighterLine.gameObject.SetActive(true);
            }

            //��ڂ̃��[�g�w�莞
            if (MovePositions.Count == 0)
            {
                //���ꂼ��̑I�𒆕��m����̃��[�g�ɏ�Q�����Ȃ����m�F
                for (int i = 0; i <= BaManager.SelectFighter.Count - 1; i++)
                {
                    Vector3 SelectFighterPosition = BaManager.SelectFighter[i].transform.position;
                    ray = new Ray2D(SelectFighterPosition, CursorPosition - SelectFighterPosition);
                    hit = Physics2D.Raycast(ray.origin, ray.direction, Vector2.Distance(SelectFighterPosition, CursorPosition), LayerMask.GetMask("Obstacle"));

                    if (hit.collider)
                    {
                        //��ł��ړ��s�̃��[�g������Έړ������s�Ƃ���
                        MoveFlg = false;
                        DrawMoveLine(BaManager.SelectFighterLine[i], hit.point, Color.red);
                    }
                    else
                    {
                        DrawMoveLine(BaManager.SelectFighterLine[i], CursorPosition, Color.cyan);
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
                    foreach (LineRenderer FighterLine in BaManager.SelectFighterLine)
                    {
                        DrawMoveLine(FighterLine, hit.point, Color.red);
                    }
                }
                else
                {
                    foreach (LineRenderer FighterLine in BaManager.SelectFighterLine)
                    {
                        DrawMoveLine(FighterLine, CursorPosition, Color.cyan);
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

                    //�ŏI�ړ��n�_�ɕ��m������ꍇ�A�ړ��Ώۂ����̕��m���I��������
                    var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter", "EnemyFighter"));
                    if (col != null && !BaManager.SelectFighter.Contains(col.gameObject))
                    {
                        ChaseTarget = col.gameObject;
                        ChaseButton.interactable = true;

                        //�ړ��Ώە��m�̌����ڂ�ύX
                        ChaseTarget.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.green;
                    }
                    else
                    {
                        ChaseTarget = null;
                        ChaseButton.interactable = false;

                        foreach (GameObject Fighter in BaManager.SelectFighter)
                        {
                            //�ړ��n�_��\��
                            GameObject movepoint = Instantiate(BaManager.ActionUI.GetComponent<ActionUI>().MovePoint, CursorPosition, Quaternion.identity);
                            movepoint.transform.parent = Fighter.transform;
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
                    currentMode = Mode.MoveDecisionAfter;
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
                        foreach (LineRenderer FighterLine in BaManager.SelectFighterLine)
                        {
                            FighterLine.positionCount++;
                            FighterLine.SetPosition(FighterLine.positionCount - 1, CursorPosition);
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

        //�e���m��targetPlace��MovePositions(�w�肵�����[�g)������
        foreach (GameObject Fighter in BaManager.SelectFighter)
        {
            FighterAction FighterA = Fighter.GetComponent<FighterAction>();

            //�����O�̃N���A
            FighterA.targetPlace.Clear();
            FighterA.targetFighter = null;
            FighterA.targetFighterSave = null;

            foreach (Vector3 pos in MovePositions)
            {
                FighterA.targetPlace.Add(pos);
            }

            //�I����ԉ���
            Fighter.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.clear;
        }

        //�ړ����[�g�w���A�I��
        BaManager.SelectFighter.Clear();
        this.gameObject.SetActive(false);
    }

    //�ǂ�
    public void DecisionChase()
    {
        //�e���m��targetPlace��MovePositions(�w�肵�����[�g)������
        foreach (GameObject Fighter in BaManager.SelectFighter)
        {
            FighterAction FighterA = Fighter.GetComponent<FighterAction>();

            //�����O�̃N���A
            FighterA.targetPlace.Clear();

            foreach (Vector3 pos in MovePositions)
            {
                FighterA.targetPlace.Add(pos);
            }

            //�ړ��ڕW�̕��m������
            FighterA.targetFighter = ChaseTarget.transform;

            //�I����ԉ���
            Fighter.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.clear;
        }

        //�ړ����[�g�w���A�I��
        BaManager.SelectFighter.Clear();
        this.gameObject.SetActive(false);
    }


    //�ړ��L�����Z��
    public void Cancel()
    {
        this.gameObject.SetActive(false);
    }
}
