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
        // Cache calculated discounts for duplicate items
        private Dictionary<int, double> _itemDiscountCache = new Dictionary<int, double>();

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

                // Check cache first for items with same ID (performance optimization)
                if (_itemDiscountCache.ContainsKey(item.Id))
                {
                    totalDiscount = _itemDiscountCache[item.Id];
                }
                else
                {
                    totalDiscount = CalculateItemDiscount(item.Id, item.Price);
                    _itemDiscountCache[item.Id] = totalDiscount;
                }

                // Calculate new price: original price - total discounts
                double newPrice = item.Price - totalDiscount;
                result.Add((item, newPrice));
            }

            return result;
        }

        /// <summary>
        /// Calculate total discount for an item from all available providers
        /// Each provider is checked independently and results are summed
        /// </summary>
        private double CalculateItemDiscount(int itemId, double originalPrice)
        {
            double totalDiscount = 0;

            // Assumption: each dll have GetDiscountableItemIds() and GetItemDiscount(int, double)
            var providers = new List<dynamic>
            {
                new LoyaltyPromotionEngine(),
                new VisaPromotionEngine()
                // Future DLLs can be added here, for example:
                // new MastercardPromotionEngine(),
            };

            // Check each provider and sum all applicable discounts
            foreach (var provider in providers)
            {
                totalDiscount += GetDiscountFromProvider(provider, itemId, originalPrice);
            }

            return totalDiscount;
        }

        /// <summary>
        /// Generic method to get discount from any provider DLL
        /// Works with any DLL that has GetDiscountableItemIds() and GetItemDiscount() methods
        /// Returns 0 if item not eligible or if provider fails
        /// </summary>
        private double GetDiscountFromProvider(dynamic provider, int itemId, double originalPrice)
        {
            try
            {
                //Check if this item is eligible for discount from this provider
                var discountableIds = provider.GetDiscountableItemIds();

                // if Item is eligible - calculate discount else return 0
                if (discountableIds.Contains(itemId))
                {
                    double discount = provider.GetItemDiscount(itemId, originalPrice);
                    string providerName = provider.GetType().Name; // Get DLL name for logging
                    Console.WriteLine($"{providerName} discount for item {itemId}: {discount:F2}");
                    return discount;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                // Provider failed - no discount from this provider
                string providerName = provider?.GetType()?.Name ?? "Unknown";
                Console.WriteLine($"{providerName} provider failed for item {itemId}: {ex.Message}");
                return 0;
            }
        }

    }
}
