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
        MoveDecisionBefore, //移動ルート決定前
        MoveDecisionAfter //移動ルート決定後
    }

    //現在のモード
    public Mode currentMode = Mode.MoveDecisionBefore;

    //移動ルート
    private List<Vector3> MovePositions = new List<Vector3>();

    //移動決定UI
    public GameObject MoveUIAfter;

    //追うボタン
    public Button ChaseButton;

    //追う対象
    private GameObject ChaseTarget;

    //移動決定UI
    public Canvas canvas;

    //メインカメラ
    public CinemachineVirtualCamera mainCam;

    //最終到着点
    private Vector3 LastPosition;

    //クリック長押し時間カウント用
    private float ClickTime = 0;

    private void OnEnable()
    {
        //選択中の兵士の移動線を新たに作成する
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

            //移動線を作り子要素にする
            LineRenderer moveline = Instantiate(BaManager.ActionUI.GetComponent<ActionUI>().MoveLine, Fighter.transform.position, Quaternion.identity).GetComponent<LineRenderer>();
            moveline.gameObject.transform.parent = Fighter.transform;
            moveline.SetPosition(0, Fighter.transform.position);
            moveline.SetPosition(1, Fighter.transform.position);
            BaManager.SelectFighterLine.Add(moveline);
        }
    }

    private void OnDisable()
    {
        //移動ターゲットを指定していた場合見た目を戻す
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
        //モードによって処理を変える
        switch (currentMode)
        {
            case Mode.MoveDecisionBefore:
                UpdateMoveDecisionBeforeMode();
                break;

            case Mode.MoveDecisionAfter:
                break;
        }
    }

    //移動ルート指定中
    void UpdateMoveDecisionBeforeMode()
    {
        //移動可能かどうか
        bool MoveFlg = true;

        //移動ルートチェック用ray
        Ray2D ray;
        RaycastHit2D hit;

        Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

        //カーソルがUI上にある場合、移動線非表示
        if (EventSystem.current.IsPointerOverGameObject())
        {
            foreach (LineRenderer FighterLine in BaManager.SelectFighterLine)
            {
                FighterLine.gameObject.SetActive(false);
            }
        }
        else
        {
            //移動線非表示
            foreach (LineRenderer FighterLine in BaManager.SelectFighterLine)
            {
                FighterLine.gameObject.SetActive(true);
            }

            //一つ目のルート指定時
            if (MovePositions.Count == 0)
            {
                //それぞれの選択中兵士からのルートに障害物がないか確認
                for (int i = 0; i <= BaManager.SelectFighter.Count - 1; i++)
                {
                    Vector3 SelectFighterPosition = BaManager.SelectFighter[i].transform.position;
                    ray = new Ray2D(SelectFighterPosition, CursorPosition - SelectFighterPosition);
                    hit = Physics2D.Raycast(ray.origin, ray.direction, Vector2.Distance(SelectFighterPosition, CursorPosition), LayerMask.GetMask("Obstacle"));

                    if (hit.collider)
                    {
                        //一つでも移動不可のルートがあれば移動処理不可とする
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
                //中継点からのルートに障害物がないか確認
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

            //障害物がない場合、移動ルート指定可
            if (MoveFlg)
            {
                //左クリックで移動ルート一時決定、決定UIを表示
                if (Input.GetMouseButtonUp(0))
                {
                    //クリック位置を保持しておく
                    LastPosition = CursorPosition;

                    //最終移動地点に兵士がいる場合、移動対象をその兵士か選択させる
                    var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter", "EnemyFighter"));
                    if (col != null && !BaManager.SelectFighter.Contains(col.gameObject))
                    {
                        ChaseTarget = col.gameObject;
                        ChaseButton.interactable = true;

                        //移動対象兵士の見た目を変更
                        ChaseTarget.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.green;
                    }
                    else
                    {
                        ChaseTarget = null;
                        ChaseButton.interactable = false;

                        foreach (GameObject Fighter in BaManager.SelectFighter)
                        {
                            //移動地点を表示
                            GameObject movepoint = Instantiate(BaManager.ActionUI.GetComponent<ActionUI>().MovePoint, CursorPosition, Quaternion.identity);
                            movepoint.transform.parent = Fighter.transform;
                        }
                    }

                    //移動最終決定用UI表示位置調整
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

                    //デフォルトは移動地点の少し右に表示させる
                    MoveUIAfter.GetComponent<RectTransform>().position = UIPosition + new Vector2(100, 0);

                    //移動最終決定用UIを表示
                    currentMode = Mode.MoveDecisionAfter;
                    MoveUIAfter.SetActive(true);
                }

                //右クリックが長押しでない場合、中継点指定　※カメラ位置変更との差別化
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

    //移動ルート描画
    void DrawMoveLine(LineRenderer line, Vector3 LinePosition, Color LineColor)
    {
        line.SetPosition(line.positionCount - 1, LinePosition);
        line.material.color = LineColor;
    }

    //移動決定
    public void DecisionMove()
    {
        MovePositions.Add(LastPosition);

        //各兵士のtargetPlaceにMovePositions(指定したルート)を入れる
        foreach (GameObject Fighter in BaManager.SelectFighter)
        {
            FighterAction FighterA = Fighter.GetComponent<FighterAction>();

            //入れる前のクリア
            FighterA.targetPlace.Clear();
            FighterA.targetFighter = null;
            FighterA.targetFighterSave = null;

            foreach (Vector3 pos in MovePositions)
            {
                FighterA.targetPlace.Add(pos);
            }

            //選択状態解除
            Fighter.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.clear;
        }

        //移動ルート指定後、終了
        BaManager.SelectFighter.Clear();
        this.gameObject.SetActive(false);
    }

    //追う
    public void DecisionChase()
    {
        //各兵士のtargetPlaceにMovePositions(指定したルート)を入れる
        foreach (GameObject Fighter in BaManager.SelectFighter)
        {
            FighterAction FighterA = Fighter.GetComponent<FighterAction>();

            //入れる前のクリア
            FighterA.targetPlace.Clear();

            foreach (Vector3 pos in MovePositions)
            {
                FighterA.targetPlace.Add(pos);
            }

            //移動目標の兵士を入れる
            FighterA.targetFighter = ChaseTarget.transform;

            //選択状態解除
            Fighter.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.clear;
        }

        //移動ルート指定後、終了
        BaManager.SelectFighter.Clear();
        this.gameObject.SetActive(false);
    }


    //移動キャンセル
    public void Cancel()
    {
        this.gameObject.SetActive(false);
    }
}
