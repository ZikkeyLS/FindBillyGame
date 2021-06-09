using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentCounterBar : MonoBehaviour
{
    private Text text;
    private PlayerInformation information;

    void Start()
    {
        text = GetComponent<Text>();
        information = PlayerInformation.information;
    }

    void LateUpdate()
    {
        text.text = information.GetComponents().ToString();
    }
}
