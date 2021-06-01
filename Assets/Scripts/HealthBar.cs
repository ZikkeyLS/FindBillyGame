using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Enemy parent;
    private TMP_Text text;

    private void Start()
    {
        parent = GetComponentInParent<RectTransform>().GetComponentInParent<Enemy>();
        text = GetComponent<TMP_Text>();
    }

    private void LateUpdate()
    {
        string currentHealth = parent.GetHealth().ToString();
       
        if (text.text == currentHealth)
            return;
        text.text = currentHealth;
    }
}