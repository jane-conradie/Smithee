using UnityEngine;

public class Piece : MonoBehaviour
{
    public float id {get; private set;}

    public bool isInPosition {get; set;} = false;
    public bool isInCorrectRotation {get; set;} = false;
    public bool isRotating {get; set;} = false;

    public void SetId(float pieceId)
    {
        id = pieceId;
    }
}
