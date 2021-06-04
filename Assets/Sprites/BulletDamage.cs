using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    private int damage = 10;

    public void SetDamage(int value) => damage = value;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {

        Enemy enemy = collision.transform.GetComponent<Enemy>();
        if(enemy != null) 
        { 
            enemy.GiveDamage(damage);
        }

        Destroy(gameObject);
    }
}
