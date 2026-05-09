using UnityEngine;
using TMPro;

public class YaziYoneticisi : MonoBehaviour
{
    public static YaziYoneticisi ornek;
    public TextMeshProUGUI gorevYazisi;
    public TextMeshProUGUI bilgiYazisi;

    // Panel yoksa burayı boş bıraksan da hata vermeyecek
    public GameObject bilgiPaneli;

    void Awake()
    {
        ornek = this;
    }

    public void GorevGuncelle(string mesaj)
    {
        if (gorevYazisi != null) gorevYazisi.text = mesaj;
    }

    public void BilgiGoster(string mesaj, bool durum)
    {
        if (bilgiYazisi != null)
        {
            bilgiYazisi.text = mesaj;
            bilgiYazisi.gameObject.SetActive(durum); // Paneli değil, direkt yazıyı açıp kapatır
        }

        // Eğer bir gün panel eklersen diye bu kontrolü de ekledim, hata vermez
        if (bilgiPaneli != null)
        {
            bilgiPaneli.SetActive(durum);
        }
    }
}