using UnityEngine;

public class KimyaSisesi : MonoBehaviour
{
    [Header("Şişe / Kutu Özellikleri")]
    public string ipucu;
    public bool dogruMu;

    // Static değişken: Bunu Awake içinde sıfırlarsak her şişe kodu bozar.
    // O yüzden sadece başlangıç değerini veriyoruz.
    public static string aktifOda = "EksiBirinciOda";
    private bool oyuncuYakinda = false;

    // --- AWAKE FONKSİYONUNU SİLDİK ---
    // Çünkü sahnede 20 şişe varsa, 20 kere "aktifOda = EksiBirinciOda" diyordu.
    // Bu da -2. odadaki adamı sistemin -1'de sanmasına sebep oluyordu.

    void Update()
    {
        if (oyuncuYakinda && Input.GetKeyDown(KeyCode.E))
        {
            EtkilesimiGonder();
        }
    }

    void EtkilesimiGonder()
    {
        // Debug ekledim ki konsoldan hatayı anında gör
        Debug.Log("SİSTEM: Etkileşim basıldı. Şu anki oda: " + aktifOda);

        if (PuanYoneticisi.ornek != null)
        {
            PuanYoneticisi.ornek.SkorHesapla(dogruMu);
        }

        // --- ODA KONTROLLERİ ---
        // Buradaki isimler OdaDegistirici'deki isimlerle birebir aynı olmalı!

        if (aktifOda == "EksiBirinciOda")
            Object.FindFirstObjectByType<EksiBirinciOdaYoneticisi>()?.SecimYap(gameObject);

        else if (aktifOda == "EksiIkinciOda")
            Object.FindFirstObjectByType<EksiIkinciOdaYoneticisi>()?.SecimYap(gameObject);

        else if (aktifOda == "AsitOdasi")
            Object.FindFirstObjectByType<BirinciOdaYoneticisi>()?.SecimYap(gameObject);

        else if (aktifOda == "BazOdasi")
            Object.FindFirstObjectByType<IkinciOdaYoneticisi>()?.SecimYap(gameObject);

        else if (aktifOda == "UcuncuOda")
            Object.FindFirstObjectByType<UcuncuOdaYoneticisi>()?.SecimYap(gameObject);

        else if (aktifOda == "DorduncuOda")
            Object.FindFirstObjectByType<DorduncuOdaYoneticisi>()?.SecimYap(gameObject);

        else if (aktifOda == "BesinciOda")
            Object.FindFirstObjectByType<BesinciOdaYoneticisi>()?.SecimYap(gameObject);

        else if (aktifOda == "AltıncıOda")
            Object.FindFirstObjectByType<AltinciOdaYoneticisi>()?.SecimYap(gameObject);

        else if (aktifOda == "YedinciOda")
            Object.FindFirstObjectByType<YedinciOdaYoneticisi>()?.SecimYap(gameObject);

        else if (aktifOda == "SekizinciOda")
            Object.FindFirstObjectByType<SekizinciOdaYoneticisi>()?.SecimYap(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            oyuncuYakinda = true;
            YaziYoneticisi.ornek?.BilgiGoster(ipucu + "\n(E İle Al)", true);
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