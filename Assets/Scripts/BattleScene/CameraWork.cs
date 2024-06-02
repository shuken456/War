using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;


public class CameraWork : MonoBehaviour
{
    //���C���J����
    private CinemachineVirtualCamera mainCam;
    //�}�E�X�̎n�_ 
    private Vector3 startMousePos;
    //�J�����̎n�_ 
    private Vector3 startCamPos;
    //�Y�[�����x
    public float ZoomSpeed;
    //�Y�[���ő�l
    public int MaxZoom;
    //�Y�[���ŏ��l
    public int MinZoom;
    //�J�����ړ����x
    public float MoveSpeed;

    void Start()
    {
        //�e�J�����ݒ�
        mainCam = this.gameObject.GetComponent<CinemachineVirtualCamera>();
        this.transform.position = Camera.main.transform.position;
    }

    void Update()
    {
        //�z�C�[�����擾�@����ɂ���ăY�[��
        var scroll = Input.mouseScrollDelta.y * Time.unscaledDeltaTime * ZoomSpeed;

        float AfterZoom = mainCam.m_Lens.OrthographicSize - scroll;
        //�Y�[���ő�l�ƍŏ��l����
        if (AfterZoom < MaxZoom && AfterZoom > MinZoom)
        {
            mainCam.m_Lens.OrthographicSize -= scroll;
        }

        //�}�E�X�J�[�\���ƃJ�����̎n�_���擾
        if (Input.GetMouseButtonDown(1))
        {
            startMousePos = Input.mousePosition;
            startCamPos = mainCam.transform.position;
        }

        //�}�E�X�J�[�\���̃h���b�O�ʒu�ɂ���ăJ�������ړ��i�E�h���b�O�j
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
