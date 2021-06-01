using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int attackDistance = 5;
    [SerializeField] private int eyeDistance = 30;
    [SerializeField] private float movementSpeed = 0.5f;
    private GameObject player;
    private Rigidbody2D physics;
    private float distance = 0;
    private Vector2 scale = Vector2.zero;

    public int GetHealth() => health;

    public void SetHealth(int value) { health = value; OnHealthChanged(); }

    public void GiveDamage(int value) { health -= value; OnHealthChanged(); }

    private void OnHealthChanged()
    {
        if(GetHealth() == 0)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = PlayerController.Player;
        physics = GetComponent<Rigidbody2D>();

        scale = transform.localScale;
    }

    private void CalculateMovement()
    {
        float direction = (player.transform.position.x - transform.position.x) > 0 ? scale.x : -scale.x;
        transform.localScale = new Vector2(direction, transform.localScale.y);
        physics.velocity = new Vector2(direction / scale.x * movementSpeed, physics.velocity.y);
    }

    private void Attack()
    {
        print("АТАКА АААА");
    }

    private void Update()
    {
        distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance > attackDistance)
        {
            CalculateMovement();
        }
        else
        {
            Attack();
        }
    }
}
