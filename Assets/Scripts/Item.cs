using UnityEngine;


public class Item : MonoBehaviour
{
    public GameObject itemPrefab;
    public Sprite itemPicture;
    public string itemName = string.Empty;

    private GameObject player;

    private void Start()
    {
        player = PlayerController.Player;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && Vector3.Distance(player.transform.position, transform.position) < 3)
        {
          //  if (player.GetComponent<PlayerController>().inventory.AddSlot(this))
          //      Destroy(gameObject);
        }
    }
}