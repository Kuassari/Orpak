using System.Collections.Generic;
using PPXModel;
using Visa;
using Loyalty;
using System;

namespace PPX_PromotionEngine
{
    /// <summary>
    /// PromotionEngine
    /// Assumptions:
    /// Each engine will get the original item price.
    /// 2 or more with the same id will have the same discounts.
    /// 
    /// </summary>
    public class PromotionEngine
    {
        // Cache for provider discount items - loaded once per provider
        private readonly Dictionary<string, HashSet<int>> _providerDiscountableItems;

        // Cache for calculated item discounts - avoids recalculation for duplicate items
        private readonly Dictionary<int, double> _itemDiscountCache;

        // List of all available providers
        private readonly List<dynamic> _providers;

        public PromotionEngine()
        {
            _providerDiscountableItems = new Dictionary<string, HashSet<int>>();
            _itemDiscountCache = new Dictionary<int, double>();

            _providers = new List<dynamic>
            {
                new LoyaltyPromotionEngine(),
                new VisaPromotionEngine()
                // Future providers can be added here
            };

            InitializeProviderCache();
        }

        /// <summary>
        /// Load discountable items from all providers once
        /// Prevents repeated API calls during item processing
        /// If provider fails during initialization, continue with empty set
        /// </summary>
        private void InitializeProviderCache()
        {
            foreach (var provider in _providers)
            {
                string providerName = provider.GetType().Name;

                try
                {
                    var discountableIds = provider.GetDiscountableItemIds();
                    _providerDiscountableItems[providerName] = new HashSet<int>(discountableIds);
                    Console.WriteLine($"Loaded {discountableIds.Count} discountable items from {providerName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: Failed to load discountable items from {providerName}: {ex.Message}");
                    _providerDiscountableItems[providerName] = new HashSet<int>();
                }
            }
        }

        /// <summary>
        /// GetDiscount method - totalDiscount for each items
        /// multiple items 
        /// </summary>
        /// <param name="items">List of items</param>
        /// <returns></returns>
        public List<(Item item, double newPrice)> GetDiscounts(List<Item> items)
        {
            var result = new List<(Item, double)>();

            foreach (var item in items)
            {
                double totalDiscount;

                // Check cache first for items with same ID, else calulate discount and cache it
                if (_itemDiscountCache.ContainsKey(item.Id))
                {
                    totalDiscount = _itemDiscountCache[item.Id];
                }
                else
                {
                    totalDiscount = CalculateItemDiscount(item.Id, item.Price);
                    _itemDiscountCache[item.Id] = totalDiscount;
                }

                double newPrice = item.Price - totalDiscount;
                result.Add((item, newPrice));
            }

            return result;
        }

        /// <summary>
        /// Calculate total discount for an item from all available providers
        /// Each provider is checked independently and results are summed
        /// If GetItemDiscount fails, continue with other providers
        /// </summary>
        private double CalculateItemDiscount(int itemId, double originalPrice)
        {
            double totalDiscount = 0;

            foreach (var provider in _providers)
            {
                string providerName = provider.GetType().Name;

                // Use cached HashSet for O(1) lookup instead of repeated API calls
                if (_providerDiscountableItems[providerName].Contains(itemId))
                {
                    try
                    {
                        double discount = provider.GetItemDiscount(itemId, originalPrice);
                        totalDiscount += discount;

                        Console.WriteLine($"{providerName} discount for item {itemId}: {discount:F2}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error getting discount from {providerName} for item {itemId}: {ex.Message}");
                    }
                }
            }

            return totalDiscount;
        }

    }
}
