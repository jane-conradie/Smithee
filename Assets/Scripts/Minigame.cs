using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    [SerializeField] List<GameObject> fixables;

    // limit for how far a piece can deviate on x and y axis
    [SerializeField] float maxPositionDeviation = 3f;
    [SerializeField] float minPositionDeviation = -3f;

    // limit for piece rotation
    [SerializeField] float maxRotationDeviation = 340f;
    [SerializeField] float minRotationDeviation = -340f;

    // movement speeds
    [SerializeField] float pieceMoveSpeed = 4f;
    [SerializeField] float pieceRotationSpeed = 70f;

    bool isGameInProgress = false;

    List<(float id, Vector2 originalPosition)> originalPositions = new List<(float id, Vector2 originalPosition)>();

    public void StartGame()
    {
        if (!isGameInProgress)
        {
            isGameInProgress = true;

            // choose a random object to fix
            GameObject fixableObject = GetRandomFixable();

            // split pieces apart
            SplitPieces(fixableObject);
        }
    }

    GameObject GetRandomFixable()
    {
        return fixables[UnityEngine.Random.Range(0, fixables.Count - 1)];
    }

    void SplitPieces(GameObject fixableObject)
    {
        // instantiate a fixable object
        GameObject fixable = Instantiate(fixableObject, new Vector3(0, 0, 0), quaternion.identity);

        // get all the children objects, exclude the parent
        Transform[] pieces = fixable.GetComponentsInChildren<Transform>(true)
            .Where(component => component.gameObject != fixable.gameObject)
            .ToArray(); ;

        // for each piece
        // save the original transform
        // place at a new transform
        for (int i = 0; i < pieces.Count(); i++)
        {
            // set the gameobject id
            Piece piece = pieces[i].gameObject.GetComponent<Piece>();
            piece.SetId(i);

            Vector2 piecePosition = pieces[i].transform.position;

            // save the original pieces
            Vector2 originalPosition = new Vector2(piecePosition.x, piecePosition.y);
            originalPositions.Add((i, originalPosition));

            // generate a new position and new rotation for the piece
            Vector3 newPosition = new Vector3(GenerateRandomFloat("Position"), GenerateRandomFloat("Position"), 0);
            Vector3 newRotation = new Vector3(0, 0, GenerateRandomFloat("Rotation"));

            // place piece at new position and rotation
            pieces[i].position = newPosition;
            pieces[i].eulerAngles = newRotation;
        }
    }

    float GenerateRandomFloat(string type)
    {
        float number;

        if (type == "Position")
        {
            number = UnityEngine.Random.Range(minPositionDeviation, maxPositionDeviation);
        }
        else
        {
            number = UnityEngine.Random.Range(minRotationDeviation, maxRotationDeviation);
        }

        return number;
    }

    public void FixPiece(GameObject pieceObject)
    {
        Piece piece = pieceObject.GetComponent<Piece>();

        // look for piece in original position
        Vector2 targetPosition = originalPositions.FirstOrDefault((x) => x.id == piece.id).originalPosition;

        StartCoroutine(MoveToOriginalPosition(pieceObject, targetPosition));
    }

    IEnumerator MoveToOriginalPosition(GameObject pieceObject, Vector2 targetPosition)
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
        // TO DO MAYBE? remove rotation to implement user having to rotate pieces

        while (Vector2.Distance(pieceObject.transform.position, targetPosition) > 0.01f || Quaternion.Angle(pieceObject.transform.rotation, targetRotation) > 0.01f)
        {
            // rotate the pieces back to original rotation
            pieceObject.transform.rotation = Quaternion.RotateTowards(pieceObject.transform.rotation, targetRotation, pieceRotationSpeed * Time.deltaTime);
            // move the piece back to its original position
            pieceObject.transform.position = Vector2.MoveTowards(pieceObject.transform.position, targetPosition, Time.deltaTime * pieceMoveSpeed);
            yield return null;
        }
    }
}
