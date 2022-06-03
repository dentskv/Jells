using System;
using DG.Tweening;
using UnityEngine;
using ViewControllers;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform spawnPointTransform;
    [SerializeField] private Transform pointTransform;
    [SerializeField] private float playerSpeed = 2f;
    
    [Inject] private GUIViewController guiViewController;

    private Rigidbody _playerRigidbody;
    private Vector3 _playerDirection;
    private float _angleRotation;
    private float _defaultPlayerSpeed = 0;
    private bool _shouldChangeDirection;
    private bool _isMoving;
    private bool _isTurning;
    
    public float GetPlayerSpeed => playerSpeed;

    public PlayerPhysicsController GetPhysicsController { get; private set; }

    private void Awake()
    {
        GetPhysicsController = GetComponent<PlayerPhysicsController>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _defaultPlayerSpeed += playerSpeed;
    }

    private void Start()
    {
        _playerDirection = new Vector3(0, 0, playerSpeed * Time.fixedDeltaTime);
        _angleRotation = transform.rotation.y;
        guiViewController.GetMainMenu.OnStartedGame += StartGameHandler;
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            pointTransform.position += _playerDirection; 
        }
        
        if (_isTurning)
        {
            pointTransform.rotation = Quaternion.Euler(transform.rotation.x, _angleRotation, transform.rotation.z);
            
            if (_shouldChangeDirection)
            {
                DefineDirection();
                _shouldChangeDirection = false;
            }
            _isTurning = false;
        }
    }

    private void StartGameHandler()
    {
        _isMoving = true;
    }

    public void EndGameHandler()
    {
        _isMoving = false;
    }

    private void DefineDirection()
    {
        switch (_angleRotation)
        {
            case 90:
                _playerDirection = new Vector3(playerSpeed * Time.fixedDeltaTime, 0, 0);
                break;
            case 180:
                _playerDirection = new Vector3(0, 0, -playerSpeed * Time.fixedDeltaTime);
                break;
            case 270:
                _playerDirection = new Vector3(-playerSpeed * Time.fixedDeltaTime, 0, 0);
                break;
            case 360:
            case 0:
                _playerDirection = new Vector3(0, 0, playerSpeed * Time.fixedDeltaTime);
                break;
        }
    }
    
    private void CorrectAngle(bool isRight)
    {
        if (isRight) _angleRotation += 90;
        else _angleRotation -= 90;
        
        if (_angleRotation < 0) _angleRotation += 360;
        if (_angleRotation > 360) _angleRotation -= 360;
        if (_angleRotation == 0) _angleRotation = 0;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Turn"))
        {
            if (other.GetComponent<RotateTargetController>().isRotated) return;
            other.GetComponent<RotateTargetController>().isRotated = true;
            _isTurning = true;
            var isRightTurn = other.gameObject.GetComponent<TargetController>().GetTurnDirection();
            CorrectAngle(isRightTurn);
            _shouldChangeDirection = true;
        }
    }

    private void UnfreezeConstraints()
    {
        var constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        _playerRigidbody.constraints = constraints;
    }
    
    public void EndGame()
    {
        _isMoving = false;
        playerSpeed = _defaultPlayerSpeed;
        guiViewController.SetActivePanel(guiViewController.GetResultMenu.name);
    }

    public void SetStartTransform()
    {
        transform.localPosition = new Vector3(0, 0.5f, 0);
        pointTransform.transform.localScale = spawnPointTransform.localScale;
        pointTransform.transform.position = spawnPointTransform.position;
        pointTransform.transform.rotation = spawnPointTransform.rotation;
        UnfreezeConstraints();
    }
}
