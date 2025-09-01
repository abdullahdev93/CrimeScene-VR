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

    public TextMeshProUGUI SuccessFailMessageText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        HideSuspects();
        NextSuspectBtn.onClick.AddListener(delegate () 
        {
            ShowNextSuspect();
        });

        JakeConfirmBtn.onClick.AddListener(delegate () { ShowSuccessFailMessage(false); });
        LanaConfirmBtn.onClick.AddListener(delegate () { ShowSuccessFailMessage(true); });
        TheoConfirmBtn.onClick.AddListener(delegate () { ShowSuccessFailMessage(false); });
    }

    private void ShowNextSuspect()
    {
        for(int i = 0; i < SuspectsData.Count; i++)
        {
            SuspectsData[i].SetActive(false);
        }
        currentSuspectIndex++;
        if (currentSuspectIndex >= SuspectsData.Count) currentSuspectIndex = 0;
        SuspectsData[currentSuspectIndex].SetActive(true);
    }

    public void ShowSuccessFailMessage(bool status)
    {
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

    public void ShowSuspects()
    {
        SuspectsData[currentSuspectIndex].SetActive(true);
        NextSuspectBtn.gameObject.SetActive(true);
        SuccessFailMessageText.gameObject.SetActive(false);
    }

}
