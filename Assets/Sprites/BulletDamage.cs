using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    private int damage = 10;
    private Camera mainCamera; 

    public void SetDamage(int value) => damage = value;

    private void OnEnable()
    {
        mainCamera = Camera.main;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Enemy enemy = collision.transform.GetComponent<Enemy>();
        if(enemy != null) 
        { 
            enemy.GiveDamage(damage);
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        Vector3 real = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight / 2));

        if (transform.position.x > real.x)
            Destroy(gameObject);
    }
}
