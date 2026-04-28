using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded; // Yerde miyiz?
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 1. Sağa Sola Hareket
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // 2. Yön Çevirme (Karakterin bakış yönü)
        if (moveInput > 0) transform.localScale = originalScale;
        else if (moveInput < 0) transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

        // 3. Zıplama (Sadece W tuşu ve yerdeyse)
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false; // Havaya kalktığı an yerle temas biter
        }

        // 4. Animasyonlar (Animator'daki isGrounded ve Speed parametrelerini günceller)
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(moveInput));
            anim.SetBool("isGrounded", isGrounded);
        }
    }

    // --- TEMAS KONTROLLERİ ---

    // Karakter bir şeye değdiği sürece çalışır
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Çarptığın objenin Tag'i "Ground" ise yerde kabul et
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // Karakter teması kestiği an çalışır
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}