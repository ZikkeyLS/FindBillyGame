using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public GameObject activeElement;

    [SerializeField] GameObject craftTable;

    public void OpenCraftTable()
    {
        craftTable.SetActive(true);
        activeElement = craftTable;
    }

    public void CloseCraftTable()
    {
        craftTable.SetActive(false);
        activeElement = null;
    }

    private void Update()
    {

        if (activeElement != null && Input.GetKeyDown(KeyCode.Escape))
        {
            activeElement.SetActive(false);
            activeElement = null;
        }
    }
}