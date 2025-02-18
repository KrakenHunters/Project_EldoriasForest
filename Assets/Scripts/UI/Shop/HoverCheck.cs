using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HoverCheck : MonoBehaviour
{
    public Canvas canvas; // Reference to the canvas
    private bool hasHovered = false; // Flag to track if hover has been detected
    public bool canHoverOverDescription;
    public float hoverDelay = 0.5f; // Delay before invoking the hover event

    void Update()
    {
        // Call the method to check for UI element under pointer
        CheckPointerOverUI();
    }

    private void CheckPointerOverUI()
    {
        // Create a PointerEventData with the current mouse position
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        // Create a list to hold the Raycast results
        List<RaycastResult> results = new List<RaycastResult>();

        // Use the GraphicRaycaster on the Canvas to perform the Raycast
        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
        raycaster.Raycast(pointerData, results);

        bool isHoveringOverButton = false;

        // Iterate through the results to find UI elements
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<OnHover>() != null)
            {
                isHoveringOverButton = true;
                if (!hasHovered)
                {
                    StartCoroutine(HoverCoroutine(result.gameObject));
                    hasHovered = true; // Set the flag to true
                }
                break; // Exit the loop once a button is found
            }
        }

        // If the pointer is not over a button, reset the hover state
        if (!isHoveringOverButton)
        {
            hasHovered = false;
            StopAllCoroutines(); // Stop all running hover coroutines
        }
    }

    private IEnumerator HoverCoroutine(GameObject hoverObject)
    {
        yield return new WaitForSeconds(hoverDelay);

        // Check if the pointer is still over the same UI element
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        List<RaycastResult> results = new List<RaycastResult>();
        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == hoverObject)
            {
                OnHover onHover = hoverObject.GetComponent<OnHover>();
                if (onHover != null)
                {
                    if (hoverObject.GetComponent<Button>() != null)
                    {
                        onHover.OnHoverEvent.Invoke(hoverObject.GetComponent<Button>().IsInteractable());
                    }
                    else
                    {
                        onHover.OnHoverEvent.Invoke(canHoverOverDescription);
                    }
                }
                break;
            }
        }
    }
}