using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    private float constSpeed = 0;
    [SerializeField] private float speedModificator = 1.5f;
    [SerializeField] private float jumpHeight = 5;
    private float constJumpHeight = 0;
    [SerializeField] private float jumpHeightModificator = 1.5f;
    [SerializeField] private float jumpDelay = 0.75f;

    private bool grounded = false;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canStare = false;
    private bool stareCore = true;

    public bool movementEnabled = true;

    private Animator animator;
    private Rigidbody2D physics;
    [SerializeField] private LayerMask jumpLayers;

    private Vector2 scale;

    private void SetScale(float x)
    {
        physics.transform.localScale = new Vector2(x, physics.transform.localScale.y);
    }

    private void Move()
    {
        int moveA = Input.GetKey(KeyCode.A) ? -1 : 0;
        int moveD = Input.GetKey(KeyCode.D) ? 1 : 0;


        float movementSpeed = (moveA + moveD) * speed;

        Vector2 movementDirection = new Vector2(movementSpeed, physics.velocity.y);
        physics.velocity = movementDirection;

        bool movement = movementSpeed != 0;

        if (movement)
        {
            float scaleX = (movementSpeed > 0) ? scale.x : -scale.x;
            SetScale(scaleX);
        }
        if (!canStare && stareCore) StartCoroutine(StareDelay());

        if (canStare && !movement)
        {
            animator.SetTrigger("stare");
            canStare = false;
        }

        animator.SetBool("walking", movement);
    }

    IEnumerator StareDelay()
    {
        stareCore = false;

        yield return new WaitForSeconds(7f);

        string animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        if (animationName != "StaticAnimation")
        {
            StartCoroutine(StareDelay());
        }
        else
        {
            canStare = true;
            stareCore = true;
        }

    }

    private void BoxJump()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position - new Vector3(0.1f, 2.5f), -transform.up, 1);
        if (raycast.collider == null)
            return;

        JumpBox box = raycast.collider.GetComponent<JumpBox>();

        if (box != null)
        {
            if (!box.canJump)
                return;

            physics.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);

            box.OnJump();
        }
    }

    public IEnumerator waitToUpdateJump()
    {
        yield return new WaitForSeconds(jumpDelay);
        canJump = true;
    }

    private void Jump()
    {
        grounded = Physics2D.OverlapCircle(transform.position + new Vector3(0.25f, -1.5f * Mathf.Abs(scale.x), 0), 0.75f, jumpLayers) != null;

        if (canJump && grounded && Input.GetKeyDown(KeyCode.Space))
        {
            physics.AddForce(Vector2.up * jumpHeight * physics.mass * 100);
            canJump = false;
            animator.SetTrigger("jump");
            StartCoroutine(waitToUpdateJump());
        }
    }

    private IEnumerator WaitSpeedPotion()
    {
        yield return new WaitForSeconds(10);
        speed = constSpeed;
    }

    public void OnStaminaPotion()
    {
        constSpeed = speed;
        speed *= speedModificator;
        StartCoroutine(WaitSpeedPotion());
    }

    private IEnumerator WaitJumpPotion()
    {
        yield return new WaitForSeconds(10);
        jumpHeight = constJumpHeight;
    }

    public void OnJumpPotion()
    {
        constJumpHeight = jumpHeight;
        jumpHeight *= jumpHeightModificator;
        StartCoroutine(WaitJumpPotion());
    }

    public void Start()
    {
        physics = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        scale = transform.localScale;
    }

    public void Update()
    {

        if (!movementEnabled)
        {
            if(physics.velocity.x != 0) { physics.velocity = Vector2.zero; }
            return;
        }


        Move();
        Jump();
        BoxJump();
    }
}
