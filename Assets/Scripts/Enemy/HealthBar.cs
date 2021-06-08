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
        float direction = parent.transform.localScale.x > 0 ? 1 : -1;
        transform.localScale = new Vector2(direction, 1);
        string currentHealth = parent.GetHealth().ToString();
       
        if (text.text == currentHealth)
            return;
        text.text = currentHealth;
    }
}