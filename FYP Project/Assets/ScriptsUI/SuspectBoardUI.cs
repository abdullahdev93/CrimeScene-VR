using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class SuspectBoardUI : MonoBehaviour
{
    [Header("Data")]
    public SuspectProfile[] suspects;
    public string correctSuspectId = "SUSPECT_JAKE";

    [Header("UI - List")]
    public Transform listContent;     // ScrollView/Viewport/Content
    public Button listItemPrefab;     // a simple Button with TMP child text

    [Header("UI - Details")]
    public Image photo;
    public TMP_Text nameText;
    public TMP_Text factsText;
    public TMP_Text alibiText;

    [Header("UI - Actions")]
    public Button selectSuspectButton;
    public Button confirmChoiceButton;
    public TMP_Text resultText;

    [Header("Evidence Requirements")]
    [Tooltip("IDs the player MUST have bagged to win. Leave empty while testing.")]
    public string[] requiredEvidenceIds;

    string _currentShownId;   // which card is displayed
    string _selectedId;       // which suspect the player picked

    void Start()
    {
        BuildList();
        selectSuspectButton.onClick.AddListener(()=> { _selectedId = _currentShownId; resultText.text = $"Selected: {_selectedId}"; });
        confirmChoiceButton.onClick.AddListener(ConfirmChoice);
        resultText.text = "";
    }

    void BuildList()
    {
        foreach (Transform c in listContent) Destroy(c.gameObject);
        foreach (var s in suspects)
        {
            var b = Instantiate(listItemPrefab, listContent);
            b.name = s.suspectId;
            b.GetComponentInChildren<TMP_Text>().text = s.fullName;
            b.onClick.AddListener(()=> Show(s));
        }
        if (suspects.Length > 0) Show(suspects[0]);
    }

    void Show(SuspectProfile s)
    {
        _currentShownId = s.suspectId;
        if (photo)     photo.sprite = s.photo;
        if (nameText)  nameText.text = s.fullName;
        if (factsText) factsText.text = s.facts;
        if (alibiText) alibiText.text = s.alibi;
    }

    void ConfirmChoice()
    {
        // evidence collected so far
        var bagged = EvidenceLog.All
            .Where(e => e.action == "BAGGED")
            .Select(e => e.evidenceId)
            .Distinct()
            .ToHashSet();

        var missing = new List<string>();
        foreach (var req in requiredEvidenceIds)
            if (!bagged.Contains(req)) missing.Add(req);

        bool allEvidence = missing.Count == 0 || requiredEvidenceIds.Length == 0;
        bool correct = _selectedId == correctSuspectId;

        if (correct && allEvidence)
            resultText.text = "✅ You WIN! Correct suspect and required evidence collected.";
        else
            resultText.text = "❌ You FAIL. "
                + (correct ? "" : "Wrong suspect. ")
                + (allEvidence ? "" : "Missing: " + string.Join(", ", missing));
    }
}
