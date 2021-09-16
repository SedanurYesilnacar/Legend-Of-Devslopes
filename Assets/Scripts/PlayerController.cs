using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 10f;
    [SerializeField]
    private float _rotationSpeed = 10f;
    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private ParticleSystem _speedParticle;

    private bool isParticleActive = false;

    private ParticleSystem _fireParticle;

    private BoxCollider[] swordColliders;

    private CharacterController _characterController;
    private SpeedPowerUp speedPowerUp;
    private Vector3 currentLookTarget = Vector3.zero;
    private Animator anim;
    private Transform _player;

    public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }

    void Start()
    {
        _player = GameManager.Instance.Player;
        _characterController = GetComponent<CharacterController>();
        speedPowerUp = new SpeedPowerUp();
        anim = GetComponent<Animator>();
        swordColliders = GetComponentsInChildren<BoxCollider>();
    }

    void Update()
    {
        Move();

        if(!GameManager.Instance.GameOver)
        {
            if(Input.GetMouseButtonDown(0))
            {
                anim.Play("DoubleChop");
            }
            if(Input.GetMouseButtonDown(1))
            {
                anim.Play("SpinAttack");
            }
        }

        if(isParticleActive)
        {
            _fireParticle.transform.position = _player.position;
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 500, Color.blue);
        
        if(Physics.Raycast(ray,out hit, 500, _layerMask, QueryTriggerInteraction.Ignore) && !GameManager.Instance.GameOver)
        {
            if(hit.point != currentLookTarget)
            {
                currentLookTarget = hit.point;
            }

            Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed);
        }


    }

    private void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _characterController.SimpleMove(moveDir * _movementSpeed * Time.deltaTime);
        if(moveDir == Vector3.zero)
        {
            anim.SetBool("IsWalking", false);
        } else
        {
            anim.SetBool("IsWalking", true);
        }
    }

    public void StartSpeedUpRoutine()
    {
        StartCoroutine(SpeedPowerUpRoutine());
    }

    private IEnumerator SpeedPowerUpRoutine()
    {
        float SpeedAmount = speedPowerUp.SpeedAmount;
        ActivateFireParticle();
        MovementSpeed *= SpeedAmount;
        yield return new WaitForSeconds(5f);
        isParticleActive = false;
        InactivateFireParticle();
        MovementSpeed /= SpeedAmount;
        yield return null;
    }

    private void ActivateFireParticle()
    {
        isParticleActive = true;
        _fireParticle = Instantiate(_speedParticle, _player.position, Quaternion.identity);
        _fireParticle.Play();
    }

    private void InactivateFireParticle()
    {
        Destroy(_fireParticle.gameObject);
    }

    private void PlayerBeginAttack()
    {
        foreach(var sword in swordColliders)
        {
            sword.enabled = true;
        }
    }

    private void PlayerEndAttack()
    {
        foreach(var sword in swordColliders)
        {
            sword.enabled = false;
        }
    }
}
