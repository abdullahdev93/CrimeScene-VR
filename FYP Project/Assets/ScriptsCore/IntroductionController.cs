using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionController : MonoBehaviour
{
    public static IntroductionController instance;
    public GameObject IntroductionCanvas;
    public List<GameObject> Grabbables = new List<GameObject>();
    public Button ProceedBtn;

    public bool interactedWithFirstGrabbable = false;
    public GameObject HighlightObject;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(DelayForRigidbody());
        ProceedBtn.onClick.AddListener(delegate ()
        { 
            ShowhideIntroCanvas(false);
            SetAllGrabbablesTrueFalse(true);

        });
    }
    IEnumerator DelayForRigidbody()
    {
        yield return new WaitForSeconds(2f);
        SoundsManager.Instance.PlayIntroSource();
        SetAllGrabbablesTrueFalse(false);

    }
    public void ShowhideIntroCanvas(bool status)
    {
        IntroductionCanvas.SetActive(status);
    }

    public void SetAllGrabbablesTrueFalse(bool status)
    {
        Debug.LogError("setting all grabbables " + status);
        for (int i = 0; i < Grabbables.Count; i++)
        {
            var go = Grabbables[i];
            if (go == null)
            {
                Debug.LogWarning($"[Grabbables] Element at index {i} is null!");
                continue;
            }

            var grabbable = go.GetComponent<Grabbable>();
            if (grabbable != null) grabbable.enabled = status;
            else Debug.LogWarning($"[Grabbables] {go.name} is missing Grabbable component");

            var grabInteractable = go.GetComponent<GrabInteractable>();
            if (grabInteractable != null) grabInteractable.enabled = status;
            else Debug.LogWarning($"[Grabbables] {go.name} is missing GrabInteractable component");

            var distanceGrab = go.GetComponent<DistanceGrabInteractable>();
            if (distanceGrab != null) distanceGrab.enabled = status;
            else Debug.LogWarning($"[Grabbables] {go.name} is missing DistanceGrabInteractable component");

            var boxCol = go.GetComponent<BoxCollider>();
            if (boxCol != null) boxCol.enabled = status;
            else Debug.LogWarning($"[Grabbables] {go.name} is missing BoxCollider component");

            var rb = go.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = !status;
            else Debug.LogWarning($"[Grabbables] {go.name} is missing Rigidbody component");
        }

    }
}
