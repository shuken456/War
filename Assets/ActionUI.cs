using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUI : MonoBehaviour
{

    public BattleManager BaManager;

    //移動ボタン
    public Button MoveButton;

    //移動ルートを表示する線
    public GameObject MoveLine;

    // Start is called before the first frame update
    private void OnEnable()
    {
        //既に移動ルートが設定されているユニットが選択された場合、そのルートを表示
        for (int i = 0; i <= BaManager.SelectUnit.Count - 1; i++)
        {
            Transform LineObject = BaManager.SelectUnit[i].transform.Find("Line(Clone)");
            if (LineObject)
            {
                LineObject.gameObject.SetActive(true);
                LineObject.gameObject.GetComponent<LineRenderer>().SetPosition(0, BaManager.SelectUnit[i].transform.position);
            }
        }

        if(BaManager.SelectUnit.Count == 0)
        {
            MoveButton.interactable = false;
        }
        else
        {
            MoveButton.interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //移動モードへ
    public void MoveMode()
    {
        for (int i = 0; i <= BaManager.SelectUnit.Count - 1; i++)
        {
            Transform LineObject = BaManager.SelectUnit[i].transform.Find("Line(Clone)");

            if (!LineObject)
            {
                //移動ルートが設定されていない場合、移動線を作り子要素にする
                GameObject moveline = Instantiate(MoveLine, new Vector3(0, 0, 0), Quaternion.identity);
                moveline.transform.parent = BaManager.SelectUnit[i].transform;
                moveline.GetComponent<LineRenderer>().SetPosition(0, BaManager.SelectUnit[i].transform.position);
            }
            else
            {
                //既に移動ルートが設定されている場合、ルート線の見た目を初期化する
                LineObject.gameObject.GetComponent<LineRenderer>().SetPosition(0, BaManager.SelectUnit[i].transform.position);
                LineObject.gameObject.GetComponent<LineRenderer>().positionCount = 2;
            }

            BaManager.SelectUnitLine.Add(BaManager.SelectUnit[i].transform.Find("Line(Clone)").gameObject.GetComponent<LineRenderer>());
        }

        //移動モードに変更
        BaManager.currentMode = BattleManager.Mode.Move;
        this.gameObject.SetActive(false);
        BaManager.MoveUI.SetActive(true);
    }

    //再開
    public void ReStart()
    {
        //選択解除
        for (int i = 0; i <= BaManager.SelectUnit.Count - 1; i++)
        {
            BaManager.SelectUnit[i].GetComponent<SpriteRenderer>().material = BaManager.NoSelectMaterial;

            //移動ルートを非表示
            Transform LineObject = BaManager.SelectUnit[i].transform.Find("Line(Clone)");
            if (LineObject)
            {
                LineObject.gameObject.SetActive(false);
            }
        }

        BaManager.SelectUnit.Clear();
        BaManager.SelectUnitLine.Clear();
        Time.timeScale = 1;
        BaManager.currentMode = BattleManager.Mode.Default;
        this.gameObject.SetActive(false);
        BaManager.InstructionButton.gameObject.SetActive(true);
    }
}
