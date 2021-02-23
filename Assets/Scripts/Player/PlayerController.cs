using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ground Checker")]
    [SerializeField] bool canCheckGround = true;

    public bool playerIsOnGround;
    [SerializeField] LayerChecker footA;
    [SerializeField] LayerChecker footB;

    [SerializeField] float jumpForce = 9;
    [SerializeField] float speed = 9;
    private Rigidbody2D _rigidbody2D;

    public Rigidbody2D rigidbody2D
    {
        get
        {
            if (_rigidbody2D == null)
            {
                _rigidbody2D = GetComponent<Rigidbody2D>();
            }
            return _rigidbody2D;
        }
        set
        {
            _rigidbody2D = value;
        }
    }

    [Header("Player States")]
    public bool playerIsShooting;
    public bool playerIsJumping;

    [Header("Attack")]
    [SerializeField] GunController gunController;
    // escalar para que indique la fuerza
    [SerializeField] float shootForce = 9;

    [Header("Animations")]
    [SerializeField] AnimatorController animatorController;

    [Header("SFX")]
    [SerializeField] AudioClip jumpSfx;
    [SerializeField] AudioClip shootSfx;

    // Control Variables
    private Vector2 movementDirection;
    private Vector2 movementDirectionNonZero;
    private float nonZeroMovementX;

    private bool jumpButtonPressed;
    private bool fireButtonPressed;

    private static PlayerController _instance;

    public static PlayerController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerController>();

                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(_instance.gameObject);
        }
        if (instance != null && instance != this)
        {
            // GetComponentInChildren<SpriteRenderer>().color = Color.red;
            Destroy(this.gameObject);
        }

        movementDirectionNonZero = Vector2.right;
        canCheckGround = true;
        animatorController.Play(AnimationId.Idle);
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleControls();
        HandleCheckGround();
        HandleMovement();
        HandleLookingDirection();
        HandleJunp();
        HandleAttack();
    }

    void HandleAttack()
    {
        if (fireButtonPressed && !playerIsShooting)
        {
            playerIsShooting = true;
            // si hay movimiento
            if (playerIsOnGround && (movementDirection.y == 0 || Mathf.Abs(movementDirection.x) > 0))
            {
                if (rigidbody2D.velocity.x == 0)
                {
                    animatorController.Play(AnimationId.StandShoot);
                    gunController.SetPosition(GunPosition.Stand);
                }
                else
                {
                    animatorController.Play(AnimationId.RunShoot);
                    gunController.SetPosition(GunPosition.Stand);
                }
            }
            StartCoroutine(RestoreAttack());
        }
    }

    Vector2 GetPriorityVector(Vector2 reference)
    {
        var result = reference;

        if (Mathf.Abs(reference.x) > Mathf.Abs(reference.y))
        {
            result.y = 0;
        }
        if (Mathf.Abs(reference.x) < Mathf.Abs(reference.y) && reference.y > 0)
        {
            result.x = 0;
        }
        if (reference.x == reference.y) // si es disparo en diagonal
        {
            result.y = 0; // se le da prioridad a la x por eso se pone y en 0
        }
        return result; //return del vector
    }

    // la función de arriba funcionaria correctamente si no tubieramos una animación de disparar pero la tenemos
    // por ello necesitamos definir una corrutina 
    IEnumerator RestoreAttack()
    {
        // hay que hacer un pequeño retrazo dependiendo de la animación
        var shootDirection = GetPriorityVector(movementDirectionNonZero.normalized);

        // cuando saltamos no podemos dispara hacia abajo y solo sucede si tocamos el suelo
        if ((shootDirection.y < 0 || movementDirection.y == 0) && playerIsOnGround)
        {
            shootDirection.x = Mathf.Sign(nonZeroMovementX);
            shootDirection.y = 0; // para no disparar hacia abajo
        }

        yield return new WaitForSeconds(0.1f);
        //invocar el efecto de sonido
        AudioManager.instance.PlaySFX(shootSfx);


        //mejora en magnitud se le aplica
        //gunController.Shoot(shootDirection * shootForce + GetPriorityVector(rigidbody2D.velocity));
        if (rigidbody2D.velocity.magnitude == 0)
        {
            // no se le suma nada
            gunController.Shoot(shootDirection * shootForce);
        }
        else
        {
            //para que permanezca constante la velocidad de saltar y correr
            gunController.Shoot(shootDirection * (shootForce + speed));
        }

        yield return new WaitForSeconds(0.2f);
        playerIsShooting = false;
    }

    void HandleCheckGround()
    {
        if (!canCheckGround) return;
        playerIsOnGround = footA.isTouching || footB.isTouching;
    }

    void HandleJunp()
    {
        if (jumpButtonPressed && playerIsOnGround)
        {
            AudioManager.instance.PlaySFX(jumpSfx);
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            // animatorController.Play(AnimationID.Jump);
            StartCoroutine(HandleJumpAnimation());
        }
    }

    IEnumerator HandleJumpAnimation()
    {
        canCheckGround = false;
        playerIsOnGround = false;
        animatorController.Play(AnimationId.Jump);
        yield return new WaitForSeconds(0.3f);
        canCheckGround = true;
    }

    void HandleControls()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (movementDirection != Vector2.zero)
        {
            movementDirectionNonZero = movementDirection;
        }

        if (Mathf.Abs(movementDirection.x) > 0)
        {
            nonZeroMovementX = movementDirection.x;
        }

        jumpButtonPressed = Input.GetButtonDown("Jump");
        fireButtonPressed = Input.GetButtonDown("Fire1");

    }

    void HandleMovement()
    {
        if (movementDirection.y == 0 || Mathf.Abs(movementDirection.x) > 0)
        {
            rigidbody2D.velocity = new Vector2(movementDirection.x * speed, rigidbody2D.velocity.y);
        }
        else
        {
            //disminuir la velocida si el jugador está en el piso
            if (playerIsOnGround)
            {
                rigidbody2D.velocity = Vector2.zero;
            }
        }
        if (playerIsOnGround && !playerIsShooting && (Mathf.Abs(movementDirection.x) > 0 || movementDirection.y == 0))
        {
            // si player se mueve
            if (rigidbody2D.velocity.magnitude > 0)
            {
                animatorController.Play(AnimationId.Run);
            }
            else
            {
                animatorController.Play(AnimationId.Idle);
            }
        }
        if (playerIsOnGround && movementDirection.x == 0)
        {
            // si se presiona W o arriba
            if (movementDirection.y > 0)
            {
                animatorController.Play(AnimationId.UpShoot);
                gunController.SetPosition(GunPosition.Up);
            }
            if (movementDirection.y < 0)
            {
                animatorController.Play(AnimationId.Duck);
                gunController.SetPosition(GunPosition.Duck);
            }
        }
    }

    void HandleLookingDirection()
    {
        if (nonZeroMovementX >= 0)
        {
            this.transform.rotation = Quaternion.identity;
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

}
