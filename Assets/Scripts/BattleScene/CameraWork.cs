using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;


public class CameraWork : MonoBehaviour
{
    public BattleManager BaManager;

    //���C���J����
    private CinemachineVirtualCamera mainCam;
    //�}�E�X�̎n�_ 
    private Vector3 startMousePos;
    //�J�����̎n�_ 
    private Vector3 startCamPos;
    //�J�����ړ��͈̓I�u�W�F�N�g
    public GameObject CameraMoveArea;
    //�~�j�}�b�v�p�J����
    public Camera MiniMapCamera;
    //�J�������
    private CameraSetting CameraSetting;

    //�o���\�͈�
    public GameObject SortieRange;

    void Start()
    {
        //DB�f�[�^�擾
        CameraSetting = Resources.Load<CameraSettingDB>("DB/CameraSettingDB").CameraSettingDBList[Common.SelectStageNum - 1]; 

        //�e�J�����ݒ�
        mainCam = this.gameObject.GetComponent<CinemachineVirtualCamera>();
        Camera.main.transform.position = CameraSetting.DefaultPosition;
        this.transform.position = Camera.main.transform.position;
        CameraMoveArea.transform.localScale = CameraSetting.MoveAreaScale;
        MiniMapCamera.orthographicSize = CameraSetting.MiniMapSize;

        EditorUtility.SetDirty(Resources.Load<CameraSettingDB>("DB/CameraSettingDB"));
        AssetDatabase.SaveAssets();

        //�o���\�͈͐ݒ�
        SortieRange.transform.localScale = CameraSetting.SortieSize;
        SortieRange.transform.position = CameraSetting.SortiePosition;
    }

    void Update()
    {
        if(BaManager.MoveUI.GetComponent<MoveUI>().currentMode != MoveUI.Mode.MoveDecisionAfter)
        {
            //�z�C�[�����擾
            var scroll = Input.mouseScrollDelta.y * Time.unscaledDeltaTime * CameraSetting.ZoomSpeed;

            float AfterZoom = mainCam.m_Lens.OrthographicSize - scroll;
            //�Y�[���ő�l�ƍŏ��l����
            if (AfterZoom < CameraSetting.MaxZoom && AfterZoom > CameraSetting.MinZoom)
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

                x = x * CameraSetting.MoveSpeed;
                y = y * CameraSetting.MoveSpeed;

                Vector3 velocity = new Vector3(x, y, 0) + startCamPos;
                mainCam.transform.position = velocity;

            }
        }
    }
}
