using UnityEngine;

public class CraftTable : MonoBehaviour
{
    private GameObject player;

    private PlayerMovement movement;
    private PlayerShoot shoot;

    private void OnEnable()
    {
        if(player == null) player = PlayerInformation.Player;
        if (movement == null) movement = player.GetComponent<PlayerMovement>();
        if (shoot == null) shoot = player.GetComponent<PlayerShoot>();

        movement.movementEnabled = false;
        shoot.shootEnabled = false;
    }

    private void OnDisable()
    {
        movement.movementEnabled = true;
        shoot.shootEnabled = true;
    }
}
