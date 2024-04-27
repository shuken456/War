using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleManager : MonoBehaviour
{
    public enum Mode
    {
        Default, //デフォルト
        Instruction, //戦術指示待ち
        MoveDecisionBefore, //移動ルート決定前
        MoveDecisionAfter, //移動ルート決定後
    }

    //現在のモード
    public Mode currentMode = Mode.Default;

    //各UI
    public GameObject ActionUI;
    public GameObject MoveUI;

    public Button InstructionButton;

    //選択中ユニットのアウトラインマテリアル
    public Material SelectMaterial;
    public Material SelectMaterial2;
    public Material NoSelectMaterial;

    //選択中のユニット
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
        //モードによって処理を変える
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
        
    }
}
