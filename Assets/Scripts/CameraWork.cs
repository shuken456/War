using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraWork : MonoBehaviour
{
    //���C���J����
    private CinemachineVirtualCamera mainCam;
    //�J�����̃Y�[���X�s�[�h
    public float ZoomSpeed;
    //�J�����̈ړ��X�s�[�h
    public float MoveSpeed;
    //�}�E�X�̎n�_ 
    private Vector3 startMousePos;
    //�J�����̎n�_ 
    private Vector3 startCamPos;


    public BattleManager BaManager;


    void Start()
    {
        mainCam = this.gameObject.GetComponent<CinemachineVirtualCamera>();//���C���J�������擾
    }

    void Update()
    {
        if(BaManager.currentMode != BattleManager.Mode.MoveDecisionAfter)
        {
            //�z�C�[�����擾
            var scroll = Input.mouseScrollDelta.y * Time.unscaledDeltaTime * ZoomSpeed;

            float AfterZoom = mainCam.m_Lens.OrthographicSize - scroll;
            //�Y�[���ő�l�ƍŏ��l����
            if (AfterZoom < 10 && AfterZoom > 4)
            {
                mainCam.m_Lens.OrthographicSize -= scroll;
            }

            //�}�E�X�J�[�\���ƃJ�����̎n�_���擾
            if (Input.GetMouseButtonDown(1))
            {
                startMousePos = Input.mousePosition;
                startCamPos = mainCam.transform.position;
            }

            //�}�E�X�J�[�\���̃h���b�O�ʒu�ɂ���ăJ�������ړ�
            if (Input.GetMouseButton(1))
            {
                //(�ړ��J�n���W - �}�E�X�̌��ݍ��W) / �𑜓x �Ő��K��
                float x = (startMousePos.x - Input.mousePosition.x) / Screen.width;
                float y = (startMousePos.y - Input.mousePosition.y) / Screen.height;

                x = x * MoveSpeed;
                y = y * MoveSpeed;

                Vector3 velocity = new Vector3(x, y, 0) + startCamPos;
                mainCam.transform.position = velocity;

            }
        }
    }
}
