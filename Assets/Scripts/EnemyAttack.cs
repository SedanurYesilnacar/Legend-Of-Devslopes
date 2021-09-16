using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private float _range = 3f;
    [SerializeField]
    private float _timeBetweenAttacks = 5f;

    private bool _playerInRange = false;
    private Transform _player;
    private Animator anim;

    private EnemyHealth enemyHealth;

    private BoxCollider[] _weaponColliders;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        _weaponColliders = GetComponentsInChildren<BoxCollider>();
        _player = GameManager.Instance.Player;
        StartCoroutine(Attack());

    }

    private void Update()
    {     
        _playerInRange = IsInRange();
    }

    private bool IsInRange()
    {
        float distance = Vector3.Distance(transform.position, _player.position);
        if(distance < _range)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private IEnumerator Attack()
    {
        if (_playerInRange && !GameManager.Instance.GameOver && enemyHealth.IsAlive)
        {
            anim.Play("Attack");
            yield return new WaitForSeconds(_timeBetweenAttacks);
        }
        yield return null;
        StartCoroutine(Attack());
    }

    public void EnemyBeginAttack()
    {
        foreach(var weapons in _weaponColliders)
        {
            weapons.enabled = true;
        }
    }

    public void EnemyEndAttack()
    {
        foreach(var weapons in _weaponColliders)
        {
            weapons.enabled = false;
        }
    }
}