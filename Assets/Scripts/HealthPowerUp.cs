using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    private int _healAmount = 10;

    private Transform _player;

    private PlayerHealth playerHealth;

    public int HealAmount { get => _healAmount; }

    private void Start()
    {
        _player = GameManager.Instance.Player;
        playerHealth = _player.GetComponent<PlayerHealth>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerHealth.Heal();
            Destroy(gameObject);
        }
    }
}
