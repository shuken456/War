using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleManager : MonoBehaviour
{
    public enum Mode
    {
        Default, //�f�t�H���g
        Instruction, //��p�w���҂�
        MoveDecisionBefore, //�ړ����[�g����O
        MoveDecisionAfter, //�ړ����[�g�����
    }

    //���݂̃��[�h
    public Mode currentMode = Mode.Default;

    //�eUI
    public GameObject ActionUI;
    public GameObject MoveUI;

    public Button InstructionButton;

    //�I�𒆃��j�b�g�̃A�E�g���C���}�e���A��
    public Material SelectMaterial;
    public Material SelectMaterial2;
    public Material NoSelectMaterial;

    //�I�𒆂̃��j�b�g
    public List<GameObject> SelectUnit = new List<GameObject>();
    public List<LineRenderer> SelectUnitLine = new List<LineRenderer>();

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

            case Mode.Instruction:
                UpdateInstructionMode();
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
        
    }
}
