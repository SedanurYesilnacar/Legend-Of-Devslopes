using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speedAmount = 2f;

    private Transform _player;

    private PlayerController playerController;


    public float SpeedAmount { get => _speedAmount; }

    private void Start()
    {
        _player = GameManager.Instance.Player;
        playerController = _player.GetComponent<PlayerController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerController.StartSpeedUpRoutine();
            Destroy(gameObject);
        }
    }
    
}
