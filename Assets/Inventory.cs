using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public struct PotionType
    {
        public string name { get; private set; }
        public Text text;
        public int amount;
        public bool available;

        public PotionType(string name, Text text, int amount)
        {
            this.name = name;
            this.text = text;
            this.amount = amount;
            available = amount > 0;   
        }

        public PotionType(string name, Text text)
        {
            this.name = name;
            this.text = text;
            amount = 0;
            available = false;
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
    [SerializeField] private Text shieldText;

    public PotionType currentPotion = new PotionType();
    private int currentPotionID = 0;

    public PotionType[] potions = new PotionType[4];

    private void Awake()
    {
        potions = new PotionType[4]
        {
           new PotionType("stamina", staminaText),
           new PotionType("health", healthText),
           new PotionType("jump", jumpText),
           new PotionType("shield", shieldText)
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
    }
}
