using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCard : MonoBehaviour
{
    public PlayerDrag2D player;

    public Button button;
    public Image cooldownMask;
    public TMP_Text cooldownText;
    public float cooldown = 5f;
    private float currentCooldown;
    private bool onCooldown = false;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public PlayerAnimation playerAnimation;

    public AudioSource audioSource;
    public AudioClip attackSound;

    void Start()
    {
        cooldownMask.fillAmount = 0;
        cooldownText.text = "";
    }

    void Update()
    {
        if (onCooldown)
        {
            currentCooldown -= Time.deltaTime;

            cooldownMask.fillAmount =
                currentCooldown / cooldown;

            cooldownText.text =
                Mathf.Ceil(currentCooldown).ToString();

            if (currentCooldown <= 0)
            {
                onCooldown = false;

                button.interactable = true;

                cooldownMask.fillAmount = 0;
                cooldownText.text = "";
            }
        }
    }

    public void UseSkill()
    {
        if (!player.isOnTile)
        {
            Debug.Log("Karakter belum di tile!");
            return;
        }

        if (onCooldown)
            return;


        // Sound serangan
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        // Mainkan animasi
        if (playerAnimation != null)
        {
            playerAnimation.PlayAttack();
        }

        Vector3 spawnPos = firePoint.position;
        spawnPos.z = 0;

        Instantiate(
            bulletPrefab,
            spawnPos,
            firePoint.rotation
        );

        onCooldown = true;
        currentCooldown = cooldown;

        button.interactable = false;
    }
}