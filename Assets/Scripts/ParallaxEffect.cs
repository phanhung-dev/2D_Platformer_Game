using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform followTarget;

    Vector2 startingPosition;
    float startingZ;

    //Neu lay cam thi khung hinh bi dut lag nen quyet dinh lay player
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    //Vector2 followTargetMoveSinceStart => (Vector2)followTarget.transform.position - startingPosition;

    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    float clippingPlane => cam.transform.position.z + (zDistanceFromTarget < 0 ? cam.nearClipPlane : cam.farClipPlane);

    float parallaxFactor => Mathf.Abs(zDistanceFromTarget / clippingPlane);

    void Start()
    {
        if (cam == null) cam = Camera.main;

        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    void LateUpdate()
    {
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;

        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
