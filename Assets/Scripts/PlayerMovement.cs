using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask minigameLayer;
    [SerializeField] private LayerMask uiLayer;
    [SerializeField] private float moveSpeed = 6f;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private string[] interactables = new string[3];

    // animation
    [SerializeField] private Animator animator;

    private PlayerControls controls;
    private Vector2 moveInput;
    public bool controlsEnabled = true;

    private string interactable;

    private QueueManager queueManager;
    private Anvil miniGame;

    private ObjectManager objectManager;
    private GameObject interactableCanvas;

    private Collider2D collidingObject;

    private AnimationManager animationManager;

    private Vector2 previousMoveInput;

    private MinigameManager minigameManager;
    private GameObject minigameObject;

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
        minigameManager = FindObjectOfType<MinigameManager>();
        objectManager = FindObjectOfType<ObjectManager>();
        animationManager = GetComponent<AnimationManager>();

        previousMoveInput = moveInput;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (controlsEnabled)
        {
            // restrict movement on one axis at a time
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                moveInput.y = 0;
            }
            else
            {
                moveInput.x = 0;
            }

            //Vector3 position = new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;
            Vector2 position = new Vector3(moveInput.x, moveInput.y);
            //transform.Translate(position);
            //rb.MovePosition(position);

            //Vector2 moveDirection = new Vector2(moveInput, 0);

            rb.MovePosition(rb.position + position * moveSpeed * Time.deltaTime);

            animationManager.TriggerMovementAnimation(position, animator, transform, previousMoveInput);

            // set previous movement
            previousMoveInput = position;
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
                case "Sellable":
                    // check which customer to serve
                    minigameManager.StartMinigame(interactable, collidingObject.gameObject);
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
            minigameManager.HandleClick(objectHit);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!interactables.Contains(other.gameObject.tag))
        {
            return;
        }

        // if collided with an interactable
        // save that interactable
        collidingObject = other;

        // anvil, sellable, cash register
        interactableCanvas = objectManager.GetInteractableCanvas(collidingObject);

        if (interactableCanvas)
        {
            bool showAction = true;

            interactable = collidingObject.tag;
            interactableCanvas.SetActive(showAction);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (interactableCanvas)
        {
            // reset colliding object
            collidingObject = null;
            // hide prompt
            interactableCanvas.SetActive(false);
            // reset interactable
            interactable = null;
        }
    }

    public void ToggleControlsOnOrOff(bool isEnabled)
    {
        controlsEnabled = isEnabled;
    }
}
