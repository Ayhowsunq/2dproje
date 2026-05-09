using UnityEngine;
using System.Collections;

public class SekizinciOdaYoneticisi : MonoBehaviour
{
    public GameObject kapi;
    public AudioSource sesKaynagi;
    public AudioClip dogruSes, yanlisSes;
    private bool kapiAciliyor = false;

    public void SecimYap(GameObject siseObj)
    {
        if (siseObj.GetComponent<KimyaSisesi>().dogruMu)
        {
            sesKaynagi.PlayOneShot(dogruSes);
            siseObj.SetActive(false);
            if (!kapiAciliyor) StartCoroutine(KapiyiKaydir());
        }
        else
        {
            sesKaynagi.PlayOneShot(yanlisSes);
        }
    }

    IEnumerator KapiyiKaydir()
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
// NOT: 8, 9 ve 10 için yukarıdaki kodun aynısını kullan, sadece "public class Yedinci..." kısmını değiştir.