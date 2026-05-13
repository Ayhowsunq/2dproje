using UnityEngine;
using TMPro;
using System.Collections;

public class GeriSayimYoneticisi : MonoBehaviour
{
    public static GeriSayimYoneticisi ornek;

    [Header("UI & Ses Bileşenleri")]
    public TextMeshProUGUI geriSayimMetni;
    public AudioSource sayimSesKaynagi;
    public AudioClip sayimSesi;

    [Header("Inspector Pitch Ayarları")]
    public float oda1Pitch = 1.0f;
    public float oda3Pitch = 1.3f;
    public float oda6Pitch = 1.6f;

    private bool sayimDevamEdiyor = false;

    void Awake()
    {
        ornek = this;
        if (geriSayimMetni != null) geriSayimMetni.gameObject.SetActive(false);
    }

    public void AtmosferTetikle(int odaNo)
    {
        BasitGirisVeKamera.ornek?.SarsintiyiAyarla(odaNo);

        if (odaNo == 1 || odaNo == 3 || odaNo == 6)
        {
            if (!sayimDevamEdiyor) StartCoroutine(SayimRutini(odaNo));
        }
        else if (odaNo == 9)
        {
            MuzikYoneticisi.ornek?.AsamayaGoreMuzikCal(9);
        }
    }

    IEnumerator SayimRutini(int odaNo)
    {
        sayimDevamEdiyor = true;

        // Karakteri dondur
        GameObject oyuncu = GameObject.FindGameObjectWithTag("Player");
        var hareket = oyuncu?.GetComponent<MonoBehaviour>();
        if (hareket != null) hareket.enabled = false;

        if (geriSayimMetni != null)
        {
            geriSayimMetni.text = ""; // Temiz başla
            geriSayimMetni.gameObject.SetActive(true);
        }

        float secilenPitch = (odaNo == 1) ? oda1Pitch : (odaNo == 3) ? oda3Pitch : oda6Pitch;

        if (sayimSesKaynagi != null && sayimSesi != null)
        {
            sayimSesKaynagi.pitch = secilenPitch;
            sayimSesKaynagi.clip = sayimSesi;
            sayimSesKaynagi.Play();

            float toplamSure = sayimSesi.length / secilenPitch;
            float saniyeBasinaDusen = toplamSure / 4f;

            // KANKA: Döngüyü daha kontrollü yapıyoruz
            string[] adimlar = { "3", "2", "1", "BAŞLA!" };

            for (int i = 0; i < adimlar.Length; i++)
            {
                geriSayimMetni.text = adimlar[i];

                if (adimlar[i] == "BAŞLA!")
                {
                    MuzikYoneticisi.ornek?.AsamayaGoreMuzikCal(odaNo);
                    // Puanlama kısmını halledince burayı açarsın
                    // if (PuanYoneticisi.ornek != null) PuanYoneticisi.ornek.PuanlamayiBaslat();
                    if (hareket != null) hareket.enabled = true;
                }

                yield return new WaitForSeconds(saniyeBasinaDusen);
            }
        }

        // KANKA: Burada metni tamamen kapatıyoruz, 
        // Döngü bittiği an "3" yazma ihtimalini ortadan kaldırdık.
        if (geriSayimMetni != null)
        {
            geriSayimMetni.text = "";
            geriSayimMetni.gameObject.SetActive(false);
        }

        sayimDevamEdiyor = false;
    }
}