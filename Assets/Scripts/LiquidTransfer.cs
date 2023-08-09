using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LiquidTransfer : MonoBehaviour
{
    XRSocketInteractor socketInteractor;
    Liquid socketLiquid;
    Liquid grabbableLiquid;
    [SerializeField, Range(0f, 1f)]
    public float transferRate = 0.2f;
    [SerializeField]
    private float foamWidthIncreaseRate = 0.5f;
    [SerializeField]
    private float foamSmoothIncreaseRate = 0.2f;

    // Material to be applied to the duplicated object
    public Material newMaterial;
    private bool canDuplicate = true;

    private void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        if (socketInteractor == null)
            Debug.LogError("No XRSocketInteractor component found on this object");
        else
            // Subscribe to the onSelectEnter event
            socketInteractor.selectEntered.AddListener(OnSelectEnter);
    }

    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        // Get the interacting object
        XRBaseInteractable baseInteractable = args.interactableObject as XRBaseInteractable;
        if (baseInteractable == null)
        {
            Debug.Log("Interactable is not an XRBaseInteractable.");
        }
        GameObject interactingObject = baseInteractable.gameObject;
        // Get the Liquid component from the child object
        grabbableLiquid = interactingObject.GetComponentInChildren<Liquid>();
        if (grabbableLiquid == null)
            Debug.Log("No Liquid component found on children of interacting object");
        // Get the Liquid component from the socket object
        socketLiquid = GetComponentInChildren<Liquid>();
        if (socketLiquid == null)
            Debug.Log("No Liquid component found on children of current socket object");
    }

    void Update()
    {
        if (grabbableLiquid && socketLiquid && socketInteractor.hasSelection)
        {
            grabbableLiquid.isSocketed = true;
            float transferAmount = transferRate * Time.deltaTime;

            if (grabbableLiquid.fillAmount > 0f && (socketLiquid.fillAmount + transferAmount < 1f || socketLiquid.fillAmount + grabbableLiquid.fillAmount < 1f))
            {
                if (grabbableLiquid.fillAmount > transferRate)
                {
                    socketLiquid.fillAmount = Mathf.Clamp(socketLiquid.fillAmount + transferAmount, 0f, 1f);
                    grabbableLiquid.fillAmount = Mathf.Clamp(grabbableLiquid.fillAmount - transferAmount, 0f, 1f);
                }
                else
                {
                    socketLiquid.fillAmount = Mathf.Clamp(socketLiquid.fillAmount + grabbableLiquid.fillAmount, 0f, 1f);
                    grabbableLiquid.fillAmount = 0f;
                    grabbableLiquid.isSocketed = false;
                }

                // Foam properties and color adjustments
                float socketLineWidth = socketLiquid.liquidRenderer.material.GetFloat("_Line");
                float socketLineSmooth = socketLiquid.liquidRenderer.material.GetFloat("_LineSmooth");

                socketLineWidth += foamWidthIncreaseRate * Time.deltaTime;
                socketLineSmooth += foamSmoothIncreaseRate * Time.deltaTime;

                socketLiquid.liquidRenderer.material.SetFloat("_LineWidth", socketLineWidth);
                socketLiquid.liquidRenderer.material.SetFloat("_LineSmooth", socketLineSmooth);

                Color grabTopColor = grabbableLiquid.liquidRenderer.material.GetColor("_TopColor");
                socketLiquid.liquidRenderer.material.SetColor("_FoamColor", grabTopColor);
            }
            else
            {
                if (canDuplicate == true && socketLiquid.fillAmount >= 0.99f)
                {
                    DuplicateObject();
                    canDuplicate = false;
                    grabbableLiquid.isSocketed = false;
                }
            }
        }
    }

    void DuplicateObject()
    {
        // Create a duplicate of the current object
        GameObject duplicate = Instantiate(gameObject, transform.position, transform.rotation);
        // Make the duplicate a child of the original object
        duplicate.transform.parent = gameObject.transform;
        // Update duplicate's local position and rotation to match original
        duplicate.transform.localPosition = Vector3.zero;
        duplicate.transform.localRotation = Quaternion.identity;
        // Make the duplicate slightly larger
        duplicate.transform.localScale = transform.localScale * 1.05f;
        // Set the y value of the duplicate's position to be less than the original
        duplicate.transform.localPosition = new Vector3(duplicate.transform.localPosition.x, duplicate.transform.localPosition.y - 0.008f, duplicate.transform.localPosition.z);
        // Remove gravity and set isKinematic to true
        Rigidbody rb = duplicate.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        // Remove the script from the duplicated object
        Destroy(duplicate.GetComponent<LiquidTransfer>());
        // Remove the XRSocketInteractor from the duplicate
        XRSocketInteractor socketInteractor = duplicate.GetComponent<XRSocketInteractor>();
        if (socketInteractor != null)
            Destroy(socketInteractor);
        // Delete all children from the duplicate
        foreach (Transform child in duplicate.transform)
        {
            Destroy(child.gameObject);
        }
        // Apply the new material to the duplicate object
        Renderer rend = duplicate.GetComponent<Renderer>();
        if (rend != null)
            rend.material = newMaterial;
        // Start a coroutine to fade in the object
        StartCoroutine(FadeIn(duplicate, 2.0f));  // 2 seconds to full visibility
    }

    IEnumerator FadeIn(GameObject obj, float duration)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null)
            yield break;

        Material mat = renderer.material;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float transparency = Mathf.Lerp(1, 0, elapsed / duration);
            mat.SetFloat("_Alpha", 1 - transparency); // Alpha values from 0 for transparent and 1 for opaque
            yield return null;
        }
    }
}

