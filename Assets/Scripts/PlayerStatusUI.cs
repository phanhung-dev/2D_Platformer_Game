using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatusUI : MonoBehaviour
{
    [Header("Health UI")]
    public Image healthFillImage;
    public TextMeshProUGUI healthText;
    public Damageable playerDamageable;

    [Header("Mana UI")]
    public Image manaFillImage;
    public TextMeshProUGUI manaText;
    public Mana playerMana;

    private void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (playerDamageable == null) playerDamageable = player.GetComponent<Damageable>();
            if (playerMana == null) playerMana = player.GetComponent<Mana>();
        }

        if (playerDamageable != null)
        {
            playerDamageable.healthChanged.AddListener(UpdateHealth);
            UpdateHealth(playerDamageable.Health, playerDamageable.MaxHealth);
        }
        if (playerMana != null)
        {
            playerMana.manaChanged.AddListener(UpdateMana);
            UpdateMana(playerMana.CurrentMana, playerMana.MaxMana);
        }
    }

    private void OnDisable()
    {
        if (playerDamageable != null) playerDamageable.healthChanged.RemoveListener(UpdateHealth);
        if (playerMana != null) playerMana.manaChanged.RemoveListener(UpdateMana);
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (healthFillImage != null && maxHealth > 0)
        {
            healthFillImage.fillAmount = (float)currentHealth / maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    public void UpdateMana(int currentMana, int maxMana)
    {
        if (manaFillImage != null && maxMana > 0)
        {
            manaFillImage.fillAmount = (float)currentMana / maxMana;
        }

        if (manaText != null)
        {
            manaText.text = $"{currentMana} / {maxMana}";
        }
    }
}