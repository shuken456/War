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

    //HP,�X�^�~�i�Q�[�W
    public GameObject HpGaugePrefab;
    public GameObject StaminaGaugePrefab;

    //�Q�[�W�\���p�L�����o�X
    public GameObject CanvasWorldSpace;

    //�o���p�I�u�W�F�N�g
    public GameObject SortieObject;

    //�o���ꏊ����t���O
    private bool SortieDecition = false;

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
            Fighter.transform.parent = SortieObject.transform;
            Fighter.transform.localPosition = pf.Position;
        }
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
                SortieObject.transform.position = CursorPosition;

                //���N���b�N�ŏꏊ�w��
                if (Input.GetMouseButtonDown(0))
                {
                    SortieDecition = true;
                    DecitionButton.interactable = true;
                }
            }
        }
    }

    //�o������
    public void DecitionButtonClick()
    {
        //�C���X�g�폜
        foreach (Transform s in SortieObject.transform)
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
            Fighter.transform.parent = SortieObject.transform;
            Fighter.transform.localPosition = pf.Position;
            Fighter.transform.parent = null;

            Common.GetFighterStatusFromDB(Fighter.GetComponent<FighterStatus>(), pf);
            CreateGauge(Fighter);
        }

        //�o���t���O��true��
        BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].SoriteFlg = true;
        BaManager.ActionUI.SetActive(true);
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
            foreach (Transform n in SortieObject.transform)
            {
                GameObject.Destroy(n.gameObject);
            }

            Common.SelectUnitNum = 0;
            BaManager.ActionUI.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    private void CreateGauge(GameObject targetObject)
    {
        var Hpgauge = Instantiate(HpGaugePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Hpgauge.transform.SetParent(CanvasWorldSpace.transform, false);
        Hpgauge.GetComponent<HpGauge>().targetFighter = targetObject;

        var Staminagauge = Instantiate(StaminaGaugePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Staminagauge.transform.SetParent(CanvasWorldSpace.transform, false);
        Staminagauge.GetComponent<StaminaGauge>().targetFighter = targetObject;
    }
}
