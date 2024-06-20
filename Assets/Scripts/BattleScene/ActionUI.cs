using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ActionUI : MonoBehaviour
{
    public BattleManager BaManager;

    //移動ボタン
    public Button MoveButton;

    //待機ボタン
    public Button WaitButton;

    //部隊出撃ボタン
    public Button SortieButton;

    //移動ルートを表示する線
    public GameObject MoveLine;

    //到着点
    public GameObject MovePoint;

    //兵士一覧
    public GameObject SortieFighterView;

    //一回のクリック中にその兵士が選択されたか否かを管理
    private List<GameObject> OneClickSelectFighter = new List<GameObject>();
    private List<GameObject> OneClickNoSelectFighter = new List<GameObject>();

    //クリック長押し時間カウント用
    private float ClickTime = 0;

    private void OnEnable()
    {
        ChangeButton();
        DeleteMoveRoute();

        Time.timeScale = 0;
        BaManager.TimeStopText.SetActive(true);

        //全味方兵士の移動ルートを表示
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
                    moveline.material.color = Color.cyan;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //左クリックで兵士選択、解除
        if (Input.GetMouseButton(0) && BaManager.StartFlg)
        {
            Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter"));
            if (col != null && !BaManager.SelectFighter.Contains(col.gameObject) && !OneClickNoSelectFighter.Contains(col.gameObject))
            {
                BaManager.ButtonSE.Play();
                BaManager.SelectFighter.Add(col.gameObject);
                OneClickSelectFighter.Add(col.gameObject);

                //選択兵士ビュー更新
                this.transform.Find("SelectFighterInfoUI").GetComponent<SelectFighterInfoUI>().UpdateView();

                //選択中の見た目を変更
                col.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.yellow;
            }

            if (col != null && BaManager.SelectFighter.Contains(col.gameObject) && !OneClickSelectFighter.Contains(col.gameObject))
            {
                BaManager.ButtonSE.Play();
                BaManager.SelectFighter.Remove(col.gameObject);
                OneClickNoSelectFighter.Add(col.gameObject);

                //選択兵士ビュー更新
                this.transform.Find("SelectFighterInfoUI").GetComponent<SelectFighterInfoUI>().UpdateView();

                //選択中の見た目を変更
                col.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.clear;
            }
            ChangeButton();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OneClickSelectFighter.Clear();
            OneClickNoSelectFighter.Clear();
        }

        //右クリックが長押しでない場合、兵士のステータスを表示
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

    //移動モードへ
    public void MoveMode()
    {
        //移動モードに変更
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
            FighterA.targetFighterSave = null;

            //選択状態解除
            Fighter.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.clear;
        }

        BaManager.SelectFighter.Clear();
        OnEnable();
    }

    public void ChangeButton()
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

        //出撃可能部隊数が0なら部隊出撃ボタンを押せないように
        if (BaManager.UnitCountUI.PossibleSortieCountNow == 0)
        {
            SortieButton.interactable = false;
        }
        else
        {
            SortieButton.interactable = true;
        }
    }

    public void DeleteMoveRoute()
    {
        //全味方兵士の移動ルートを削除
        GameObject[] tagObjects = GameObject.FindGameObjectsWithTag("MoveObject");

        foreach (GameObject MoveObject in tagObjects)
        {
            Destroy(MoveObject);
        }
    }

    //再開
    public void ReStart()
    {
        DeleteMoveRoute();

        //それぞれの変数と画面状態を元に戻す
        foreach (GameObject Fighter in BaManager.SelectFighter)
        {
            Fighter.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.clear;
        }
        BaManager.SelectFighter.Clear();
        BaManager.SelectFighterLine.Clear();
        BaManager.InstructionButton.gameObject.SetActive(true);
        Time.timeScale = 1;
        BaManager.TimeStopText.SetActive(false);
        this.gameObject.SetActive(false);
    }

    //出撃
    public void Sortie()
    {
        //バトルシーンを非アクティブ化して保持したままユニット一覧を呼び出す
        DeleteMoveRoute();
        foreach (var root in gameObject.scene.GetRootGameObjects())
        {
            root.SetActive(false);
        }

        Common.SortieMode = true;
        SceneManager.LoadSceneAsync ("UnitEditScene",LoadSceneMode.Additive);
    }

    //兵士一覧画面を開く
    public void Search()
    {
        SortieFighterView.SetActive(true);
    }
}
