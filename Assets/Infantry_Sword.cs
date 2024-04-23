using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infantry_Sword : MonoBehaviour
{
    // �A�j���[�V����
    private Animator anim;

    //�ݒ肳�ꂽ�ڕW�ړ��n�_
    public List<Vector3> targetPlace = new List<Vector3>();

    //���݂̖ڕW�n�_
    private Vector3 NowTargetPlace;

    //�����̃X�e�[�^�X
    private Status MyStatus;

    //�A�j���[�V�����̃A�N�V�����Ǘ�
    public int StandAction = 0;
    public int RunAction = 1;
    public int AttackAction = 2;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        MyStatus = GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //�ړ�
    private void Move()
    {
        //�ړ��ڕW������ꍇ�A�ړ�
        if (targetPlace.Count > 0 && Time.timeScale >= 1)
        {
            anim.SetInteger("Action", RunAction);
            Transform LineObject = this.transform.Find("Line(Clone)");

            //���ړ��ڕW�ɒ������ꍇ
            if (Mathf.Abs(transform.position.x - NowTargetPlace.x) < 0.1f && Mathf.Abs(transform.position.y - NowTargetPlace.y) < 0.1f)
            {
                //���ړ��ڕW���N���A
                NowTargetPlace = new Vector3(0,0,0);
                targetPlace.RemoveAt(0);

                //�ړ��ڕW��0�ɂȂ����ꍇ�A�ړ���������
                if (targetPlace.Count == 0)
                {
                    Destroy(LineObject.gameObject);
                    anim.SetInteger("Action", StandAction);
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    //���̖ڕW��ݒ�
                    for (int i = 0; i <= targetPlace.Count; i++)
                    {
                        LineObject.gameObject.GetComponent<LineRenderer>().SetPosition(i, LineObject.gameObject.GetComponent<LineRenderer>().GetPosition(i + 1));
                    }
                    LineObject.gameObject.GetComponent<LineRenderer>().positionCount--;
                }
            }
            else
            {
                //�ړ�
                NowTargetPlace = targetPlace[0];
                var v = NowTargetPlace - transform.position;
                transform.position += v.normalized * MyStatus.MoveSpeed * Time.deltaTime;

                //�E������
                if (v.x >= 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                //��������
                else
                {
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }
        }
    }
}
