using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionUI : MonoBehaviour
{
    public BattleManager BaManager;

    //�N���b�N���������ԃJ�E���g�p
    private float ClickTime = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BaManager.SelectFighter.Clear();
        }

        //���N���b�N�ŕ��m�I��
        if (Input.GetMouseButton(0) && BaManager.StartFlg)
        {
            Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

            var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter"));
            if (col != null && !BaManager.SelectFighter.Contains(col.gameObject))
            {
                Time.timeScale = 0;
                BaManager.ButtonSE.Play();
                BaManager.SelectFighter.Add(col.gameObject);

                //�I�𒆂̌����ڂ�ύX
                col.gameObject.transform.Find("SelectImage").GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }

        //�A�N�V����UI���J���đҋ@
        if (Input.GetMouseButtonUp(0) && BaManager.SelectFighter.Count > 0)
        {
            BaManager.ActionUI.SetActive(true);
            this.gameObject.SetActive(false);
        }

        //�E�N���b�N���������łȂ��ꍇ�A���m�̃X�e�[�^�X��\��
        if (Input.GetMouseButton(1))
        {
            ClickTime += Time.unscaledDeltaTime;
        }
        if (Input.GetMouseButtonUp(1))
        {
            if (ClickTime < 0.2)
            {
                Vector3 CursorPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

                var col = Physics2D.OverlapPoint(CursorPosition, LayerMask.GetMask("PlayerFighter", "EnemyFighter"));
                if(col != null)
                {
                    BaManager.ButtonSE.Play();
                    BaManager.FighterStatusInfoUI.SetActive(true);
                    BaManager.FighterStatusInfoUI.GetComponent<FighterStatusInfo>().TextWrite(col.GetComponent<FighterStatus>());
                    BaManager.FighterStatusInfoUI.GetComponent<FighterStatusInfo>().ImageWrite(col.GetComponent<SpriteRenderer>().sprite, col.GetComponent<SpriteRenderer>().color);
                    ClickTime = 0;
                }
            }
            ClickTime = 0;
        }
    }

    public void InstractionButtonClick()
    {
        BaManager.ActionUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
