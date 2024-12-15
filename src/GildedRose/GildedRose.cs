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

        IList<Item> Items;
        public GildedRose(IList<Item> Items)
        {
            this.Items = Items;
        }

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
                if (item.Name == Sulfuras)
                {
                    continue;
                }

                // degrade quality before degrading sellin
                item.Quality = item.Name switch
                {
                    Sulfuras => throw new InvalidOperationException("Sulfuras should never change quality"),
                    
                    AgedBrie => item.SellIn > 0 
                        ? Math.Min(MaxStandardQuality, item.Quality + 1)
                        : Math.Min(MaxStandardQuality, item.Quality + 2),


                    BackstagePasses =>
                        item.SellIn switch
                        {
                            <= 0 => 0,
                            < 6 => Math.Min(MaxStandardQuality, item.Quality + 3),
                            < 11 => Math.Min(MaxStandardQuality, item.Quality + 2),
                            _ => Math.Min(MaxStandardQuality, item.Quality + 1),
                        },

                    Conjured => item.SellIn > 0
                        ? Math.Max(0, item.Quality - 2)
                        : Math.Max(0, item.Quality - 4),

                    _ => item.Quality = item.SellIn > 0 
                        ? Math.Max(0, item.Quality - 1)
                        : Math.Max(0, item.Quality - 2)
                };

                // reduce sell-in                
                item.SellIn--;   
            }
        }
    }
}
