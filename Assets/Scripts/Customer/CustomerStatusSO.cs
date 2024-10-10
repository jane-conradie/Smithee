using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create New Customer Status", fileName = "Customer Status")]
public class CustomerStatusSO : ScriptableObject
{
    [SerializeField] private GameObject holder;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private string sentiment;

    public List<Sprite> GetSprites()
    {
        return sprites;
    }

    public string GetSentiment()
    {
        return sentiment;
    }

    public Sprite GetRandomSprite()
    {
        return sprites[Random.Range(0, sprites.Count - 1)];
    }
}
