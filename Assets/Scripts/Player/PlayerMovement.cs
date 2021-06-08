using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool grounded = false;
    private bool canJump = true;
    private bool canStare = false;
    private bool stareCore = true;

    private float speed = 5;
    private float jumpHeight = 500;

    private Animator animator;
    private Rigidbody2D physics = new Rigidbody2D();
    private LayerMask jumpLayers;

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
            print("1");
            animator.SetTrigger("stare");
            canStare = false;
        }

        animator.SetBool("walking", movement);
    }

    IEnumerator StareDelay()
    {
        stareCore = false;

        yield return new WaitForSeconds(7f);
        if (animator.playableGraph.GetEditorName() != "StaticAnimation")
        {
            StartCoroutine(StareDelay());
        }
        else
        {
            canStare = true;
        }

        stareCore = true;
    }

    private void Jump()
    {
        grounded = Physics2D.OverlapCircle(transform.position + new Vector3(0.25f, -1.5f * Mathf.Abs(scale.x), 0), 0.75f, jumpLayers) != null;

        if (canJump && grounded && Input.GetKeyDown(KeyCode.Space))
        {
            physics.AddForce(Vector2.up * jumpHeight);
            canJump = false;
            animator.SetTrigger("jump");
            StartCoroutine(waitToUpdateJump());
        }
    }

    public void Start()
    {
        physics = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpLayers = LayerMask.GetMask("Ground");
        scale = transform.localScale;
    }

    public void Update()
    {
        Move();
        Jump();
    }

    public IEnumerator waitToUpdateJump()
    {
        yield return new WaitForSeconds(0.75f);
        canJump = true;

    }
}
