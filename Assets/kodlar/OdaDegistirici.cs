using UnityEngine;

public class OdaDegistirici : MonoBehaviour
{
    public string gecilecekOdaAdi;
    public string yeniGorevMesaji;
    private bool tetiklendi = false; // KANKA: Bu değişken sistemin bir kez çalışmasını sağlar

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Odayı ve görevi güncellemek her seferinde olabilir, sorun yaratmaz
            KimyaSisesi.aktifOda = gecilecekOdaAdi;
            YaziYoneticisi.ornek?.GorevGuncelle(yeniGorevMesaji);

            // ASİT ODASI: Sayımı başlat ama sadece daha önce tetiklenmediyse!
            if (gecilecekOdaAdi == "AsitOdasi" && !tetiklendi)
            {
                tetiklendi = true; // Kilidi vuruyoruz, bir daha bu if bloğuna giremez
                GeriSayimYoneticisi.ornek?.SayimiBaslat();
            }

            // FİNAL ODASI: Müzik değişimi
            if (gecilecekOdaAdi == "FinalOda")
            {
                MuzikYoneticisi.ornek?.FinalMuzigineGec();
            }
        }
    }
}