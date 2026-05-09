using UnityEngine;
using UnityEngine.UI;

public class BasitGirisVeKamera : MonoBehaviour
{
    public Transform oyuncu;        // Takip edilecek karakter
    public Button oynaButonu;      // Sahnedeki OYNA butonu
    public float gecisSuresi = 1.5f; // Yavaş-hızlı-yavaş hareket süresi (Saniye)
    public float sabitY = 0f;       // Kameranın sabit duracağı yükseklik
    public Vector3 ofset = new Vector3(0, 0, -10);

    private Vector3 mevcutHiz = Vector3.zero;
    private bool oynaBasildi = false;

    void Start()
    {
        // Oyun başında her şeyi donduruyoruz
        Time.timeScale = 0f;

        if (oynaButonu != null)
            oynaButonu.onClick.AddListener(OyunuBaslat);
    }

    void OyunuBaslat()
    {
        // Butona basınca zamanı akıt ve butonu gizle
        Time.timeScale = 1f;
        oynaBasildi = true;
        oynaButonu.gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        // Butona basıldıysa oyuncuya doğru süzül
        if (oynaBasildi && oyuncu != null)
        {
            Vector3 hedefPozisyon = new Vector3(oyuncu.position.x + ofset.x, sabitY, ofset.z);

            // SmoothDamp: Yavaş başlar, ortada hızlanır, sonda süzülerek durur
            transform.position = Vector3.SmoothDamp(
                transform.position,
                hedefPozisyon,
                ref mevcutHiz,
                gecisSuresi
            );
        }
    }
}