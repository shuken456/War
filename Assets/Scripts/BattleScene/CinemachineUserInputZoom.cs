using Cinemachine;
using UnityEngine;

public class CinemachineUserInputZoom : CinemachineExtension
{
    // Input Manager�̓��͖�
    [SerializeField] private string _inputName = "Mouse ScrollWheel";

    // ���͒l�Ɋ|����l
    [SerializeField] private float _inputScale = 100;

    // FOV�̍ŏ��E�ő�
    [SerializeField, Range(1, 179)] private float _minFOV = 10;
    [SerializeField, Range(1, 179)] private float _maxFOV = 90;

    // ���[�U�[���͂�K�v�Ƃ���
    public override bool RequiresUserInput => true;

    private float _scrollDelta;
    private float _adjustFOV;

    private void Update()
    {
        // �X�N���[���ʂ����Z
        _scrollDelta += Input.GetAxis(_inputName);
    }

    // �e�X�e�[�W���Ɏ��s�����R�[���o�b�N
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime
    )
    {
        // Aim�̒��ゾ�����������{
        if (stage != CinemachineCore.Stage.Aim)
            return;

        var lens = state.Lens;

        if (!Mathf.Approximately(_scrollDelta, 0))
        {
            // FOV�̕␳�ʂ��v�Z
            _adjustFOV = Mathf.Clamp(
                _adjustFOV - _scrollDelta * _inputScale,
                _minFOV - lens.FieldOfView,
                _maxFOV - lens.FieldOfView
            );

            _scrollDelta = 0;
        }

        // state�̓��e�͖��񃊃Z�b�g�����̂ŁA
        // ����␳����K�v������
        lens.FieldOfView += _adjustFOV;

        state.Lens = lens;
    }
}