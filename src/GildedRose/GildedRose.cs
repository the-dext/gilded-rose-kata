namespace GildedRoseKata
{
    using System;
    using System.Collections.Generic;
    public class GildedRose
    {
        private const string AgedBrie = "Aged Brie";
        private const string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";
        private const string Sulfuras = "Sulfuras, Hand of Ragnaros";

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
                    item.Quality = Math.Max(0, item.Quality-1);                    
                }
                else
                {
                    if (item.Quality < 50)
                    {
                        item.Quality++;

                        if (item.Name == BackstagePasses)
                        {
                            if (item.SellIn < 11 && item.Quality < 50)
                            {
                                item.Quality++;
                            }

                            if (item.SellIn < 6 && item.Quality < 50)
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

                // degrade quality again when passed sell-in date
                if (item.SellIn < 0)
                {
                    if (item.Name != AgedBrie)
                    {
                        if (item.Name != BackstagePasses && item.Quality > 0)
                        {
                            item.Quality--;
                        }
                        else
                        {
                            item.Quality = 0;
                        }
                    }
                    else
                    {
                        if (item.Quality < 50)
                        {
                            item.Quality++;
                        }
                    }
                }
            }
        }
    }
}
