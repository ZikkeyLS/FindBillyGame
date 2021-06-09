using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    public static GameObject Player;
    public static PlayerInformation information;

    private void Awake()
    {
        Player = gameObject;
        information = gameObject.GetComponent<PlayerInformation>();
    }

    [SerializeField] private int health = 100;

    public int GetHealth() => health;

    public void SetHealth(int value) { health = value; OnHealthChanged(); }

    public void GiveDamage(int value) { health -= value; OnHealthChanged(); }

    private void OnHealthChanged()
    {
        if (GetHealth() <= 0)
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private int experience = 0;
    [SerializeField] private int needableExpirience = 100;
    [SerializeField] private int level = 0;

    public int GetExperience() => experience;

    public int GetLevel() => level;

    public int GetNeedableExperience() => needableExpirience;

    public float GetExperienceRatio()
    {
        return (float)experience / (float)needableExpirience;
    }

    private void OnGiveExperience()
    {
        while (experience >= needableExpirience)
        {
            experience -= needableExpirience;
            level++;
            float newNeedableExperience = (float)needableExpirience + (float)needableExpirience * 0.2f;
            needableExpirience = (int)newNeedableExperience;
        }
    }

    public void GiveExperience(int amount)
    {
        experience += amount;
        OnGiveExperience();
    }

    [SerializeField] private int components = 0;

    public int GetComponents() => components;

    public void GiveComponents(int amount)
    {
        components += amount;
    }
}

