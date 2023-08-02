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

    private void Start()
    {
        grabbableLiquid.isSocketed = true;
    }

    void Update()
    {
        if (socketInteractor.selectTarget)
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
        }
        else if (grabbableLiquid.isSocketed == true)
        {
            grabbableLiquid.isSocketed = false;
        }
    }
}

