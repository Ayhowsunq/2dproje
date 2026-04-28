using UnityEngine;
using UnityEngine.UI; // UI kütüphanesi - Image objesini tanıması için şart!

public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 2f; // Hızı 2 yaptım
    public float jumpForce = 12f;

    [Header("Zıplama Hissiyatı (Basılı Tutma)")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Fırlatma Ayarları")]
    public float firlatmaGucuX = 10f;
    public float firlatmaGucuY = 5f;

    [Header("UI Bağlantısı")]
    public Image envanterResmi; // Siyah üçgen UI objesini buraya sürükle

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private Vector3 originalScale;

    private GameObject elimdekiEsya;
    private bool elDoluMu = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalScale = transform.localScale;

        // Oyun başlarken envanter resmi kapalı olsun
        if (envanterResmi != null) envanterResmi.enabled = false;
    }

    void Update()
    {
        // 1. Yatay Hareket
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Yön Çevirme
        if (moveInput > 0) transform.localScale = originalScale;
        else if (moveInput < 0) transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

        // 2. Zıplama ve Basılı Tutma Mantığı
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Mario tarzı: W'ya basılı tutarsan yüksek, basıp çekersen alçak zıplar
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.W))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // 3. Eşya İşlemleri (E ve X)
        if (Input.GetKeyDown(KeyCode.E) && !elDoluMu) EsyaAl();
        if (Input.GetKeyDown(KeyCode.X) && elDoluMu) EsyaFirlat();

        // 4. Animasyonlar
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(moveInput));
            anim.SetBool("isGrounded", isGrounded);
        }
    }

    void EsyaAl()
    {
        // Karakterin etrafındaki 1.2 metrelik alanda eşyaları tara
        Collider2D[] yakinlar = Physics2D.OverlapCircleAll(transform.position, 1.2f);
        foreach (var nesne in yakinlar)
        {
            if (nesne.CompareTag("Esya"))
            {
                elimdekiEsya = nesne.gameObject;
                elDoluMu = true;

                // UI Resmini Güncelle
                if (envanterResmi != null)
                {
                    // Yerden aldığın eşyanın Sprite Renderer'ındaki resmi UI'ya aktar
                    envanterResmi.sprite = elimdekiEsya.GetComponent<SpriteRenderer>().sprite;
                    envanterResmi.enabled = true;
                }

                elimdekiEsya.SetActive(false); // Sahneden gizle
                break;
            }
        }
    }

    void EsyaFirlat()
    {
        // El noktası yok, direkt karakterin merkezinden çıkar
        elimdekiEsya.transform.position = transform.position;
        elimdekiEsya.SetActive(true);

        // UI'yı temizle
        if (envanterResmi != null) envanterResmi.enabled = false;

        Rigidbody2D esyaRb = elimdekiEsya.GetComponent<Rigidbody2D>();
        if (esyaRb != null)
        {
            float yon = transform.localScale.x > 0 ? 1 : -1;
            // İleri ve hafif yukarı doğru kavisli fırlatma
            esyaRb.linearVelocity = new Vector2(firlatmaGucuX * yon, firlatmaGucuY);
        }

        elDoluMu = false;
        elimdekiEsya = null;
    }

    // --- Zemin Kontrolü ---
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }
}