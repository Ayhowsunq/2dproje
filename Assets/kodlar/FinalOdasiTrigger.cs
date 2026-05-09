using UnityEngine;

public class FinalOdasiTrigger : MonoBehaviour
{
    // Oyuncu bu görünmez alana girdiğinde Unity bu fonksiyonu otomatik çalıştırır
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. KONTROL: Çarpan objenin Tag'ı (Etiketi) "Player" mı?
        if (other.CompareTag("Player"))
        {
            // 2. İŞLEM: PuanYoneticisi scriptindeki bitiş fonksiyonunu tetikle
            if (PuanYoneticisi.ornek != null)
            {
                PuanYoneticisi.ornek.OyunuBitir();
                Debug.Log("Final Odası Tetiklendi: Oyun Bitti!");
            }
            else
            {
                Debug.LogError("HATA: Sahnede PuanYoneticisi bulunamadı!");
            }

            // 3. GÜVENLİK: Bu trigger objesini yok et (İkinci kez çalışmasın diye)
            Destroy(gameObject);
        }
    }
}