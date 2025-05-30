using TMPro;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class StoreUI : MonoBehaviour
{
    // [Header("Coins Display")]
    // public TextMeshProUGUI coinsText;

    // [Header("Armor Row")]
    // public TextMeshProUGUI  armorLevelText;
    // public TextMeshProUGUI  armorPriceText;
    // public Button           armorBuyButton;

    // [Header("Gloves Row")]
    // public TextMeshProUGUI  glovesLevelText;
    // public TextMeshProUGUI  glovesPriceText;
    // public Button           glovesBuyButton;

    // [Header("Boots Row")]
    // public TextMeshProUGUI  bootsLevelText;
    // public TextMeshProUGUI  bootsPriceText;
    // public Button           bootsBuyButton;

    // [Header("Rocket Row")]
    // public TextMeshProUGUI  rocketLevelText;
    // public TextMeshProUGUI  rocketPriceText;
    // public Button           rocketBuyButton;

    // private void Start()
    // {
    //     // Subscribe to coin changes
    //     CoinManager.Instance.OnCoinsChanged += UpdateCoinsDisplay;
    //     UpdateCoinsDisplay(CoinManager.Instance.GetCoins());

    //     // Setup each row
    //     RefreshArmorRow();
    //     armorBuyButton.onClick.AddListener(() =>
    //     {
    //         if (UpgradeManager.Instance.BuyArmor())
    //             RefreshArmorRow();
    //     });

    //     // likewise for glove/boot/rocket rows…
    // }

    // private void UpdateCoinsDisplay(int newAmount)
    // {
    //     coinsText.text = newAmount.ToString();
    // }

    // private void RefreshArmorRow()
    // {
    //     int lvl   = UpgradeManager.Instance.GetArmorLevel();
    //     int price = UpgradeManager.Instance.GetArmorPrice();
    //     armorLevelText.text = $"Lv. {lvl}";
    //     armorPriceText.text = price > 0 ? price.ToString() : "—";
    //     armorBuyButton.interactable = (price > 0 && CoinManager.Instance.GetCoins() >= price);
    // }

    /// <summary>
    /// Call this from your “Back” button to return to the Main Menu scene.
    /// </summary>
    public void OnBackPressed()
    {
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Call this from your “Play” (or “Start Game”) button to go into the Game scene.
    /// </summary>
    public void OnPlayPressed()
    {
        SceneManager.LoadScene("Game");  // or whatever your gameplay scene is called
    }
}
