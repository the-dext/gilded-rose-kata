namespace GildedRoseKata
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GildedRose
    {
        private const int MaxStandardQuality = 50;
        private const int MinimumStandardQuality = 0;

        // Functions for applying end of day item adjustments are here.
        // For new items or changes in item quality degradation, define function and include it in this map for it to be applied.
        Dictionary<string, Func<Item, (int, int)>> endOfDayAdjustmentFunctions = new()
            {
                { "Aged Brie", AgedBrieEndOfDayAdjustment },
                { "Backstage passes", BackstagePassesEndOfDayAdjustment },
                { "Sulfuras", SulfurasEndOfDayAdjustment },
                { "Conjured", ConjuredEndOfDayAdjustment },
                { "*", DefaultEndOfDayAdjustment },
            };

        IList<Item> Items;
        public GildedRose(IList<Item> Items)
        {
            this.Items = Items;
        }

        public void UpdateQuality()
        {
            Func<Item, (int, int)> GetEndOfDayAdjustmentFunction (string itemName) =>
                endOfDayAdjustmentFunctions.SingleOrDefault(kvp => itemName.StartsWith(kvp.Key)).Value
                ?? DefaultEndOfDayAdjustment;

            foreach (var item in Items)
            {                
                var (sellin, quality) = GetEndOfDayAdjustmentFunction(item.Name)(item);
                item.SellIn = sellin;
                item.Quality = quality;
            }
        }

        /// <summary>
        /// Accepts an item and applies end of day quality adjustment rules applicable to Aged Brie items.
        /// Returns the new Quality and SellIn values. Does not modifie the Item.
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Tuple of (int sellIn, int quality)</returns>
        private static (int sellIn, int quality) AgedBrieEndOfDayAdjustment(Item item)
        {
            var newQuality = item.SellIn > 0
                        ? Math.Min(MaxStandardQuality, item.Quality + 1)
                        : Math.Min(MaxStandardQuality, item.Quality + 2);
            return (item.SellIn - 1, newQuality);
        }

        /// <summary>
        /// Accepts an item and applies end of day quality adjustment rules applicable to Backstage pass items.
        /// Returns the new Quality and SellIn values. Does not modifie the Item.
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Tuple of (int sellIn, int quality)</returns>
        private static (int sellIn, int quality) BackstagePassesEndOfDayAdjustment(Item item)
        {
            var newQuality = item.SellIn switch
            {
                <= 0 => MinimumStandardQuality,
                < 6 => Math.Min(MaxStandardQuality, item.Quality + 3),
                < 11 => Math.Min(MaxStandardQuality, item.Quality + 2),
                _ => Math.Min(MaxStandardQuality, item.Quality + 1),
            };
            return (item.SellIn - 1, newQuality);
        }

        /// <summary>
        /// Accepts an item and applies end of day quality adjustment rules applicable to Conjured items.
        /// Returns the new Quality and SellIn values. Does not modifie the Item.
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Tuple of (int sellIn, int quality)</returns>
        private static (int sellIn, int quality) ConjuredEndOfDayAdjustment(Item item)
        {
            var newQuality = item.SellIn > 0
                        ? Math.Max(MinimumStandardQuality, item.Quality - 2)
                        : Math.Max(MinimumStandardQuality, item.Quality - 4);
            return (item.SellIn - 1, newQuality);
        }

        /// <summary>
        /// Accepts an item and applies default end of day quality adjustment rules applicable to non-specialist items.
        /// Returns the new Quality and SellIn values. Does not modifie the Item.
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Tuple of (int sellIn, int quality)</returns>
        private static (int sellIn, int quality) DefaultEndOfDayAdjustment(Item item)
        {
            var newQuality = item.SellIn > 0
                       ? Math.Max(MinimumStandardQuality, item.Quality - 1)
                       : Math.Max(MinimumStandardQuality, item.Quality - 2);
            return (item.SellIn - 1, newQuality);
        }

        /// <summary>
        /// Accepts an item and applies end of day quality adjustment rules applicable to Sulfuras items.
        /// Returns the new Quality and SellIn values. Does not modifie the Item.
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Tuple of (int sellIn, int quality)</returns>
        private static (int sellIn, int quality) SulfurasEndOfDayAdjustment(Item item)
            => (item.SellIn, item.Quality);       
    }
}
