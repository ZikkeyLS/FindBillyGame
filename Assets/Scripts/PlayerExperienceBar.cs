using UnityEngine;
using UnityEngine.UI;
using static PlayerController;

public class PlayerExperienceBar : MonoBehaviour
{
    private Image image;
    private PlayerInformation information;

    void Start()
    {
        information = PlayerController.Player.GetComponent<PlayerController>().information;
        image = GetComponent<Image>();
        information.GiveExperience(100);
    }
    void Update()
    {
        image.fillAmount = (information.GetExperienceRatio() + image.fillAmount) / 2;
    }
}
