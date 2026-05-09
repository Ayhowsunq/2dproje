using UnityEngine;

public class HeykelKontrol : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Heykelin yanına gelince yazıyı aç
            if (PuanYoneticisi.ornek.heykelYazisiObjesi != null)
                PuanYoneticisi.ornek.heykelYazisiObjesi.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Uzaklaşınca kapat
            if (PuanYoneticisi.ornek.heykelYazisiObjesi != null)
                PuanYoneticisi.ornek.heykelYazisiObjesi.SetActive(false);
        }
    }
}