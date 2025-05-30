// Assets/Scripts/UpgradeManager.cs
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    // price tiers for each upgrade
    private readonly int[] armorPrices  = {  10,  40, 100, 200 };
    private readonly int[] glovePrices  = {  10,  40, 100, 200 };
    private readonly int[] bootPrices   = {  10,  40, 100, 200 };
    private readonly int[] rocketPrices = {  10,  40, 100, 200 };

    private int armorLevel, gloveLevel, bootLevel, rocketLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            armorLevel  = PlayerPrefs.GetInt("Upgrade_Armor",  0);
            gloveLevel  = PlayerPrefs.GetInt("Upgrade_Gloves", 0);
            bootLevel   = PlayerPrefs.GetInt("Upgrade_Boots",  0);
            rocketLevel = PlayerPrefs.GetInt("Upgrade_Rocket", 0);
        }
        else Destroy(gameObject);
    }

    public int GetArmorLevel()  => armorLevel;
    public int GetGloveLevel()  => gloveLevel;
    public int GetBootLevel()   => bootLevel;
    public int GetRocketLevel() => rocketLevel;

    public int GetArmorPrice()  
        => armorLevel < armorPrices.Length ? armorPrices[armorLevel] : -1;
    // likewise for gloves, boots, rocket…
    public int GetGlovePrice()  => gloveLevel  < glovePrices.Length  ? glovePrices[gloveLevel]  : -1;
    public int GetBootPrice()   => bootLevel   < bootPrices.Length   ? bootPrices[bootLevel]   : -1;
    public int GetRocketPrice() => rocketLevel < rocketPrices.Length ? rocketPrices[rocketLevel] : -1;

    public bool BuyArmor()
    {
        int price = GetArmorPrice();
        if (price < 0 || !CoinManager.Instance.TrySpend(price)) 
            return false;
        armorLevel++;
        PlayerPrefs.SetInt("Upgrade_Armor", armorLevel);
        PlayerPrefs.Save();
        return true;
    }

    // similarly BuyGloves(), BuyBoots(), BuyRocket()…
    public bool BuyGloves()
    {
        int price = GetGlovePrice();
        if (price < 0 || !CoinManager.Instance.TrySpend(price)) 
            return false;
        gloveLevel++;
        PlayerPrefs.SetInt("Upgrade_Gloves", gloveLevel);
        PlayerPrefs.Save();
        return true;
    }

    public bool BuyBoots()
    {
        int price = GetBootPrice();
        if (price < 0 || !CoinManager.Instance.TrySpend(price)) 
            return false;
        bootLevel++;
        PlayerPrefs.SetInt("Upgrade_Boots", bootLevel);
        PlayerPrefs.Save();
        return true;
    }

    public bool BuyRocket()
    {
        int price = GetRocketPrice();
        if (price < 0 || !CoinManager.Instance.TrySpend(price)) 
            return false;
        rocketLevel++;
        PlayerPrefs.SetInt("Upgrade_Rocket", rocketLevel);
        PlayerPrefs.Save();
        return true;
    }
}
