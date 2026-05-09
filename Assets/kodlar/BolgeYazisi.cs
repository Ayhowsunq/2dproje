using UnityEngine;

public class BolgeYazisi : MonoBehaviour
{
    [Header("Ekranda Açılacak Yazı")]
    // Ekranda görünmesini istediğin o "Asitleri bul" yazısını buraya sürükleyeceksin
    public GameObject acilacakYaziObjesi;

    void Start()
    {
        // Oyun başladığında o yazının kesinlikle kapalı olduğundan emin oluyoruz
        if (acilacakYaziObjesi != null)
        {
            acilacakYaziObjesi.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Karakter (Player) tabelanın yanındaki yeşil alana GİRİNCE
        if (other.CompareTag("Player") && acilacakYaziObjesi != null)
        {
            acilacakYaziObjesi.SetActive(true); // Yazıyı AÇ
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Karakter alandan ÇIKINCA
        if (other.CompareTag("Player") && acilacakYaziObjesi != null)
        {
            acilacakYaziObjesi.SetActive(false); // Yazıyı KAPAT
        }
    }
}