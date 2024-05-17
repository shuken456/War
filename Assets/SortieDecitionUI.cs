using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SortieDecitionUI : MonoBehaviour
{
    public BattleManager BaManager;

    //����{�^��
    public Button DecitionButton;

    //���m�v���n�u
    public GameObject EmptyInfantry;
    public GameObject EmptyArcher;

    public GameObject InfantryPrefab;
    public GameObject ArcherPrefab;

    //�o���p�I�u�W�F�N�g
    public GameObject SortieTarget;
    public GameObject SortieRange;

    //�o���ꏊ����t���O
    private bool SortieDecition = false;

    //�N���b�N���������ԃJ�E���g�p
    private float ClickTime = 0;

    private void OnEnable()
    {
        //DB�f�[�^�擾
        BaManager.PlayerUnitDataBaseAllList = Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //���j�b�g�ԍ����ɕ��ёւ�
        BaManager.PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList.FindAll(n => n.UnitNum == Common.SelectUnitNum).ToList();

        //�I�����ꂽ���j�b�g�̕��m����ʂɕ\��������
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
            Fighter.GetComponent<SpriteRenderer>().color = BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].UnitColor - new Color(0, 0, 0, 0.5f);�@//�F�𔖂����ĕ\������
            Fighter.transform.parent = SortieTarget.transform;
            Fighter.transform.localPosition = pf.Position;
        }

        SortieRange.SetActive(true);
    }

    private void OnDisable()
    {
        //��]�����ɖ߂�
        SortieTarget.transform.eulerAngles -= new Vector3(0, 0, 0);

        Common.SelectUnitNum = 0;

        if (BaManager.StartFlg)
        {
            BaManager.ActionUI.SetActive(true);
        }
        else
        {
            BaManager.StartUI.SetActive(true);
        }

        SortieRange.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //�������_���ŏo���\
        if (!SortieDecition && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerBase"));

            if(col != null)
            {
                SortieTarget.transform.position = CursorPosition;

                //���N���b�N�ŏꏊ�w��
                if (Input.GetMouseButtonDown(0))
                {
                    SortieDecition = true;
                    DecitionButton.interactable = true;
                }
            }
        }

        //�E�N���b�N���������łȂ��ꍇ�A�ʒu����]
        if (!SortieDecition && Input.GetMouseButton(1))
        {
            ClickTime += Time.unscaledDeltaTime;
        }
        if (!SortieDecition && Input.GetMouseButtonUp(1))
        {
            if (ClickTime < 0.2)
            {
                SortieTarget.transform.eulerAngles -= new Vector3(0, 0, 90);

                foreach(Transform Fighter in SortieTarget.transform)
                {
                    Fighter.eulerAngles += new Vector3(0, 0, 90);
                }
            }
            ClickTime = 0;
        }
    }

    //�o������
    public void DecitionButtonClick()
    {
        //�C���X�g�폜
        foreach (Transform s in SortieTarget.transform)
        {
            GameObject.Destroy(s.gameObject);
        }

        //���m�̃I�u�W�F�N�g���쐬
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
            Fighter.transform.parent = SortieTarget.transform;
            Fighter.transform.localPosition = pf.Position;
            Fighter.transform.parent = null;

            Common.GetFighterStatusFromDB(Fighter.GetComponent<FighterStatus>(), pf);
            Common.FighterBuff(Fighter.GetComponent<FighterStatus>(), BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].Strategy, false);
            BaManager.CreateGauge(Fighter);
        }

        //�����̏o���t���O��true��
        BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].SoriteFlg = true;
        //�o���\��������-1
        BaManager.UnitCountUI.Count -= 1;
        BaManager.UnitCountUI.TextDraw();
        
        this.gameObject.SetActive(false);
    }

    //�L�����Z��
    public void CancelButtonClick()
    {
        //�ʒu�����̏ꍇ�͌���O�ɖ߂��A����O�̏ꍇ�͏o�����[�h���I��������
        if(SortieDecition)
        {
            SortieDecition = false;
            DecitionButton.interactable = false;
        }
        else
        {
            foreach (Transform n in SortieTarget.transform)
            {
                GameObject.Destroy(n.gameObject);
            }
            this.gameObject.SetActive(false);
        }
    }
}
