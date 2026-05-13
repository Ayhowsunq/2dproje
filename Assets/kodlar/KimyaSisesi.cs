using UnityEngine;

public class KimyaSisesi : MonoBehaviour
{
    [HideInInspector] // Unity arayüzünde kafanı karıştırmasın, kalp bunu gizlice dolduracak
    public string ipucu;

    [HideInInspector]
    public bool dogruMu; // Kalp buna true veya false basacak

    private bool oyuncuYakinda = false;

    void Update()
    {
        if (oyuncuYakinda && Input.GetKeyDown(KeyCode.E))
        {
            // Şişe burada kendi karar vermiyor, puan yöneticisine bendeki değer neyse onu gönder diyor
            PuanYoneticisi.ornek?.SkorHesapla(dogruMu);
            ANAKALP.ornek.SecimYap(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            oyuncuYakinda = true;
            // Kalbin içine yazdığı o ipucunu ekranda gösteriyoruz
            YaziYoneticisi.ornek?.BilgiGoster(ipucu, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            oyuncuYakinda = false;
            YaziYoneticisi.ornek?.BilgiGoster("", false);
        }
    }
}