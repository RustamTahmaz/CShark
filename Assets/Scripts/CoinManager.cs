// Assets/Scripts/CoinManager.cs
using UnityEngine;
using System;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    public event Action<int> OnCoinsChanged;

    private int _coins;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _coins = PlayerPrefs.GetInt("Coins", 0);
        }
        else Destroy(gameObject);
    }

    public int GetCoins() => _coins;

    public void AddCoins(int amount)
    {
        _coins += amount;
        PlayerPrefs.SetInt("Coins", _coins);
        PlayerPrefs.Save();
        OnCoinsChanged?.Invoke(_coins);
    }

    public bool TrySpend(int amount)
    {
        if (_coins < amount) return false;
        _coins -= amount;
        PlayerPrefs.SetInt("Coins", _coins);
        PlayerPrefs.Save();
        OnCoinsChanged?.Invoke(_coins);
        return true;
    }
}
