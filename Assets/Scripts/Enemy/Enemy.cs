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
    [SerializeField] private int experience = 25;
    [SerializeField] private string enemyName = string.Empty;

    [Header("Attack")]
    [SerializeField] private int attackDistance = 5;
    [SerializeField] private int damage = 25;
    [SerializeField] private float attackDelay = 2;

    [Header("Animations")]
    [SerializeField] Animator animationController;

    [Header("OnDeath")]
    [SerializeField] GameObject component;
    [SerializeField] int minComponents = 2;
    [SerializeField] int maxComponents = 4;

    private GameObject player;
    private PlayerInformation playerInformation;
    private Rigidbody2D physics;
    private float distance = 0;
    private Vector2 scale = Vector2.zero;
    private Vector2 boxColliderSize;
    private BoxCollider2D boxCollider;

    private bool attacking = false;
    private bool sized = true;
   

    public int GetHealth() => health;

    public void SetHealth(int value) { health = value; OnHealthChanged(); }

    public void GiveDamage(int value) { health -= value; OnHealthChanged(); }

    private void OnHealthChanged()
    {
        if(health <= 0)
        {
            playerInformation.GiveExperience(experience);

            for (int i = 0; i < Random.Range(minComponents, maxComponents); i++)
            {
                Instantiate(component, transform.position + new Vector3(Random.Range(-1, 2), 0, 0), Quaternion.identity);
            }
   
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = PlayerInformation.Player;
        physics = GetComponent<Rigidbody2D>();
        playerInformation = player.GetComponent<PlayerInformation>();

        scale = transform.localScale;

        if (enemyName == "Zombie")
        {
            boxCollider = GetComponent<BoxCollider2D>();
            boxColliderSize = boxCollider.size;
        }
    }

    private void CalculateMovement()
    {
        float direction =  invertable * (player.transform.position.x - transform.position.x) > 0 ? -scale.x : scale.x;


        transform.localScale = new Vector2(direction, transform.localScale.y);
        physics.velocity = new Vector2(-direction / scale.x * movementSpeed, physics.velocity.y);

        if(boxCollider.size != null)
        {
            if(direction != 0)
            {
                boxCollider.size = boxColliderSize - new Vector2(0, 0.15f);
            }
            else
            {
                boxCollider.size = boxColliderSize + new Vector2(0, 0.15f);
            }
        }

        
        if (lastPosition == new Vector3(0.000001f, 0.000001f, 0.000001f)) 
        {
            lastPosition = transform.position;
            StartCoroutine(OnMove());
        }
        else if(invertable < 0)
        {
            RaycastHit2D raycast = Physics2D.Raycast(transform.position - new Vector3(0, 2f), -transform.up, 1);
            if (raycast.transform != null && (int)lastPosition.y != (int)transform.position.y && (int)lastPosition.x != (int)transform.position.x)
            {
                invertable *= -1;
                lastPosition = new Vector3(0.000001f, 0.000001f, 0.000001f);
            }
        }
    }

    private IEnumerator OnMove()
    {
        yield return new WaitForSeconds(stupiedTime);
        if (Vector3.Distance(transform.position, lastPosition) < stupiedTime * 2)
        {
            invertable *= -1;
        }
    }


    private IEnumerator OnPlayerAttacked()
    {
        if (player.transform.localScale.x > 1.4f && sized)
        {
            player.transform.localScale -= new Vector3(0.01f, 0.01f, 0);
            yield return new WaitForSeconds(0.05f);
            StartCoroutine(OnPlayerAttacked());
        }
        else if(player.transform.localScale.x < 1.5f)
        {
            sized = false;
            player.transform.localScale += new Vector3(0.01f, 0.01f, 0);
            yield return new WaitForSeconds(0.05f);
            StartCoroutine(OnPlayerAttacked());
        }
        else if(player.transform.localScale.x == 1.5f)
        {
            yield return null;
        }
    }

    private void Attack()
    {
        attacking = true;
        playerInformation.GiveDamage(damage);
        CameraController.Camera.GetComponent<CameraController>().OnHitted();
        StartCoroutine(AttackDelay());
        // StartCoroutine(OnPlayerAttacked());
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        attacking = false;
    }

    private void Update()
    {
        if (!player)
            return;

        distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance > attackDistance && distance < eyeDistance)
        {
            animationController.SetBool("Walk", true);
            CalculateMovement();
        }
        if(distance > eyeDistance)
        {
            animationController.SetBool("Walk", false);
        }

        if(!attacking && distance < attackDistance)
        {
            Attack();
            animationController.SetTrigger("Attack");
        }
    }
}
