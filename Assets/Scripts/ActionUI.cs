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

    //一回のクリック中にその兵士が選択されたか否かを管理
    private List<GameObject> OneClickSelectFighter = new List<GameObject>();
    private List<GameObject> OneClickNoSelectFighter = new List<GameObject>();


    // Start is called before the first frame update
    private void OnEnable()
    {
        ChangeButton();

        //全味方兵士の移動ルートを表示
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

                //移動目標数分、折れ線にする
                for (int i = 0; i <= targetList.Count - 1; i++)
                {
                    moveline.positionCount++;
                    moveline.SetPosition(i + 1, targetList[i]);
                }

                //移動対象兵士がいる場合、追加
                if (targetFighter)
                {
                    moveline.positionCount++;
                    moveline.SetPosition(moveline.positionCount - 1, targetFighter.position);
                    moveline.material.color = Color.green;
                }
                else
                {
                    //最終移動地点表示
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

        //左クリックで兵士選択、解除
        if (Input.GetMouseButton(0))
        {
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter", "SelectFighter"));
            if (col != null && !BaManager.SelectFighter.Contains(col.gameObject) && !OneClickNoSelectFighter.Contains(col.gameObject))
            {
                BaManager.SelectFighter.Add(col.gameObject);
                OneClickSelectFighter.Add(col.gameObject);

                //選択中の見た目を変更
                col.gameObject.layer = LayerMask.NameToLayer("SelectFighter");
            }

            if (col != null && BaManager.SelectFighter.Contains(col.gameObject) && !OneClickSelectFighter.Contains(col.gameObject))
            {
                BaManager.SelectFighter.Remove(col.gameObject);
                OneClickNoSelectFighter.Add(col.gameObject);

                //選択中の見た目を変更
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

    //移動モードへ
    public void MoveMode()
    {
        //移動モードに変更
        BaManager.currentMode = BattleManager.Mode.MoveDecisionBefore;
        BaManager.MoveUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //選択中の兵士をその場で待機
    public void Wait()
    {
        foreach (GameObject Fighter in BaManager.SelectFighter)
        {
            FighterAction FighterA = Fighter.GetComponent<FighterAction>();
            //クリア
            FighterA.targetPlace.Clear();
            FighterA.targetFighter = null;
        }

        DeleteMoveRoute();
        OnEnable();
    }

    private void ChangeButton()
    {
        //選択中の兵士がいるか否かで使用可能な機能を変える
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
        //全味方兵士の移動ルートを削除
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

    //再開
    public void ReStart()
    {
        DeleteMoveRoute();

        //それぞれの変数と画面状態を元に戻す
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
