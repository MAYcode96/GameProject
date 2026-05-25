using UnityEngine;

public class FadeStage : MonoBehaviour
{
    // Masukkan objek Image UI yang memiliki komponen Canvas Group ke sini via Inspector
    [Header("UI Reference")]
    public CanvasGroup canvasGroup;

    [Header("Settings")]
    // Kecepatan memudar (makin besar nilainya, makin cepat hilangnya)
    public float fadeSpeed = 2f;

    // Variabel untuk mengecek apakah proses fade sedang berjalan
    private bool isFading = false;

    void Start()
    {
        // Kita hapus GetComponent otomatisnya agar tidak membingungkan, 
        // karena Canvas Group-nya sekarang ada di objek lain (Image UI).
        if (canvasGroup == null)
        {
            Debug.LogError("Canvas Group belum dimasukkan ke dalam Inspector di " + gameObject.name);
        }
    }

    void Update()
    {
        // Pastikan Canvas Group sudah diisi di Inspector sebelum menjalankan kode
        if (canvasGroup == null) return;

        // 1. Mendeteksi jika ada tombol apa saja yang ditekan DAN belum dalam proses memudar
        if (Input.anyKeyDown && !isFading)
        {
            isFading = true;

            // Matikan interaksi tombol sejak awal ditekan
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        // 2. Jika proses fade aktif, turunkan alpha secara perlahan
        if (isFading)
        {
            // Mengurangi nilai alpha berdasarkan waktu
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime;

            // Jika alpha sudah habis, hentikan proses fade
            if (canvasGroup.alpha <= 0f)
            {
                canvasGroup.alpha = 0f;
                isFading = false;
            }
        }
    }
}