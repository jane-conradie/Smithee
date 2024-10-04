using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create New Fixable", fileName = "Fixable")]
public class FixablesSO : ScriptableObject
{
    [SerializeField] private GameObject fixablePrefab;
    [SerializeField] private Sprite fixableSprite;
    [SerializeField] private Transform fixableHolder;

    public GameObject GetFixablePrefab()
    {
        return fixablePrefab;
    }

    public Sprite GetFixableSprite()
    {
        return fixableSprite;
    }

    public Transform GetFixableHolder()
    {
        return fixableHolder;
    }
}
