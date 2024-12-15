namespace GildedRoseKata
{
    using System;
    using System.Collections.Generic;
    public class GildedRose
    {
        private const string AgedBrie = "Aged Brie";
        private const string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";
        private const string Sulfuras = "Sulfuras, Hand of Ragnaros";
        private const int MaxQuality = 50;

        IList<Item> Items;
        public GildedRose(IList<Item> Items)
        {
            this.Items = Items;
        }

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
                // degrade quality before degrading sellin
                if (item.Name != AgedBrie && item.Name != BackstagePasses && item.Name != Sulfuras)
                {
                    item.Quality = Math.Max(0, item.Quality - 1);
                }
                else
                {
                    if (item.Quality < MaxQuality)
                    {
                        item.Quality++;

                        if (item.Name == BackstagePasses)
                        {
                            if (item.SellIn < 11 && item.Quality < MaxQuality)
                            {
                                item.Quality++;
                            }

                            if (item.SellIn < 6 && item.Quality < MaxQuality)
                            {
                                item.Quality++;
                            }
                        }
                    }
                }

                // reduce sell-in
                if (item.Name == Sulfuras)
                {
                    continue;
                }
                else
                {
                    item.SellIn--;
                }

                if (item.SellIn >= 0)
                {
                    continue;
                }

                // degrade quality again when passed sell-in date
                item.Quality = item.Name switch
                {
                    AgedBrie => Math.Min(MaxQuality, item.Quality + 1),
                    BackstagePasses => 0,
                    _ => Math.Max(0, item.Quality - 1)
                };          
            }
        }
    }
}
