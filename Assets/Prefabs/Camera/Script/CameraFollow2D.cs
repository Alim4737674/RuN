using UnityEngine;
using System.Collections;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;   // Player

    [Header("Einstellungen")]
    [SerializeField] private float smoothSpeed = 5f;  // Wie smooth die Kamera folgt
    [SerializeField] private Vector3 offset;          // Offset zur Kamera (z. B. (0, 0, -10))

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // Zielposition
        Vector3 targetPosition = target.position + offset;

        // Smooth folgen
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / smoothSpeed);
    }

    // === Shake Funktion ===
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
    }
}
