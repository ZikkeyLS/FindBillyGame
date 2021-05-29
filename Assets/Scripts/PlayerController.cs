using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public class PlayerMovement
    {
        private bool grounded = false;
        private bool canJump = true;

        private float speed = 5;
        private float jumpHeight = 175;

        private Animator animator;
        private Rigidbody2D physics = new Rigidbody2D();
        private PlayerController controller;
        private Transform transform;
        private LayerMask jumpLayers;

        private void SetScale(int x)
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
                int scaleX = (movementSpeed > 0) ? -1 : 1;
                SetScale(scaleX);
            }
            animator.SetBool("walking", movement);
        }
        private void Jump()
        {
            grounded = Physics2D.OverlapCircle(transform.position + new Vector3(0.25f, -1.5f, 0), 0.75f, jumpLayers) != null;

            if (canJump && grounded && Input.GetKeyDown(KeyCode.Space))
            {
                physics.AddForce(Vector2.up * jumpHeight);
                canJump = false;
                animator.SetTrigger("jump");
                controller.StartCoroutine(waitToUpdateJump());
            }
        }

        public void Start(PlayerController playerController)
        {
            physics = playerController.GetComponent<Rigidbody2D>();
            animator = playerController.GetComponent<Animator>();
            transform = physics.transform;
            controller = playerController;
            jumpLayers = LayerMask.GetMask("Ground");
        }

        public void Update()
        {
            Move();
            Jump();
        }

        public IEnumerator waitToUpdateJump()
        {
            yield return new WaitForSeconds(1f);
            canJump = true;

        }
    }

    public class PlayerShoot
    {
        private PlayerController controller;
        public Animator animator;
        public bool canShoot = true;

        public void Start(PlayerController playerController)
        {
            controller = playerController;
            animator = playerController.GetComponent<Animator>();
        }

        private void Shoot()
        {
            RaycastHit2D hit = Physics2D.Raycast(controller.transform.position, controller.transform.position + new Vector3(10 * -controller.transform.localScale.x, 0, 0), 100, LayerMask.GetMask("Enemy"));

            if(hit.collider != null)
            {
                Destroy(hit.collider.gameObject);
            }
        }

        public void Update()
        {
            if (Input.GetKey(KeyCode.Mouse0) && canShoot)
            {
                canShoot = false;
                controller.StartCoroutine(sprayTime());
                Shoot();
            }
            Debug.DrawLine(controller.transform.position, controller.transform.position + new Vector3(10 * -controller.transform.localScale.x, 0, 0), Color.yellow);
        }

        IEnumerator sprayTime()
        {
            yield return new WaitForSeconds(0.25f);
            canShoot = true;
        }
    }

    private PlayerMovement movement = new PlayerMovement();
    private PlayerShoot shoot = new PlayerShoot();

    private void Start()
    {
        movement.Start(this);
        shoot.Start(this);
    }

    private void Update()
    {
        movement.Update();
        shoot.Update();
    }
}
