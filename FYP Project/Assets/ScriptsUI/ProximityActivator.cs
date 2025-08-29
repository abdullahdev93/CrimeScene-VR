using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ProximityActivator : MonoBehaviour
{
    public GameObject targetUI;   // the Canvas object
    public string playerTag = "Player";

    void Reset(){ var c = GetComponent<Collider>(); c.isTrigger = true; }

    void OnTriggerEnter(Collider other){ if (other.CompareTag(playerTag)) targetUI.SetActive(true); }
    void OnTriggerExit (Collider other){ if (other.CompareTag(playerTag)) targetUI.SetActive(false); }
}
