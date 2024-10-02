using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    PlayerControls controls;
    Vector2 moveInput;

    string interactable;

    QueueManager queueManager;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Interact.performed += ctx => Interact();
    }

    void Start()
    {
        queueManager = QueueManager.instance;
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector2 movement = new Vector2(moveInput.x, moveInput.y) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    void Interact()
    {
        switch (interactable)
        {
            // when player presses interact at cash register
            // check the customer out
            case "Cash Register":
                queueManager.CheckoutCustomer();
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Cash Register")
        {
            // show action button
            ToggleActionPrompt(other, true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Cash Register")
        {
            ToggleActionPrompt(other, false);
        }
    }

    void ToggleActionPrompt(Collider2D other, bool showAction)
    {
        // find canvas
        GameObject canvas = other.transform.Find("Canvas").gameObject;

        // set canvas active/not active
        canvas.SetActive(showAction);

        interactable = showAction ? other.name : "";
    }


}
