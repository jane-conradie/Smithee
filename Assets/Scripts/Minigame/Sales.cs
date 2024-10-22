using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Sales : MonoBehaviour
{
    [SerializeField] private GameObject minigamePrefab;
    [SerializeField] private Indicator indicator;

    public void StartSale()
    {
        // instantiate sales minigame prefab
        Instantiate(minigamePrefab, minigamePrefab.transform.position, quaternion.identity);
    }

    public void SellItem()
    {
        // stop indicator
        indicator.ToggleMoveIndicator();

        // check where indicator is
        Debug.Log(indicator.zone);

        // timer = bonus tip

        // zone = influences mood


        // check time left to get bonus
    }

}
