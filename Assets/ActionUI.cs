using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUI : MonoBehaviour
{
    public BattleManager BaManager;

    //移動ボタン
    public Button MoveButton;

    //待機ボタン
    public Button WaitButton;

    //移動ルートを表示する線
    public GameObject MoveLine;

    //到着点
    public GameObject MovePoint;

    //一回のクリック中にそのユニットが選択されたか否かを管理
    private List<GameObject> OneClickSelectUnit = new List<GameObject>();
    private List<GameObject> OneClickNoSelectUnit = new List<GameObject>();


    // Start is called before the first frame update
    private void OnEnable()
    {
        ChangeButton();

        //全味方ユニットの移動ルートを表示
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

                //移動目標数分、折れ線にする
                for (int i = 0; i <= targetList.Count - 1; i++)
                {
                    moveline.positionCount++;
                    moveline.SetPosition(i + 1, targetList[i]);
                }

                //移動対象ユニットがいる場合、追加
                if (targetUnit)
                {
                    moveline.positionCount++;
                    moveline.SetPosition(moveline.positionCount - 1, targetUnit.position);
                    moveline.material.color = Color.green;
                }
                else
                {
                    //最終移動地点表示
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

        //左クリックでユニット選択、解除
        if (Input.GetMouseButton(0))
        {
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerUnit"));
            if (col != null && !BaManager.SelectUnit.Contains(col.gameObject) && !OneClickNoSelectUnit.Contains(col.gameObject))
            {
                BaManager.SelectUnit.Add(col.gameObject);
                OneClickSelectUnit.Add(col.gameObject);

                //選択中の見た目を変更
                col.gameObject.GetComponent<SpriteRenderer>().material = BaManager.SelectMaterial;
            }

            if (col != null && BaManager.SelectUnit.Contains(col.gameObject) && !OneClickSelectUnit.Contains(col.gameObject))
            {
                BaManager.SelectUnit.Remove(col.gameObject);
                OneClickNoSelectUnit.Add(col.gameObject);

                //選択中の見た目を変更
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

    //移動モードへ
    public void MoveMode()
    {
        //移動モードに変更
        BaManager.currentMode = BattleManager.Mode.MoveDecisionBefore;
        BaManager.MoveUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //選択中のユニットをその場で待機
    public void Wait()
    {
        foreach (GameObject Unit in BaManager.SelectUnit)
        {
            UnitAction UnitA = Unit.GetComponent<UnitAction>();
            //クリア
            UnitA.targetPlace.Clear();
            UnitA.targetUnit = null;
        }

        DeleteMoveRoute();
        OnEnable();
    }

    private void ChangeButton()
    {
        //選択中のユニットがいるか否かで使用可能な機能を変える
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
        //全味方ユニットの移動ルートを削除
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

    //再開
    public void ReStart()
    {
        DeleteMoveRoute();

        //それぞれの変数と画面状態を元に戻す
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
