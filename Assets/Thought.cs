using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thought : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private CustomerStatusSO customerStatus;

    private void Start() 
    {
        ChangeSprite();
    }

    private void ChangeSprite()
    {
        // change own sprite
        sr.sprite = customerStatus.GetRandomSprite();
    }
}
