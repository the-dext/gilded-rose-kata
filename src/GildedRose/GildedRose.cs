namespace GildedRoseKata
{
    using System;
    using System.Collections.Generic;
    public class GildedRose
    {
        private const string AgedBrie = "Aged Brie";
        private const string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";
        private const string Sulfuras = "Sulfuras, Hand of Ragnaros";
        private const string Conjured = "Conjured Mana Cake";

        private const int MaxStandardQuality = 50;

        Dictionary<string, Func<Item, (int, int)>> adjustmentMap = new()
            {
                { AgedBrie, AgedBrieEndOfDayAdjustment },
                { BackstagePasses, BackstagePassesEndOfDayAdjustment },
                { Sulfuras, SulfurasEndOfDayAdjustment },
                { Conjured, ConjuredEndOfDayAdjustment },
                { "*", DefaultEndOfDayAdjustment },
            };

        IList<Item> Items;
        public GildedRose(IList<Item> Items)
        {
            this.Items = Items;
        }

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
                var endOfDayAdjustmentFunction = adjustmentMap.GetValueOrDefault(item.Name, DefaultEndOfDayAdjustment);

                var (sellin, quality) = endOfDayAdjustmentFunction(item);
                item.SellIn = sellin;
                item.Quality = quality;
            }
        }

        private static (int sellIn, int quality) AgedBrieEndOfDayAdjustment(Item item)
        {
            var newQuality = item.SellIn > 0
                        ? Math.Min(MaxStandardQuality, item.Quality + 1)
                        : Math.Min(MaxStandardQuality, item.Quality + 2);
            return (item.SellIn - 1, newQuality);
        }

        private static (int sellIn, int quality) ConjuredEndOfDayAdjustment(Item item)
        {
            var newQuality = item.SellIn > 0
                        ? Math.Max(0, item.Quality - 2)
                        : Math.Max(0, item.Quality - 4);
            return (item.SellIn - 1, newQuality);
        }

        private static (int sellIn, int quality) BackstagePassesEndOfDayAdjustment(Item item)
        {
            var newQuality = item.SellIn switch
            {
                <= 0 => 0,
                < 6 => Math.Min(MaxStandardQuality, item.Quality + 3),
                < 11 => Math.Min(MaxStandardQuality, item.Quality + 2),
                _ => Math.Min(MaxStandardQuality, item.Quality + 1),
            };
            return (item.SellIn - 1, newQuality);
        }

        private static (int sellIn, int quality) SulfurasEndOfDayAdjustment(Item item)
            => (item.SellIn, item.Quality);

        private static (int sellIn, int quality) DefaultEndOfDayAdjustment(Item item)
        {
            var newQuality = item.SellIn > 0
                       ? Math.Max(0, item.Quality - 1)
                       : Math.Max(0, item.Quality - 2);
            return (item.SellIn - 1, newQuality);
        }
    }
}
