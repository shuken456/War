using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;


public class CameraWork : MonoBehaviour
{
    public BattleManager BaManager;

    //メインカメラ
    private CinemachineVirtualCamera mainCam;
    //マウスの始点 
    private Vector3 startMousePos;
    //カメラの始点 
    private Vector3 startCamPos;
    //カメラ移動範囲オブジェクト
    public GameObject CameraMoveArea;
    //ミニマップ用カメラ
    public Camera MiniMapCamera;
    //カメラ情報
    private CameraSetting CameraSetting;

    //出撃可能範囲
    public GameObject SortieRange;

    void Start()
    {
        //DBデータ取得
        CameraSetting = Resources.Load<CameraSettingDB>("DB/CameraSettingDB").CameraSettingDBList[Common.SelectStageNum - 1]; 

        //各カメラ設定
        mainCam = this.gameObject.GetComponent<CinemachineVirtualCamera>();
        Camera.main.transform.position = CameraSetting.DefaultPosition;
        this.transform.position = Camera.main.transform.position;
        CameraMoveArea.transform.localScale = CameraSetting.MoveAreaScale;
        MiniMapCamera.orthographicSize = CameraSetting.MiniMapSize;

        EditorUtility.SetDirty(Resources.Load<CameraSettingDB>("DB/CameraSettingDB"));
        AssetDatabase.SaveAssets();

        //出撃可能範囲設定
        SortieRange.transform.localScale = CameraSetting.SortieSize;
        SortieRange.transform.position = CameraSetting.SortiePosition;
    }

    void Update()
    {
        if(BaManager.MoveUI.GetComponent<MoveUI>().currentMode != MoveUI.Mode.MoveDecisionAfter)
        {
            //ホイールを取得
            var scroll = Input.mouseScrollDelta.y * Time.unscaledDeltaTime * CameraSetting.ZoomSpeed;

            float AfterZoom = mainCam.m_Lens.OrthographicSize - scroll;
            //ズーム最大値と最小値制限
            if (AfterZoom < CameraSetting.MaxZoom && AfterZoom > CameraSetting.MinZoom)
            {
                mainCam.m_Lens.OrthographicSize -= scroll;
            }

            //マウスカーソルとカメラの始点を取得
            if (Input.GetMouseButtonDown(1))
            {
                startMousePos = Input.mousePosition;
                startCamPos = mainCam.transform.position;
            }

            //マウスカーソルのドラッグ位置によってカメラを移動
            if (Input.GetMouseButton(1))
            {
                //(移動開始座標 - マウスの現在座標) / 解像度 で正規化
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
