using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FormationListManager : MonoBehaviour
{
    public GameObject[] UnitUI;
    public GameObject[] UnitOblect;

    public GameObject LockButton;

    public GameObject EmptyInfantry;
    public GameObject EmptyArcher;

    private GameObject Fighter;

    //ユニットDBと兵士DB
    private List<PlayerUnit> PlayerUnitDataBaseAllList;
    private List<PlayerFighter> PlayerFighterDataBaseAllList;

    // Start is called before the first frame update
    void Start()
    {
        //DBデータ取得
        PlayerUnitDataBaseAllList = Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //ユニット番号順に並び替え
        PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList.OrderByDescending((n) => n.UnitLeader).ToList(); //ユニットリーダーが頭に来るようにに並び替え

        int UnitCount = PlayerUnitDataBaseAllList.Count;

        for (int i = 0; i < UnitCount; i++)
        {
            UnitUI[i].transform.Find("Text (UnitName)").GetComponent<Text>().text = PlayerUnitDataBaseAllList[i].Name;
            UnitOblect[i].transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.white;

            //ユニット内の兵士情報を格納
            List<PlayerFighter> PlayerFighterDataBaseUnitList = PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == i + 1);
            int FighterCount = PlayerFighterDataBaseUnitList.Count;

            for (int j = 0; j < FighterCount; j++)
            {
                if(PlayerFighterDataBaseUnitList[j].type == 1)
                {
                    Fighter = Instantiate(EmptyInfantry, new Vector3(0, 0, 0), Quaternion.identity);
                }
                else if (PlayerFighterDataBaseUnitList[j].type == 2)
                {
                    Fighter = Instantiate(EmptyArcher, new Vector3(0, 0, 0), Quaternion.identity);
                }

                Fighter.GetComponent<SpriteRenderer>().color = PlayerUnitDataBaseAllList[i].UnitColor;
                Fighter.transform.parent = UnitOblect[i].transform;
                Fighter.transform.localPosition = PlayerFighterDataBaseUnitList[j].Position;
            }
        }

        //部隊を解放しきってない場合、ロックボタンを表示
        if(UnitCount < 10)
        {
            LockButton.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, UnitOblect[UnitCount].transform.position);
            LockButton.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
