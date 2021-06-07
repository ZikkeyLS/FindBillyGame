using UnityEngine;
using UnityEngine.UI;
using static PlayerController;

public class PlayerHealthBar : MonoBehaviour
{
    private Image image;
    private PlayerInformation information;

    void Start()
    {
        information = PlayerController.Player.GetComponent<PlayerController>().information;
        image = GetComponent<Image>();
    }
    void Update()
    {
        image.fillAmount = (((float)information.GetHealth() / 100f) + image.fillAmount) / 2;
    }
}
