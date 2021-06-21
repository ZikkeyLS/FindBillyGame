using UnityEngine;

public class PlayerEquipper : MonoBehaviour
{
    private PlayerInformation playerInformation;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerInformation = GetComponent<PlayerInformation>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void EquipHealthPotion(int amount)
    {
        playerInformation.SetHealth(Mathf.Clamp(playerInformation.GetHealth() + amount, 0, 100));
    }

    public void EquipStaminaPotion()
    {
        playerMovement.OnStaminaPotion();
    }

    public void EquipJumpPotion()
    {
        playerMovement.OnJumpPotion();
    }

    public void EquipShieldPotion()
    {

    }
}
