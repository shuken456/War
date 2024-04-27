using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using UnityEngine.UI;

public class MoveUI : MonoBehaviour
{
    public BattleManager BaManager;

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
        //選択中のユニットの移動線を新たに作成する
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

            //移動線を作り子要素にする
            LineRenderer moveline = Instantiate(BaManager.ActionUI.GetComponent<ActionUI>().MoveLine, Unit.transform.position, Quaternion.identity).GetComponent<LineRenderer>();
            moveline.gameObject.transform.parent = Unit.transform;
            moveline.SetPosition(0, Unit.transform.position);
            moveline.SetPosition(1, Unit.transform.position);
            BaManager.SelectUnitLine.Add(moveline);
        }
    }

    private void OnDisable()
    {
        //アクションUIが開かれるため、移動線を一括削除する
        BaManager.ActionUI.GetComponent<ActionUI>().DeleteMoveRoute();

        //それぞれの変数と画面状態を元に戻す
        MoveUIAfter.SetActive(false);
        MovePositions.Clear();
        BaManager.SelectUnitLine.Clear();
        BaManager.currentMode = BattleManager.Mode.Instruction;
        BaManager.ActionUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //モードによって処理を変える
        switch (BaManager.currentMode)
        {
            case BattleManager.Mode.MoveDecisionBefore:
                UpdateMoveDecisionBeforeMode();
                break;

            case BattleManager.Mode.MoveDecisionAfter:
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
            foreach (LineRenderer UnitLine in BaManager.SelectUnitLine)
            {
                UnitLine.gameObject.SetActive(false);
            }
        }
        else
        {
            //移動線非表示
            foreach (LineRenderer UnitLine in BaManager.SelectUnitLine)
            {
                UnitLine.gameObject.SetActive(true);
            }

            //一つ目のルート指定時
            if (MovePositions.Count == 0)
            {
                //それぞれの選択中ユニットからのルートに障害物がないか確認
                for (int i = 0; i <= BaManager.SelectUnit.Count - 1; i++)
                {
                    Vector3 SelectUnitPosition = BaManager.SelectUnit[i].transform.position;
                    ray = new Ray2D(SelectUnitPosition, CursorPosition - SelectUnitPosition);
                    hit = Physics2D.Raycast(ray.origin, ray.direction, Vector2.Distance(SelectUnitPosition, CursorPosition), LayerMask.GetMask("Obstacle"));

                    if (hit.collider)
                    {
                        //一つでも移動不可のルートがあれば移動処理不可とする
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
                //中継点からのルートに障害物がないか確認
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

            //障害物がない場合、移動ルート指定可
            if (MoveFlg)
            {
                //左クリックで移動ルート一時決定、決定UIを表示
                if (Input.GetMouseButtonUp(0))
                {
                    //クリック位置を保持しておく
                    LastPosition = CursorPosition;
                    
                    //最終移動地点にユニットがいる場合、移動対象をそのユニットか選択させる
                    var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerUnit", "EnemyUnit"));
                    if (col != null && !BaManager.SelectUnit.Contains(col.gameObject))
                    {
                        ChaseTarget = col.gameObject;
                        ChaseButton.interactable = true;

                        //移動対象ユニットの見た目を変更
                        ChaseTarget.GetComponent<SpriteRenderer>().material = BaManager.SelectMaterial2;
                    }
                    else
                    {
                        ChaseTarget = null;
                        ChaseButton.interactable = false;

                        foreach (GameObject Unit in BaManager.SelectUnit)
                        {
                            //移動地点を表示
                            GameObject movepoint = Instantiate(BaManager.ActionUI.GetComponent<ActionUI>().MovePoint, CursorPosition, Quaternion.identity);
                            movepoint.transform.parent = Unit.transform;
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
                    BaManager.currentMode = BattleManager.Mode.MoveDecisionAfter;
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

        //各ユニットのtargetPlaceにMovePositions(指定したルート)を入れる
        foreach (GameObject Unit in BaManager.SelectUnit)
        {
            UnitAction UnitA = Unit.GetComponent<UnitAction>();

            //入れる前のクリア
            UnitA.targetPlace.Clear();
            UnitA.targetUnit = null;

            foreach (Vector3 pos in MovePositions)
            {
                UnitA.targetPlace.Add(pos);
            }

            //選択状態解除
            Unit.GetComponent<SpriteRenderer>().material = BaManager.NoSelectMaterial;
        }

        //移動ルート指定後、終了
        BaManager.SelectUnit.Clear();
        this.gameObject.SetActive(false);
    }

    //追う
    public void DecisionChase()
    {
        //各ユニットのtargetPlaceにMovePositions(指定したルート)を入れる
        foreach (GameObject Unit in BaManager.SelectUnit)
        {
            UnitAction UnitA = Unit.GetComponent<UnitAction>();

            //入れる前のクリア
            UnitA.targetPlace.Clear();

            foreach (Vector3 pos in MovePositions)
            {
                UnitA.targetPlace.Add(pos);
            }

            //移動目標のユニットを入れる
            UnitA.targetUnit = ChaseTarget.transform;

            //選択状態解除
            Unit.GetComponent<SpriteRenderer>().material = BaManager.NoSelectMaterial;
            ChaseTarget.GetComponent<SpriteRenderer>().material = BaManager.NoSelectMaterial;
        }

        //移動ルート指定後、終了
        BaManager.SelectUnit.Clear();
        this.gameObject.SetActive(false);
    }


    //移動キャンセル
    public void Cancel()
    {
        this.gameObject.SetActive(false);
    }
}
