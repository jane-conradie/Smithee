using UnityEngine;

[CreateAssetMenu(menuName = "Create Mood", fileName = "Mood")]
public class MoodSO : ScriptableObject
{
    [SerializeField] private float score = 0f;
    [SerializeField] private Sprite sprite;

    public float GetScore()
    {
        return score;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }
}
