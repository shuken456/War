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
    public Text Instruction;

    //選択中の兵士
    public List<GameObject> SelectFighter = new List<GameObject>();
    public List<LineRenderer> SelectFighterLine = new List<LineRenderer>();

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

    //行動する味方兵士を選択できる状態
    void UpdateDefaultMode()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectFighter.Clear();
        }

        //左クリックで兵士選択
        if (Input.GetMouseButton(0))
        {
            Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter"));
            if (col != null && !SelectFighter.Contains(col.gameObject))
            {
                SelectFighter.Add(col.gameObject);

                //選択中の見た目を変更
                col.gameObject.layer = LayerMask.NameToLayer("SelectFighter");
            }
        }

        //アクションUIを開いて待機
        if (Input.GetMouseButtonUp(0) && SelectFighter.Count > 0)
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
