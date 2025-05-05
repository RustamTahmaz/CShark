using UnityEngine;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    public enum UpgradeType { Health, Damage, Boots, RocketSpeed }

    public UpgradeType upgradeType;

    public TMP_Text valueText;
    public TMP_Text balanceText;

    private float cost = 100f;
    private float balance;

    void Start()
    {
        // Load cost from text or leave as 100f default
        float.TryParse(valueText.text, out cost);

        // Load balance from PlayerPrefs (or default to 9566 if not set)
        balance = PlayerPrefs.GetFloat("Player_Balance", 9566f);
        UpdateTexts();
    }

    public void TryPurchase()
    {
        balance = PlayerPrefs.GetFloat("Player_Balance", 9566f); // refresh latest value

        if (balance >= cost)
        {
            balance -= cost;
            PlayerPrefs.SetFloat("Player_Balance", balance);  // save new balance
            ApplyUpgradeToPrefs();  // Save upgrade
            cost += cost * 0.5f;    // increase cost
            UpdateTexts();          // update UI
        }
        else
        {
            Debug.Log("Not enough balance!");
        }
    }

    void UpdateTexts()
    {
        valueText.text = Mathf.RoundToInt(cost).ToString();
        balance = PlayerPrefs.GetFloat("Player_Balance", 9566f); // get latest balance again
        balanceText.text = Mathf.RoundToInt(balance).ToString();
    }

    void ApplyUpgradeToPrefs()
    {
        switch (upgradeType)
        {
            case UpgradeType.Health:
                PlayerPrefs.SetInt("Upgrade_Health", PlayerPrefs.GetInt("Upgrade_Health", 0) + 20);
                break;
            case UpgradeType.Damage:
                PlayerPrefs.SetInt("Upgrade_Damage", PlayerPrefs.GetInt("Upgrade_Damage", 0) + 5);
                break;
            case UpgradeType.Boots:
                PlayerPrefs.SetFloat("Upgrade_Dash", PlayerPrefs.GetFloat("Upgrade_Dash", 0f) + 5f);
                break;
            case UpgradeType.RocketSpeed:
                PlayerPrefs.SetFloat("Upgrade_Rocket", PlayerPrefs.GetFloat("Upgrade_Rocket", 0f) + 2f);
                break;
        }

        PlayerPrefs.Save(); // ensure it's saved to disk
        Debug.Log("Upgrade applied via PlayerPrefs.");
    }
}
