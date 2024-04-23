using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public enum Mode
    {
        Default, //�f�t�H���g
        Instruction, //��p�w���҂�
        Move //�ړ�
    }

    //���݂̃��[�h
    public Mode currentMode = Mode.Default;

    //�eUI
    public GameObject ActionUI;
    public GameObject MoveUI;

    public Button InstructionButton;

    //�I�𒆃��j�b�g�̃A�E�g���C���}�e���A��
    public Material SelectMaterial;
    public Material NoSelectMaterial;

    //�I�𒆂̃��j�b�g
    public List<GameObject> SelectUnit = new List<GameObject>();
    public List<LineRenderer> SelectUnitLine = new List<LineRenderer>();

    //�ړ����[�g
    private List<Vector3> MovePositions = new List<Vector3>();
    
    // Start is called before the first frame update
    void Start()
    {
        InstructionButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //���[�h�ɂ���ď�����ς���
        switch (currentMode)
        {
            case Mode.Default:
                UpdateDefaultMode();
                break;
        }

        switch (currentMode)
        {
            case Mode.Instruction:
                UpdateInstructionMode();
                break;
        }

        switch (currentMode)
        {
            case Mode.Move:
                UpdateMoveMode();
                break;
        }
    }

    //�s�����閡�����j�b�g��I���ł�����
    void UpdateDefaultMode()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectUnit.Clear();
        }

        //���N���b�N�Ń��j�b�g�I��
        if (Input.GetMouseButton(0))
        {
            Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerUnit"));
            if (col != null && !SelectUnit.Contains(col.gameObject))
            {
                SelectUnit.Add(col.gameObject);

                //�I�𒆂̌����ڂ�ύX
                col.gameObject.GetComponent<SpriteRenderer>().material = SelectMaterial;
            }
        }

        //�A�N�V����UI���J���đҋ@
        if (Input.GetMouseButtonUp(0) && SelectUnit.Count > 0)
        {
            Time.timeScale = 0;
            InstructionButton.gameObject.SetActive(false);
            ActionUI.SetActive(true);
            currentMode = Mode.Instruction;
        }
    }

    //��p�w���{�^��
    public void InstructionMode()
    {
        Time.timeScale = 0;
        InstructionButton.gameObject.SetActive(false);
        ActionUI.SetActive(true);
        currentMode = Mode.Instruction;
    }

    //�w���҂�
    void UpdateInstructionMode()
    {
        Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

        //���N���b�N�Ń��j�b�g�I��
        if (Input.GetMouseButton(0))
        {
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerUnit"));
            if (col != null && !SelectUnit.Contains(col.gameObject))
            {
                SelectUnit.Add(col.gameObject);

                //�I�𒆂̌����ڂ�ύX
                col.gameObject.GetComponent<SpriteRenderer>().material = SelectMaterial;
            }
        }

        //�A�N�V����UI���J���đҋ@
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            ActionUI.SetActive(false);
            ActionUI.SetActive(true);
            currentMode = Mode.Instruction;
        }
    }

    //�ړ����[�g�\��
    void DrawMoveLine(LineRenderer line,Vector3 LinePosition,Color LineColor)
    {
        line.SetPosition(line.positionCount - 1, LinePosition);
        line.material.color = LineColor;
    }

    //�ړ����[�h
    void UpdateMoveMode()
    {
        Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

        //�ړ��\���ǂ���
        bool MoveFlg = true;

        //�ړ����[�g�`�F�b�N�pray
        Ray2D ray;
        RaycastHit2D hit;

        //��ڂ̃��[�g�w�莞
        if (MovePositions.Count == 0)
        {
            //�I�𒆂̃��j�b�g����̃��[�g�ɏ�Q�����Ȃ����m�F
            for (int i = 0; i <= SelectUnit.Count - 1; i++)
            {
                Vector3 SelectUnitPosition = SelectUnit[i].transform.position;
                ray = new Ray2D(SelectUnitPosition, CursorPosition - SelectUnitPosition);
                hit = Physics2D.Raycast(ray.origin, ray.direction, Vector2.Distance(SelectUnitPosition, CursorPosition), LayerMask.GetMask("Obstacle"));

                if (hit.collider)
                {
                    MoveFlg = false;
                    DrawMoveLine(SelectUnitLine[i], hit.point, Color.red);
                }
                else
                {
                    DrawMoveLine(SelectUnitLine[i], CursorPosition, Color.yellow);
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
                for (int i = 0; i <= SelectUnit.Count - 1; i++)
                {
                    DrawMoveLine(SelectUnitLine[i], hit.point, Color.red);
                }
            }
            else
            {
                for (int i = 0; i <= SelectUnit.Count - 1; i++)
                {
                    DrawMoveLine(SelectUnitLine[i], CursorPosition, Color.yellow);
                }
            }
        }

        //��Q�����Ȃ��ꍇ�A�ړ����[�g�w���
        if (MoveFlg) //&&
        {
            //���N���b�N�ňړ����[�g����
            if (Input.GetMouseButtonDown(0))
            {
                MovePositions.Add(CursorPosition);

                //�e���j�b�g��targetPlace��MovePositions(�w�肵�����[�g)������
                for (int i = 0; i <= SelectUnit.Count - 1; i++)
                {
                    SelectUnitLine[i].gameObject.SetActive(false);
                    SelectUnit[i].GetComponent<SpriteRenderer>().material = NoSelectMaterial;
                    SelectUnit[i].GetComponent<Infantry_Sword>().targetPlace.Clear(); //�����O�̃N���A

                    for (int j = 0; j <= MovePositions.Count - 1; j++)
                    {
                        SelectUnit[i].GetComponent<Infantry_Sword>().targetPlace.Add(MovePositions[j]);
                    }

                }

                //�ړ����[�g�w���A���ꂼ��̕ϐ��Ɖ�ʏ�Ԃ����ɖ߂�
                MovePositions.Clear();
                SelectUnit.Clear();
                SelectUnitLine.Clear();

                MoveUI.SetActive(false);
                ActionUI.SetActive(false);
                ActionUI.SetActive(true);
                currentMode = Mode.Instruction;
            }

            //�X�y�[�X�L�[�Œ��p�_�w��
            if (Input.GetKeyDown(KeyCode.Space))
            {
                for (int i = 0; i <= SelectUnit.Count - 1; i++)
                {
                    SelectUnitLine[i].positionCount++;
                    SelectUnitLine[i].SetPosition(SelectUnitLine[i].positionCount - 1, CursorPosition);
                }

                MovePositions.Add(CursorPosition);
            }
        }
    }

}
