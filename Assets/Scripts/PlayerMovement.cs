using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask minigameLayer;
    [SerializeField] private LayerMask uiLayer;
    [SerializeField] private float moveSpeed = 6f;

    private PlayerControls controls;
    private Vector2 moveInput;
    public bool controlsEnabled = true;

    private string interactable;

    private QueueManager queueManager;
    private Minigame miniGame;

    Customer customerCollidingWith;

    ObjectManager objectManager;
    GameObject interactableCanvas;

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
        objectManager = FindObjectOfType<ObjectManager>();
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
                case "Customer":
                    customerCollidingWith.HelpCustomer();
                    break;
                default:
                    break;
            }
        }
    }

    private void Click(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePosition), Vector2.down, 0, minigameLayer);

        if (hit.collider != null)
        {
            GameObject objectHit = hit.collider.gameObject;

            // if clicked on a piece, trigger putting it back together
            if (objectHit.tag == "Fixable Piece")
            {
                miniGame.FixPiece(objectHit);
            }

            if (objectHit.tag == "Cancel")
            {
                miniGame.CancelGame();
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        // hide prompt
        interactableCanvas.SetActive(false);
        // reset interactable
        interactable = null;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // anvil, customer, cash register
        interactableCanvas = objectManager.GetInteractableCanvas(other);

        bool showAction = true;

        if (other.tag == "Customer")
        {
            // get customer object
            Customer customer = other.gameObject.GetComponent<Customer>();
            customerCollidingWith = customer;

            interactableCanvas = objectManager.GetCustomerCanvas(customer);

            // if at register or walking, do not show canvas
            if (customer.isAtRegister || !customer.isWaiting)
            {
                showAction = false;
            }
        }

        interactable = other.tag;
        interactableCanvas.SetActive(showAction);
    }

    public void ToggleControlsOnOrOff(bool isEnabled)
    {
        controlsEnabled = isEnabled;
    }
}
