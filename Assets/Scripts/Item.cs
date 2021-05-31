using System.Collections;
using UnityEngine;


public class Item : MonoBehaviour
{
    public GameObject itemPrefab;
    public Sprite itemPicture;
    public string itemName = string.Empty;

    private void Start()
    {
        StartCoroutine(AddSlot());
    }

    private IEnumerator AddSlot()
    {
        PlayerController.Player.GetComponent<PlayerController>().inventory.AddSlot(this);
        yield return new WaitForSeconds(3f);
        StartCoroutine(AddSlot());
    }
}