using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    // =========================
    // MOVEMENT
    // =========================
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runMultiplier = 1.5f;

    // =========================
    // ECONOMY
    // =========================
    [Header("Economy")]
    [SerializeField] private int money = 100;
    public int Money => money;

    // =========================
    // INVENTORY (OWNED BY PLAYER)
    // =========================
    [Header("Inventory")]
    public Inventory inventory;

    // =========================
    // REFERENCES
    // =========================
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [Header("UI")]
    [SerializeField] private UI_Inventory uiInventory;

    // =========================
    // INTERNAL STATE
    // =========================
    private Vector2 moveInput;

    // =========================
    // UNITY LIFECYCLE
    // =========================

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (inventory == null)
            inventory = new Inventory();
    }

    private void Start()
    {
        if (uiInventory != null)
            uiInventory.SetInventory(inventory);
    }

    private void Update()
    {
        ReadInput();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    // =========================
    // INPUT & MOVEMENT
    // =========================

    private void ReadInput()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
    }

    private void Move()
    {
        float speed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            speed *= runMultiplier;

        rb.linearVelocity = moveInput * speed;
    }

    private void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetFloat("X", moveInput.x);
        animator.SetFloat("Y", moveInput.y);
        animator.SetBool("Moving", moveInput.sqrMagnitude > 0.01f);
    }

    // =========================
    // INVENTORY API
    // =========================

    public bool UseItem(ItemType type)
    {
        return inventory.UseItem(type);
    }

    public void GainItem(ItemType type, int amount)
    {
        inventory.GainItem(type, amount);
    }

    public int GetItemAmount(ItemType type)
    {
        return inventory.GetItemAmount(type);
    }

    // =========================
    // STORE API
    // =========================

    public bool TryBuy(int totalPrice, ItemType type, int amount)
    {
        if (money < totalPrice)
            return false;

        money -= totalPrice;
        inventory.GainItem(type, amount);
        return true;
    }

    public bool TrySell(int totalPrice, ItemType type, int amount)
    {
        if (inventory.GetItemAmount(type) < amount)
            return false;

        inventory.RemoveItem(type, amount);
        money += totalPrice;
        GameSession.Instance.RecordMoney(totalPrice);
        return true;
    }

    // =========================
    // DEBUG / RESET
    // =========================

    public void DebugInventory()
    {
        string content = "";
        foreach (var item in inventory.GetItems())
            content += $"{item.itemType}({item.amount}) ";

        Debug.Log($"Money: {money} | Inventory: {content}");
    }

    public void ResetPlayer()
    {
        money = 100;
        inventory = new Inventory();

        if (uiInventory != null)
            uiInventory.SetInventory(inventory);
    }
}
