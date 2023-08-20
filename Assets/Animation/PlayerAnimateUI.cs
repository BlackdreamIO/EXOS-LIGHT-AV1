using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimateUI : MonoBehaviour
{
    public RectTransform uiObject;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float lerpSpeed = 1.0f;

    [ContextMenu("StartAnimation")]
    public void StartAnimation()
    {
        StartCoroutine(lerpStart());
    }
    private IEnumerator lerpStart()
    {
        float t = 0;
        float duration = 1.0f / lerpSpeed;
        
        while (t < 1)
        {
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t);
            uiObject.anchoredPosition = newPosition;
            t += Time.deltaTime / duration;
            yield return null;
        }

        // Ensure that the final position is set exactly to the endPosition
        uiObject.anchoredPosition = endPosition;
    }

    [ContextMenu("StopAnimation")]
    public void StopAnimation()
    {
        StartCoroutine(lerpStop());
    }
    private IEnumerator lerpStop()
    {
        float t = 0; // Initialize t to 0
        float duration = 1.0f / lerpSpeed; // Calculate the duration based on lerpSpeed

        while (t < 1)
        {
            Vector3 newPosition = Vector3.Lerp(endPosition, startPosition, t);
            uiObject.anchoredPosition = newPosition;
            t += Time.deltaTime / duration; // Increment t based on time and duration
            yield return null;
        }

        // Ensure that the final position is set exactly to the startPosition
        uiObject.anchoredPosition = startPosition;
    }
}
