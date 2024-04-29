using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraWork : MonoBehaviour
{
    //メインカメラ
    private CinemachineVirtualCamera mainCam;
    //カメラのズームスピード
    public float ZoomSpeed;
    //カメラの移動スピード
    public float MoveSpeed;
    //マウスの始点 
    private Vector3 startMousePos;
    //カメラの始点 
    private Vector3 startCamPos;


    public BattleManager BaManager;


    void Start()
    {
        mainCam = this.gameObject.GetComponent<CinemachineVirtualCamera>();//メインカメラを取得
    }

    void Update()
    {
        if(BaManager.currentMode != BattleManager.Mode.MoveDecisionAfter)
        {
            //ホイールを取得
            var scroll = Input.mouseScrollDelta.y * Time.unscaledDeltaTime * ZoomSpeed;

            float AfterZoom = mainCam.m_Lens.OrthographicSize - scroll;
            //ズーム最大値と最小値制限
            if (AfterZoom < 10 && AfterZoom > 4)
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

                x = x * MoveSpeed;
                y = y * MoveSpeed;

                Vector3 velocity = new Vector3(x, y, 0) + startCamPos;
                mainCam.transform.position = velocity;

            }
        }
    }
}
