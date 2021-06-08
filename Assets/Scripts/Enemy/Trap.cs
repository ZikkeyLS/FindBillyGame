using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private List<Enemy> currentEnemies = new List<Enemy>();
    [SerializeField] private int damage = 25;
    private bool attack = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.transform.GetComponent<Enemy>();
        PlayerInformation playerInformation = collision.transform.GetComponent<PlayerInformation>();

        if (attack) 
        {
            if(enemy != null)
            {
                StartCoroutine(AttackEnemy(enemy));
            }
            if (playerInformation != null)
            {
                StartCoroutine(AttackPlayer(playerInformation));
            }
        } 
    }

    private IEnumerator AttackEnemy(Enemy enemy)
    {
        enemy.GiveDamage(damage);
        yield return new WaitForSeconds(1f);
        if(enemy.GetHealth() > 0)
            StartCoroutine(AttackEnemy(enemy));
    }

    private IEnumerator AttackPlayer(PlayerInformation playerInformation)
    {
        playerInformation.GiveDamage(damage);
        yield return new WaitForSeconds(1f);
        if (playerInformation.GetHealth() > 0)
            StartCoroutine(AttackPlayer(playerInformation));
    }
}
