using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int _startingHealth = 20;
    [SerializeField]
    private float _timeSinceLastHit = 0.5f;
    [SerializeField]
    private float _disappearSpeed = 2f;

    private float _timer = 0f;
    private int _currentHealth;
    private bool _isAlive = true;
    private bool _disappearEnemy = false;
    private float _waitToSink = 4f;

    private AudioSource audioSource;
    private Animator anim;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;

    [SerializeField]
    private ParticleSystem orcBlood;

    public bool IsAlive { get => _isAlive; }


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        GameManager.Instance.RegisterEnemy(this);
        _currentHealth = _startingHealth;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_disappearEnemy)
        {
            RemoveEnemy();
        }
    }

    private void TakeHit()
    {
        if(_currentHealth > 0)
        {
            anim.Play("Hurt");
            Bleed();
            audioSource.PlayOneShot(audioSource.clip);
            _currentHealth -= 10;
        }
        if(_currentHealth <= 0)
        {
            _isAlive = false;
            KillEnemy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerWeapon") && _timer >= _timeSinceLastHit)
        {
            TakeHit();
            _timer = 0f;
        }
    }

    private void KillEnemy()
    {
        GameManager.Instance.KilledEnemy(this);
        anim.SetTrigger("EnemyDie");
        capsuleCollider.enabled = false;
        rigidbody.isKinematic = true;
        navMeshAgent.enabled = false;
        StartCoroutine(SinkEnemyDown());
    }

    private IEnumerator SinkEnemyDown()
    {
        yield return new WaitForSeconds(_waitToSink);
        _disappearEnemy = true;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void RemoveEnemy()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _disappearSpeed, Space.Self);
    }

    private void Bleed()
    {
        ParticleSystem blood = Instantiate(orcBlood, transform.position, Quaternion.identity, this.transform) as ParticleSystem;
        blood.Play();
    }
}
