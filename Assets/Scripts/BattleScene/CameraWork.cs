using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;


public class CameraWork : MonoBehaviour
{
    //メインカメラ
    private CinemachineVirtualCamera mainCam;
    //マウスの始点 
    private Vector3 startMousePos;
    //カメラの始点 
    private Vector3 startCamPos;
    //ズーム速度
    public float ZoomSpeed;
    //ズーム最大値
    public int MaxZoom;
    //ズーム最小値
    public int MinZoom;
    //カメラ移動速度
    public float MoveSpeed;

    void Start()
    {
        //各カメラ設定
        mainCam = this.gameObject.GetComponent<CinemachineVirtualCamera>();
        this.transform.position = Camera.main.transform.position;
    }

    void Update()
    {
        //ホイールを取得　それによってズーム
        var scroll = Input.mouseScrollDelta.y * Time.unscaledDeltaTime * ZoomSpeed;

        float AfterZoom = mainCam.m_Lens.OrthographicSize - scroll;
        //ズーム最大値と最小値制限
        if (AfterZoom < MaxZoom && AfterZoom > MinZoom)
        {
            mainCam.m_Lens.OrthographicSize -= scroll;
        }

        //マウスカーソルとカメラの始点を取得
        if (Input.GetMouseButtonDown(1))
        {
            startMousePos = Input.mousePosition;
            startCamPos = mainCam.transform.position;
        }

        //マウスカーソルのドラッグ位置によってカメラを移動（右ドラッグ）
        if (Input.GetMouseButton(1))
        {
            //(移動開始座標 - マウスの現在座標) / 解像度 で正規化
            float x = (startMousePos.x - Input.mousePosition.x) / Screen.width;
            float y = (startMousePos.y - Input.mousePosition.y) / Screen.height;

            x = x * MoveSpeed;
            y = y * MoveSpeed;

            Vector3 velocity = new Vector3(x, y, 0) + startCamPos;
            mainCam.transform.position = velocity;

        }
    }
}
