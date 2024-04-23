using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public enum Mode
    {
        Default, //デフォルト
        Instruction, //戦術指示待ち
        Move //移動
    }

    //現在のモード
    public Mode currentMode = Mode.Default;

    //各UI
    public GameObject ActionUI;
    public GameObject MoveUI;

    public Button InstructionButton;

    //選択中ユニットのアウトラインマテリアル
    public Material SelectMaterial;
    public Material NoSelectMaterial;

    //選択中のユニット
    public List<GameObject> SelectUnit = new List<GameObject>();
    public List<LineRenderer> SelectUnitLine = new List<LineRenderer>();

    //移動ルート
    private List<Vector3> MovePositions = new List<Vector3>();
    
    // Start is called before the first frame update
    void Start()
    {
        InstructionButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //モードによって処理を変える
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

    //行動する味方ユニットを選択できる状態
    void UpdateDefaultMode()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectUnit.Clear();
        }

        //左クリックでユニット選択
        if (Input.GetMouseButton(0))
        {
            Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerUnit"));
            if (col != null && !SelectUnit.Contains(col.gameObject))
            {
                SelectUnit.Add(col.gameObject);

                //選択中の見た目を変更
                col.gameObject.GetComponent<SpriteRenderer>().material = SelectMaterial;
            }
        }

        //アクションUIを開いて待機
        if (Input.GetMouseButtonUp(0) && SelectUnit.Count > 0)
        {
            Time.timeScale = 0;
            InstructionButton.gameObject.SetActive(false);
            ActionUI.SetActive(true);
            currentMode = Mode.Instruction;
        }
    }

    //戦術指示ボタン
    public void InstructionMode()
    {
        Time.timeScale = 0;
        InstructionButton.gameObject.SetActive(false);
        ActionUI.SetActive(true);
        currentMode = Mode.Instruction;
    }

    //指示待ち
    void UpdateInstructionMode()
    {
        Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

        //左クリックでユニット選択
        if (Input.GetMouseButton(0))
        {
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerUnit"));
            if (col != null && !SelectUnit.Contains(col.gameObject))
            {
                SelectUnit.Add(col.gameObject);

                //選択中の見た目を変更
                col.gameObject.GetComponent<SpriteRenderer>().material = SelectMaterial;
            }
        }

        //アクションUIを開いて待機
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            ActionUI.SetActive(false);
            ActionUI.SetActive(true);
            currentMode = Mode.Instruction;
        }
    }

    //移動ルート表示
    void DrawMoveLine(LineRenderer line,Vector3 LinePosition,Color LineColor)
    {
        line.SetPosition(line.positionCount - 1, LinePosition);
        line.material.color = LineColor;
    }

    //移動モード
    void UpdateMoveMode()
    {
        Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

        //移動可能かどうか
        bool MoveFlg = true;

        //移動ルートチェック用ray
        Ray2D ray;
        RaycastHit2D hit;

        //一つ目のルート指定時
        if (MovePositions.Count == 0)
        {
            //選択中のユニットからのルートに障害物がないか確認
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
            //中継点からのルートに障害物がないか確認
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

        //障害物がない場合、移動ルート指定可
        if (MoveFlg) //&&
        {
            //左クリックで移動ルート決定
            if (Input.GetMouseButtonDown(0))
            {
                MovePositions.Add(CursorPosition);

                //各ユニットのtargetPlaceにMovePositions(指定したルート)を入れる
                for (int i = 0; i <= SelectUnit.Count - 1; i++)
                {
                    SelectUnitLine[i].gameObject.SetActive(false);
                    SelectUnit[i].GetComponent<SpriteRenderer>().material = NoSelectMaterial;
                    SelectUnit[i].GetComponent<Infantry_Sword>().targetPlace.Clear(); //入れる前のクリア

                    for (int j = 0; j <= MovePositions.Count - 1; j++)
                    {
                        SelectUnit[i].GetComponent<Infantry_Sword>().targetPlace.Add(MovePositions[j]);
                    }

                }

                //移動ルート指定後、それぞれの変数と画面状態を元に戻す
                MovePositions.Clear();
                SelectUnit.Clear();
                SelectUnitLine.Clear();

                MoveUI.SetActive(false);
                ActionUI.SetActive(false);
                ActionUI.SetActive(true);
                currentMode = Mode.Instruction;
            }

            //スペースキーで中継点指定
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
