using UnityEngine;

public class Cauldron : MonoBehaviour
{
    [SerializeField] GameObject text;
    private GameObject player;
    private PlayerUI ui;
    [SerializeField] private float contactDistance = 2;

    private void Start()
    {
        player = PlayerInformation.Player;
        ui = player.GetComponent<PlayerUI>();
    }

    void Update()
    {
        if (player == null)
            return;

        bool canContact = Vector3.Distance(transform.position, player.transform.position) < contactDistance;
        if(text.activeSelf != canContact) text.SetActive(canContact);
        if (canContact) PlayerUI.canContact = true;
        if (canContact && Input.GetKeyDown(KeyCode.E))
        {
            ui.OpenCraftTable();
        }

    }
}
