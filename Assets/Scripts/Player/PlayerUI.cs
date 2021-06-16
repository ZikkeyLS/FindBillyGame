using System.Collections;
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

    [Header("PotionInformation")]
    [SerializeField] Image potionStatus;
    [SerializeField] private Image potionChild;
    [SerializeField] Sprite HealthIcon;
    [SerializeField] Sprite StaminaIcon;
    [SerializeField] Sprite JumpIcon;
    [SerializeField] Text hpInfo;


    private void UpdateCurrentPotion()
    {
        currentPotion = potions[currentPotionID];
    }

    public void AddHealthPotion()
    {
        information.GiveComponents(-potions[1].cost);
        potions[1].AddPotion();
        UpdateCurrentPotion();
    }

    public void AddStaminaPotion()
    {
        information.GiveComponents(-potions[0].cost);
        potions[0].AddPotion();
        UpdateCurrentPotion();
    }

    public void AddJumpPotion()
    {
        information.GiveComponents(-potions[2].cost);
        potions[2].AddPotion();
        UpdateCurrentPotion();
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

    public enum Potions
    {
        health,
        stamina,
        jump
    }

    private IEnumerator CircleEffect(float secondsTime)
    {

        while(potionStatus.fillAmount > 0)
        {
            potionStatus.fillAmount -= 0.01f;
            yield return new WaitForSeconds(secondsTime / 100);
        }

        potionStatus.gameObject.SetActive(false);
    }

    public IEnumerator ShowHpInfo(float secondsTime)
    {
        hpInfo.gameObject.SetActive(true);
        yield return new WaitForSeconds(secondsTime);
        hpInfo.gameObject.SetActive(false);
    }

    public void OnPotion(Potions potionType)
    {
        potionStatus.gameObject.SetActive(true);
        potionStatus.fillAmount = 1;

        switch (potionType)
        {
            case Potions.health:
                potionChild.sprite = HealthIcon;
                StartCoroutine(CircleEffect(2));
                break;
            case Potions.stamina:
                potionChild.sprite = StaminaIcon;
                StartCoroutine(CircleEffect(10));
                break;
            case Potions.jump:
                potionChild.sprite = JumpIcon;
                StartCoroutine(CircleEffect(10));
                break;
        }


    }
}