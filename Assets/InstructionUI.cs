using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionUI : MonoBehaviour
{
    public BattleManager BaManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BaManager.SelectFighter.Clear();
        }

        //左クリックで兵士選択
        if (Input.GetMouseButton(0))
        {
            Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter"));
            if (col != null && !BaManager.SelectFighter.Contains(col.gameObject))
            {
                BaManager.SelectFighter.Add(col.gameObject);

                //選択中の見た目を変更
                col.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }

        //アクションUIを開いて待機
        if (Input.GetMouseButtonUp(0) && BaManager.SelectFighter.Count > 0)
        {
            Time.timeScale = 0;
            BaManager.ActionUI.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    public void InstractionButtonClick()
    {
        Time.timeScale = 0;
        BaManager.ActionUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
