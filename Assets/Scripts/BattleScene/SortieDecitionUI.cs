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
    public GameObject InfantryPrefab;
    public GameObject ArcherPrefab;
    public GameObject ShielderPrefab;
    public GameObject CavalryPrefab;

    //出撃用オブジェクト
    public GameObject SortieTarget;
    public GameObject SortieFighter;

    //出撃場所決定フラグ
    private bool SortieDecition;

    //矢印
    public GameObject Cursol;

    //クリック長押し時間カウント用
    private float ClickTime = 0;

    //選択されたユニットの兵士リスト
    private List<PlayerFighter> SelectPlayerFighterDataBaseList;

    private void OnEnable()
    {
        SortieDecition = false;
        DecitionButton.interactable = false;

        //物理判定を使いたいため一時的に時間を変更
        Time.timeScale = 0.01f;
        Time.fixedDeltaTime = 0.00001f;

        //DBデータ取得
        BaManager.PlayerUnitDataBaseAllList = BaManager.PlayerUnitTable.PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //ユニット番号順に並び替え
        SelectPlayerFighterDataBaseList = BaManager.PlayerFighterTable.PlayerFighterDBList.FindAll(n => n.UnitNum == Common.SelectUnitNum).ToList();

        //選択されたユニットの兵士を画面に作成する
        foreach (PlayerFighter pf in SelectPlayerFighterDataBaseList)
        {
            GameObject Fighter = null;

            switch (pf.Type)
            {
                case 1:
                    Fighter = Instantiate(InfantryPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    break;
                case 2:
                    Fighter = Instantiate(ArcherPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    break;
                case 3:
                    Fighter = Instantiate(ShielderPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    break;
                case 4:
                    Fighter = Instantiate(CavalryPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    break;
                default:
                    break;
            }

            Fighter.GetComponent<SpriteRenderer>().color = BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].UnitColor - new Color(0, 0, 0, 0.5f);　//色を薄くして表示する
            Fighter.transform.parent = SortieFighter.transform;
            Fighter.transform.localPosition = pf.Position;
            Fighter.GetComponent<FighterAction>().SettingPosition = pf.Position;　//編成画面の位置通りに表示する
            Common.GetFighterStatusFromDB(Fighter.GetComponent<FighterStatus>(), pf);
            Common.FighterBuff(Fighter.GetComponent<FighterStatus>(), BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].Strategy, false);
            Fighter.layer = LayerMask.NameToLayer("SortieSettingFighter");　//障害物にのみ判定があるレイヤーに一時的に変更
            Fighter.tag = "SortieSettingFighter";
        }

        Cursol.SetActive(true);
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;

        //回転を元に戻す
        SortieTarget.transform.eulerAngles = new Vector3(0, 0, 0);

        Common.SelectUnitNum = 0;

        if (BaManager.StartFlg)
        {
            BaManager.ActionUI.SetActive(true);
        }
        else
        {
            BaManager.StartUI.SetActive(true);
        }

        Cursol.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //味方拠点内で出撃可能
        if (!SortieDecition && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerHealRange"));

            if(col != null)
            {
                SortieTarget.transform.position = CursorPosition;

                //左クリックで場所指定
                if (Input.GetMouseButtonDown(0))
                {
                    BaManager.ButtonSE.Play();
                    SortieDecition = true;
                    DecitionButton.interactable = true;

                    //色を濃く表示
                    foreach (Transform Fighter in SortieFighter.transform)
                    {
                        Fighter.gameObject.GetComponent<SpriteRenderer>().color = BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].UnitColor;
                    }
                }
            }
        }

        //右クリックが長押しでない場合、位置を回転
        if (!SortieDecition && Input.GetMouseButton(1))
        {
            ClickTime += Time.unscaledDeltaTime;
        }
        if (!SortieDecition && Input.GetMouseButtonUp(1))
        {
            if (ClickTime < 0.2)
            {
                BaManager.ButtonSE.Play();
                SortieTarget.transform.eulerAngles -= new Vector3(0, 0, 90);

                foreach (Transform Fighter in SortieFighter.transform)
                {
                    Fighter.eulerAngles += new Vector3(0, 0, 90);
                }
            }
            ClickTime = 0;
        }
    }

    //出撃決定
    public void DecitionButtonClick()
    {
        //兵士の付属オブジェクトを作成、タグとレイヤー変更
        foreach (Transform Fighter in SortieFighter.transform)
        {
            BaManager.CreateGaugeAndFlag(Fighter.gameObject);
            Fighter.gameObject.layer = LayerMask.NameToLayer("PlayerFighter");
            Fighter.tag = "PlayerFighter";
        }

        SortieFighter.transform.DetachChildren();

        //部隊の出撃フラグをtrueに
        BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].SoriteFlg = true;
        //出撃可能部隊数を-1
        BaManager.UnitCountUI.PossibleSortieCountNow -= 1;
        BaManager.UnitCountUI.TextDraw();

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

            //色を薄く表示
            foreach (Transform Fighter in SortieFighter.transform)
            {
                Fighter.gameObject.GetComponent<SpriteRenderer>().color = BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].UnitColor - new Color(0, 0, 0, 0.5f);
            }
        }
        else
        {
            //出撃兵士オブジェクト削除
            foreach (Transform n in SortieFighter.transform)
            {
                GameObject.Destroy(n.gameObject);
            }
            this.gameObject.SetActive(false);
        }
    }
}
