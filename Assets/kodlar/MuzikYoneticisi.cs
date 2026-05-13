using UnityEngine;

public class MuzikYoneticisi : MonoBehaviour
{
    public static MuzikYoneticisi ornek;
    public AudioSource muzikKaynagi;
    public AudioClip[] odaMuzikleri; // 0: Pulse, 1: Pursuit, 2: Panic
    public AudioClip finalMuzigi;

    void Awake() { ornek = this; }

    public void AsamayaGoreMuzikCal(int odaNo)
    {
        // KANKA: Müzik çalmadan önce hızı her zaman normale (1.0) çekiyoruz
        if (muzikKaynagi != null) muzikKaynagi.pitch = 1f;

        if (odaNo == 1) MuzikOynat(0);
        else if (odaNo == 3) MuzikOynat(1);
        else if (odaNo == 6) MuzikOynat(2);
        else if (odaNo == 9) FinalMuzigineGec();
    }

    private void MuzikOynat(int index)
    {
        if (index < odaMuzikleri.Length && odaMuzikleri[index] != null)
        {
            muzikKaynagi.clip = odaMuzikleri[index];
            muzikKaynagi.loop = true;
            muzikKaynagi.Play();
        }
    }

    public void FinalMuzigineGec()
    {
        if (finalMuzigi != null)
        {
            muzikKaynagi.clip = finalMuzigi;
            muzikKaynagi.loop = true;
            muzikKaynagi.Play();
        }
    }
}