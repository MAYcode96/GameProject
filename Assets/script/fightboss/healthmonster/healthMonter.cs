using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthBar;

    public Animator animator;
    public string deathAnimationTrigger = "Die";

    public AudioSource audioSource;
    public AudioClip deathSound;

    public CutsceneManager cutsceneManager;

    public Image flashImage;
    public float flashDuration = 0.3f;
    public float waitBeforeCutscene = 0.2f;
    public float waitBeforeFadeOut = 0.5f;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (animator == null)
            animator = GetComponent<Animator>();

        if (flashImage != null)
        {
            flashImage.raycastTarget = false;

            Color c = flashImage.color;
            c.a = 0f;
            flashImage.color = c;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        if (healthBar != null)
            healthBar.value = currentHealth;

        Debug.Log("Monster HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("Monster Mati!");

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        if (animator != null)
        {
            animator.SetTrigger(deathAnimationTrigger);
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        Collider2D col2D = GetComponent<Collider2D>();
        if (col2D != null)
            col2D.enabled = false;
    }

    public void OnDeathAnimationReachFrame()
    {
        Debug.Log("Animasi mati selesai -> Flash");

        StartCoroutine(FlashAndPlayCutscene());
    }

    IEnumerator FlashAndPlayCutscene()
    {
        if (flashImage == null)
        {
            if (cutsceneManager != null)
                cutsceneManager.PlayCutscene();

            yield break;
        }

        Color c = flashImage.color;

        // =====================
        // FLASH MASUK 
        // =====================
        float timer = 0f;

        while (timer < flashDuration)
        {
            timer += Time.deltaTime;

            c.a = Mathf.Lerp(0f, 1f, timer / flashDuration);
            flashImage.color = c;

            yield return null;
        }

        c.a = 1f;
        flashImage.color = c;

        yield return new WaitForSeconds(waitBeforeCutscene);

        // =====================
        //  CUTSCENE
        // =====================
        if (cutsceneManager != null)
        {
            cutsceneManager.PlayCutscene();
        }

        yield return new WaitForSeconds(waitBeforeFadeOut);

        // =====================
        // FLASH 
        // =====================
        timer = 0f;

        while (timer < flashDuration)
        {
            timer += Time.deltaTime;

            c.a = Mathf.Lerp(1f, 0f, timer / flashDuration);
            flashImage.color = c;

            yield return null;
        }

        c.a = 0f;
        flashImage.color = c;
    }

    // Sound event
    public void PlayDeathSound()
    {
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }

}