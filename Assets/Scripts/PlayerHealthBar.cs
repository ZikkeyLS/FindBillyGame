using UnityEngine;
using UnityEngine.UI;
using static PlayerController;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] Text healthText;
    private PlayerInformation information;

    void Start()
    {
        information = PlayerController.Player.GetComponent<PlayerController>().information;
    }
    void Update()
    {
        transform.localScale = new Vector2((float)information.GetHealth() / 100, transform.localScale.y);
        healthText.text = information.GetHealth() + "/100";
    }
}
