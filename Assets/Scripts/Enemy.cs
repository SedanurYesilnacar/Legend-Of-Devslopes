using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyType _enemyType;
    [SerializeField]
    private int _attackDamage;
    [SerializeField]
    private int _enemyHealth;


    private void Awake()
    {
    }

    public EnemyType EnemyType
    {
        get
        {
            return _enemyType;
        }
    }

    public int AttackDamage
    {
        get
        {
            return _attackDamage;
        }
    }


    public int EnemyHealth { get => _enemyHealth; }
}