using UnityEngine;
using System.Collections.Generic;

public class OdaDegistirici : MonoBehaviour
{
    [Header("Oda Ayarları")]
    public int odaNo; // Unity Inspector'dan mutlaka ayarla (Tutorial: 1-2, Puan: 3+, Final: 9)
    public GameObject odaKapisi;
    public List<KimyaSisesi> odadakiSiseler;

    // Bu kilit sistemi odaya her girişte kodun tekrar çalışmasını engeller
    private bool odayaGirildi = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sadece Oyuncu (Player) girdiğinde tetiklenir
        if (other.CompareTag("Player"))
        {
            if (!odayaGirildi)
            {
                odayaGirildi = true; // Odayı "ziyaret edildi" olarak işaretle

                // 1. MEKANİK VE SORU KURULUMU
                if (ANAKALP.ornek != null)
                {
                    ANAKALP.ornek.OdaKurulumu(odaNo, odadakiSiseler, odaKapisi);
                }

                // 2. ATMOSFER VE SARSINTI TETİKLEME
                if (GeriSayimYoneticisi.ornek != null)
                {
                    GeriSayimYoneticisi.ornek.AtmosferTetikle(odaNo);
                }

                // 3. PUAN SİSTEMİ YÖNETİMİ
                if (PuanYoneticisi.ornek != null)
                {
                    // Tutorial odalarında (1 ve 2) puanlama başlamaz
                    if (odaNo >= 1)
                    {
                        PuanYoneticisi.ornek.ZorlukKatlayaniAyarla(odaNo);
                        PuanYoneticisi.ornek.PuanlamayiBaslat();

                        Debug.Log("Oda " + odaNo + ": Puanlama Aktif!");
                    }

                    // Eğer girilen oda Final Odası (9) ise oyunu bitir
                    if (odaNo == 9)
                    {
                        PuanYoneticisi.ornek.OyunuBitir();
                        Debug.Log("Final Odası: Oyun Bitti Ekranı Getiriliyor...");
                    }
                }
            }
        }
    }
}