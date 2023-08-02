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

    private void Start()
    {
        grabbableLiquid.isSocketed = true;
    }

    void Update()
    {
        if (socketInteractor.selectTarget)
        {
            float transferAmount = transferRate * Time.deltaTime;

            if (grabbableLiquid.fillAmount > 0f)
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
                Debug.Log("grabbableLiquid.fillAmount " + grabbableLiquid.fillAmount.ToString());
                Debug.Log("socketedLiquid.fillAmount " + socketLiquid.fillAmount.ToString());
            }
        }
        else if (grabbableLiquid.isSocketed == true)
        {
            grabbableLiquid.isSocketed = false;
        }
    }
}

