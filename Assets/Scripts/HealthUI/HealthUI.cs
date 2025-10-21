using UnityEngine;
using TMPro;

namespace Simulacro
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private PlayerMovement player;

        private int lastHealth = -1; // Para evitar actualizaciones innecesarias

        void Update()
        {
            // Solo actualiza si la salud cambió
            if (player != null && player.health != lastHealth)
            {
                UpdateHealthDisplay();
                lastHealth = player.health;
            }
        }

        private void UpdateHealthDisplay()
        {
            if (healthText != null)
            {
                healthText.text = $"Health: {player.health}";

                if (player.health <= 10)
                {
                    healthText.color = Color.red;
                }
                else if (player.health <= 20)
                {
                    healthText.color = Color.yellow;
                }
                else
                {
                    healthText.color = Color.green;
                }
            }
        }
    }
}