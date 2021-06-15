using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBox : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D physics;
    private Vector3 startVector = Vector3.zero;
    [SerializeField] private float walkingDistance = 3;
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private float speedDelay = 0.25f;
    public bool canJump = true;
    public float effectPower = 10;
    private float constSpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        physics = GetComponent<Rigidbody2D>();
        startVector = transform.position;
        constSpeed = speed;
    }

    private void Move()
    {
        physics.velocity = new Vector2(-transform.localScale.x * speed, 0);

        bool readyToRotate = transform.position.x > startVector.x + walkingDistance * -transform.localScale.x;
        if ((readyToRotate && -transform.localScale.x > 0) || (!readyToRotate && -transform.localScale.x < 0))
        {
            transform.localScale *= new Vector2(-1, 1);
        }
    }

    IEnumerator StopDelay()
    {
        canJump = false;
        speed = 0;
        yield return new WaitForSeconds(speedDelay);
        speed = constSpeed;
        canJump = true;
    }

    public void OnJump()
    {
        animator.SetTrigger("jump");
        StartCoroutine(StopDelay());
    }

    void Update()
    {
        Move();
    }
}
