using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

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
    private float liquidChangeAmount = 0f;

    // Material to be applied to the duplicated object
    public Material newMaterial;
    private bool canDuplicate = true;

    [SerializeField]
    private Color nonLiquidColor;
    [SerializeField]
    private bool limitedNonLiquid = false;
    [SerializeField, Range(0f, 1f)]
    float nonLiquidAmount = float.NaN;
    [SerializeField]
    string nonLiquidSubstance = "";

    private GameObject grabbableObject;
    private LiquidTransfer grabbableLiquidTransfer;
    private bool grabbableLimitedNonLiquid;
    private Color grabbableNonLiquidColor;
    private float grabbableNonLiquidAmount;
    private float grabbabletransferRate;
    private string grabbableNonLiquidSubstance;

    private bool isFirstInteractionInstance = true;
    private GameObject duplicatedObject = null;
    private bool canReact = false;

    private string substanceOne;
    private string substanceTwo;
    private string substanceResult;

    private float fillAmountOfSubstanceOne;
    private float fillAmountOfSubstanceTwo;
    private float fillAmountOfSubstanceResult;

    private void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        if (socketInteractor != null)
            // Subscribe to the onSelectEnter event
            socketInteractor.selectEntered.AddListener(OnSelectEnter);
        else
            Debug.LogError("No XRSocketInteractor component found on this object");

        substanceOne = GlobalChemistryData.instance.substanceOne;
        substanceTwo = GlobalChemistryData.instance.substanceTwo;
        substanceResult = GlobalChemistryData.instance.substanceResult;

        fillAmountOfSubstanceOne = .1f * GlobalChemistryData.instance.molesOfSubstanceOne;
        fillAmountOfSubstanceTwo = .1f * GlobalChemistryData.instance.molesOfSubstanceTwo;
        fillAmountOfSubstanceResult = .1f * GlobalChemistryData.instance.molesOfSubstanceResult;
    }

    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        // Get the interacting object
        XRBaseInteractable baseInteractable = args.interactableObject as XRBaseInteractable;
        if (baseInteractable == null)
            Debug.Log("Interactable is not an XRBaseInteractable.");

        grabbableObject = baseInteractable.gameObject;
        // Get the Liquid component from the child object
        grabbableLiquid = grabbableObject.GetComponentInChildren<Liquid>();
        // Get the Liquid component from the socket object
        socketLiquid = GetComponentInChildren<Liquid>();

        if (grabbableLiquid == null)
            Debug.Log("No Liquid component found on children of interacting object");
        if (socketLiquid == null)
            Debug.Log("No Liquid component found on children of current socket object");
    }

    void Update()
    {
        CheckAndDestroyDuplicateIfNecessary();

        if (grabbableLiquid && socketInteractor.hasSelection)
            {
                if (socketLiquid)
                {
                    grabbableLiquid.isSocketed = true;
                    float transferAmount = transferRate * Time.deltaTime;

                    if (grabbableLiquid.fillAmount > 0f && (grabbableLiquid.foamSubstanceAmount > 0f || grabbableLiquid.topSubstanceAmount > 0f) && (socketLiquid.fillAmount + transferAmount < 1f || socketLiquid.fillAmount + grabbableLiquid.fillAmount < 1f) &&
                    !canReact)
                    {
                        if (grabbableLiquid.fillAmount > transferAmount)
                        {
                            liquidChangeAmount = transferAmount;
                            socketLiquid.fillAmount = Mathf.Clamp(socketLiquid.fillAmount + transferAmount, 0f, 1f);
                            grabbableLiquid.fillAmount = Mathf.Clamp(grabbableLiquid.fillAmount - transferAmount, 0f, 1f);
                        }
                        else
                        {
                            liquidChangeAmount = socketLiquid.fillAmount - Mathf.Clamp(socketLiquid.fillAmount + grabbableLiquid.fillAmount, 0f, 1f);
                            socketLiquid.fillAmount = Mathf.Clamp(socketLiquid.fillAmount + grabbableLiquid.fillAmount, 0f, 1f);
                            grabbableLiquid.fillAmount = 0f;
                            grabbableLiquid.isSocketed = false;
                        }

                        // Foam properties and color adjustments
                        float socketLineWidth = socketLiquid.liquidRenderer.material.GetFloat("_Line");
                        float socketLineSmooth = socketLiquid.liquidRenderer.material.GetFloat("_LineSmooth");

                        float newSocketLineWidth = Mathf.Clamp(socketLineWidth + foamWidthIncreaseRate * Time.deltaTime, 0f, 1f);
                        float newSocketLineSmooth = Mathf.Clamp(socketLineSmooth + foamSmoothIncreaseRate * Time.deltaTime, 0f, .1f);

                        socketLiquid.liquidRenderer.material.SetFloat("_Line", newSocketLineWidth);
                        socketLiquid.liquidRenderer.material.SetFloat("_LineSmooth", newSocketLineSmooth);

                        Color grabTopColor = grabbableLiquid.liquidRenderer.material.GetColor("_TopColor");

                        if (isFirstInteractionInstance)
                        {
                            Color socketTopColor = socketLiquid.liquidRenderer.material.GetColor("_FoamColor");
                            socketLiquid.liquidRenderer.material.SetColor("_TopColor", socketTopColor);
                            if (socketLiquid.foamSubstance != "")
                                socketLiquid.liquidRenderer.material.SetColor("_TopColor", socketTopColor);
                            else
                                socketLiquid.liquidRenderer.material.SetColor("_TopColor", grabTopColor);
                            socketLiquid.topSubstanceAmount = socketLiquid.foamSubstanceAmount;
                            socketLiquid.foamSubstanceAmount = 0f;
                            socketLiquid.topSubstance = socketLiquid.foamSubstance;
                            isFirstInteractionInstance = false;
                        }

                        socketLiquid.liquidRenderer.material.SetColor("_FoamColor", grabTopColor);
                        socketLiquid.foamSubstanceAmount += liquidChangeAmount;
                        socketLiquid.foamSubstance = grabbableLiquid.topSubstance;
                        if (grabbableLiquid.topSubstanceAmount > 0f && (grabbableLiquid.topSubstanceAmount - liquidChangeAmount) > 0f)
                            grabbableLiquid.topSubstanceAmount -= liquidChangeAmount;
                        else if (grabbableLiquid.foamSubstanceAmount > 0f && (grabbableLiquid.foamSubstanceAmount - liquidChangeAmount) > 0f)
                            grabbableLiquid.foamSubstanceAmount -= liquidChangeAmount;

                        if ((socketLiquid.topSubstanceAmount >= fillAmountOfSubstanceOne && socketLiquid.foamSubstanceAmount >= fillAmountOfSubstanceTwo) ||
                        (socketLiquid.topSubstanceAmount >= fillAmountOfSubstanceTwo && socketLiquid.foamSubstanceAmount >= fillAmountOfSubstanceOne))
                            canReact = true;
                }
                    else
                    {
                        if (canDuplicate == true &&
                        ((socketLiquid.topSubstanceAmount >= fillAmountOfSubstanceOne && socketLiquid.foamSubstanceAmount >= fillAmountOfSubstanceTwo) ||
                        (socketLiquid.topSubstanceAmount >= fillAmountOfSubstanceTwo && socketLiquid.foamSubstanceAmount >= fillAmountOfSubstanceOne)))
                        {
                            DuplicateObject(this.gameObject);
                            canDuplicate = false;
                            grabbableLiquid.isSocketed = false;
                        }
                    }
                    GlobalChemistryData.instance.mixedChemicalOne = socketLiquid.topSubstance;
                    GlobalChemistryData.instance.mixedChemicalTwo = socketLiquid.foamSubstance;
                    GlobalChemistryData.instance.mixedChemicalOneAmount = socketLiquid.topSubstanceAmount;
                    GlobalChemistryData.instance.mixedChemicalTwoAmount = socketLiquid.foamSubstanceAmount;
                }
                else
                {
                    grabbableLiquid.isSocketed = true;
                    float transferAmount = transferRate * Time.deltaTime;

                    if ((!limitedNonLiquid || nonLiquidAmount > 0f) && (grabbableLiquid.fillAmount + transferAmount < 1f || grabbableLiquid.fillAmount + nonLiquidAmount < 1f)
                    && !canReact)
                    {
                        if (!limitedNonLiquid) {
                            liquidChangeAmount = transferAmount;
                            grabbableLiquid.fillAmount = Mathf.Clamp(grabbableLiquid.fillAmount + transferAmount, 0f, 1f);
                        }
                        else
                        {
                            if (nonLiquidAmount > transferAmount)
                            {
                                liquidChangeAmount = transferAmount;
                                grabbableLiquid.fillAmount = Mathf.Clamp(grabbableLiquid.fillAmount + transferAmount, 0f, 1f);
                                nonLiquidAmount = nonLiquidAmount - transferAmount;
                            }
                            else
                            {
                                liquidChangeAmount = grabbableLiquid.fillAmount - Mathf.Clamp(grabbableLiquid.fillAmount + nonLiquidAmount, 0f, 1f); ;
                                grabbableLiquid.fillAmount = Mathf.Clamp(grabbableLiquid.fillAmount + nonLiquidAmount, 0f, 1f);
                                nonLiquidAmount = 0f;

                                foreach (Transform child in gameObject.transform)
                                {
                                    if (child.CompareTag("Non Liquid"))
                                        Destroy(child.gameObject);
                                }
                            }
                        }

                        // Foam properties and color adjustments
                        float grabbableLineWidth = grabbableLiquid.liquidRenderer.material.GetFloat("_Line");
                        float grabbableLineSmooth = grabbableLiquid.liquidRenderer.material.GetFloat("_LineSmooth");

                        float newGrabbableLineWidth = Mathf.Clamp(grabbableLineWidth + foamWidthIncreaseRate * Time.deltaTime, 0f, 1f);
                        float newGrabbableLineSmooth = Mathf.Clamp(grabbableLineSmooth + foamSmoothIncreaseRate * Time.deltaTime, 0f, .1f);

                        grabbableLiquid.liquidRenderer.material.SetFloat("_Line", newGrabbableLineWidth);
                        grabbableLiquid.liquidRenderer.material.SetFloat("_LineSmooth", newGrabbableLineSmooth);

                        if (isFirstInteractionInstance)
                        {
                            Color grabbableTopColor = grabbableLiquid.liquidRenderer.material.GetColor("_FoamColor");
                            if (grabbableLiquid.foamSubstance != "")
                                grabbableLiquid.liquidRenderer.material.SetColor("_TopColor", grabbableTopColor);
                            else
                                grabbableLiquid.liquidRenderer.material.SetColor("_TopColor", nonLiquidColor);
                            grabbableLiquid.topSubstanceAmount = grabbableLiquid.foamSubstanceAmount;
                            grabbableLiquid.foamSubstanceAmount = 0f;
                            grabbableLiquid.topSubstance = grabbableLiquid.foamSubstance;
                            isFirstInteractionInstance = false;
                        }
                        grabbableLiquid.liquidRenderer.material.SetColor("_FoamColor", nonLiquidColor);
                        grabbableLiquid.foamSubstanceAmount += liquidChangeAmount;
                        grabbableLiquid.foamSubstance = nonLiquidSubstance;

                    if ((grabbableLiquid.topSubstanceAmount >= fillAmountOfSubstanceOne && grabbableLiquid.foamSubstanceAmount >= fillAmountOfSubstanceTwo) ||
                        (grabbableLiquid.topSubstanceAmount >= fillAmountOfSubstanceTwo && grabbableLiquid.foamSubstanceAmount >= fillAmountOfSubstanceOne))
                            canReact = true;
                    }
                    else
                    {
                        if (canDuplicate == true &&
                        ((grabbableLiquid.topSubstanceAmount >= fillAmountOfSubstanceOne && grabbableLiquid.foamSubstanceAmount >= fillAmountOfSubstanceTwo) ||
                        (grabbableLiquid.topSubstanceAmount >= fillAmountOfSubstanceTwo && grabbableLiquid.foamSubstanceAmount >= fillAmountOfSubstanceOne)))
                        {
                            

                            DuplicateObject(grabbableObject);
                            canDuplicate = false;
                            grabbableLiquid.isSocketed = false;
                        }
                    }
                    GlobalChemistryData.instance.mixedChemicalOne = grabbableLiquid.topSubstance;
                    GlobalChemistryData.instance.mixedChemicalTwo = grabbableLiquid.foamSubstance;
                    GlobalChemistryData.instance.mixedChemicalOneAmount = grabbableLiquid.topSubstanceAmount;
                    GlobalChemistryData.instance.mixedChemicalTwoAmount = grabbableLiquid.foamSubstanceAmount;
                }
            }
            else if (socketLiquid && socketInteractor.hasSelection)
            {
                grabbableLiquidTransfer = grabbableObject.GetComponent<LiquidTransfer>();

                grabbableLimitedNonLiquid = grabbableLiquidTransfer.limitedNonLiquid;
                grabbableNonLiquidColor = grabbableLiquidTransfer.nonLiquidColor;
                grabbableNonLiquidAmount = grabbableLiquidTransfer.nonLiquidAmount;
                grabbabletransferRate = grabbableLiquidTransfer.transferRate;
                grabbableNonLiquidSubstance = grabbableLiquidTransfer.nonLiquidSubstance;

                socketLiquid.isSocketed = true;
                float transferAmount = grabbabletransferRate * Time.deltaTime;

                if ((!grabbableLimitedNonLiquid || grabbableNonLiquidAmount > 0f) &&
                (socketLiquid.fillAmount + transferAmount < 1f || socketLiquid.fillAmount + grabbableNonLiquidAmount < 1f) && !canReact)
                {
                    if (!grabbableLimitedNonLiquid)
                        socketLiquid.fillAmount = Mathf.Clamp(socketLiquid.fillAmount + transferAmount, 0f, 1f);
                    else
                    {
                        if (grabbableNonLiquidAmount > transferAmount)
                        {
                            liquidChangeAmount = transferAmount;
                            socketLiquid.fillAmount = Mathf.Clamp(socketLiquid.fillAmount + transferAmount, 0f, 1f);
                            grabbableNonLiquidAmount = grabbableNonLiquidAmount - transferAmount;
                            grabbableLiquidTransfer.nonLiquidAmount = grabbableNonLiquidAmount;
                        }
                        else
                        {
                            liquidChangeAmount = socketLiquid.fillAmount - Mathf.Clamp(socketLiquid.fillAmount + grabbableNonLiquidAmount, 0f, 1f);
                            socketLiquid.fillAmount = Mathf.Clamp(socketLiquid.fillAmount + grabbableNonLiquidAmount, 0f, 1f);
                            grabbableNonLiquidAmount = 0f;
                            grabbableLiquidTransfer.nonLiquidAmount = 0f;

                            foreach (Transform child in grabbableObject.transform)
                            {
                                if (child.CompareTag("Non Liquid"))
                                    Destroy(child.gameObject);
                            }
                        }
                    }

                    // Foam properties and color adjustments
                    float socketLineWidth = socketLiquid.liquidRenderer.material.GetFloat("_Line");
                    float socketLineSmooth = socketLiquid.liquidRenderer.material.GetFloat("_LineSmooth");

                    float newSocketLineWidth = Mathf.Clamp(socketLineWidth + foamWidthIncreaseRate * Time.deltaTime, 0f, 1f);
                    float newSocketLineSmooth = Mathf.Clamp(socketLineSmooth + foamSmoothIncreaseRate * Time.deltaTime, 0f, .1f);

                    socketLiquid.liquidRenderer.material.SetFloat("_Line", newSocketLineWidth);
                    socketLiquid.liquidRenderer.material.SetFloat("_LineSmooth", newSocketLineSmooth);

                    if (isFirstInteractionInstance)
                    {
                        Color socketTopColor = socketLiquid.liquidRenderer.material.GetColor("_FoamColor");
                        socketLiquid.liquidRenderer.material.SetColor("_TopColor", socketTopColor);
                        if (socketLiquid.foamSubstance == "")
                            socketLiquid.liquidRenderer.material.SetColor("_FoamColor", socketTopColor);
                        if (socketLiquid.foamSubstance != "")
                            socketLiquid.liquidRenderer.material.SetColor("_TopColor", socketTopColor);
                        else
                            socketLiquid.liquidRenderer.material.SetColor("_TopColor", grabbableNonLiquidColor);
                        socketLiquid.topSubstanceAmount = socketLiquid.foamSubstanceAmount;
                        socketLiquid.foamSubstanceAmount = 0f;
                        socketLiquid.topSubstance = socketLiquid.foamSubstance;
                        isFirstInteractionInstance = false;
                    }

                    socketLiquid.liquidRenderer.material.SetColor("_FoamColor", grabbableNonLiquidColor);
                    socketLiquid.foamSubstanceAmount += liquidChangeAmount;
                    socketLiquid.foamSubstance = grabbableNonLiquidSubstance;

                    if ((socketLiquid.topSubstanceAmount >= fillAmountOfSubstanceOne && socketLiquid.foamSubstanceAmount >= fillAmountOfSubstanceTwo) ||
                    (socketLiquid.topSubstanceAmount >= fillAmountOfSubstanceTwo && socketLiquid.foamSubstanceAmount >= fillAmountOfSubstanceOne))
                        canReact = true;
            }
            else
            {
                if (canDuplicate == true &&
                ((socketLiquid.topSubstanceAmount >= fillAmountOfSubstanceOne && socketLiquid.foamSubstanceAmount >= fillAmountOfSubstanceTwo) ||
                (socketLiquid.topSubstanceAmount >= fillAmountOfSubstanceTwo && socketLiquid.foamSubstanceAmount >= fillAmountOfSubstanceOne)))
                {
                    DuplicateObject(this.gameObject);
                    canDuplicate = false;
                    socketLiquid.isSocketed = false;
                }
            }
            GlobalChemistryData.instance.mixedChemicalOne = socketLiquid.topSubstance;
            GlobalChemistryData.instance.mixedChemicalTwo = socketLiquid.foamSubstance;
            GlobalChemistryData.instance.mixedChemicalOneAmount = socketLiquid.topSubstanceAmount;
            GlobalChemistryData.instance.mixedChemicalTwoAmount = socketLiquid.foamSubstanceAmount;
        }

    }

    void DuplicateObject(GameObject gameObjectDupl = null)
    {
        GameObject duplicate;
        if (gameObjectDupl == null)
        {
            // Create a duplicate of the current object
            duplicate = Instantiate(gameObject, transform.position, transform.rotation);
            // Make the duplicate a child of the original object
            duplicate.transform.parent = gameObject.transform;

            // Make the duplicate slightly larger
            duplicate.transform.localScale = transform.localScale * 1.05f;
        }
        else
        {
            duplicate = Instantiate(gameObjectDupl, transform.position, transform.rotation);
            duplicate.transform.parent = gameObjectDupl.transform;
            duplicate.transform.localScale = Vector3.one * 1.05f;
        }
        duplicatedObject = duplicate;
        // Update duplicate's local position and rotation to match original
        duplicate.transform.localPosition = Vector3.zero;
        duplicate.transform.localRotation = Quaternion.identity;
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
        {
            //NextScene();
            yield break;
        }

        Material mat = renderer.material;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float transparency = Mathf.Lerp(1, 0, elapsed / duration);
            mat.SetFloat("_Alpha", 1 - transparency); // Alpha values from 0 for transparent and 1 for opaque
            yield return null;
        }

        //NextScene();
    }

    public void NextScene()
    {
        SceneManager.LoadSceneAsync("TestSharon");
    }

    void CheckAndDestroyDuplicateIfNecessary()
    {
        if (duplicatedObject != null)
        {
            Liquid duplicateLiquid = gameObject.GetComponentInChildren<Liquid>(true);
            if (duplicateLiquid != null &&
                !((duplicateLiquid.topSubstanceAmount >= fillAmountOfSubstanceOne && duplicateLiquid.foamSubstanceAmount >= fillAmountOfSubstanceTwo) ||
                (duplicateLiquid.topSubstanceAmount >= fillAmountOfSubstanceTwo && duplicateLiquid.foamSubstanceAmount >= fillAmountOfSubstanceOne)))
            {
                Destroy(duplicatedObject);
                duplicatedObject = null;
                canReact = false;
            }
        }
    }
}