using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererRemover : MonoBehaviour
{
    public float removalTime = 1.0f;  // Time in seconds over which to remove all positions
    protected LineRenderer lineRenderer; // Reference to the LineRenderer component

    public void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void StartAnimation()
    {
        // Start the coroutine to remove positions
        StartCoroutine(RemovePositions());
    }

    private IEnumerator RemovePositions()
    {
        // Calculate the delay time for each position based on the total number of positions and removalTime
        float delay = removalTime / lineRenderer.positionCount;

        // While there are still positions in the LineRenderer
        while (lineRenderer.positionCount > 0)
        {
            // Iterate over each position, moving them one step backward
            for (int i = 0; i < lineRenderer.positionCount - 1; i++)
            {
                lineRenderer.SetPosition(i, lineRenderer.GetPosition(i + 1));
            }

            // Decrease the positionCount by 1, effectively removing the last position
            lineRenderer.positionCount--;

            // Wait for the calculated delay before the next iteration
            yield return new WaitForSeconds(delay);
        }

        Destroy(gameObject);
    }
}