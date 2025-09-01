using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Grab;
using System.Collections;

public class EvidenceBagController : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<ReleaseController>() == null) { return; }
        if(other.gameObject.GetComponent<ReleaseController>().EvidenceName == EvidenceListController.HammerEvidence)
        {
            other.gameObject.GetComponent<ReleaseController>().Marker.SetActive(true);
            InputsController.Instance.evidenceCollected++;
            other.gameObject.GetComponent<ReleaseController>().Marker.GetComponent<MaarkerEntity>().SetMarkerNumber(InputsController.Instance.evidenceCollected);
            EvidenceListController.instance.SetEvidenceCollected(EvidenceListController.HammerEvidence);
            Destroy(other.gameObject);
            InputsController.Instance.Leftbag.SetActive(false);
            InputsController.Instance.Rightbag.SetActive(false);
        }
        else if (other.gameObject.GetComponent<ReleaseController>().EvidenceName == EvidenceListController.InjectionEvidence)
        {
            other.gameObject.GetComponent<ReleaseController>().Marker.SetActive(true);
            InputsController.Instance.evidenceCollected++;
            other.gameObject.GetComponent<ReleaseController>().Marker.GetComponent<MaarkerEntity>().SetMarkerNumber(InputsController.Instance.evidenceCollected);
            EvidenceListController.instance.SetEvidenceCollected(EvidenceListController.InjectionEvidence);
            Destroy(other.gameObject);
            InputsController.Instance.Leftbag.SetActive(false);
            InputsController.Instance.Rightbag.SetActive(false);
        }
        else if (other.gameObject.GetComponent<ReleaseController>().EvidenceName == EvidenceListController.CombEvidence)
        {
            other.gameObject.GetComponent<ReleaseController>().Marker.SetActive(true);
            InputsController.Instance.evidenceCollected++;
            other.gameObject.GetComponent<ReleaseController>().Marker.GetComponent<MaarkerEntity>().SetMarkerNumber(InputsController.Instance.evidenceCollected);
            EvidenceListController.instance.SetEvidenceCollected(EvidenceListController.CombEvidence);
            Destroy(other.gameObject);
            InputsController.Instance.Leftbag.SetActive(false);
            InputsController.Instance.Rightbag.SetActive(false);
        }
        else if (other.gameObject.GetComponent<ReleaseController>().EvidenceName == EvidenceListController.BottlesEvidence1)
        {
            other.gameObject.GetComponent<ReleaseController>().Marker.SetActive(true);
            InputsController.Instance.evidenceCollected++;
            other.gameObject.GetComponent<ReleaseController>().Marker.GetComponent<MaarkerEntity>().SetMarkerNumber(InputsController.Instance.evidenceCollected);
            EvidenceListController.instance.SetEvidenceCollected(EvidenceListController.BottlesEvidence1);
            Destroy(other.gameObject);
            InputsController.Instance.Leftbag.SetActive(false);
            InputsController.Instance.Rightbag.SetActive(false);
        }
        else if (other.gameObject.GetComponent<ReleaseController>().EvidenceName == EvidenceListController.BottlesEvidence2)
        {
            other.gameObject.GetComponent<ReleaseController>().Marker.SetActive(true);
            InputsController.Instance.evidenceCollected++;
            other.gameObject.GetComponent<ReleaseController>().Marker.GetComponent<MaarkerEntity>().SetMarkerNumber(InputsController.Instance.evidenceCollected);
            EvidenceListController.instance.SetEvidenceCollected(EvidenceListController.BottlesEvidence2);
            Destroy(other.gameObject);
            InputsController.Instance.Leftbag.SetActive(false);
            InputsController.Instance.Rightbag.SetActive(false);
        }
        else if (other.gameObject.GetComponent<ReleaseController>().EvidenceName == EvidenceListController.BottlesEvidence3)
        {
            other.gameObject.GetComponent<ReleaseController>().Marker.SetActive(true);
            InputsController.Instance.evidenceCollected++;
            other.gameObject.GetComponent<ReleaseController>().Marker.GetComponent<MaarkerEntity>().SetMarkerNumber(InputsController.Instance.evidenceCollected);
            EvidenceListController.instance.SetEvidenceCollected(EvidenceListController.BottlesEvidence3);
            Destroy(other.gameObject);
            InputsController.Instance.Leftbag.SetActive(false);
            InputsController.Instance.Rightbag.SetActive(false);
        }
        else if (other.gameObject.GetComponent<ReleaseController>().EvidenceName == EvidenceListController.BottlesEvidence4)
        {
            other.gameObject.GetComponent<ReleaseController>().Marker.SetActive(true);
            InputsController.Instance.evidenceCollected++;
            other.gameObject.GetComponent<ReleaseController>().Marker.GetComponent<MaarkerEntity>().SetMarkerNumber(InputsController.Instance.evidenceCollected);
            EvidenceListController.instance.SetEvidenceCollected(EvidenceListController.BottlesEvidence4);
            Destroy(other.gameObject);
            InputsController.Instance.Leftbag.SetActive(false);
            InputsController.Instance.Rightbag.SetActive(false);
        }
        else if (other.gameObject.GetComponent<ReleaseController>().EvidenceName == EvidenceListController.TabletEvidence)
        {
            other.gameObject.GetComponent<ReleaseController>().Marker.SetActive(true);
            InputsController.Instance.evidenceCollected++;
            other.gameObject.GetComponent<ReleaseController>().Marker.GetComponent<MaarkerEntity>().SetMarkerNumber(InputsController.Instance.evidenceCollected);
            EvidenceListController.instance.SetEvidenceCollected(EvidenceListController.TabletEvidence);
            Destroy(other.gameObject);
            InputsController.Instance.Leftbag.SetActive(false);
            InputsController.Instance.Rightbag.SetActive(false);
        }
        else if (other.gameObject.GetComponent<ReleaseController>().EvidenceName == EvidenceListController.DrugsEvidence)
        {
            other.gameObject.GetComponent<ReleaseController>().Marker.SetActive(true);
            InputsController.Instance.evidenceCollected++;
            other.gameObject.GetComponent<ReleaseController>().Marker.GetComponent<MaarkerEntity>().SetMarkerNumber(InputsController.Instance.evidenceCollected);
            EvidenceListController.instance.SetEvidenceCollected(EvidenceListController.DrugsEvidence);
            Destroy(other.gameObject);
            InputsController.Instance.Leftbag.SetActive(false);
            InputsController.Instance.Rightbag.SetActive(false);
        }
        else if (other.gameObject.GetComponent<ReleaseController>().EvidenceName == EvidenceListController.ClothPieceEvidence)
        {
            other.gameObject.GetComponent<ReleaseController>().Marker.SetActive(true);
            InputsController.Instance.evidenceCollected++;
            other.gameObject.GetComponent<ReleaseController>().Marker.GetComponent<MaarkerEntity>().SetMarkerNumber(InputsController.Instance.evidenceCollected);
            EvidenceListController.instance.SetEvidenceCollected(EvidenceListController.ClothPieceEvidence);
            Destroy(other.gameObject);
            InputsController.Instance.Leftbag.SetActive(false);
            InputsController.Instance.Rightbag.SetActive(false);
        }
        else if (other.gameObject.GetComponent<ReleaseController>().EvidenceName == EvidenceListController.CoffeeCupEvidence)
        {
            other.gameObject.GetComponent<ReleaseController>().Marker.SetActive(true);
            InputsController.Instance.evidenceCollected++;
            other.gameObject.GetComponent<ReleaseController>().Marker.GetComponent<MaarkerEntity>().SetMarkerNumber(InputsController.Instance.evidenceCollected);
            EvidenceListController.instance.SetEvidenceCollected(EvidenceListController.CoffeeCupEvidence);
            Destroy(other.gameObject);
            InputsController.Instance.Leftbag.SetActive(false);
            InputsController.Instance.Rightbag.SetActive(false);
        }
        else if (other.gameObject.GetComponent<ReleaseController>().EvidenceName == EvidenceListController.GalssesEvidence1)
        {
            other.gameObject.GetComponent<ReleaseController>().Marker.SetActive(true);
            InputsController.Instance.evidenceCollected++;
            other.gameObject.GetComponent<ReleaseController>().Marker.GetComponent<MaarkerEntity>().SetMarkerNumber(InputsController.Instance.evidenceCollected);
            EvidenceListController.instance.SetEvidenceCollected(EvidenceListController.GalssesEvidence1);
            Destroy(other.gameObject);
            InputsController.Instance.Leftbag.SetActive(false);
            InputsController.Instance.Rightbag.SetActive(false);
        }
        else if (other.gameObject.GetComponent<ReleaseController>().EvidenceName == EvidenceListController.GalssesEvidence2)
        {
            other.gameObject.GetComponent<ReleaseController>().Marker.SetActive(true);
            InputsController.Instance.evidenceCollected++;
            other.gameObject.GetComponent<ReleaseController>().Marker.GetComponent<MaarkerEntity>().SetMarkerNumber(InputsController.Instance.evidenceCollected);
            EvidenceListController.instance.SetEvidenceCollected(EvidenceListController.GalssesEvidence2);
            Destroy(other.gameObject);
            InputsController.Instance.Leftbag.SetActive(false);
            InputsController.Instance.Rightbag.SetActive(false);
        }
    }
}
