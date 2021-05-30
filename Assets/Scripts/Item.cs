using UnityEngine;


public class Item : MonoBehaviour
{
    public GameObject itemPrefab;
    public Sprite itemPicture;
    public string itemName = string.Empty;

    private void Start()
    {
        PlayerController.Player.GetComponent<PlayerController>().inventory.AddSlot(this);
    }
}