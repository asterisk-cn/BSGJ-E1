using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shake(float duration, float posMagnitude, float rotMagnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, posMagnitude, rotMagnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float posMagnitude, float rotMagnitude)
    {
        var originalPos = transform.localPosition;
        var originalRot = transform.localRotation;

        var elapsed = 0f;

        while (elapsed < duration)
        {
            var pos = originalPos + Random.insideUnitSphere * posMagnitude;
            var rot = originalRot * Quaternion.Euler(Random.Range(-rotMagnitude, rotMagnitude), Random.Range(-rotMagnitude, rotMagnitude), Random.Range(-rotMagnitude, rotMagnitude));

            transform.localPosition = pos;
            transform.localRotation = rot;

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
        transform.localRotation = originalRot;
    }
}
