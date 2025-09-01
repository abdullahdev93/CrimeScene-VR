using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class EvidenceListController : MonoBehaviour
{
    public Toggle HammerToggle;
    public Toggle InjectionToggle;
    public Toggle CombToggle;
    public Toggle PepsiCanToggle;
    public Toggle CokeCanToggle;
    public Toggle PlasticBottleToggle;
    public Toggle WineBottleToggle;
    public Toggle TabletToggle;
    public Toggle DrugsToggle;
    public Toggle ClothPieceToggle;
    public Toggle CoffeeCupToggle;
    public Toggle Glass1Toggle;
    public Toggle Glass2Toggle;

    public static EvidenceListController instance;

    public int EvidenceCollected = 0;

    public TextMeshProUGUI Message;

    private void Awake()
    {
        instance = this;
    }

    public void SetEvidenceCollected(string evidenceName)
    {
        switch (evidenceName) 
        {
           case HammerEvidence:
           HammerToggle.isOn = true;
           break;
           case InjectionEvidence:
           InjectionToggle.isOn = true;
           break;
           case CombEvidence:
           CombToggle.isOn = true;
           break;
           case BottlesEvidence1:
           PepsiCanToggle.isOn = true;
           break;
            case BottlesEvidence2:
                CokeCanToggle.isOn = true;
                break;
            case BottlesEvidence3:
                PlasticBottleToggle.isOn = true;
                break;
            case BottlesEvidence4:
                WineBottleToggle.isOn = true;
                break;
            case TabletEvidence:
           TabletToggle.isOn = true;
           break;
           case DrugsEvidence: 
           DrugsToggle.isOn = true;
           break;
           case ClothPieceEvidence:
           ClothPieceToggle.isOn = true;
           break;
           case CoffeeCupEvidence:
           CoffeeCupToggle.isOn = true;
           break;
           case GalssesEvidence1:
           Glass1Toggle.isOn = true;
           break;
            case GalssesEvidence2:
                Glass2Toggle.isOn = true;
                break;
        }
        EvidenceCollected++;
        if(EvidenceCollected == 13) 
        { 
            Message.gameObject.SetActive(true);
            SuspectBoardUIHandler.instance.ShowSuspects(); 
            Debug.Log("All evidence collected, now pick the suspect"); 
        }
    }

    public const string HammerEvidence = "Hammer";
    public const string InjectionEvidence = "Injection";
    public const string CombEvidence = "Comb";
    public const string BottlesEvidence1 = "Bottles1";
    public const string BottlesEvidence2= "Bottles2";
    public const string BottlesEvidence3 = "Bottles3";
    public const string BottlesEvidence4 = "Bottles4";
    public const string TabletEvidence = "Tablet";
    public const string DrugsEvidence = "Drugs";
    public const string ClothPieceEvidence = "ClothPiece";
    public const string CoffeeCupEvidence = "CoffeeCup";
    public const string GalssesEvidence1 = "Glasses1";
    public const string GalssesEvidence2 = "Glasses2";

}
