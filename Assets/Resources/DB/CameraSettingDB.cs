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
    //�Y�[���ő�l�ƍŏ��l
    public int MaxZoom;
    public int MinZoom;
    //�J�����̃Y�[���X�s�[�h
    public float ZoomSpeed;
    //�J�����̈ړ��X�s�[�h
    public float MoveSpeed;
    //�J�����𓮂�����͈�
    public Vector3 MoveAreaScale;
    //�~�j�}�b�v�Ɏʂ��͈�
    public int MiniMapSize;
    //�f�t�H���g�̃J�����T�C�Y
    public int DefaultSize;
    //�f�t�H���g�̃J�����ʒu
    public Vector3 DefaultPosition;

    //�o���\�͈�
    public Vector3 SortieSize;
    public Vector3 SortiePosition;
}
