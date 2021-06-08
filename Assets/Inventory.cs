using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public struct PotionType
    {
        public string name { get; private set; }
        public Text text;
        public int amount;

        public PotionType(string name, Text text, int amount)
        {
            this.name = name;
            this.text = text;
            this.amount = amount;
        }

        public PotionType(string name, Text text)
        {
            this.name = name;
            this.text = text;
            amount = 0;
        }
    }

    [SerializeField] private Text healthText;
    [SerializeField] private Text staminaText;
    [SerializeField] private Text jumpText;
    [SerializeField] private Text shieldText;

    public PotionType currentPotion = new PotionType();
    private int currentPotionID = -1;

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

    public void Update()
    {
        float mw = Input.GetAxis("Mouse ScrollWheel");

        if (mw > 0.05f)
        {
            currentPotionID += 1;
            if (currentPotionID > potions.Length - 1) currentPotionID = 0;

            Image image = potions[currentPotionID].text.GetComponentInParent<Image>();
            Color color = image.color;
            image.color = new Color(color.r, color.g, color.b, 255);
            currentPotion = potions[currentPotionID];
        }
        if(mw < -0.05f)
        {
            currentPotionID -= 1;
            if (currentPotionID < 0) currentPotionID = potions.Length - 1;

            Image image = potions[currentPotionID].text.GetComponentInParent<Image>();
            Color color = image.color;
            image.color = new Color(color.r, color.g, color.b, 255);
            currentPotion = potions[currentPotionID];
        }

        for(int i = 0; i < potions.Length; i++)
        {
            if(potions[i].name != currentPotion.name)
            {
                Image image = potions[i].text.GetComponentInParent<Image>();
                Color color = image.color;
                image.color = new Color(color.r, color.g, color.b, 0.372f);
            }
        }
    }
}
