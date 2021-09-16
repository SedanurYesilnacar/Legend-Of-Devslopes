using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAttack : MonoBehaviour
{
    [SerializeField]
    private float _range = 10f;
    [SerializeField]
    private float _timeBetweenAttacks = 1f;
    [SerializeField]
    private float _rotationSpeed = 10f; // Enemy rotation
    [SerializeField]
    private float _arrowSpeed = 10f;
    [SerializeField]
    private Transform _fireLocation;

    private Rigidbody arrowRB;

    private bool _playerInRange;

    [SerializeField]
    private GameObject _arrow;

    private Transform _player;
    private Animator anim;
    private EnemyHealth enemyHealth;

    private void Start()
    {
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        _player = GameManager.Instance.Player;
        StartCoroutine(Attack());
    }

    private void Update()
    {
        if(enemyHealth.IsAlive)
        {
            _playerInRange = IsInRange();
        }
    }

    private bool IsInRange()
    {
        if(Vector3.Distance(transform.position, _player.position) <= _range)
        {
            anim.SetBool("PlayerInRange", true);
            RotateTowards(_player);
            return true;
        } else
        {
            anim.SetBool("PlayerInRange", false);
            return false;
        }
    }

    private IEnumerator Attack()
    {
        if(enemyHealth.IsAlive && _playerInRange && !GameManager.Instance.GameOver)
        {
            // attack
            anim.Play("Attack");
            yield return new WaitForSeconds(_timeBetweenAttacks);
        }
        yield return null;
        StartCoroutine(Attack());
    }

    private void RotateTowards(Transform player)
    {
        var dir = (_player.position - transform.position).normalized;
        var angle = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime * _rotationSpeed);
    }

    public void FireArrow()
    {
        GameObject newArrow = Instantiate(_arrow, _fireLocation.position, transform.rotation);
        arrowRB = newArrow.GetComponent<Rigidbody>();
        arrowRB.velocity = transform.forward * _arrowSpeed;
        Destroy(newArrow, 5f);
    }
}
