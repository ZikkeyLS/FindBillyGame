using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public struct PotionType
    {
        public string name { get; private set; }
        public Text text;
        public int amount;
        public int cost;
        public bool available;

        public PotionType(string name, Text text, int amount, int cost)
        {
            this.name = name;
            this.text = text;
            this.amount = amount;
            available = amount > 0;
            this.cost = cost;
        }

        public PotionType(string name, Text text, int cost)
        {
            this.name = name;
            this.text = text;
            amount = 0;
            available = false;
            this.cost = cost;
        }


        public void AddPotion()
        {
            amount += 1;
            OnAmountChanged();
        }

        public void RemovePotion()
        {
            if(amount > 0) amount -= 1;
            if (amount == 0) available = false;
            OnAmountChanged();
        }

        private void OnAmountChanged()
        {
            text.text = amount.ToString();
        }
    }

    [SerializeField] private Text healthText;
    [SerializeField] private Text staminaText;
    [SerializeField] private Text jumpText;

    public static PotionType currentPotion;
    public static int currentPotionID = 0;
    [SerializeField] private PlayerEquipper equipper;

    public static PotionType[] potions = new PotionType[4];
    [SerializeField] private int healAmount = 25;

    private PlayerInformation playerInformation;
    private PlayerUI playerUI;

    private void Awake()
    { 
        potions = new PotionType[3]
        {
           new PotionType("stamina", staminaText, 2),
           new PotionType("health", healthText, 3),
           new PotionType("jump", jumpText, 4)
        };
    }

    private void SelectPotion(int id, float a)
    {
        Image image = potions[id].text.GetComponentInParent<Image>();
        Color color = image.color;
        image.color = new Color(color.r, color.g, color.b, a);
    }

    private void Start()
    {
        playerInformation = PlayerInformation.information;
        playerUI = playerInformation.GetComponent<PlayerUI>();
        SelectPotion(currentPotionID, 1);
        currentPotion = potions[currentPotionID];
    }

    public void Update()
    {
        float mw = Input.GetAxis("Mouse ScrollWheel");

        if (mw > 0.05f)
        {
            currentPotionID += 1;
            if (currentPotionID > potions.Length - 1) currentPotionID = 0;

            SelectPotion(currentPotionID, 1);
            currentPotion = potions[currentPotionID];
        }
        if(mw < -0.05f)
        {
            currentPotionID -= 1;
            if (currentPotionID < 0) currentPotionID = potions.Length - 1;

            SelectPotion(currentPotionID, 1);
            currentPotion = potions[currentPotionID];
        }

        for(int i = 0; i < potions.Length; i++)
        {
            if(potions[i].name != currentPotion.name)
            {
                SelectPotion(i, 0.372f);
            }
        }


        if (Input.GetKeyDown(KeyCode.E) && !PlayerUI.canContact)
        {
            if (currentPotion.amount == 0)
                return;



            switch (currentPotion.name) 
            {
                case "health":
                    if(PlayerInformation.information.GetHealth() == 100)
                    {
                        StartCoroutine(playerUI.ShowHpInfo(2));
                        return;
                    }
                    equipper.EquipHealthPotion(healAmount);
                    playerUI.OnPotion(PlayerUI.Potions.health);
                    break;
                case "stamina":
                    equipper.EquipStaminaPotion();
                    playerUI.OnPotion(PlayerUI.Potions.stamina);
                    break;
                case "jump":
                    equipper.EquipJumpPotion();
                    playerUI.OnPotion(PlayerUI.Potions.jump);
                    break;
            }

            currentPotion.RemovePotion();
        }
    }
}
