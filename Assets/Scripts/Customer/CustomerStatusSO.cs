using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create New Customer Status", fileName = "Customer Status")]
public class CustomerStatusSO : ScriptableObject
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private string sentiment;

    public Sprite GetRandomSprite()
    {
        Sprite sprite = sprites[Random.Range(0, sprites.Count - 1)];

        return sprite;
    }

    public string GetSentiment()
    {
        return sentiment;
    }
}
