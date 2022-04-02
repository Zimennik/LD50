using DG.Tweening;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] public FirstPersonAIO _firstPersonAIO;
    [SerializeField] public PlayerInteraction _playerInteraction;
    [SerializeField] private Transform _startLookAt;

    private PullableObject _currentObjectInHands = null;
    private Vector3 _startPos;

    private void Awake()
    {
        _startPos = transform.position;
        LookAt(_startLookAt, true);
    }

    private void Update()
    {
        CheckPlayerPosition();
    }

    public void SetMovement(bool value)
    {
        _firstPersonAIO.playerCanMove = value;
        _firstPersonAIO.enableCameraMovement = value;
    }

    public void SetCursorLock(bool value)
    {
        _firstPersonAIO.lockAndHideCursor = value;
    }

    public void SetInteraction(bool value)
    {
        _playerInteraction.SetInteract(value);
    }

    //Set mouse sensitivity
    public void SetMouseSensitivity(float value)
    {
        _firstPersonAIO.mouseSensitivity = value;
    }

    //if player Y position is less then -10, teleport player to start position
    public void CheckPlayerPosition()
    {
        if (transform.position.y < -10)
        {
            transform.position = _startPos;
            LookAt(_startLookAt, false);
        }
    }

    //smooth look at transform. Smoothing implements with DOTween. If isSmooth = false, then look at transform instantly
    public async void LookAt(Transform target, bool isSmooth)
    {
        //disable player movement
        SetMovement(false);
        _firstPersonAIO.playerCamera.transform.localRotation = Quaternion.identity;
        await _firstPersonAIO.transform.DOLookAt(target.position, isSmooth ? 2f : 0.1f).AsyncWaitForCompletion();
        //enable player movement
        SetMovement(true);
    }
}