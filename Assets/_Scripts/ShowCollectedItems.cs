using TMPro;
using UnityEngine;

public class ShowCollectedItems : MonoBehaviour
{
    [SerializeField] CollectedItems collectedItems;

    private TMP_Text textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    void Update()
    {
        textMesh.text = collectedItems.count.ToString();
    }
}
