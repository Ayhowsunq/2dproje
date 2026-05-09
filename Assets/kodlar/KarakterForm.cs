using UnityEngine;

public class KarakterForm : MonoBehaviour
{
    private PlayerMovement hareketScripti;

    private bool formDustuMu = false;
    private float normalHiz;
    private float normalZip;

    void Start()
    {
        hareketScripti = GetComponent<PlayerMovement>();

        if (hareketScripti != null)
        {
            normalHiz = hareketScripti.moveSpeed;
            normalZip = hareketScripti.jumpForce;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            FormuDegistir();
        }

        float moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0)
        {
            float targetScale = formDustuMu ? 0.4f : 1f;
            float yon = moveInput > 0 ? targetScale : -targetScale;
            transform.localScale = new Vector3(yon, targetScale, transform.localScale.z);
        }
    }

    void FormuDegistir()
    {
        if (hareketScripti == null) return;

        formDustuMu = !formDustuMu;

        if (formDustuMu)
        {
            // --- KÜÇÜK MOD ---
            transform.localScale = new Vector3(0.4f, 0.4f, transform.localScale.z);
            hareketScripti.moveSpeed = normalHiz * 0.75f;
            hareketScripti.jumpForce = normalZip * 0.60f;

            // PUAN YÖNETİCİSİNE BAĞLAMA: Saniyede 1 puan gitmeye başlasın
            if (PuanYoneticisi.ornek != null)
                PuanYoneticisi.ornek.kucukModAktif = true;
        }
        else
        {
            // --- NORMAL MOD ---
            transform.localScale = new Vector3(1f, 1f, transform.localScale.z);
            hareketScripti.moveSpeed = normalHiz;
            hareketScripti.jumpForce = normalZip;

            // PUAN YÖNETİCİSİNE BAĞLAMA: Saniyede 0.5 puan gitmeye geri dönsün
            if (PuanYoneticisi.ornek != null)
                PuanYoneticisi.ornek.kucukModAktif = false;
        }
    }
}