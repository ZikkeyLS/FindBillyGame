using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private Image image;
    private PlayerInformation information;

    void Start()
    {
        information = PlayerInformation.information;
        image = GetComponent<Image>();
    }
    void Update()
    {
        image.fillAmount = (((float)information.GetHealth() / 100f) + image.fillAmount) / 2;
    }
}
