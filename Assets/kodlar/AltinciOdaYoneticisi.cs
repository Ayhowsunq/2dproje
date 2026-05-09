using UnityEngine;
using System.Collections;

public class AltinciOdaYoneticisi : MonoBehaviour
{
    public GameObject kapi;
    public AudioSource sesKaynagi;
    public AudioClip dogruSes, yanlisSes;
    private bool kapiAciliyor = false;

    public void SecimYap(GameObject siseObj)
    {
        KimyaSisesi sise = siseObj.GetComponent<KimyaSisesi>();

        if (sise.dogruMu)
        {
            // --- SADECE DOĞRUYSA YAPILACAKLAR ---
            if (sesKaynagi) sesKaynagi.PlayOneShot(dogruSes);

            siseObj.SetActive(false); // Şişeyi sadece doğruysa yok et (al)

            if (!kapiAciliyor) StartCoroutine(KapiyiYukariKaydir());
        }
        else
        {
            // --- YANLIŞSA YAPILACAKLAR ---
            if (sesKaynagi) sesKaynagi.PlayOneShot(yanlisSes);

            // siseObj.SetActive(false); satırını buradan sildik! 
            // Şişe artık sahnede kalmaya devam edecek.
        }
    }

    IEnumerator KapiyiYukariKaydir()
    {
        kapiAciliyor = true;
        Vector3 hedef = kapi.transform.position + new Vector3(0, 1f, 0);
        while (Vector3.Distance(kapi.transform.position, hedef) > 0.01f)
        {
            kapi.transform.position = Vector3.MoveTowards(kapi.transform.position, hedef, 2.5f * Time.deltaTime);
            yield return null;
        }
    }
}