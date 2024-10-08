using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private PlayerControls controls;
    private Vector2 moveInput;

    private string interactable;

    private QueueManager queueManager;
    private Minigame miniGame;

    public bool controlsEnabled = true;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Interact.performed += ctx => Interact();

        controls.Player.Click.performed += Click;
    }

    private void Start()
    {
        queueManager = QueueManager.instance;
        miniGame = FindObjectOfType<Minigame>();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (controlsEnabled)
        {
            Vector2 movement = new Vector2(moveInput.x, moveInput.y) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }
    }

    private void Interact()
    {
        if (controlsEnabled)
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
                    controlsEnabled = false;
                    break;
                default:
                    break;
            }
        }
    }

    private void Click(InputAction.CallbackContext context)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        ToggleActionPrompt(other, true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ToggleActionPrompt(other, false);
    }

    private void ToggleActionPrompt(Collider2D other, bool showAction)
    {
        // find canvas
        GameObject canvas = other.transform.Find("Canvas").gameObject;

        // set canvas active/not active
        canvas.SetActive(showAction);

        interactable = showAction ? other.name : "";
    }

    public void ToggleControlsOnOrOff(bool isEnabled)
    {
        controlsEnabled = isEnabled;
    }
}
