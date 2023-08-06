using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LiquidTransfer : MonoBehaviour
{
    public XRSocketInteractor socketInteractor;
    public Liquid socketLiquid;
    public Liquid grabbableLiquid;
    [SerializeField, Range(0f, 1f)]
    public float transferRate = 0.2f;
    [SerializeField]
    private float foamWidthIncreaseRate = 0.5f;
    [SerializeField]
    private float foamSmoothIncreaseRate = 0.2f;

    // Material to be applied to the duplicated object
    public Material newMaterial;
    private bool canDuplicate = true;

    private void Start()
    {
        grabbableLiquid.isSocketed = true;
    }

    void Update()
    {
        if (socketInteractor.hasSelection)
        {
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
                grabbableLiquid.isSocketed = false;
                if (canDuplicate == true && socketLiquid.fillAmount >= 0.99f)
                {
                    DuplicateObject();
                    canDuplicate = false;
                }
            }
        }
    }

    void DuplicateObject()
    {
        // Create a duplicate of the current object
        GameObject duplicate = Instantiate(gameObject, transform.position, transform.rotation);
        // Set the parent of the duplicate to the original object's parent
        duplicate.transform.parent = transform.parent;
        // Make the duplicate slightly larger
        duplicate.transform.localScale = transform.localScale * 1.05f;
        // Remove the script from the duplicated object
        Destroy(duplicate.GetComponent<LiquidTransfer>());
        // Remove the XRSocketInteractor from the duplicate
        XRSocketInteractor socketInteractor = duplicate.GetComponent<XRSocketInteractor>();
        if (socketInteractor != null)
        {
            Destroy(socketInteractor);
        }
        // Delete all children from the duplicate
        foreach (Transform child in duplicate.transform)
        {
            Destroy(child.gameObject);
        }
        // Apply the new material to the duplicate object
        Renderer rend = duplicate.GetComponent<Renderer>();
        if (rend != null)
            rend.material = newMaterial;
    }
}

