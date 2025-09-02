using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuspectBoardUIHandler : MonoBehaviour
{

    public static SuspectBoardUIHandler instance;
    public List<GameObject> SuspectsData;

    public Button NextSuspectBtn;

    int currentSuspectIndex = 0;

    public Button JakeConfirmBtn;
    public Button LanaConfirmBtn;
    public Button TheoConfirmBtn;

    public Button Play1Btn;
    public Button Play2Btn;
    public Button Play3Btn;

    public AudioSource Suspect1Sound;
    public AudioSource Suspect2Sound;
    public AudioSource Suspect3Sound;

    public TextMeshProUGUI SuccessFailMessageText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //HideSuspects();
        ShowSuspects(false);
        NextSuspectBtn.onClick.AddListener(delegate () 
        {
            ShowNextSuspect();
        });

        JakeConfirmBtn.interactable = false;
        LanaConfirmBtn.interactable = false;
        TheoConfirmBtn.interactable = false;

        JakeConfirmBtn.onClick.AddListener(delegate () { ShowSuccessFailMessage(false); });
        LanaConfirmBtn.onClick.AddListener(delegate () { ShowSuccessFailMessage(true); });
        TheoConfirmBtn.onClick.AddListener(delegate () { ShowSuccessFailMessage(false); });

        Play1Btn.onClick.AddListener(delegate () 
        {
            Suspect1Sound.Play();
        });
        Play2Btn.onClick.AddListener(delegate ()
        {
            Suspect2Sound.Play();
        });
        Play3Btn.onClick.AddListener(delegate ()
        {
            Suspect3Sound.Play();
        });
    }

    private void ShowNextSuspect()
    {
        for(int i = 0; i < SuspectsData.Count; i++)
        {
            SuspectsData[i].SetActive(false);
        }
        currentSuspectIndex++;
        if (currentSuspectIndex == 1) { Suspect1Sound.Stop(); Suspect2Sound.Play(); }
        if(currentSuspectIndex == 2) {  Suspect2Sound.Stop(); Suspect3Sound.Play();}
        if (currentSuspectIndex >= SuspectsData.Count) currentSuspectIndex = 0;
        SuspectsData[currentSuspectIndex].SetActive(true);
    }

    public void ShowSuccessFailMessage(bool status)
    {
        if (InputsController.Instance.evidenceCollected < 13) return;
        for (int i = 0;i < SuspectsData.Count;i++)
        {
            SuspectsData[i].SetActive(false);
        }
        NextSuspectBtn.gameObject.SetActive(false);
        SuccessFailMessageText.gameObject.SetActive(true);
        if(status)
        {
            SuccessFailMessageText.text = "Congratulations, you have successfully caught the murderer";
        }
        else
        {
            SuccessFailMessageText.text = "You have picked the wrong person";
        }
    }

    public void HideSuspects()
    {
        for (int i = 0; i < SuspectsData.Count; i++)
        {
            SuspectsData[i].SetActive(false);
        }
        NextSuspectBtn.gameObject.SetActive(false);
        SuccessFailMessageText.gameObject.SetActive(true);
        SuccessFailMessageText.text = "Please complete evidence list to see all the suspects";
    }

    public void ShowSuspects(bool status)
    {
        if(status)
        {
            JakeConfirmBtn.interactable = true;
            LanaConfirmBtn.interactable = true;
            TheoConfirmBtn.interactable = true;
        }
        SuspectsData[currentSuspectIndex].SetActive(true);
        NextSuspectBtn.gameObject.SetActive(true);
        SuccessFailMessageText.gameObject.SetActive(false);
    }

}
