using UnityEngine;
using TMPro;

public class HeykelKontrol : MonoBehaviour
{
    [Header("Heykel Bileşenleri")]
    public GameObject yaziObjesi; // Yazının içinde olduğu Canvas/Panel
    public TextMeshProUGUI isimMetni;

    private void Start()
    {
        if (yaziObjesi != null) yaziObjesi.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PuanYoneticisi.ornek != null)
            {
                string enSanli = PuanYoneticisi.ornek.EnYuksekIsmiGetir();
                if (isimMetni != null)
                {
                    // İsmi yazdırıyoruz, AltinEfekt scripti rengi halledecek!
                    isimMetni.text = string.IsNullOrEmpty(enSanli) ? "" : "EN ŞANLI:\n" + enSanli;
                }
            }
            if (yaziObjesi != null) yaziObjesi.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (yaziObjesi != null) yaziObjesi.SetActive(false);
        }
    }
}