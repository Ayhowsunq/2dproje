using UnityEngine;
using System.Collections;
using TMPro;

public class BirinciOdaYoneticisi : MonoBehaviour
{
    [Header("Oda Hedefleri")]
    public int gerekenAsitSayisi = 3; // 6 şişeden kaçı asitse buraya o sayıyı yaz (Örn: 3)
    private int toplananAsit = 0;

    [Header("Objeler")]
    public GameObject kapi;
    public TextMeshProUGUI ekranYazisi;

    [Header("Sesler (Opsiyonel)")]
    public AudioSource sesKaynagi;
    public AudioClip dogruSes, yanlisSes, kapiSesi;

    public void DogruSecim(GameObject sise)
    {
        toplananAsit++;
        sise.SetActive(false); // Doğru şişeyi sahneden gizle

        if (sesKaynagi && dogruSes) sesKaynagi.PlayOneShot(dogruSes);
        if (ekranYazisi) ekranYazisi.text = "Harika! Doğru asidi buldun.";

        // Eğer hedef sayıya ulaşıldıysa kapıyı aç
        if (toplananAsit >= gerekenAsitSayisi)
        {
            StartCoroutine(KapiyiAc());
        }
    }

    public void YanlisSecim()
    {
        if (sesKaynagi && yanlisSes) sesKaynagi.PlayOneShot(yanlisSes);
        if (ekranYazisi) ekranYazisi.text = "Dikkat et! Bu bir asit değil.";
    }

    IEnumerator KapiyiAc()
    {
        if (ekranYazisi) ekranYazisi.text = "Tüm asitler bulundu! Kapı açılıyor...";
        if (sesKaynagi && kapiSesi) sesKaynagi.PlayOneShot(kapiSesi);

        Vector3 hedef = kapi.transform.position + new Vector3(0, 4.5f, 0); // Kapı 4.5 birim yukarı kalkar
        while (Vector3.Distance(kapi.transform.position, hedef) > 0.1f)
        {
            kapi.transform.position = Vector3.MoveTowards(kapi.transform.position, hedef, Time.deltaTime * 2f);
            yield return null;
        }
    }
}