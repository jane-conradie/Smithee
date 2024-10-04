using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create New Customer Status", fileName = "Customer Status")]
public class CustomerStatusSO : ScriptableObject
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private string sentiment;
    [SerializeField] private string statusType;

    public List<Sprite> GetSprites()
    {
        return sprites;
    }

    public string GetSentiment()
    {
        return sentiment;
    }

    public string GetStatusType()
    {
        return statusType;
    }

    public Sprite GetRandomSprite()
    {
        return sprites[Random.Range(0, sprites.Count - 1)];
    }
}
