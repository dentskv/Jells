using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewControllers;
using Zenject;

public class PlayerPhysicsController : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private Rigidbody _playerRigidbody;
    private float _playerSpeed;
    private float _defaultPlayerSpeed = 0;
    private int _obstacleCounter;

    public event Action OnPlayerWon;
    public event Action OnPlayerFell;
    public event Action OnCrossOneObstacle;
    public event Action OnCrossTenObstacles;
    public event Action OnBumpIntoObstacle;
    public event Action OnTakeCoin;

    private void Awake()
    {
       _playerMovement = GetComponent<PlayerMovement>();
       _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
        _playerSpeed = _playerMovement.GetPlayerSpeed;
    }

    private void OnEnable()
    {
        OnPlayerWon += _playerMovement.EndGame;
    }

#region Triggers&Collisions

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            OnTakeCoin?.Invoke();
            other.GetComponent<CoinController>().TurnMeshRendererOff();
        }

        if (other.gameObject.CompareTag("CrossPoint"))
        {
            _obstacleCounter++;
            if (!other.GetComponent<CrossTriggerController>().IsTouched)
            {
                if (_playerSpeed <= 7.6f)
                {
                    _playerSpeed += 0.2f;
                }
                OnCrossOneObstacle?.Invoke();
            }
            Debug.Log(_obstacleCounter);
            if (_obstacleCounter % 10 == 0)
            {
                OnCrossTenObstacles?.Invoke();
            }
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            if(_playerSpeed > _defaultPlayerSpeed) _playerSpeed -= 0.2f;
            OnBumpIntoObstacle?.Invoke();
            Debug.Log("Obstacle");
        }
        
        if (other.gameObject.CompareTag("DeathZone"))
        {
            _playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _playerMovement.EndGameHandler();
            OnPlayerFell?.Invoke();
        }
        
        if (other.gameObject.CompareTag("Finish"))
        {
            OnPlayerWon?.Invoke();
        }
    }
    
#endregion
}
