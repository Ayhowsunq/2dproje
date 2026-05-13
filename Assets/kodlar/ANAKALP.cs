using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; // Karıştırma (Shuffle) için şart

public class ANAKALP : MonoBehaviour
{
    public static ANAKALP ornek;

    [Header("Görsel & Ses Ayarları")]
    public float kapiYukselmeMiktari = 1.0f;
    public AudioSource sesKaynagi;
    public AudioClip dogruSes, yanlisSes;

    [Header("Oyun Durumu")]
    public int suAnkiOdaNo;
    private List<KimyaSisesi> odadakiSiseler;
    private GameObject mevcutKapi;
    private int toplananDogru = 0;
    private bool kapiAciliyor = false;

    // --- SORU HAFIZA SİSTEMİ (Aynı soru bir daha gelmez) ---
    private List<int> kullanilanBasitIndices = new List<int>();
    private List<int> kullanilanOrtaIndices = new List<int>();
    private List<int> kullanilanZorIndices = new List<int>();

    void Awake() { ornek = this; }

    public void OdaKurulumu(int odaNo, List<KimyaSisesi> siseler, GameObject kapi)
    {
        suAnkiOdaNo = odaNo;
        odadakiSiseler = siseler;
        mevcutKapi = kapi;
        toplananDogru = 0;
        kapiAciliyor = false;
        SoruUretici();
    }

    // Benzersiz İndeks Seçici
    int GetUniqueIndex(List<int> kullanilanlar, int maxRange)
    {
        if (kullanilanlar.Count >= maxRange) kullanilanlar.Clear();
        int r;
        do { r = Random.Range(0, maxRange); } while (kullanilanlar.Contains(r));
        kullanilanlar.Add(r);
        return r;
    }

    void SoruUretici()
    {
        string soru = "";
        List<string> dogrular = new List<string>();
        List<string> yanlislar = new List<string>();

        // ==========================================
        // ⚪ TUTORIAL (-1, -2)
        // ==========================================
        if (suAnkiOdaNo == -1)
        {
            soru = "HAREKET ETMEK İÇİN 'W,A,S,D' TUŞLARINI KULLAN ODALARDA DOĞRU OLDUĞUNU DÜŞÜNDÜĞÜN ŞİŞELERİN ÜSTÜNDE E BAS!";
            dogrular.Add("Hareketi anladım");
            yanlislar.AddRange(new string[] { "BEKLE", "DUR", "GERİ GİT", "ZIPLA", "YAT", "DÖN" });
        }
        else if (suAnkiOdaNo == -2)
        {
            soru = "DAR ALANLARDAN GEÇMEK İÇİN 'SHIFT' TUŞUNA BASILI TUTARAK FORM DEĞİŞTİR!";
            dogrular.Add("Bunuda anladım");
            yanlislar.AddRange(new string[] { "BASAMADIM", "HATA", "OLMADI", "YAVAŞLA", "KİTLENDİ", "HIZLI" });
        }

        // ==========================================
        // 🔴 BASİT SEVİYE (1, 2) - 81 DOĞRU / 162 YANLIŞ
        // ==========================================
        else if (suAnkiOdaNo == 1 || suAnkiOdaNo == 2)
        {
            int r = GetUniqueIndex(kullanilanBasitIndices, 9);
            switch (r)
            {
                case 0:
                    soru = "Hangileri METALDİR? (Elektriği iletir, parlaktır)";
                    dogrular.AddRange(new string[] { "Demir", "Altın", "Gümüş", "Bakır", "Alüminyum", "Magnezyum", "Çinko", "Kurşun", "Platin" });
                    yanlislar.AddRange(new string[] { "Oksijen", "Helyum", "Klor", "Kükürt", "Neon", "Kömür", "Tahta", "Su", "Alkol", "Azot", "Argon", "Plastik", "Cam", "Şeker", "Hava", "İyot", "Flor", "Fosfor" }); break;
                case 1:
                    soru = "Hangileri BAZİK (Acımtırak, ele kayganlık veren) maddelerdir?";
                    dogrular.AddRange(new string[] { "Sabun", "Deterjan", "Diş Macunu", "Çamaşır Suyu", "Mide İlacı", "Amonyak", "Karbonat", "Kireç", "Kostik" });
                    yanlislar.AddRange(new string[] { "Limon", "Sirke", "Elma", "Portakal", "Tuz Ruhu", "Kezzap", "Domates", "Yoğurt", "Süt", "Kola", "Gazoz", "Kahve", "Üzüm", "Turşu", "Akü Asidi", "Vişne", "Mide Asidi", "Şarap" }); break;
                case 2:
                    soru = "Maddenin KİMYASAL (İç yapısı değişen,kimyasal değişim olan) değişimlerini seç:";
                    dogrular.AddRange(new string[] { "Yanma", "Paslanma", "Çürüme", "Mayalanma", "Pişme", "Ekşime", "Küflenme", "Fotosentez", "Sindirim" });
                    yanlislar.AddRange(new string[] { "Erime", "Donma", "Kırılma", "Yırtılma", "Buharlaşma", "Yoğuşma", "Süblimleşme", "Ufalanma", "Kesilme", "Rendelenme", "Bükülme", "Dövülme", "Isınma", "Soğuma", "Karışma", "Çözünme", "Ezilme", "Buharlaşma" }); break;
                case 3:
                    soru = "Hangileri ASİDİK (Ekşi, aşındırıcı) maddelerdir?";
                    dogrular.AddRange(new string[] { "Limon", "Sirke", "Kezzap", "Tuz Ruhu", "Elma", "Yoğurt", "Gazoz", "Mide Asidi", "Akü Sıvısı" });
                    yanlislar.AddRange(new string[] { "Sabun", "Amonyak", "Kireç", "Kostik", "Diş Macunu", "Şampuan", "Çamaşır Suyu", "Deterjan", "Karbonat", "Sönmüş Kireç", "Lavabo Açıcı", "Saf Su", "Tuzlu Su", "Toprak", "Demir", "Bakır", "Plastik", "Cam" }); break;
                case 4:
                    soru = "Oda koşullarında GAZ halinde bulunan maddeleri seç:";
                    dogrular.AddRange(new string[] { "Oksijen", "Azot", "Hidrojen", "Helyum", "Neon", "Argon", "Karbondioksit", "Metan", "Su Buharı" });
                    yanlislar.AddRange(new string[] { "Demir", "Bakır", "Altın", "Cıva", "Tuz", "Şeker", "Alkol", "Yağ", "Taş", "Toprak", "Sülfür", "Fosfor", "Karbon", "Gümüş", "Plastik", "Cam", "Kağıt", "Tahta" }); break;
                case 5:
                    soru = "Hangileri birer BİLEŞİK formülüdür?";
                    dogrular.AddRange(new string[] { "H2O", "NaCl", "HCl", "NaOH", "CO2", "NH3", "CH4", "CaO", "HNO3" });
                    yanlislar.AddRange(new string[] { "Fe", "Au", "Ag", "Cu", "H", "O", "N", "C", "Na", "O2", "Cl2", "He", "Ne", "Ar", "S8", "P4", "F2", "Br2" }); break;
                case 6:
                    soru = "Hangileri birer ELEMENT sembolüdür?";
                    dogrular.AddRange(new string[] { "Fe", "Au", "Ag", "Cu", "H", "O", "N", "C", "Na" });
                    yanlislar.AddRange(new string[] { "H2O", "NaCl", "CO2", "HCl", "NH3", "CH4", "CaO", "NaOH", "KOH", "H2SO4", "HNO3", "CaCO3", "C6H12O6", "SO2", "NO2", "MgO", "KCl", "HF" }); break;
                case 7:
                    soru = "Simyacıların (Kimya öncesi) keşfettiği maddeleri seç:";
                    dogrular.AddRange(new string[] { "Mürekkep", "Barut", "Cam", "Seramik", "Sabun", "Esans", "Zaç Yağı", "Kezzap", "Tuz Ruhu" });
                    yanlislar.AddRange(new string[] { "Plastik", "Teflon", "Naylon", "PVC", "Aspirin", "Antibiyotik", "Deterjan", "Silikon", "Kevlar", "Polyester", "Uranyum", "Lityum Pil", "Nanotüp", "PET", "Rayon", "Bakalit", "Pleksi", "Kauçuk" }); break;
                case 8:
                    soru = "Suda çözündüğünde OH- iyonu veren BAZLARI seç:";
                    dogrular.AddRange(new string[] { "NaOH", "KOH", "Ca(OH)2", "Mg(OH)2", "Ba(OH)2", "LiOH", "NH3", "Al(OH)3", "Fe(OH)3" });
                    yanlislar.AddRange(new string[] { "HCl", "HNO3", "H2SO4", "HF", "HBr", "HI", "CH3COOH", "H3PO4", "H2CO3", "NaCl", "KCl", "MgCl2", "Na2SO4", "KNO3", "CaCO3", "H2O", "C2H5OH" }); break;
            }
        }
        // ==========================================
        // 🟡 ORTA SEVİYE (3, 4, 5)
        // ==========================================
        else if (suAnkiOdaNo >= 3 && suAnkiOdaNo <= 5)
        {
            int r = GetUniqueIndex(kullanilanOrtaIndices, 9);
            switch (r)
            {
                case 0:
                    soru = "Normal Koşullarda (NK) 1 mol ideal gaz kaç litre hacim kaplar?"; dogrular.Add("22,4 L");
                    yanlislar.AddRange(new string[] { "11,2 L", "44,8 L", "5,6 L", "24,5 L", "67,2 L", "2,24 L" }); break;
                case 1:
                    soru = "Elektronegatifliği en yüksek olan Flor hangi gruptadır?"; dogrular.Add("7A (Halojen)");
                    yanlislar.AddRange(new string[] { "1A", "8A", "2A", "3B", "6A", "4A" }); break;
                case 2:
                    soru = "Simyacıların 'Tuz Ruhu' dediği asit hangisidir?"; dogrular.Add("HCl");
                    yanlislar.AddRange(new string[] { "HNO3", "H2SO4", "CH3COOH", "NaOH", "NH3" }); break;
                case 3:
                    soru = "NK'da 0.5 mol ideal gaz kaç litredir?"; dogrular.Add("11,2 L");
                    yanlislar.AddRange(new string[] { "22,4 L", "5,6 L", "44,8 L", "33,6 L", "15,5 L" }); break;
                case 4:
                    soru = "Limon suyu gibi pH < 7 olan maddelerin genel adı nedir?"; dogrular.Add("Asit");
                    yanlislar.AddRange(new string[] { "Baz", "Tuz", "Nötr", "Metal", "Soygaz" }); break;
                case 5:
                    soru = "Akü sıvısı olarak bilinen 'Zaç Yağı' hangisidir?"; dogrular.Add("H2SO4");
                    yanlislar.AddRange(new string[] { "HCl", "HNO3", "CH4", "NaOH", "KOH" }); break;
                case 6:
                    soru = "Modern Periyodik Sistemde toplam kaç tane grup bulunur?"; dogrular.Add("18 Grup");
                    yanlislar.AddRange(new string[] { "7 Grup", "8 Grup", "10 Grup", "32 Grup", "5 Grup" }); break;
                case 7:
                    soru = "Sulu çözeltilerde H+ iyonu veren maddeler nasıldır?"; dogrular.Add("Asidik");
                    yanlislar.AddRange(new string[] { "Bazik", "Nötr", "İnert", "Soy", "Alkali" }); break;
                default:
                    soru = "Simyacıların 'Kezzap' dediği asit hangisidir?"; dogrular.Add("HNO3");
                    yanlislar.AddRange(new string[] { "HCl", "H2SO4", "HF", "KOH", "CaO" }); break;
            }
        }
        // ==========================================
        // 🟣 ZOR SEVİYE (6, 7, 8)
        // ==========================================
        else
        {
            int r = GetUniqueIndex(kullanilanZorIndices, 9);
            switch (r)
            {
                case 0:
                    soru = "4M 400ml çözeltiye su eklenip hacmi 800ml yapılırsa yeni Molarite?"; dogrular.Add("2 M");
                    yanlislar.AddRange(new string[] { "1 M", "1.25 M", "0.75 M", "0.5 M", "1.5 M", "3 M" }); break;
                case 1:
                    soru = "2M 500ml çözeltiye su eklenip hacmi 1000ml yapılırsa yeni Molarite?"; dogrular.Add("1 M");
                    yanlislar.AddRange(new string[] { "2 M", "1.25 M", "0.75 M", "0.5 M", "1.5 M", "4 M" }); break;
                case 2:
                    soru = "Ekzotermik (Isı Veren) tepkimelerde Entalpi Değişimi (ΔH) işareti nedir?";
                    dogrular.Add("Negatif (-)");
                    yanlislar.AddRange(new string[] { "Pozitif (+)", "Belirsiz", "Nötr", "Sıfır", "Ölçülemez" }); break;
                case 3:
                    soru = "PV=nRT denklemindeki 'R' ideal gaz sabiti yaklaşık kaçtır?"; dogrular.Add("0,082");
                    yanlislar.AddRange(new string[] { "8,314", "22,4", "273", "6,02", "1,082" }); break;
                case 4:
                    soru = "1 mol Su (H2O) yaklaşık kaç gramdır? (H:1, O:16)"; dogrular.Add("18 Gram");
                    yanlislar.AddRange(new string[] { "16 Gram", "2 Gram", "20 Gram", "34 Gram", "32 Gram" }); break;
                case 5:
                    soru = "Halk arasında 'Sönmemiş Kireç' olarak bilinen bileşik?"; dogrular.Add("CaO");
                    yanlislar.AddRange(new string[] { "CaCO3", "Ca(OH)2", "MgO", "NaCl" }); break;
                case 6:
                    soru = "Mermerin ana maddesi olan 'Kireç Taşı' hangisidir?"; dogrular.Add("CaCO3");
                    yanlislar.AddRange(new string[] { "CaO", "Ca(OH)2", "HCl", "NaCl" }); break;
                case 7:
                    soru = "n+l kuralına göre 3d dolduktan sonra elektron nereye girer?"; dogrular.Add("4p");
                    yanlislar.AddRange(new string[] { "4s", "5s", "4d", "3p", "5p" }); break;
                default:
                    soru = "Sabit hacimde sıcaklık artarsa gaz basıncı nasıl değişir?"; dogrular.Add("Artar");
                    yanlislar.AddRange(new string[] { "Azalır", "Değişmez", "Sıfırlanır", "Önce azalır" }); break;
            }
        }

        YaziYoneticisi.ornek?.GorevGuncelle(soru);
        SiseDagitim(dogrular, yanlislar);
    }

    void SiseDagitim(List<string> dogrular, List<string> yanlislar)
    {
        // Şık Havuzlarını Karıştır
        yanlislar = yanlislar.OrderBy(x => Random.value).ToList();
        dogrular = dogrular.OrderBy(x => Random.value).ToList();

        // Şişe Dizilimini Karıştır (Doğru şişe her seferinde farklı yerde)
        odadakiSiseler = odadakiSiseler.OrderBy(x => Random.value).ToList();

        int gereken = (suAnkiOdaNo == 1 || suAnkiOdaNo == 2) ? 2 : 1;

        for (int i = 0; i < odadakiSiseler.Count; i++)
        {
            if (i < gereken)
            {
                odadakiSiseler[i].dogruMu = true;
                odadakiSiseler[i].ipucu = dogrular[i % dogrular.Count];
            }
            else
            {
                odadakiSiseler[i].dogruMu = false;
                odadakiSiseler[i].ipucu = yanlislar[i % yanlislar.Count];
            }
        }
    }

    public void SecimYap(KimyaSisesi sise)
    {
        if (sise.dogruMu)
        {
            sesKaynagi.PlayOneShot(dogruSes);
            sise.gameObject.SetActive(false);
            toplananDogru++;
            int gereken = (suAnkiOdaNo == 1 || suAnkiOdaNo == 2) ? 2 : 1;
            if (toplananDogru >= gereken && !kapiAciliyor) StartCoroutine(KapiAc(mevcutKapi));
        }
        else
        {
            sesKaynagi.PlayOneShot(yanlisSes);
        }
    }

    IEnumerator KapiAc(GameObject acilacakKapi)
    {
        kapiAciliyor = true;
        Vector3 hedef = acilacakKapi.transform.position + new Vector3(0, kapiYukselmeMiktari, 0);
        while (Vector3.Distance(acilacakKapi.transform.position, hedef) > 0.01f)
        {
            acilacakKapi.transform.position = Vector3.MoveTowards(acilacakKapi.transform.position, hedef, 2f * Time.deltaTime);
            yield return null;
        }
    }
}