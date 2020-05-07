using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public const string SELECTABLE_TAG = "Selectable";
    private Transform newSelection = null;
    private Transform oldSelection = null;
    private Vector3 originalScale = Vector3.zero;

    private void Update()
    {
        RaycastHit raycastHit;
        
        // cast a sphere from the center of the screen, check to see if it hits anything
        if (Physics.SphereCast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f)), 8f, out raycastHit))
        {
            // get the transform of the first object the sphere hit
            newSelection = raycastHit.transform;
            // check if the object is selectable / already selected
            if (newSelection.CompareTag(SELECTABLE_TAG) && (newSelection != oldSelection))
            {
                deselect(oldSelection, originalScale);
                originalScale = select(newSelection);
                oldSelection = newSelection;
            }
            else if (!newSelection.CompareTag(SELECTABLE_TAG))
            {
                deselect(oldSelection, originalScale);
                oldSelection = null;
            }
        }
        else
        {
            deselect(newSelection, originalScale);
            newSelection = null;
            oldSelection = null;
        }
    }

    private Vector3 select(Transform selection)
    {
        if (selection == null) { return Vector3.zero; }
        Vector3 originalScale = selection.localScale;
        selection.localScale = new Vector3(8f, 8f, 8f);
        return originalScale;
    }

    private void deselect(Transform selection, Vector3 originalScale)
    {
        if (selection == null) { return; }
        selection.localScale = originalScale;
    }
}
