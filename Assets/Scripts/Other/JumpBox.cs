using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBox : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D physics;
    private Vector3 startVector = Vector3.zero;
    private float walkingDistance = 3;
    [SerializeField] private float speed = 2.5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        physics = GetComponent<Rigidbody2D>();
        startVector = transform.position;
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

    void Update()
    {
        Move();

    }
}
