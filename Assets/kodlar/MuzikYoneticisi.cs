using UnityEngine;

public class MuzikYoneticisi : MonoBehaviour
{
    public static MuzikYoneticisi ornek;

    public AudioSource muzikKaynagi;
    public AudioClip asitVeDigerOdalarMuzigi; // Asit -> Final arası
    public AudioClip finalOdaMuzigi;          // Sadece FinalOda

    void Awake()
    {
        ornek = this;
        if (muzikKaynagi == null) muzikKaynagi = GetComponent<AudioSource>();
        muzikKaynagi.loop = true;
    }

    public void OyunMuziginiCal()
    {
        if (muzikKaynagi.clip == asitVeDigerOdalarMuzigi) return;
        muzikKaynagi.clip = asitVeDigerOdalarMuzigi;
        muzikKaynagi.Play();
    }

    public void FinalMuzigineGec()
    {
        if (muzikKaynagi.clip == finalOdaMuzigi) return;
        muzikKaynagi.Stop();
        muzikKaynagi.clip = finalOdaMuzigi;
        muzikKaynagi.Play();
    }
}