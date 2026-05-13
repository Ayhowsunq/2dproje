using UnityEngine;
using TMPro;

public class AltinEfekt : MonoBehaviour
{
    private TextMeshProUGUI metin;

    // Altın Renk Kodları
    Color koyuAltin = new Color(1f, 0.84f, 0f);    // #FFD700
    Color parlakAltin = new Color(1f, 1f, 0.6f);   // Işıltı rengi

    void Start() => metin = GetComponent<TextMeshProUGUI>();

    void Update()
    {
        // Sinüs dalgası ile yumuşak geçiş (0 ile 1 arası)
        float dalga = (Mathf.Sin(Time.time * 3f) + 1f) / 2f;

        // Metnin ana rengini akıcı bir şekilde değiştirir
        metin.color = Color.Lerp(koyuAltin, parlakAltin, dalga);

        // Bonus: Hafif bir büyüme/küçülme efekti (opsiyonel, istersen sil)
        float olcek = 1f + (dalga * 0.05f);
        transform.localScale = new Vector3(olcek, olcek, 1f);
    }
}