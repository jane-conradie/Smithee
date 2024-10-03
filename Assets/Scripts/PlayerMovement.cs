using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    PlayerControls controls;
    Vector2 moveInput;

    string interactable;

    QueueManager queueManager;
    Minigame miniGame;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Interact.performed += ctx => Interact();

        controls.Player.Click.performed += Click;
    }

    void Start()
    {
        queueManager = QueueManager.instance;
        miniGame = FindObjectOfType<Minigame>();
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
            case "Anvil":
                miniGame.StartGame();
                break;
            default:
                break;
        }
    }

    void Click(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            GameObject objectHit = hit.collider.gameObject;

            // if clicked on a piece, trigger putting it back together
            if (objectHit.tag == "Fixable Piece")
            {
                miniGame.FixPiece(objectHit);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ToggleActionPrompt(other, true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        ToggleActionPrompt(other, false);
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
