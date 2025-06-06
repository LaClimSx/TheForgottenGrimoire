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
        
        UpdateDisplayPage();
        pinchAction.action.Enable();
        
        leftPage.selectExited.AddListener(_ => ChangePage(true));
        rightPage.selectExited.AddListener(_ => ChangePage(false));
    }

    private void OnDisable()
    {
        if (!everythingInstancied) return;
        
        pinchAction.action.Disable();
        leftPage.selectExited.RemoveListener(_ => ChangePage(true));
        rightPage.selectExited.RemoveListener(_ => ChangePage(false));
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
