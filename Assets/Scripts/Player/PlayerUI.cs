using UnityEngine;
using UnityEngine.UI;
using static Inventory;

public class PlayerUI : MonoBehaviour
{
    public static bool canContact;
    public GameObject activeElement;

    [SerializeField] GameObject craftTable;
    private PlayerInformation information;

    [Header("CraftTable")]
    [SerializeField] private Text healthStatus;
    [SerializeField] private Text staminaStatus;
    [SerializeField] private Text jumpStatus;
    [SerializeField] private Button healthButton;
    [SerializeField] private Button staminaButton;
    [SerializeField] private Button jumpButton;
    private PotionType[] potions;

    public void AddHealthPotion()
    {
        information.GiveComponents(-potions[1].cost);
        potions[1].AddPotion();
    }

    public void AddStaminaPotion()
    {
        information.GiveComponents(-potions[0].cost);
        potions[0].AddPotion();
    }

    public void AddJumpPotion()
    {
        information.GiveComponents(-potions[2].cost);
        potions[2].AddPotion();
    }

    public void AddShieldPotion()
    {
        information.GiveComponents(-potions[3].cost);
        potions[3].AddPotion();
    }

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

    private void Start()
    {
        potions = Inventory.potions;
        information = PlayerInformation.information;
    }

    private void ChangePotionInformationText(int potion, Text text)
    {
        if (text.text != potions[potion].cost.ToString()) { text.text = potions[potion].cost.ToString(); }
    }

    private void ChangeButtonState(int needableAmount, int currentAmount, Button button)
    {
        button.interactable = needableAmount <= currentAmount;
    }

    private void Update()
    {
        if (canContact) canContact = false;
        ChangePotionInformationText(1, healthStatus);
        ChangePotionInformationText(0, staminaStatus);
        ChangePotionInformationText(2, jumpStatus);

        int components = information.GetComponents();
        ChangeButtonState(potions[1].cost, components, healthButton);
        ChangeButtonState(potions[0].cost, components, staminaButton);
        ChangeButtonState(potions[2].cost, components, jumpButton);

        if (activeElement != null && Input.GetKeyDown(KeyCode.Escape))
        {
            activeElement.SetActive(false);
            activeElement = null;
        }
    }
}