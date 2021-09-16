using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int _playerHealth = 100;
    [SerializeField]
    private float _timeSinceLastHit = 2f;

    private float _timer = 0f;
    private int _currentHealth;

    private CharacterController characterController;
    private Animator anim;
    private AudioSource audioSource;
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private ParticleSystem humanBlood;

    private EnemyHealth enemyHealth;

    private HealthPowerUp healthPowerUp;

    public int CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            if (value < 0)
            {
                _currentHealth = 0;
            }
            else if (value >= _playerHealth)
            {
                _currentHealth = _playerHealth;
            } else
            {
                _currentHealth = value;
            }
            UpdateHealthBar();
        }
    }


    private void Awake()
    {
        healthPowerUp = new HealthPowerUp();
        enemyHealth = new EnemyHealth();
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _currentHealth = _playerHealth;
        healthBar.maxValue = _playerHealth;
        healthBar.value = healthBar.maxValue;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
    }

    private void TakeHit()
    {
        if(_currentHealth > 0 && enemyHealth.IsAlive)
        {
            anim.Play("Hurt");
            Bleed();
            audioSource.PlayOneShot(audioSource.clip);
            _currentHealth -= 20;
            UpdateHealthBar();
            GameManager.Instance.PlayerHit(_currentHealth);
        }
        if(_currentHealth <= 0)
        {
            GameManager.Instance.PlayerHit(_currentHealth);
            KillPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.CompareTag("Weapon") || other.CompareTag("Arrow")) && _timer >= _timeSinceLastHit && !GameManager.Instance.GameOver)
        {
            TakeHit();
            _timer = 0f;
        }
        if(other.CompareTag("Arrow"))
        {
            Destroy(other.gameObject);
        }
    }

    private void KillPlayer()
    {
        anim.SetTrigger("HeroDie");
        characterController.enabled = false;
    }

    private void UpdateHealthBar()
    {
        if(healthBar.value != _currentHealth)
        {
            healthBar.value = _currentHealth;
        }
    }

    private void Bleed()
    {
        ParticleSystem blood = Instantiate(humanBlood,transform.position,Quaternion.identity,this.transform) as ParticleSystem;
        blood.Play();
    }

    public void Heal()
    {
        CurrentHealth += healthPowerUp.HealAmount;
    }
}
