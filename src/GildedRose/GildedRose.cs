namespace GildedRoseKata
{
    using System;
    using System.Collections.Generic;
    public class GildedRose
    {
        private const string AgedBrie = "Aged Brie";
        private const string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";
        private const string Sulfuras = "Sulfuras, Hand of Ragnaros";

        private const int SulfurasQuality = 50;
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
                int maxQualityAllowed = item.Name == Sulfuras 
                    ? 80 
                    : MaxStandardQuality;


                // degrade quality before degrading sellin
                item.Quality = item.Name switch
                {
                    Sulfuras => item.Quality,
                    AgedBrie => item.Quality = Math.Min(maxQualityAllowed, item.Quality + 1),
                    BackstagePasses =>
                        item.SellIn switch
                        {
                            < 6 => Math.Min(maxQualityAllowed, item.Quality + 3),
                            < 11 => Math.Min(maxQualityAllowed, item.Quality + 2),
                            _ => Math.Min(maxQualityAllowed, item.Quality + 1),
                        },

                    _ => item.Quality = Math.Max(0, item.Quality - 1),
                };

                // reduce sell-in
                if (item.Name == Sulfuras)
                {
                    continue;
                }
                
                item.SellIn--;                

                if (item.SellIn >= 0)
                {
                    continue;
                }

                // degrade quality again when passed sell-in date
                item.Quality = item.Name switch
                {
                    AgedBrie => Math.Min(maxQualityAllowed, item.Quality + 1),
                    BackstagePasses => 0,
                    _ => Math.Max(0, item.Quality - 1)
                };          
            }
        }
    }
}
