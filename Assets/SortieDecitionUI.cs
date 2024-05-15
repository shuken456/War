using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SortieDecitionUI : MonoBehaviour
{
    public BattleManager BaManager;

    //決定ボタン
    public Button DecitionButton;

    //兵士プレハブ
    public GameObject EmptyInfantry;
    public GameObject EmptyArcher;

    public GameObject InfantryPrefab;
    public GameObject ArcherPrefab;

    //出撃用オブジェクト
    public GameObject SortieObject;
    public GameObject SortieRange;

    //出撃場所決定フラグ
    private bool SortieDecition = false;

    private void OnEnable()
    {
        //DBデータ取得
        BaManager.PlayerUnitDataBaseAllList = Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //ユニット番号順に並び替え
        BaManager.PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList.FindAll(n => n.UnitNum == Common.SelectUnitNum).ToList();

        //選択されたユニットの兵士を画面に表示させる
        foreach (PlayerFighter pf in BaManager.PlayerFighterDataBaseAllList)
        {
            GameObject Fighter = null;

            if (pf.Type == 1)
            {
                Fighter = Instantiate(EmptyInfantry, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else if (pf.Type == 2)
            {
                Fighter = Instantiate(EmptyArcher, new Vector3(0, 0, 0), Quaternion.identity);
            }

            Fighter.transform.localScale = Fighter.transform.localScale * 2;
            Fighter.GetComponent<SpriteRenderer>().color = BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].UnitColor - new Color(0, 0, 0, 0.5f);　//色を薄くして表示する
            Fighter.transform.parent = SortieObject.transform;
            Fighter.transform.localPosition = pf.Position;
        }

        SortieRange.SetActive(true);
    }

    private void OnDisable()
    {
        SortieRange.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //味方拠点内で出撃可能
        if (!SortieDecition && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerBase"));

            if(col != null)
            {
                SortieObject.transform.position = CursorPosition;

                //左クリックで場所指定
                if (Input.GetMouseButtonDown(0))
                {
                    SortieDecition = true;
                    DecitionButton.interactable = true;
                }
            }
        }
    }

    //出撃決定
    public void DecitionButtonClick()
    {
        //イラスト削除
        foreach (Transform s in SortieObject.transform)
        {
            GameObject.Destroy(s.gameObject);
        }

        //兵士のオブジェクトを作成
        foreach (PlayerFighter pf in BaManager.PlayerFighterDataBaseAllList)
        {
            GameObject Fighter = null;

            if (pf.Type == 1)
            {
                Fighter = Instantiate(InfantryPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else if (pf.Type == 2)
            {
                Fighter = Instantiate(ArcherPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            }

            Fighter.transform.localScale = Fighter.transform.localScale;
            Fighter.GetComponent<SpriteRenderer>().color = BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].UnitColor;
            Fighter.transform.parent = SortieObject.transform;
            Fighter.transform.localPosition = pf.Position;
            Fighter.transform.parent = null;

            Common.GetFighterStatusFromDB(Fighter.GetComponent<FighterStatus>(), pf);
            Common.FighterBuff(Fighter.GetComponent<FighterStatus>(), BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].Strategy, false);
            BaManager.CreateGauge(Fighter);
        }

        //出撃フラグをtrueに
        BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].SoriteFlg = true;
        BaManager.ActionUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //キャンセル
    public void CancelButtonClick()
    {
        //位置決定後の場合は決定前に戻す、決定前の場合は出撃モードを終了させる
        if(SortieDecition)
        {
            SortieDecition = false;
            DecitionButton.interactable = false;
        }
        else
        {
            foreach (Transform n in SortieObject.transform)
            {
                GameObject.Destroy(n.gameObject);
            }

            Common.SelectUnitNum = 0;
            BaManager.ActionUI.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
