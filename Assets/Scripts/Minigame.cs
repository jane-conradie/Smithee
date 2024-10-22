// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using Unity.Mathematics;
// using UnityEngine;

// public class Anvil : MonoBehaviour
// {
//     [SerializeField] private GameObject miniGamePrefab;
//     [SerializeField] private List<FixablesSO> fixables;

//     [Header("Piece Postitioning and Rotation")]
//     // limit for how far a piece can deviate on x and y axis
//     [SerializeField] private float minPositionDeviation = -1.5f;
//     [SerializeField] private float maxPositionDeviationX = 5f;
//     [SerializeField] private float maxPositionDeviationY = 1.5f;
//     // limit for piece rotation
//     [SerializeField] private float maxRotationDeviation = 360f;
//     [SerializeField] private float minRotationDeviation = -360f;
//     [SerializeField] private float singleRotationAmount = -90f;
//     // movement speeds
//     [SerializeField] private float pieceMoveSpeed = 10f;
//     [SerializeField] private float pieceRotationSpeed = 260f;
//     private float numberOfPiecesToRotateCorrectly;
//     private List<(float id, Vector2 originalPosition)> originalPositions = new List<(float id, Vector2 originalPosition)>();

//     [Header("Tip & Timing")]
//     [SerializeField] private float countdown = 0.05f;
//     [SerializeField] private float timeToComplete = 1f;
//     private float timeLeft;

//     public bool isGameInProgress = false;

//     private GameObject minigame;
//     private GameObject fixableObject;

//     // dependencies
//     private MinigameManager minigameManager;
//     private PlayerMovement playerMovement;

//     private bool shouldCountdown = false;
//     private Customer customerToServe;

//     private void Start()
//     {
//         playerMovement = FindObjectOfType<PlayerMovement>();
//     }

//     private void Update()
//     {
//         if (shouldCountdown)
//         {
//             // countdown timer and set the value
//             timeLeft -= countdown * Time.deltaTime;
//             minigameManager.UpdateTimeSlider(timeLeft);

//             if (timeLeft <= 0)
//             {
//                 // customer rage quit
//                 StartCoroutine(customerToServe.RageQuit());

//                 // end mini game
//                 CancelGame();
//             }
//         }
//     }

//     public void StartGame()
//     {
//         if (!isGameInProgress && customerToServe)
//         {
//             // reset time left
//             timeLeft = timeToComplete;

//             // clear the original positions of the previous pieces
//             originalPositions.Clear();

//             // disable player movement
//             playerMovement.ToggleControlsOnOrOff(false);

//             // set game in progress
//             isGameInProgress = true;

//             // instantiate a mini game
//             minigame = Instantiate(miniGamePrefab, miniGamePrefab.transform.position, quaternion.identity);
//             minigameManager = minigame.GetComponent<MinigameManager>();

//             // choose a random object to fix
//             FixablesSO fixableObject = GetRandomFixable();
//             // split pieces apart
//             SplitPieces(fixableObject);

//             // set example image
//             minigameManager.UpdateExampleSprite(fixableObject.GetFixableSprite());

//             // start timer
//             shouldCountdown = true;
//         }
//     }

//     private FixablesSO GetRandomFixable()
//     {
//         return fixables[UnityEngine.Random.Range(0, fixables.Count)];
//     }

//     private void SplitPieces(FixablesSO fixable)
//     {
//         // get the prefab to split apart
//         GameObject fixableToBreak = fixable.GetFixablePrefab();

//         // instantiate a fixable object
//         fixableObject = Instantiate(fixableToBreak, fixableToBreak.transform.position, quaternion.identity);

//         // get all the children objects, exclude the parent
//         Transform[] pieces = fixableObject.GetComponentsInChildren<Transform>(true)
//             .Where(component => component.gameObject != fixableObject.gameObject)
//             .ToArray(); ;

//         numberOfPiecesToRotateCorrectly = pieces.Count();

//         // for each piece
//         // save the original transform
//         // place at a new transform
//         for (int i = 0; i < numberOfPiecesToRotateCorrectly; i++)
//         {
//             // set the gameobject id
//             Piece piece = pieces[i].gameObject.GetComponent<Piece>();
//             piece.SetId(i);

//             Vector2 piecePosition = pieces[i].transform.position;

//             // save the original pieces
//             Vector2 originalPosition = new Vector2(piecePosition.x, piecePosition.y);
//             originalPositions.Add((i, originalPosition));

//             float xPos = GenerateRandomFloat("Position", true);

//             // generate a new position and new rotation for the piece
//             Vector3 newPosition = new Vector3(xPos, GenerateRandomFloat("Position", false), 0);
//             Vector3 newRotation = new Vector3(0, 0, GenerateRandomFloat("Rotation", false));

//             // place piece at new position and rotation
//             pieces[i].position = newPosition;
//             pieces[i].eulerAngles = newRotation;
//         }
//     }

//     private float GenerateRandomFloat(string type, bool isForXAxis)
//     {
//         float number;

//         if (type == "Position")
//         {
//             if (!isForXAxis)
//             {
//                 number = UnityEngine.Random.Range(minPositionDeviation, maxPositionDeviationY);
//             }
//             else
//             {
//                 number = UnityEngine.Random.Range(minPositionDeviation, maxPositionDeviationX);
//             }
//         }
//         else
//         {
//             // choose a random number to dictate the amount of degrees to rotate the piece
//             // this limits a user to only having to spin 1 - 3 times
//             float optionNumber = UnityEngine.Random.Range(1, 3);
//             number = optionNumber * singleRotationAmount;
//         }

//         return number;
//     }

//     public void FixPiece(GameObject pieceObject)
//     {
//         Piece piece = pieceObject.GetComponent<Piece>();

//         // look for piece in original position
//         Vector2 targetPosition = originalPositions.FirstOrDefault((x) => x.id == piece.id).originalPosition;

//         if (!piece.isInPosition)
//         {
//             StartCoroutine(MoveToOriginalPosition(pieceObject, targetPosition, piece));
//         }
//         else if (!piece.isInCorrectRotation && !piece.isRotating)
//         {
//             StartCoroutine(RotatePiece(pieceObject, piece));
//         }
//     }

//     private IEnumerator MoveToOriginalPosition(GameObject pieceObject, Vector2 targetPosition, Piece piece)
//     {
//         piece.isInPosition = true;

//         while (Vector2.Distance(pieceObject.transform.position, targetPosition) > 0.01f)
//         {
//             // move the piece back to its original position
//             pieceObject.transform.position = Vector2.MoveTowards(pieceObject.transform.position, targetPosition, Time.deltaTime * pieceMoveSpeed);
//             yield return null;
//         }
//     }

//     private IEnumerator RotatePiece(GameObject pieceObject, Piece piece)
//     {
//         if (isGameInProgress)
//         {
//             piece.isRotating = true;

//             Quaternion correctRotation = Quaternion.Euler(0, 0, 0);

//             Quaternion rotationToDo = Quaternion.Euler(0, 0, singleRotationAmount);
//             Quaternion targetRotation = Quaternion.Euler(0, 0, rotationToDo.eulerAngles.z + pieceObject.transform.rotation.eulerAngles.z);

//             while (Quaternion.Angle(pieceObject.transform.rotation, targetRotation) > 0.01f)
//             {
//                 // rotate piece to target rotation
//                 pieceObject.transform.rotation = Quaternion.RotateTowards(pieceObject.transform.rotation, targetRotation, pieceRotationSpeed * Time.deltaTime);
//                 yield return null;
//             }

//             piece.isRotating = false;

//             // if piece rotation matches correct rotatiom
//             // mark as in correct rotation
//             if (Quaternion.Angle(pieceObject.transform.rotation, correctRotation) <= 0.5f)
//             {
//                 piece.isInCorrectRotation = true;
//                 numberOfPiecesToRotateCorrectly--;
//             }

//             if (numberOfPiecesToRotateCorrectly == 0)
//             {
//                 StartCoroutine(EndMiniGame());
//             }
//         }
//     }

//     private IEnumerator EndMiniGame()
//     {
//         // stop timer
//         shouldCountdown = false;

//         // change text to tell user game is done
//         minigameManager.UpdateDisplay();

//         // TO DO CHANGE ALL AREAS WHERE WAIT FOR SECONDS IS HARDCODED
//         yield return new WaitForSecondsRealtime(2);

//         // calculate bonus tip for customer
//         customerToServe.CalculateBonusTip(timeLeft);

//         CancelGame();
//     }

//     public void CancelGame()
//     {
//         // end game in progress
//         isGameInProgress = false;

//         // stop countdown
//         shouldCountdown = false;

//         // destory minigame, whooo hooo
//         // TO DO IF TIME - COMBINE THESE TWO SO ONLY DESTROY ONE
//         Destroy(minigame);
//         Destroy(fixableObject);

//         // reenable the controls
//         playerMovement.ToggleControlsOnOrOff(true);
//     }

//     public void SetCustomerToServe(Customer customer)
//     {
//         customerToServe = customer;
//     }
// }
