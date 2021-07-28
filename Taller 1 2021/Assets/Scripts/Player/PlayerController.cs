using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public enum PlayerState
{
    walk,
    attack,
    //interact,
    stagger,
    idle,
    jumping
}

public enum MovementType
{
    FREE,
    TILED
}

public enum ControlType
{
    Arrows,
    WASD,
}

public enum MovimientoPermitido
{
    Todo,
    Solo_Horizontal,
    Solo_Vertical
}

public class PlayerController : MonoBehaviour
{
    public bool useAnimator = true;

    public PlayerState currentState;

    [Header("Move Settings")]
    public ControlType controlType = ControlType.WASD;
    public MovementType movementType;
    public MovimientoPermitido movimientoPermitido;
    public float moveSpeed = 5f;
    public float airMoveSpeed = 1f;
    public bool flipLeft = false;

    [Header("Tile Movement")]
    [SerializeField]
    private float moveTime = .2f;

    [SerializeField]
    private float tileSize = 1f;

    public LayerMask collisionLayer;
    private bool moving;
    private Vector3 startingPos;
    private Vector3 targetPos;

    [Header("Weapons")]
    public PlayerWeapon equippedWeapon;

    public enum FlipDirection
    {
        NONE,
        IZQ,
        DER
    }

    public FlipDirection flipXAnimation = FlipDirection.NONE;

    SpriteRenderer playerRenderer;
    public Animator animator;
    public Collider2D playerCollider;

    Vector2 colliderCenterPosition;

    // Collision detection
    Vector3 pointA;
    Vector3 pointB;

    float overlapWidth = 0.4f;
    float overlapHeight = 1.3f;

    Vector2 movement;

    public Rigidbody2D rb;

    [Header("Stats")]
    public float startingHealth = 10;
    float maxHealth, currentHealth;
    public float startingStamina = 100;
    float maxStamina, currentStamina;
    public float staminaRegenSpeed = 10f;

    [Header("Colliders Ataque")]
    public Collider2D colliderArriba;
    public Collider2D colliderAbajo;
    public Collider2D colliderDerecha;
    public Collider2D colliderIzquierda;

    private Vector2Int playerDirection = Vector2Int.down;

    private void Start()
    {
        currentState = PlayerState.idle;

        animator = GetComponent<Animator>();
        playerRenderer = GetComponent<SpriteRenderer>();

        playerCollider = GetComponent<Collider2D>();
        colliderCenterPosition = playerCollider.bounds.center;

        rb = GetComponent<Rigidbody2D>();

        // Health
        maxHealth = startingHealth;
        currentHealth = maxHealth;

        // Stamina
        maxStamina = startingStamina;
        currentStamina = maxStamina;
    }


    void Update()
    {
        // Regeneración stamina
        currentStamina += staminaRegenSpeed * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        if (UserInterface.instance != null) 
            if (UserInterface.instance.playerStaminaWheel != null) 
                UserInterface.instance.playerStaminaWheel.fillAmount = currentStamina / maxStamina;

        colliderCenterPosition = playerCollider.bounds.center;

        if (movementType == MovementType.TILED)
        {
            ManageTileMovement();
        }
        else if (movementType == MovementType.FREE)
        {
            ManageFreeMovement();
        }

        // Ataque
        if (Input.GetButtonDown("Attack") && currentState != PlayerState.attack)
        {
            StartCoroutine(AttackCoroutine());
        }

        // Lanzar arma
        if (Input.GetMouseButtonDown(1) && equippedWeapon != null)
        {
            ThrowWeapon();
        }
    }

    IEnumerator AttackCoroutine()
    {
        // Llamar a la animación de ataque y setear el estado "atacando"
        animator?.SetTrigger("attack");
        currentState = PlayerState.attack;

        // Obtener dirección en que estoy mirando y activar el collider correspondiente
        DesactivarColliders();

        if (playerDirection == Vector2Int.right) colliderDerecha.enabled = true;
        if (playerDirection == Vector2Int.left) colliderIzquierda.enabled = true;
        if (playerDirection == Vector2Int.up) colliderArriba.enabled = true;
        if (playerDirection == Vector2Int.down) colliderAbajo.enabled = true;

        // Esperar un frame
        yield return null;

        // Obtener tiempo de la animación de ataque
        float attackTime = (animator) ? animator.GetCurrentAnimatorClipInfo(0)[0].clip.length : .5f;

        // Esperar ese tiempo
        yield return new WaitForSeconds(attackTime);

        // Terminar el estado de atacar
        DesactivarColliders();
        currentState = PlayerState.idle;
    }

    private void DesactivarColliders()
    {
        colliderArriba.enabled = false;
        colliderAbajo.enabled = false;
        colliderDerecha.enabled = false;
        colliderIzquierda.enabled = false;
    }

    private void FixedUpdate()
    {
        if (movementType == MovementType.FREE && 
            currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            Move();
            ManageAnimation();
        }
    }

    private void Move()
    {
        // MOVIMIENTO: Mover el RigidBody del personaje
        change = movement.normalized;
        // Restricciones de movimiento
        if (movimientoPermitido == MovimientoPermitido.Solo_Horizontal) change.y = 0f;
        if (movimientoPermitido == MovimientoPermitido.Solo_Vertical) change.x = 0f;

        // Movimiento en el suelo
        if (currentState != PlayerState.jumping)
        {
            // Aplicar vector de movimiento al RigidBody
            rb.MovePosition((Vector2)transform.position + change * moveSpeed * Time.fixedDeltaTime);
        }
        // Movimiento en el aire
        else
        {
            rb.AddForce(change * airMoveSpeed);
        }
    }

    private void ManageAnimation()
    {
        if (useAnimator)
        {
            // El personaje se está moviendo
            if (movement != Vector2.zero)
            {
                animator?.SetBool("moving", true);

                // Dar vuelta el sprite al caminar a la izquierda
                playerRenderer.flipX = false;
                if (flipLeft && movement.x < 0)
                {
                    playerRenderer.flipX = true;
                }

                animator?.SetFloat("moveX", change.x);
                animator?.SetFloat("moveY", change.y);
            }
            // El personaje NO se está moviendo
            else
            {
                animator?.SetBool("moving", false);
            }
        }
    }

    [HideInInspector]
    public Vector2 change;
    private void ManageFreeMovement()
    {
        change = Vector2.zero;

        // Moving with the arrow keys
        if (controlType == ControlType.Arrows)
        {
            movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (Input.GetAxisRaw("Horizontal") == 0) movement.x = 0;
            if (Input.GetAxisRaw("Vertical") == 0) movement.y = 0;
        }
        else
        {
            movement = new Vector2(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"));
            if (Input.GetAxisRaw("Horizontal2") == 0) movement.x = 0;
            if (Input.GetAxisRaw("Vertical2") == 0) movement.y = 0;
        }

        change = movement.normalized;

        // Guardar dirección del player para el ataque
        // Eje X
        if (Mathf.Abs(change.x) > Mathf.Abs(change.y))
        {
            // Derecha
            if (change.x > 0)
            {
                playerDirection = Vector2Int.right;
            }
            // Izquierda
            else if (change.x < 0)
            {
                playerDirection = Vector2Int.left;
            }
        }
        // Eje Y
        else
        {
            // Arriba
            if (change.y > 0)
            {
                playerDirection = Vector2Int.up;
            }
            // Abajo
            else if (change.y < 0)
            {
                playerDirection = Vector2Int.down;
            }
        }
    }

    public void ManageTileMovement()
    {
        // UP
        if (Input.GetKey(KeyCode.W) && !moving)
        {
            pointA = new Vector3(colliderCenterPosition.x - tileSize * overlapWidth, colliderCenterPosition.y - tileSize * overlapWidth, 0);
            pointB = new Vector3(colliderCenterPosition.x + tileSize * overlapWidth, colliderCenterPosition.y + tileSize * overlapHeight, 0);

            if (!Physics2D.OverlapArea(pointA, pointB, collisionLayer))
            {
                StartCoroutine(MovePlayer(Vector3.up));
                movement = Vector3.up;
            }
        }

        // LEFT
        if (Input.GetKey(KeyCode.A) && !moving)
        {
            pointA = new Vector3(colliderCenterPosition.x - tileSize * overlapHeight, colliderCenterPosition.y - tileSize * overlapWidth, 0);
            pointB = new Vector3(colliderCenterPosition.x + tileSize * overlapWidth, colliderCenterPosition.y + tileSize * overlapWidth, 0);

            if (!Physics2D.OverlapArea(pointA, pointB, collisionLayer))
            {
                StartCoroutine(MovePlayer(Vector3.left));
                movement = Vector3.left;
            }
        }

        // RIGHT
        if (Input.GetKey(KeyCode.S) && !moving)
        {
            pointA = new Vector3(colliderCenterPosition.x - tileSize * overlapWidth, colliderCenterPosition.y - tileSize * overlapHeight, 0);
            pointB = new Vector3(colliderCenterPosition.x + tileSize * overlapWidth, colliderCenterPosition.y + tileSize * overlapWidth, 0);

            if (!Physics2D.OverlapArea(pointA, pointB, collisionLayer))
            {
                StartCoroutine(MovePlayer(Vector3.down));
                movement = Vector3.down;
            }
        }

        // DOWN
        if (Input.GetKey(KeyCode.D) && !moving)
        {
            pointA = new Vector3(colliderCenterPosition.x - tileSize * overlapWidth, colliderCenterPosition.y - tileSize * overlapWidth, 0);
            pointB = new Vector3(colliderCenterPosition.x + tileSize * overlapHeight, colliderCenterPosition.y + tileSize * overlapWidth, 0);

            if (!Physics2D.OverlapArea(pointA, pointB, collisionLayer))
            {
                StartCoroutine(MovePlayer(Vector3.right));
                movement = Vector3.right;
            }
        }
    }

    // Tile Movement
    private IEnumerator MovePlayer(Vector3 direction)
    {
        moving = true;

        float elapsedTime = 0;

        startingPos = transform.position;
        targetPos = startingPos + direction;

        while (elapsedTime < moveTime)
        {
            // TODO: Mover Rigidbody con MovePosition
            transform.position = Vector3.Lerp(startingPos, targetPos, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        moving = false;
        movement = Vector3.zero;
    }

    public void Hit(float knockTime, float damage)
    {
        if (currentState != PlayerState.stagger)
        {
            //Debug.Log("PLAYER HIT");

            DamagePlayer(damage);

            if (currentHealth > 0)
            {
                StartCoroutine(KnockCo(knockTime));
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    const float fallDamage = 10f;
    public void Fall()
    {
        DamagePlayer(fallDamage);
        animator.SetBool("falling", true);
        currentState = PlayerState.stagger;
    }

    public void FinishFall()
    {
        animator.SetBool("falling", false);
        currentState = PlayerState.idle;
    }

    public void DamagePlayer(float amount)
    {
        // Resto la vida
        currentHealth -= amount;
        // Limito la vida entre 0 y la vida máxima
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (UserInterface.instance != null) UserInterface.instance.playerHealthBar.fillAmount = currentHealth / maxHealth;
    }

    IEnumerator KnockCo(float knockTime)
    {
        if (currentState != PlayerState.stagger)
        {
            currentState = PlayerState.stagger;
            yield return new WaitForSeconds(knockTime);

            rb.velocity = Vector2.zero;
            currentState = PlayerState.idle;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointA, 0.1f);
        Gizmos.DrawWireSphere(pointB, 0.1f);
    }

    public void PickUpWeapon(Transform newWeapon)
    {
        // Drop current
        DropEquippedWeapon();

        newWeapon.SetParent(transform);
        newWeapon.localPosition = Vector3.zero;
        equippedWeapon = newWeapon.GetComponent<PlayerWeapon>();
        equippedWeapon.Equip();
    }

    public void DropEquippedWeapon()
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.Unequip();
            equippedWeapon.transform.SetParent(null);
            equippedWeapon = null;
        }
    }

    public void ThrowWeapon()
    {
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float throwForce = 50f;

        equippedWeapon.Throw(dir * throwForce);
        DropEquippedWeapon();
    }

    public bool UseStamina(int amount)
    {
        // Solamente si tenemos suficiente stamina
        if (currentStamina - amount >= 0)
        {
            currentStamina -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
}
