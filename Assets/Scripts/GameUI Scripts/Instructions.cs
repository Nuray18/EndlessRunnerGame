using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TextVisibility : MonoBehaviour
{
    public TMP_Text instructionsText;  // UI Text objeniz
    public float displayTime = 5f; // Ne kadar süreyle görünsün (saniye cinsinden)

    void Start()
    {
        StartCoroutine(ShowAndHideText());
    }

    IEnumerator ShowAndHideText()
    {
        // Metni görünür yap
        instructionsText.gameObject.SetActive(true);

        // Kırmızı renkte tuş talimatlarını ayarla
        instructionsText.text = "Sliding: <color=red>S</color> key\n" +
                                "Jumping: <color=red>Space</color> key\n" +
                                "Left and Right: <color=red>A</color> and <color=red>D</color> key";

        // Belirlenen süre kadar bekle
        yield return new WaitForSeconds(displayTime);

        // Metni gizle
        instructionsText.gameObject.SetActive(false);
    }
}
