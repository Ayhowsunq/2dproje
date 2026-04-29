using UnityEngine;
using TMPro;

public class KimyaSisesi : MonoBehaviour
{
    [Header("Şişe Ayarları")]
    public string siseIsmi;
    public bool asitMi;
    public float etkilesimMesafesi = 2.5f; // Dokunma için maksimum uzaklık

    private bool oyuncuYakinda = false;
    private Transform oyuncuTransform;
    private TextMeshProUGUI ekranYazisi;
    private BirinciOdaYoneticisi odaYoneticisi;

    void Start()
    {
        ekranYazisi = GameObject.Find("BilgiYazisi")?.GetComponent<TextMeshProUGUI>();
        odaYoneticisi = Object.FindFirstObjectByType<BirinciOdaYoneticisi>();

        // Oyuncuyu sahne içinde otomatik bulalım
        GameObject oyuncuObjesi = GameObject.FindGameObjectWithTag("Player");
        if (oyuncuObjesi) oyuncuTransform = oyuncuObjesi.transform;
    }

    void Update()
    {
        // Klavyeden oynayanlar için hala E tuşu aktif
        if (oyuncuYakinda && Input.GetKeyDown(KeyCode.E))
        {
            SecimiOnayla();
        }
    }

    // TELEFON İÇİN: Şişeye tıklandığında/dokunulduğunda çalışır
    private void OnMouseDown()
    {
        if (oyuncuTransform == null) return;

        // Oyuncu ile şişe arasındaki mesafeyi ölçüyoruz
        float mesafe = Vector2.Distance(transform.position, oyuncuTransform.position);

        if (mesafe <= etkilesimMesafesi)
        {
            SecimiOnayla();
        }
        else
        {
            if (ekranYazisi)
            {
                ekranYazisi.gameObject.SetActive(true);
                ekranYazisi.text = "Çok uzaktasın, biraz yaklaş!";
            }
        }
    }

    void SecimiOnayla()
    {
        if (asitMi) odaYoneticisi.DogruSecim(this.gameObject);
        else odaYoneticisi.YanlisSecim();
    }

    // Yaklaşınca yazı çıkması (Trigger hala lazım)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            oyuncuYakinda = true;
            if (ekranYazisi)
            {
                ekranYazisi.gameObject.SetActive(true);
                ekranYazisi.text = siseIsmi + "\n(Dokun veya E'ye bas)";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            oyuncuYakinda = false;
            if (ekranYazisi) ekranYazisi.gameObject.SetActive(false);
        }
    }
}