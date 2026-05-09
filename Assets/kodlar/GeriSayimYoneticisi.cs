using UnityEngine;
using TMPro;
using System.Collections;

public class GeriSayimYoneticisi : MonoBehaviour
{
    public static GeriSayimYoneticisi ornek;

    [Header("UI Ayarları")]
    public TextMeshProUGUI geriSayimMetni;

    [Header("Ses ve Hız Ayarları")]
    public AudioSource sesKaynagi;
    public AudioClip sayimSesi;
    [Range(0.1f, 2f)] public float sesHizi = 1f; // KANKA: Buradan sesi yavaşlatıp hızlandırabilirsin (0.8f yavaşlatır)

    private bool sayimBasladi = false;

    void Awake()
    {
        ornek = this;
        if (geriSayimMetni != null) geriSayimMetni.gameObject.SetActive(false);
    }

    public void SayimiBaslat()
    {
        if (sayimBasladi) return;
        sayimBasladi = true;

        StartCoroutine(SayimRutini());
    }

    IEnumerator SayimRutini()
    {
        // 1. Karakteri dondur
        GameObject oyuncu = GameObject.FindGameObjectWithTag("Player");
        var hareket = oyuncu?.GetComponent<MonoBehaviour>();
        if (hareket != null) hareket.enabled = false;

        if (geriSayimMetni != null) geriSayimMetni.gameObject.SetActive(true);

        // 2. SES AYARI: Sesin hızını buradan ayarlıyoruz
        if (sesKaynagi != null && sayimSesi != null)
        {
            sesKaynagi.pitch = sesHizi; // Sesi yavaşlatır/hızlandırır
            sesKaynagi.clip = sayimSesi;
            sesKaynagi.Play();

            // KANKA: Sesin toplam süresini hızına göre hesaplıyoruz
            float toplamSure = sayimSesi.length / sesHizi;
            float saniyeBasinaDusen = toplamSure / 4f; // 3-2-1-BAŞLA toplam 4 aşama

            // Görsel sayımı sesin gerçek süresine bölüyoruz
            geriSayimMetni.text = "3";
            yield return new WaitForSeconds(saniyeBasinaDusen);

            geriSayimMetni.text = "2";
            yield return new WaitForSeconds(saniyeBasinaDusen);

            geriSayimMetni.text = "1";
            yield return new WaitForSeconds(saniyeBasinaDusen);

            geriSayimMetni.text = "BAŞLA!";

            // BAŞLA dedikten sonra sistemleri aç
            MuzikYoneticisi.ornek?.OyunMuziginiCal();
            if (PuanYoneticisi.ornek != null) PuanYoneticisi.ornek.PuanlamayiBaslat();
            if (hareket != null) hareket.enabled = true;

            yield return new WaitForSeconds(saniyeBasinaDusen);
        }

        if (geriSayimMetni != null) geriSayimMetni.gameObject.SetActive(false);
    }
}