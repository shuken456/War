using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//選択されている兵士を表示するUI 画面左
public class SelectFighterInfoUI : MonoBehaviour
{
    public BattleManager BaManager;

    //選択兵士 スクロールビュー
    public GameObject SelectFighterView;
    //選択兵士　テキスト
    public GameObject SelectFighterText;

    private void OnEnable()
    {
        UpdateView();
    }

    //ビュー記載
    public void UpdateView()
    {
        foreach (Transform s in SelectFighterView.transform)
        {
            GameObject.Destroy(s.gameObject);
        }

        //選択されている兵士の名前等を表示
        foreach (GameObject Fighter in BaManager.SelectFighter)
        {
            FighterStatus fs = Fighter.GetComponent<FighterStatus>();
            GameObject Text = Instantiate(SelectFighterText, SelectFighterView.transform);
            Text.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            Text.transform.Find("Text (FighterName)").GetComponent<Text>().text = fs.FighterName;
            Text.transform.Find("Text (Level)").GetComponent<Text>().text = "Lv" + fs.Level.ToString();
            Text.transform.Find("Text (UnitName)").GetComponent<Text>().text = "(" + BaManager.PlayerUnitDataBaseAllList[fs.UnitNum - 1].Name + ")";

            if(fs.UnitLeader)
            {
                Text.transform.Find("Text (UnitLeader)").gameObject.SetActive(true);
            }
        }
    }
}
