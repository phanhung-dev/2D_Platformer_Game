using UnityEngine;

public class ItemHover : MonoBehaviour
{
    [Header("Hover Settings")]
    [SerializeField] private float hoverSpeed = 2f;   
    [SerializeField] private float hoverHeight = 0.2f;

    private Vector3 startPosition;

    private void Start() 
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float wave = (Mathf.Sin(Time.time * hoverSpeed) + 1f) / 2f;

        float newY = startPosition.y + (wave * hoverHeight);

        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
