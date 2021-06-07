using System.Collections;
using UnityEngine;
using static PlayerController;

public class Trap : MonoBehaviour
{
    [SerializeField] private int damage = 25;
    private bool attack = true;

    private void OnCollisionStay2D(Collision2D collision)
    {
        Enemy enemy = collision.transform.GetComponent<Enemy>();
        PlayerInformation playerInformation = collision.transform.GetComponent<PlayerController>().information;

        if (attack) 
        {
            if(enemy != null)
            {
                AttackEnemy(enemy);
            }
            if (playerInformation != null)
            {
                AttackPlayer(playerInformation);
            }
        } 
    }

    private void AttackEnemy(Enemy enemy)
    {
        attack = false;
        enemy.GiveDamage(damage);
        StartCoroutine(AttackDelay());
    }

    private void AttackPlayer(PlayerInformation playerInformation)
    {
        attack = false;
        playerInformation.GiveDamage(damage);
        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(1f);
        attack = true;
    }
}
