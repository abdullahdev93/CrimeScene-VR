using TMPro;
using UnityEngine;

public class MaarkerEntity : MonoBehaviour
{
    public TextMeshProUGUI MarkerNumber;
   
    public void SetMarkerNumber(int number)
    {
        MarkerNumber.text = number.ToString();
    }
}
