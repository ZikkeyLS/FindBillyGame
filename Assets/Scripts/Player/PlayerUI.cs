using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public GameObject activeElement;

    private void Update()
    {

        if (activeElement != null && Input.GetKeyDown(KeyCode.Escape))
        {
            activeElement.SetActive(false);
        }
    }
}