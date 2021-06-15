using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformation : MonoBehaviour
{
    public static GameObject Player;
    public static PlayerInformation information;
    [SerializeField] private GameObject hitIndicator;

    private void Awake()
    {
        Player = gameObject;
        information = gameObject.GetComponent<PlayerInformation>();
    }

    [SerializeField] private int health = 100;

    public int GetHealth() => health;

    public void SetHealth(int value) { health = value; OnHealthChanged(false); }

    public void GiveDamage(int value) { health -= value; OnHealthChanged(true); }

    private void OnHealthChanged(bool damaged)
    {
        if (damaged)
        {
            hitIndicator.GetComponent<Image>().color = new Color(255, 0, 0, 0.196f);
        }
        else
        {
            hitIndicator.GetComponent<Image>().color = new Color(0, 255, 0, 0.294f);
        }

        StartCoroutine(AttackScreen());
        if (GetHealth() <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator AttackScreen()
    {
        hitIndicator.SetActive(true);

        yield return new WaitForSeconds(0.33f);
        hitIndicator.SetActive(false);
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

