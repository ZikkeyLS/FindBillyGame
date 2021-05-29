using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public int GetHealth() => health;

    public void SetHealth(int value) { health = value; OnHealthChanged(); }

    public void GiveDamage(int value) { health = health - value; OnHealthChanged(); }

    private void OnHealthChanged()
    {
        if(GetHealth() == 0)
        {
            Destroy(gameObject);
        }
    }
}
