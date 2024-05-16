using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CameraSettingDB : ScriptableObject
{
    public List<CameraSetting> CameraSettingDBList;
}

[System.Serializable]
public class CameraSetting
{
    //ズーム最大値と最小値
    public int MaxZoom;
    public int MinZoom;
    //カメラのズームスピード
    public float ZoomSpeed;
    //カメラの移動スピード
    public float MoveSpeed;
    //カメラを動かせる範囲
    public Vector3 MoveAreaScale;
    //ミニマップに写す範囲
    public int MiniMapSize;
    //デフォルトのカメラサイズ
    public int DefaultSize;
    //デフォルトのカメラ位置
    public Vector3 DefaultPosition;

    //出撃可能範囲
    public Vector3 SortieSize;
    public Vector3 SortiePosition;
}
