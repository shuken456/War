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
    public GameObject InfantryPrefab;
    public GameObject ArcherPrefab;
    public GameObject ShielderPrefab;
    public GameObject CavalryPrefab;

    //�o���p�I�u�W�F�N�g
    public GameObject SortieTarget;
    public GameObject SortieFighter;

    //�o���ꏊ����t���O
    private bool SortieDecition;

    //���
    public GameObject Cursol;

    //�N���b�N���������ԃJ�E���g�p
    private float ClickTime = 0;

    //�I�����ꂽ���j�b�g�̕��m���X�g
    private List<PlayerFighter> SelectPlayerFighterDataBaseList;

    private void OnEnable()
    {
        SortieDecition = false;
        DecitionButton.interactable = false;

        //����������g���������߈ꎞ�I�Ɏ��Ԃ�ύX
        Time.timeScale = 0.01f;
        Time.fixedDeltaTime = 0.00001f;

        //DB�f�[�^�擾
        BaManager.PlayerUnitDataBaseAllList = BaManager.PlayerUnitTable.PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //���j�b�g�ԍ����ɕ��ёւ�
        SelectPlayerFighterDataBaseList = BaManager.PlayerFighterTable.PlayerFighterDBList.FindAll(n => n.UnitNum == Common.SelectUnitNum).ToList();

        //�I�����ꂽ���j�b�g�̕��m����ʂɍ쐬����
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

            Fighter.GetComponent<SpriteRenderer>().color = BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].UnitColor - new Color(0, 0, 0, 0.5f);�@//�F�𔖂����ĕ\������
            Fighter.transform.parent = SortieFighter.transform;
            Fighter.transform.localPosition = pf.Position;
            Fighter.GetComponent<FighterAction>().SettingPosition = pf.Position;�@//�Ґ���ʂ̈ʒu�ʂ�ɕ\������
            Common.GetFighterStatusFromDB(Fighter.GetComponent<FighterStatus>(), pf);
            Common.FighterBuff(Fighter.GetComponent<FighterStatus>(), BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].Strategy, false);
            Fighter.layer = LayerMask.NameToLayer("SortieSettingFighter");�@//��Q���ɂ̂ݔ��肪���郌�C���[�Ɉꎞ�I�ɕύX
            Fighter.tag = "SortieSettingFighter";
        }

        Cursol.SetActive(true);
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;

        //��]�����ɖ߂�
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
        //�������_���ŏo���\
        if (!SortieDecition && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);
            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerHealRange"));

            if(col != null)
            {
                SortieTarget.transform.position = CursorPosition;

                //���N���b�N�ŏꏊ�w��
                if (Input.GetMouseButtonDown(0))
                {
                    BaManager.ButtonSE.Play();
                    SortieDecition = true;
                    DecitionButton.interactable = true;

                    //�F��Z���\��
                    foreach (Transform Fighter in SortieFighter.transform)
                    {
                        Fighter.gameObject.GetComponent<SpriteRenderer>().color = BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].UnitColor;
                    }
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

    //�o������
    public void DecitionButtonClick()
    {
        //���m�̕t���I�u�W�F�N�g���쐬�A�^�O�ƃ��C���[�ύX
        foreach (Transform Fighter in SortieFighter.transform)
        {
            BaManager.CreateGaugeAndFlag(Fighter.gameObject);
            Fighter.gameObject.layer = LayerMask.NameToLayer("PlayerFighter");
            Fighter.tag = "PlayerFighter";
        }

        SortieFighter.transform.DetachChildren();

        //�����̏o���t���O��true��
        BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].SoriteFlg = true;
        //�o���\��������-1
        BaManager.UnitCountUI.PossibleSortieCountNow -= 1;
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

            //�F�𔖂��\��
            foreach (Transform Fighter in SortieFighter.transform)
            {
                Fighter.gameObject.GetComponent<SpriteRenderer>().color = BaManager.PlayerUnitDataBaseAllList[Common.SelectUnitNum - 1].UnitColor - new Color(0, 0, 0, 0.5f);
            }
        }
        else
        {
            //�o�����m�I�u�W�F�N�g�폜
            foreach (Transform n in SortieFighter.transform)
            {
                GameObject.Destroy(n.gameObject);
            }
            this.gameObject.SetActive(false);
        }
    }
}
