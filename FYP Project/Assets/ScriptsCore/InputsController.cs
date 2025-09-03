using TMPro;
using UnityEngine;

public class InputsController : MonoBehaviour
{
    public static InputsController Instance;
    public GameObject EvidenceCanvas;

    public GameObject Leftbag;
    public GameObject Rightbag;

    public int evidenceCollected = 0;

    //public TextMeshProUGUI logger;
    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        if(OculusInput.GetButtonDownA())
        {
            EvidenceCanvas.SetActive(!EvidenceCanvas.activeSelf);
            
        }
    }

}
