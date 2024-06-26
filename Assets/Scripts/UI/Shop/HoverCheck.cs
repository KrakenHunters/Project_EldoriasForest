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
            if (result.gameObject.GetComponent<Button>() != null)
            {
                isHoveringOverButton = true;
                if (!hasHovered)
                {
                    if (result.gameObject.GetComponent<OnHover>() != null)
                    {
                        result.gameObject.GetComponent<OnHover>().OnHoverEvent.Invoke(result.gameObject.GetComponent<Button>().IsInteractable());
                    }
                    hasHovered = true; // Set the flag to true
                }
                break; // Exit the loop once a button is found
            }
        }

        // If the pointer is not over a button, reset the hover state
        if (!isHoveringOverButton)
        {
            hasHovered = false;
        }
    }
}