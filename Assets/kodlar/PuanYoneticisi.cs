using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PuanYoneticisi : MonoBehaviour
{
    public static PuanYoneticisi ornek;

    [Header("Ayarlar")]
    public bool tumSkorlariSil = false;
    public string silmeSifresi = "RESET123";
    public bool puanlamaBasladi = false;
    public bool kucukModAktif = false; // KarakterForm hatası için

    [Header("UI Elemanları")]
    public CanvasGroup finalUIGrubu;
    public TMP_InputField isimGirisAlani;
    public GameObject kaydetButonu;
    public TextMeshProUGUI liderlikYazisi;
    public TextMeshProUGUI anlikPuanYazisi;

    [Header("Skor Verileri")]
    public bool oyunBitti = false;
    public float mevcutPuan = 0f;
    public int kombo = 0;
    private float zorlukKatlayani = 1f;

    void Awake() { ornek = this; if (tumSkorlariSil) PlayerPrefs.DeleteKey("Skorlar"); }

    void Start()
    {
        if (finalUIGrubu != null) { finalUIGrubu.alpha = 0; finalUIGrubu.interactable = false; finalUIGrubu.blocksRaycasts = false; }
        string veri = PlayerPrefs.GetString("Skorlar", "");
        TabloyuDoldur(veri);
        if (liderlikYazisi != null) liderlikYazisi.gameObject.SetActive(false);
    }

    void Update()
    {
        if (puanlamaBasladi && !oyunBitti && mevcutPuan > 0)
        {
            mevcutPuan -= Time.deltaTime;
            if (anlikPuanYazisi != null) anlikPuanYazisi.text = "PUAN: " + Mathf.RoundToInt(mevcutPuan);
        }

        if (oyunBitti && Input.GetKeyDown(KeyCode.E))
        {
            if (isimGirisAlani == null || !isimGirisAlani.isFocused)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // ODA DEGISTIRICI ICIN GEREKLI OLAN METOT (Hata Buradaydı)
    public void ZorlukKatlayaniAyarla(int odaNo)
    {
        // Oda numarasına göre puan çarpanını ayarlar
        if (odaNo <= 3) zorlukKatlayani = 1f;
        else if (odaNo <= 6) zorlukKatlayani = 1.5f;
        else zorlukKatlayani = 2f;
    }

    public string EnYuksekIsmiGetir()
    {
        string veri = PlayerPrefs.GetString("Skorlar", "");
        List<SkorVerisi> skorlar = VeriyiParcala(veri);
        return skorlar.Count > 0 ? skorlar[0].Isim : "";
    }

    public void PuanlamayiBaslat() { StartCoroutine(GecikmeliBaslat()); }
    IEnumerator GecikmeliBaslat() { yield return new WaitForSeconds(3.5f); puanlamaBasladi = true; }

    public void SkorHesapla(bool d)
    {
        if (!puanlamaBasladi || oyunBitti) return;
        if (d) { mevcutPuan += (20f * zorlukKatlayani) + (kombo * 5f); kombo++; }
        else { mevcutPuan -= 10f; if (mevcutPuan < 0) mevcutPuan = 0; kombo = 0; }
    }

    public void OyunuBitir() { if (!oyunBitti) { oyunBitti = true; StartCoroutine(FinalEkraniniGetir()); } }

    IEnumerator FinalEkraniniGetir()
    {
        if (finalUIGrubu != null)
        {
            finalUIGrubu.interactable = true; finalUIGrubu.blocksRaycasts = true;
            while (finalUIGrubu.alpha < 1) { finalUIGrubu.alpha += Time.deltaTime * 1.5f; yield return null; }
        }
        if (isimGirisAlani != null) isimGirisAlani.ActivateInputField();
    }

    public void SkoruKaydetVeGoster()
    {
        string isim = string.IsNullOrEmpty(isimGirisAlani.text) ? "Adsız" : isimGirisAlani.text;
        if (isim == silmeSifresi) { PlayerPrefs.DeleteKey("Skorlar"); PlayerPrefs.Save(); TabloyuDoldur(""); return; }

        int finalSkor = Mathf.RoundToInt(mevcutPuan);
        string veri = PlayerPrefs.GetString("Skorlar", "");
        veri += isim + ":" + finalSkor + "|";
        PlayerPrefs.SetString("Skorlar", veri);
        PlayerPrefs.Save();

        if (isimGirisAlani != null) isimGirisAlani.gameObject.SetActive(false);
        if (kaydetButonu != null) kaydetButonu.SetActive(false);
        if (liderlikYazisi != null) { liderlikYazisi.gameObject.SetActive(true); TabloyuDoldur(veri); }
    }

    void TabloyuDoldur(string veri)
    {
        List<SkorVerisi> skorlar = VeriyiParcala(veri);
        string txt = "<color=#FFD700>EN ŞANLI KİMYAGERLER</color>\n\n";
        if (skorlar.Count == 0) txt += "HENÜZ KAYIT YOK...";
        else { for (int i = 0; i < Mathf.Min(5, skorlar.Count); i++) txt += (i + 1) + ". " + skorlar[i].Isim + " - " + skorlar[i].Puan + "\n"; }
        if (liderlikYazisi != null) liderlikYazisi.text = txt + "\n\n<size=20>(E) YENİDEN BAŞLAT</size>";
    }

    private List<SkorVerisi> VeriyiParcala(string v)
    {
        List<SkorVerisi> liste = new List<SkorVerisi>();
        if (string.IsNullOrEmpty(v)) return liste;
        string[] parcalar = v.Split('|');
        foreach (string s in parcalar)
        {
            if (!string.IsNullOrEmpty(s))
            {
                string[] p = s.Split(':');
                if (p.Length == 2) liste.Add(new SkorVerisi { Isim = p[0], Puan = int.Parse(p[1]) });
            }
        }
        return liste.OrderByDescending(x => x.Puan).ToList();
    }
}

[System.Serializable]
public class SkorVerisi { public string Isim; public int Puan; }