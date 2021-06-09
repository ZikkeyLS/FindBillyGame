﻿using UnityEngine;
using UnityEngine.UI;

public class PlayerExperienceBar : MonoBehaviour
{
    private Image image;
    private PlayerInformation information;

    void Start()
    {
        information = PlayerInformation.Player.GetComponent<PlayerInformation>();
        image = GetComponent<Image>();
    }
    void Update()
    {
        image.fillAmount = (information.GetExperienceRatio() + image.fillAmount) / 2;
    }
}
