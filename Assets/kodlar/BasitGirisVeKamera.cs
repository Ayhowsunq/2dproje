using UnityEngine;
using UnityEngine.UI;

public class BasitGirisVeKamera : MonoBehaviour
{
    public static BasitGirisVeKamera ornek;

    public Transform oyuncu;
    public Button oynaButonu;
    public float gecisSuresi = 1.5f;
    public float sabitY = 0f;
    public Vector3 ofset = new Vector3(0, 0, -10);

    [Header("Sürekli Sarsıntı Ayarları")]
    public float oda3SarsintiGucu = 0.05f; // Orta (Hafif titreme)
    public float oda6SarsintiGucu = 0.15f; // Zor (Daha fazla titreme)

    private Vector3 mevcutHiz = Vector3.zero;
    private bool oynaBasildi = false;
    private float aktifSarsintiGucu = 0f;

    void Awake() => ornek = this;

    void Start()
    {
        Time.timeScale = 0f;
        if (oynaButonu != null)
            oynaButonu.onClick.AddListener(OyunuBaslat);
    }

    void OyunuBaslat()
    {
        Time.timeScale = 1f;
        oynaBasildi = true;
        oynaButonu.gameObject.SetActive(false);
    }

    // GeriSayimYoneticisi veya PuanYoneticisi tarafından çağrılır
    public void SarsintiyiAyarla(int odaNo)
    {
        if (odaNo == 1)
            aktifSarsintiGucu = 0f;
        else if (odaNo == 3)
            aktifSarsintiGucu = oda3SarsintiGucu;
        else if (odaNo == 6)
            aktifSarsintiGucu = oda6SarsintiGucu;
        // KANKA: Oda 9 ise veya 9'dan büyükse sarsıntıyı kökten kesiyoruz
        else if (odaNo >= 9)
            aktifSarsintiGucu = 0f;
    }

    // Ekstra Garanti: PuanYoneticisi OyunuBitir() içinden bunu direkt çağırabilirsin
    public void SarsintiyiKapat()
    {
        aktifSarsintiGucu = 0f;
    }

    void LateUpdate()
    {
        if (oynaBasildi && oyuncu != null)
        {
            Vector3 hedefPozisyon = new Vector3(oyuncu.position.x + ofset.x, sabitY, ofset.z);
            Vector3 yeniPozisyon = Vector3.SmoothDamp(transform.position, hedefPozisyon, ref mevcutHiz, gecisSuresi);

            if (aktifSarsintiGucu > 0)
            {
                yeniPozisyon += (Vector3)Random.insideUnitCircle * aktifSarsintiGucu;
            }

            transform.position = yeniPozisyon;
        }
    }
}