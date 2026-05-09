using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PuanYoneticisi : MonoBehaviour
{
    public static PuanYoneticisi ornek;

    [Header("Geliştirici Ayarları")]
    public bool tumSkorlariSil = false;
    public bool kucukModAktif = false;
    public bool puanlamaBasladi = false;
    // KANKA: Silme şifresini buradan belirleyebilirsin
    public string silmeSifresi = "RESET123"; 

    [Header("UI Ayarları")]
    public CanvasGroup finalUIGrubu;
    public float fadeHizi = 1f;
    public TMP_InputField isimGirisAlani;
    public GameObject kaydetButonu;
    public TextMeshProUGUI liderlikYazisi;
    public TextMeshProUGUI anlikPuanYazisi;
    [SerializeField] private TextMeshProUGUI geriSayimYazisi;

    [Header("Heykel Ayarları")]
    public GameObject heykelYazisiObjesi;
    public TextMeshProUGUI heykelIsimMetni;

    [Header("Skor Verileri")]
    public bool oyunBitti = false;
    public float mevcutPuan = 500f;
    public int kombo = 0;
    public int hataSayisi = 0;

    void Awake()
    {
        ornek = this;
        if (tumSkorlariSil) PlayerPrefs.DeleteKey("Skorlar");
    }

    void Start()
    {
        if (finalUIGrubu != null) { finalUIGrubu.alpha = 0; finalUIGrubu.interactable = false; finalUIGrubu.blocksRaycasts = false; }
        if (liderlikYazisi != null) liderlikYazisi.gameObject.SetActive(false);
        if (heykelYazisiObjesi != null) heykelYazisiObjesi.SetActive(false);
        if (geriSayimYazisi != null) geriSayimYazisi.gameObject.SetActive(false);

        HeykelIsminiGuncelle();
    }

    void Update()
    {
        if (puanlamaBasladi && !oyunBitti)
        {
            float dususHizi = kucukModAktif ? 7f : 2.5f;
            mevcutPuan -= dususHizi * Time.deltaTime;

            if (anlikPuanYazisi != null)
                anlikPuanYazisi.text = "PUAN: " + Mathf.RoundToInt(mevcutPuan);
        }

        if (oyunBitti && Input.GetKeyDown(KeyCode.E))
        {
            if (isimGirisAlani == null || !isimGirisAlani.isFocused)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void PuanlamayiBaslat()
    {
        if (puanlamaBasladi) return;
        StartCoroutine(GeriSayimSistemi());
    }

    IEnumerator GeriSayimSistemi()
    {
        GameObject oyuncu = GameObject.FindGameObjectWithTag("Player");
        if (oyuncu != null)
        {
            var hareketScripti = oyuncu.GetComponent<MonoBehaviour>();
            if (hareketScripti != null) hareketScripti.enabled = false;
        }

        if (geriSayimYazisi != null)
        {
            geriSayimYazisi.gameObject.SetActive(true);
            geriSayimYazisi.text = "3";
            yield return new WaitForSeconds(1f);
            geriSayimYazisi.text = "2";
            yield return new WaitForSeconds(1f);
            geriSayimYazisi.text = "1";
            yield return new WaitForSeconds(1f);
            geriSayimYazisi.text = "BAŞLA!";
            yield return new WaitForSeconds(0.5f);
            geriSayimYazisi.gameObject.SetActive(false);
        }

        if (oyuncu != null)
        {
            var hareketScripti = oyuncu.GetComponent<MonoBehaviour>();
            if (hareketScripti != null) hareketScripti.enabled = true;
        }
        puanlamaBasladi = true;
    }

    public void SkorHesapla(bool dogruMu)
    {
        if (!puanlamaBasladi) return;

        if (dogruMu)
        {
            float eklenecekPuan = Mathf.Pow(2f, kombo);
            mevcutPuan += eklenecekPuan;
            kombo++;
            hataSayisi = 0;
        }
        else
        {
            hataSayisi++;
            float silinecekPuan = hataSayisi * 5f;
            mevcutPuan -= silinecekPuan;
            kombo = 0;
        }

        if (anlikPuanYazisi != null)
            anlikPuanYazisi.text = "PUAN: " + Mathf.RoundToInt(mevcutPuan);
    }

    public void YakitAl(float miktar)
    {
        if (puanlamaBasladi) mevcutPuan += (miktar * 2f);
    }

    public void OyunuBitir()
    {
        if (oyunBitti) return;
        oyunBitti = true;
        StartCoroutine(FinalEkraniniGetir());
    }

    IEnumerator FinalEkraniniGetir()
    {
        if (finalUIGrubu != null)
        {
            finalUIGrubu.interactable = true;
            finalUIGrubu.blocksRaycasts = true;
            while (finalUIGrubu.alpha < 1)
            {
                finalUIGrubu.alpha += Time.deltaTime * fadeHizi;
                yield return null;
            }
        }
        if (isimGirisAlani != null) isimGirisAlani.ActivateInputField();
    }

    public void SkoruKaydetVeGoster()
    {
        // KANKA: İşte o özel isim kontrolü burada!
        if (isimGirisAlani != null && isimGirisAlani.text == silmeSifresi)
        {
            PlayerPrefs.DeleteKey("Skorlar");
            PlayerPrefs.Save();
            Debug.Log("Kanka tüm skorlar temizlendi!");
            
            // Temizlendikten sonra tabloyu boş gösterelim
            TabloyuDoldur(""); 
            HeykelIsminiGuncelle();
            
            if (isimGirisAlani != null) isimGirisAlani.text = "TEMİZLENDİ!";
            return; // Normal kayıt işlemine devam etme, burada kes.
        }

        int finalSkor = Mathf.RoundToInt(mevcutPuan);
        string isim = string.IsNullOrEmpty(isimGirisAlani.text) ? "Adsız" : isimGirisAlani.text;
        string veri = PlayerPrefs.GetString("Skorlar", "");
        veri += isim + ":" + finalSkor + "|";
        PlayerPrefs.SetString("Skorlar", veri);

        if (isimGirisAlani != null) isimGirisAlani.gameObject.SetActive(false);
        if (kaydetButonu != null) kaydetButonu.SetActive(false);
        if (liderlikYazisi != null) liderlikYazisi.gameObject.SetActive(true);

        TabloyuDoldur(veri);
        HeykelIsminiGuncelle();
    }

    void TabloyuDoldur(string veri)
    {
        var skorlar = VeriyiParcala(veri);
        string txt = "EN İYİ KİMYAGERLER\n\n";
        
        if (skorlar.Count == 0) txt += "LİSTE BOŞ...";
        else
        {
            for (int i = 0; i < Mathf.Min(5, skorlar.Count); i++)
                txt += (i + 1) + ". " + skorlar[i].Isim + " - " + skorlar[i].Puan + " PUAN\n";
        }

        liderlikYazisi.text = txt + "\n(E) BAS VE YENİDEN BAŞLA";
    }

    public void HeykelIsminiGuncelle()
    {
        string veri = PlayerPrefs.GetString("Skorlar", "");
        var skorlar = VeriyiParcala(veri);
        if (heykelIsimMetni != null)
        {
            if (skorlar.Count > 0) heykelIsimMetni.text = "EN ŞANLI:\n" + skorlar[0].Isim;
            else heykelIsimMetni.text = "EN ŞANLI:\nBEKLENİYOR...";
        }
    }

    List<SkorVerisi> VeriyiParcala(string v)
    {
        List<SkorVerisi> l = new List<SkorVerisi>();
        if (string.IsNullOrEmpty(v)) return l;

        foreach (string s in v.Split('|'))
        {
            if (!string.IsNullOrEmpty(s))
            {
                string[] p = s.Split(':');
                if (p.Length == 2) l.Add(new SkorVerisi { Isim = p[0], Puan = int.Parse(p[1]) });
            }
        }
        return l.OrderByDescending(x => x.Puan).ToList();
    }

    public class SkorVerisi { public string Isim; public int Puan; }
}