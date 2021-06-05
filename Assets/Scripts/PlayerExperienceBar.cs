using UnityEngine;
using UnityEngine.UI;
using static PlayerController;

public class PlayerExperienceBar : MonoBehaviour
{
    [SerializeField] Text experienceText;
    private PlayerInformation information;

    void Start()
    {
        information = PlayerController.Player.GetComponent<PlayerController>().information;
        information.GiveExperience(100);
    }
    void Update()
    {
        transform.localScale = new Vector2(information.GetExperienceRatio(), transform.localScale.y);
        experienceText.text = information.GetExperience() + "/" + information.GetNeedableExperience();
    }
}
