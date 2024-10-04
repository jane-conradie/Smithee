using UnityEngine;

public class Piece : MonoBehaviour
{
    public float id {get; private set;}

    public void SetId(float pieceId)
    {
        id = pieceId;
    }
}
