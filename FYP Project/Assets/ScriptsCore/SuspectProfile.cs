using UnityEngine;

[CreateAssetMenu(menuName="CSI/Suspect Profile")]
public class SuspectProfile : ScriptableObject
{
    public string suspectId;     // e.g. "SUSPECT_JAKE"
    public string fullName;      // display name
    [TextArea] public string facts;
    [TextArea] public string alibi;
    public Sprite photo;         // headshot image (Sprite)
}
