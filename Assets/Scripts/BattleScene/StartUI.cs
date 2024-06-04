using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    public BattleManager BaManager;

    //��J�n�{�^��
    public Button GoButton;

    //�����o���{�^��
    public Button SortieButton;

    //�����Ґ��{�^��
    public Button EditUnitButton;

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

    //�N���b�N���������ԃJ�E���g�p
    private float ClickTime = 0;

    private void OnEnable()
    {
        ChangeButton();
        DeleteMoveRoute();
        Time.timeScale = 0;

        //�����̓s���̂��߁@�X�e�[�W1�ł͕Ґ��ł��Ȃ��悤��
        if(Common.Progress == 1)
        {
            EditUnitButton.interactable = false;
        }

        //�S�������m�̈ړ����[�g��\��
        GameObject[] tagObjects = GameObject.FindGameObjectsWithTag("PlayerFighter");

        foreach (GameObject Fighter in tagObjects)
        {
            var targetList = Fighter.GetComponent<FighterAction>().targetPlace;
            Transform targetFighter;

            if (Fighter.GetComponent<FighterAction>().targetFighter)
            {
                targetFighter = Fighter.GetComponent<FighterAction>().targetFighter;
            }
            else
            {
                targetFighter = Fighter.GetComponent<FighterAction>().targetFighterSave;
            }

            if (targetList.Count > 0 || targetFighter)
            {
                LineRenderer moveline = Instantiate(MoveLine, Fighter.transform.position, Quaternion.identity).GetComponent<LineRenderer>();
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
                    moveline.material.color = Color.cyan;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //���N���b�N�ŕ��m�I���A����
        if (Input.GetMouseButton(0))
        {
            Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter"));
            if (col != null && !BaManager.SelectFighter.Contains(col.gameObject) && !OneClickNoSelectFighter.Contains(col.gameObject))
            {
                BaManager.ButtonSE.Play();
                BaManager.SelectFighter.Add(col.gameObject);
                OneClickSelectFighter.Add(col.gameObject);

                //�I�𕺎m�r���[�X�V
                this.transform.Find("SelectFighterInfoUI").GetComponent<SelectFighterInfoUI>().UpdateView();

                //�I�𒆂̌����ڂ�ύX
                col.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.yellow;
            }

            if (col != null && BaManager.SelectFighter.Contains(col.gameObject) && !OneClickSelectFighter.Contains(col.gameObject))
            {
                BaManager.ButtonSE.Play();
                BaManager.SelectFighter.Remove(col.gameObject);
                OneClickNoSelectFighter.Add(col.gameObject);

                //�I�𕺎m�r���[�X�V
                this.transform.Find("SelectFighterInfoUI").GetComponent<SelectFighterInfoUI>().UpdateView();

                //�I�𒆂̌����ڂ�ύX
                col.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.clear;
            }
            ChangeButton();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OneClickSelectFighter.Clear();
            OneClickNoSelectFighter.Clear();
        }

        //�E�N���b�N���������łȂ��ꍇ�A���m�̃X�e�[�^�X��\��
        if (Input.GetMouseButton(1))
        {
            ClickTime += Time.unscaledDeltaTime;
        }
        if (Input.GetMouseButtonUp(1))
        {
            if (ClickTime < 0.2)
            {
                Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

                var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter", "EnemyFighter"));
                if (col != null)
                {
                    BaManager.ButtonSE.Play();
                    BaManager.FighterStatusInfoUI.SetActive(true);
                    BaManager.FighterStatusInfoUI.GetComponent<FighterStatusInfo>().TextWrite(col.GetComponent<FighterStatus>());
                    BaManager.FighterStatusInfoUI.GetComponent<FighterStatusInfo>().ImageWrite(col.GetComponent<SpriteRenderer>().sprite, col.GetComponent<SpriteRenderer>().color);
                    ClickTime = 0;
                }
            }
            ClickTime = 0;
        }
    }

    //�ړ����[�h��
    public void MoveMode()
    {
        //�ړ����[�h�ɕύX
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
            FighterA.targetFighterSave = null;
        }

        OnEnable();
    }

    public void ChangeButton()
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

        //�o���\��������0�Ȃ畔���o���{�^���������Ȃ��悤��
        if(BaManager.UnitCountUI.PossibleSortieCountNow == 0)
        {
            SortieButton.interactable = false;
        }
        else
        {
            SortieButton.interactable = true;
        }

        //�o�����Ă����J�n�{�^����������悤��
        if(BaManager.UnitCountUI.PossibleSortieCountNow < BaManager.UnitCountUI.PossibleSortieCountList[Common.Progress - 1])
        {
            GoButton.interactable = true;
        }
    }

    public void DeleteMoveRoute()
    {
        //�S�������m�̈ړ����[�g���폜
        GameObject[] tagObjects = GameObject.FindGameObjectsWithTag("MoveObject");

        foreach (GameObject MoveObject in tagObjects)
        {
            Destroy(MoveObject);
        }
    }

    //�����Ґ�
    public void UnitEdit()
    {
        //�o�g���V�[�����A�N�e�B�u�����ĕێ������܂܃��j�b�g�ꗗ���Ăяo��
        DeleteMoveRoute();
        foreach (var root in gameObject.scene.GetRootGameObjects())
        {
            root.SetActive(false);
        }

        Common.SortieMode = false;
        SceneManager.LoadSceneAsync("UnitEditScene", LoadSceneMode.Additive);
    }

    //�o��
    public void Sortie()
    {
        //�o�g���V�[�����A�N�e�B�u�����ĕێ������܂܃��j�b�g�ꗗ���Ăяo��
        DeleteMoveRoute();
        foreach (var root in gameObject.scene.GetRootGameObjects())
        {
            root.SetActive(false);
        }

        Common.SortieMode = true;
        SceneManager.LoadSceneAsync("UnitEditScene", LoadSceneMode.Additive);
    }
}
