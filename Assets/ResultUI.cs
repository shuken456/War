using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    public BattleManager BaManager;

    //兵士リザルト スクロールビュー
    public GameObject FighterResultView;
    //兵士リザルトプレハブ
    public GameObject FighterResultUI;

    //出撃した兵士リスト
    public List<PlayerFighter> SortieFighterList;

    // Start is called before the first frame update
    void Start()
    {
        //出撃した兵士を取得
        List<PlayerUnit> SortieUnit = BaManager.PlayerUnitDataBaseAllList.FindAll((n) => n.SoriteFlg);
        foreach (PlayerUnit pu in SortieUnit)
        {
            List<PlayerFighter> SortieFighter = BaManager.PlayerFighterDataBaseAllList.FindAll((n) => n.UnitNum == pu.Num);
            foreach (PlayerFighter pf in SortieFighter)
            {
                SortieFighterList.Add(pf);
            } 
        }

        //出撃した兵士分リザルトを作成
        foreach (PlayerFighter pf in SortieFighterList)
        {
            GameObject button = Instantiate(FighterResultUI, FighterResultView.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
