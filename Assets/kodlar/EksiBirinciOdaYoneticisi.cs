using UnityEngine;
using System.Collections;

public class EksiIkinciOdaYoneticisi : MonoBehaviour
{
    public GameObject kapi;
    public AudioSource sesKaynagi;
    public AudioClip dogruSes, yanlisSes;

    private bool kapiAciliyor = false;

    public void SecimYap(GameObject siseObj)
    {
        KimyaSisesi sise = siseObj.GetComponent<KimyaSisesi>();

        if (sise != null && sise.dogruMu)
        {
            sesKaynagi.PlayOneShot(dogruSes);
            siseObj.SetActive(false); // Doğru şişe kaybolur

            if (!kapiAciliyor)
            {
                StartCoroutine(KapiyiYukariKaydir());
            }
        }
        else
        {
            sesKaynagi.PlayOneShot(yanlisSes);
        }
    }

    IEnumerator KapiyiYukariKaydir()
    {
        kapiAciliyor = true;
        // Kapıyı 3.5 birim yukarı süzerek taşır
        Vector3 hedefPozisyon = kapi.transform.position + new Vector3(0, 1f, 0);
        float hiz = 2.5f;

        while (Vector3.Distance(kapi.transform.position, hedefPozisyon) > 0.01f)
        {
            kapi.transform.position = Vector3.MoveTowards(kapi.transform.position, hedefPozisyon, hiz * Time.deltaTime);
            yield return null;
        }
    }
}