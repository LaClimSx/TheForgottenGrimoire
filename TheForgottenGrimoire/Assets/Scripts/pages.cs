using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class pages : MonoBehaviour
{
    [Header("Page Interactables")]
    [SerializeField] private XRSimpleInteractable leftPage;
    [SerializeField] private XRSimpleInteractable rightPage;

    [Header("Pinch Input")]
    [SerializeField] private InputActionProperty pinchAction;

    [Header("Images and Rendering")]
    [SerializeField] private Renderer leftPageRenderer;           
    [SerializeField] private Renderer rightPageRenderer;           
    [SerializeField] private Texture2D[] pageImages;          // The page to display
    private int currentPageIndex = 0;

    private bool leftHovering = false;
    private bool rightHovering = false;
    
    private bool everythingInstancied = false;

    private void OnEnable()
    {
        
        everythingInstancied = leftPageRenderer != null && rightPageRenderer != null && pageImages.Length > 0 & leftPage != null && rightPage != null;

        if (everythingInstancied)
        {
            UpdateDisplayPage();
        }
        else {
            Debug.LogError($"Error {leftPageRenderer} and {rightPageRenderer} are required and at least 1 page is needed {pageImages.Length}. {leftPageRenderer} and {rightPageRenderer} are required.");
        }
        
        pinchAction.action.Enable();
        
        

        if (!everythingInstancied) return;
        leftPage.hoverEntered.AddListener(OnLeftHoverEntered);
        leftPage.hoverExited.AddListener(OnLeftHoverExited);
        rightPage.hoverEntered.AddListener(OnRightHoverEntered);
        rightPage.hoverExited.AddListener(OnRightHoverExited);
    }

    private void OnDisable()
    {
        pinchAction.action.Disable();

        if (!everythingInstancied) return;
        leftPage.hoverEntered.RemoveListener(OnLeftHoverEntered);
        leftPage.hoverExited.RemoveListener(OnLeftHoverExited);
        rightPage.hoverEntered.RemoveListener(OnRightHoverEntered);
        rightPage.hoverExited.RemoveListener(OnRightHoverExited);
    }

    private void Update()
    {
        if (pinchAction.action.WasReleasedThisFrame())
        {
            if (leftHovering)
            {
                ChangePage(true);
            }
            else if (rightHovering)
            {
                ChangePage(false);
            }
        }
    }

    private void OnLeftHoverEntered(HoverEnterEventArgs args) => leftHovering = true;
    private void OnLeftHoverExited(HoverExitEventArgs args) => leftHovering = false;

    private void OnRightHoverEntered(HoverEnterEventArgs args) => rightHovering = true;

    private void OnRightHoverExited(HoverExitEventArgs args) => rightHovering = false;

    private void ChangePage(bool left)
    {
        currentPageIndex = left 
            ? Mathf.Max(0, currentPageIndex - 1) 
            : Mathf.Min(pageImages.Length/2 - 1, currentPageIndex + 1);

        UpdateDisplayPage();
    }

    private void UpdateDisplayPage()
    {
        leftPageRenderer.material.SetTexture("_BaseMap", pageImages[currentPageIndex*2]);
        rightPageRenderer.material.SetTexture("_BaseMap", pageImages[currentPageIndex*2+1]);
    }
}
