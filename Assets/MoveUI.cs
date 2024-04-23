using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUI : MonoBehaviour
{
    public BattleManager BaManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cancel()
    {
        for (int i = 0; i <= BaManager.SelectUnit.Count - 1; i++)
        {
            Destroy(BaManager.SelectUnit[i].transform.Find("Line(Clone)").gameObject);
        }

        BaManager.SelectUnitLine.Clear();
        BaManager.currentMode = BattleManager.Mode.Instruction;
        this.gameObject.SetActive(false);
        BaManager.ActionUI.SetActive(true);
    }
}
