using Currencies;
using UI;
using UnityEngine;

namespace Helpers
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private HUDController currencyController;
        public static PlayerInventory Instance { get; private set; }

        [SerializeField] private Energy energy;
        [SerializeField] private Coin coins;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool HasEnoughEnergy(int cost)
        {
            return energy.GetAmount() >= cost;
        }

        public void SpendEnergy(int amount)
        {
            if (HasEnoughEnergy(amount))
            {
                energy.IncreaseByAmount(-amount);
                energy.UpdateUI();
            }
        }

        public void IncreaseCurrency(ItemType type, int amount)
        {
            if(type == ItemType.Coin)
                AddCoins(amount);
            if(type == ItemType.Coin)
                AddEnergy(amount);
        }

        private void AddCoins(int amount)
        {
            coins.IncreaseByAmount(amount);
        }

        private void AddEnergy(int amount)
        {
            energy.IncreaseByAmount(amount);
        }

        public int GetEnergyAmount() => energy.GetAmount();
        public int GetCoinAmount() => coins.GetAmount();
    }
}
