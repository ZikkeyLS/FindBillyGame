﻿using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private int eyeDistance = 30;
    [SerializeField] private float movementSpeed = 0.5f;
    [SerializeField] private float stupiedTime = 2f;
    private Vector3 lastPosition = new Vector3(0.000001f, 0.000001f, 0.000001f);
    private int invertable = 1;

    [Header("Parametres")]
    [SerializeField] private int health = 100;

    [Header("Attack")]
    [SerializeField] private int attackDistance = 5;
    [SerializeField] private int damage = 25;
    [SerializeField] private float attackDelay = 2;

    private GameObject player;
    private PlayerController controller;
    private Rigidbody2D physics;
    private float distance = 0;
    private Vector2 scale = Vector2.zero;

    private bool attacking = false;
   

    public int GetHealth() => health;

    public void SetHealth(int value) { health = value; OnHealthChanged(); }

    public void GiveDamage(int value) { health -= value; OnHealthChanged(); }

    private void OnHealthChanged()
    {
        if(health == 0)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = PlayerController.Player;
        physics = GetComponent<Rigidbody2D>();
        controller = player.GetComponent<PlayerController>();

        scale = transform.localScale;
    }

    private void CalculateMovement()
    {
        float direction =  invertable * (player.transform.position.x - transform.position.x) > 0 ? scale.x : -scale.x;
        transform.localScale = new Vector2(direction, transform.localScale.y);
        physics.velocity = new Vector2(direction / scale.x * movementSpeed, physics.velocity.y);
        
        if (lastPosition == new Vector3(0.000001f, 0.000001f, 0.000001f)) 
        { 
            lastPosition = transform.position;
            StartCoroutine(OnMove());
        }
        else
        {
            RaycastHit2D raycast = Physics2D.Raycast(transform.position - new Vector3(0, 2f), -transform.up, 1);
            if (raycast.transform != null && lastPosition.y != transform.position.y)
            {
                invertable *= -1;
                lastPosition = new Vector3(0.000001f, 0.000001f, 0.000001f);
            }

            Debug.DrawRay(transform.position - new Vector3(0, 1.75f), -transform.up, Color.white, 1);
        }
    }

    private IEnumerator OnMove()
    {
        yield return new WaitForSeconds(stupiedTime);
        if(Vector3.Distance(transform.position, lastPosition) < 2) 
        {
            invertable *= -1;
        }
    }

    private void Attack()
    {
        attacking = true;
        controller.information.GiveDamage(damage);
        print("Hit. Player have: " + controller.information.GetHealth());
        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        attacking = false;
    }

    private void Update()
    {
        distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance > attackDistance)
        {
            CalculateMovement();
        }
        else if(!attacking)
        {
            Attack();
        }
    }
}
