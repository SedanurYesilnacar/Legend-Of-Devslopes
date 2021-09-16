using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    private NavMeshAgent navMeshAgent;
    private Animator anim;

    private EnemyHealth enemyHealth;

    private void Start()
    {
        _target = GameManager.Instance.Player;
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        if(!GameManager.Instance.GameOver && enemyHealth.IsAlive)
        {
            Move();
        }
        else if(GameManager.Instance.GameOver && enemyHealth.IsAlive)
        {
            navMeshAgent.enabled = false;
            anim.Play("Idle");
        } else
        {
            navMeshAgent.enabled = false;
        }
    }

    private void Move()
    {
        navMeshAgent.SetDestination(_target.position);
    }
}