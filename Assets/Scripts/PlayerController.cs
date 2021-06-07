using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public class PlayerMovement
    {
        private bool grounded = false;
        private bool canJump = true;
        private bool canStare = false;
        private bool stareCore =  true;

        private float speed = 5;
        private float jumpHeight = 500;

        private Animator animator;
        private Rigidbody2D physics = new Rigidbody2D();
        private PlayerController controller;
        private Transform transform;
        private LayerMask jumpLayers;

        private Vector2 scale;

        private void SetScale(float x)
        {
            physics.transform.localScale = new Vector2(x, physics.transform.localScale.y);
        }

        private void Move()
        {
            int moveA = Input.GetKey(KeyCode.A) ? -1 : 0;
            int moveD = Input.GetKey(KeyCode.D) ? 1 : 0;


            float movementSpeed = (moveA + moveD) * speed;

            Vector2 movementDirection = new Vector2(movementSpeed, physics.velocity.y);
            physics.velocity = movementDirection;

            bool movement = movementSpeed != 0;

            if (movement)
            {
                float scaleX = (movementSpeed > 0) ?  scale.x : -scale.x;
                SetScale(scaleX);
            }
            if (!canStare && stareCore) controller.StartCoroutine(StareDelay());

            if(canStare && !movement)
            {
                print("1");
                animator.SetTrigger("stare");
                canStare = false;
            }

            animator.SetBool("walking", movement);
        }

        IEnumerator StareDelay()
        {
            stareCore = false;

            yield return new WaitForSeconds(7f);
            if (animator.playableGraph.GetEditorName() != "StaticAnimation")
            {
                controller.StartCoroutine(StareDelay());
            }
            else
            {
                canStare = true;
            }

            stareCore = true;
        }

        private void Jump()
        {
            grounded = Physics2D.OverlapCircle(transform.position + new Vector3(0.25f, -1.5f * Mathf.Abs(scale.x), 0), 0.75f, jumpLayers) != null;

            if (canJump && grounded && Input.GetKeyDown(KeyCode.Space))
            {
                physics.AddForce(Vector2.up * jumpHeight);
                canJump = false;
                animator.SetTrigger("jump");
                controller.StartCoroutine(waitToUpdateJump());
            }
        }

        public void Start(PlayerController playerController)
        {
            physics = playerController.GetComponent<Rigidbody2D>();
            animator = playerController.GetComponent<Animator>();
            transform = physics.transform;
            controller = playerController;
            jumpLayers = LayerMask.GetMask("Ground");
            scale = transform.localScale;
        }

        public void Update()
        {
            Move();
            Jump();
        }

        public IEnumerator waitToUpdateJump()
        {
            yield return new WaitForSeconds(2f);
            canJump = true;

        }
    }

    public class PlayerShoot
    {
        private PlayerController controller;
        public Animator animator;
        public bool canShoot = true;
        private GameObject bullet;

        public void Start(PlayerController playerController, GameObject bullet)
        {
            controller = playerController;
            animator = playerController.GetComponent<Animator>();
            this.bullet = bullet;
        }

        private void Shoot()
        {
            CameraController.Camera.GetComponent<CameraController>().OnAttack();
            animator.SetTrigger("shoot");

            GameObject currentBullet = Instantiate(bullet, controller.transform.position + new Vector3(-controller.transform.localScale.x * 1.25f, -0.6f), controller.transform.rotation);
            currentBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(-controller.transform.localScale.x * 5, 0), ForceMode2D.Impulse);
        }

        public void Update()
        {

            if (Input.GetKey(KeyCode.Mouse0) && canShoot)
            {
                canShoot = false;
                controller.StartCoroutine(sprayTime());
                Shoot();
            }
            Debug.DrawLine(controller.transform.position, controller.transform.position + new Vector3(10 * -controller.transform.localScale.x, 0, 0), Color.yellow);
        }

        IEnumerator sprayTime()
        {
            yield return new WaitForSeconds(0.75f);
            canShoot = true;
        }
    }

    public class Inventory
    {
        public struct Slot
        {
            public GameObject itemPrefab;
            public Sprite itemPicture;
            public string itemName;

            public void Initialize(GameObject itemPrefab, Sprite itemPicture, string itemName)
            {
                this.itemPrefab = itemPrefab;
                this.itemPicture = itemPicture;
                this.itemName = itemName;
            }

            public void DropObject(Transform player)
            {
                Instantiate(itemPrefab, player.position + new Vector3(2 * -player.localScale.x, 0), Quaternion.identity);
            }
        }

        private readonly int slotsCount = 15;
        private Slot[] slots = new Slot[15];
        private int currentSlot = 1;
        private GameObject[] slotsContainer;
        private Text slotNameContainer;
        private Button slotDropButton;
        private Sprite itemIcon;
        private Slot selectedSlot;
        private int selectedID = 0;

        public void Start(GameObject[] slotsContainer, Text slotNameContainer, Button slotDropButton, Sprite itemIcon)
        {
            this.slotsContainer = slotsContainer;
            this.slotNameContainer = slotNameContainer;
            this.slotDropButton = slotDropButton;
            this.itemIcon = itemIcon;

            for(int i = 0; i < slotsContainer.Length; i++)
            {
                Button button = slotsContainer[i].GetComponent<Button>();
                button.onClick.AddListener(delegate { OnInfoButtonClick(button.gameObject); });
            }

            slotDropButton.onClick.AddListener(OnDropButtonClick);
        }

        private void OnDropButtonClick()
        {
            slotDropButton.gameObject.SetActive(false);
            slotNameContainer.text = string.Empty;
            selectedSlot.DropObject(Player.transform);
            slots[selectedID] = new Slot();
            slotsContainer[selectedID].GetComponent<Image>().sprite = itemIcon;
            currentSlot--;
        }

        private void OnInfoButtonClick(GameObject gameObject)
        {
            int id = Int32.Parse(gameObject.name);
            if (slots[id - 1].itemPrefab == null)
                return;

            slotNameContainer.text = slots[id - 1].itemName;
            slotDropButton.gameObject.SetActive(true);
            selectedSlot = slots[id - 1];
            selectedID = id - 1;
        }

        public bool AddSlot(Item item)
        {
            Slot slot = new Slot();
            slot.Initialize(item.itemPrefab, item.itemPicture, item.itemName);

            if (currentSlot > slotsCount)
                return false;

            for (int i = 0; i < slotsCount; i++)
            {
                if(slots[i].itemPrefab == null)
                {
                    slots[i] = slot;
                    slotsContainer[i].GetComponent<Image>().sprite = item.itemPicture;
                    break;
                }
            }

            currentSlot++;

            return true;
        }
    }

    public class PlayerInformation
    {
        private GameObject gameObject;

        public void Start(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        private int health = 100;

        public int GetHealth() => health;

        public void SetHealth(int value) { health = value; OnHealthChanged(); }

        public void GiveDamage(int value) { health -= value; OnHealthChanged(); }

        private void OnHealthChanged()
        {
            if (GetHealth() <= 0)
            {
                Destroy(gameObject);
            }
        }

        private int experience = 0;
        private int needableExpirience = 100;
        private int level = 0;

        public int GetExperience() => experience;
        public int GetNeedableExperience() => needableExpirience;

        public float GetExperienceRatio()
        {
            return (float)experience / (float)needableExpirience;
        }

        private void OnGiveExperience()
        {
            while(experience >= needableExpirience)
            {
                experience -= needableExpirience;
                level++;
                float newNeedableExperience = (float)needableExpirience + (float)needableExpirience * 0.2f;
                needableExpirience = (int)newNeedableExperience;
            }
        }

        public void GiveExperience(int amount)
        {
            experience += amount;
            OnGiveExperience();
        }
    }

    public static GameObject Player;

    public GameObject activeElement;

    [Header("Shoot")]
    [SerializeField] private GameObject bullet;

    private PlayerMovement movement = new PlayerMovement();
    private PlayerShoot shoot = new PlayerShoot();
    public PlayerInformation information = new PlayerInformation();

    private bool needToUpdate = true;

    private void Awake()
    {
        Player = gameObject;
        movement.Start(this);
        shoot.Start(this, bullet);
        information.Start(gameObject);
    }

    private void Update()
    {

        if (activeElement != null && Input.GetKeyDown(KeyCode.Escape))
        {
            activeElement.SetActive(false);
        }

        if (needToUpdate)
        {
            movement.Update();
            shoot.Update();
        }
    }
}