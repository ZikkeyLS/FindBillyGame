using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class PlayerController : MonoBehaviour
{
    public class PlayerMovement
    {
        private bool grounded = false;
        private bool canJump = true;

        private float speed = 5;
        private float jumpHeight = 175;

        private Animator animator;
        private Rigidbody2D physics = new Rigidbody2D();
        private PlayerController controller;
        private Transform transform;
        private LayerMask jumpLayers;

        private void SetScale(int x)
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
                int scaleX = (movementSpeed > 0) ? -1 : 1;
                SetScale(scaleX);
            }
            animator.SetBool("walking", movement);
        }
        private void Jump()
        {
            grounded = Physics2D.OverlapCircle(transform.position + new Vector3(0.25f, -1.5f, 0), 0.75f, jumpLayers) != null;

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
        }

        public void Update()
        {
            Move();
            Jump();
        }

        public IEnumerator waitToUpdateJump()
        {
            yield return new WaitForSeconds(1f);
            canJump = true;

        }
    }

    public class PlayerShoot
    {
        private PlayerController controller;
        public Animator animator;
        public bool canShoot = true;
        private float distance = 100f;

        public void Start(PlayerController playerController)
        {
            controller = playerController;
            animator = playerController.GetComponent<Animator>();
        }

        private void Shoot()
        {
            RaycastHit2D hit = Physics2D.Raycast(controller.transform.position, controller.transform.position + new Vector3(-controller.transform.localScale.x * distance, 0, 0), distance, LayerMask.GetMask("Enemy"));
            animator.SetTrigger("shoot");

            if (hit.collider == null)
                return;
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
                enemy.GiveDamage(10);
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

        private Slot[] slots = new Slot[15];
        private int currentSlot = 1;
        private GameObject[] slotsContainer;
        private Text slotNameContainer;

        public void Start(GameObject[] slotsContainer, Text slotNameContainer)
        {
            this.slotsContainer = slotsContainer;
            this.slotNameContainer = slotNameContainer;

            print(slotsContainer.Length);

            for(int i = 0; i < slotsContainer.Length; i++)
            {
                Button button = slotsContainer[i].GetComponent<Button>();
                button.onClick.AddListener(delegate { OnButtonClick(button.gameObject); });
            }
        }

        private void OnButtonClick(GameObject gameObject)
        {
           int id = Int32.Parse(gameObject.name);
           slotNameContainer.text = slots[id - 1].itemName;
        }

        private void test(string name)
        {
            print(name);
        }


        public void AddSlot(Item item)
        {
            Slot slot = new Slot();
            slot.Initialize(item.itemPrefab, item.itemPicture, item.itemName);

            if (currentSlot == 15)
                return;

            slots[currentSlot - 1] = slot;
            slotsContainer[currentSlot - 1].GetComponent<Image>().sprite = item.itemPicture;

            currentSlot++;
        }
    }

    public static GameObject Player;

    [SerializeField] private GameObject[] slotsContainer;
    [SerializeField] private GameObject slotNameContainer;

    private PlayerMovement movement = new PlayerMovement();
    private PlayerShoot shoot = new PlayerShoot();
    public Inventory inventory = new Inventory();

    private void Awake()
    {
        Player = gameObject;
        movement.Start(this);
        shoot.Start(this);
        inventory.Start(slotsContainer, slotNameContainer.GetComponent<Text>());
    }

    private void Update()
    {
        movement.Update();
        shoot.Update();
        
    }
}
