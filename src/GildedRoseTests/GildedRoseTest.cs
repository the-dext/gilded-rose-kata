namespace GildedRoseTests
{
    using GildedRoseKata;
    using Xunit;
    using System.Collections.Generic;
    using FluentAssertions;
    using System.ComponentModel;
    using System.CodeDom;

    public class GildedRoseTest
    {
        [Theory]
        [Trait("Category", "Standard Item")]
        [InlineData("+5 Dexterity Vest", 2)]
        [InlineData("Aged Brie", 2)]
        [InlineData("Elixir of the Mongoose", 2)]        
        [InlineData("Backstage passes to a TAFKAL80ETC concert", 2)]
        [InlineData("Conjured Mana Cake", 2)]
        [InlineData("+5 Dexterity Vest", 0)]
        [InlineData("Aged Brie", 0)]
        [InlineData("Elixir of the Mongoose", 0)]        
        [InlineData("Backstage passes to a TAFKAL80ETC concert", 0)]
        [InlineData("Conjured Mana Cake", 0)]

        public void UpdateQuality_Should_Reduce_SellIn_By_1_Except_Sulfuras(string itemName, int currentSellIn)
        {
            var items = new List<Item> { new Item { Name=itemName, SellIn = currentSellIn } };
            GildedRose sut = new GildedRose(items);
            
            sut.UpdateQuality();

            items[0].SellIn.Should().Be(currentSellIn-1);
        }

        [Theory]
        [Trait("Category", "Legendary Item")]
        [InlineData("Sulfuras, Hand of Ragnaros", 2)]
        [InlineData("Sulfuras, Hand of Ragnaros", 0)]
        [InlineData("Sulfuras, Hand of Ragnaros", -1)]

        public void UpdateQuality_Should_Never_Reduce_SellIn_For_Sulfuras(string itemName, int currentSellIn)
        {
            var items = new List<Item> { new Item { Name = itemName, SellIn = currentSellIn, Quality = 1 } };
            GildedRose app = new GildedRose(items);

            app.UpdateQuality();

            // Sell in property should never be changed
            items[0].SellIn.Should().Be(currentSellIn);
        }

        [Theory]
        [Trait("Category", "Legendary Item")]
        [InlineData("Sulfuras, Hand of Ragnaros", 2)]
        [InlineData("Sulfuras, Hand of Ragnaros", 0)]
        [InlineData("Sulfuras, Hand of Ragnaros", -1)]

        public void UpdateQuality_Quality_Should_Never_Degrade_For_Sulfuras(string itemName, int currentSellIn)
        {
            const int Sulfuras_Quality = 80;
            var items = new List<Item> { new Item { Name = itemName, SellIn = currentSellIn, Quality = Sulfuras_Quality } };
            GildedRose app = new GildedRose(items);

            app.UpdateQuality();

            // Sell in property should never be changed
            items[0].Quality.Should().Be(Sulfuras_Quality);
        }

        [Theory]
        [Trait("Category", "Standard Items")]
        [InlineData("+5 Dexterity Vest", 2)]
        [InlineData("Elixir of the Mongoose", 2)]
        [InlineData("Conjured Mana Cake", 2)]
        public void UpdateQuality_Quality_Should_Degrade_By_1_When_SellIn_Has_Not_Passed(string itemName, int currentQuality)
        {
            var items = new List<Item> { new Item { Name = itemName, SellIn=1, Quality = currentQuality } };
            GildedRose app = new GildedRose(items);

            app.UpdateQuality();

            items[0].Quality.Should().Be(currentQuality-1);
        }

        [Theory]
        [Trait("Category", "Standard Items")]
        [InlineData("+5 Dexterity Vest", 2)]
        [InlineData("Elixir of the Mongoose", 2)]
        [InlineData("Conjured Mana Cake", 2)]
        public void UpdateQuality_Quality_Should_Degrade_By_2_When_SellIn_Has_Passed(string itemName, int currentQuality)
        {            
            var items = new List<Item> { new Item { Name = itemName, SellIn = -1, Quality = currentQuality } };
            GildedRose app = new GildedRose(items);

            app.UpdateQuality();

            items[0].Quality.Should().Be(currentQuality - 2);
        }

        [Theory]
        [Trait("Category", "Standard Items")]
        [InlineData("+5 Dexterity Vest", 1)]
        [InlineData("Elixir of the Mongoose", 1)]
        [InlineData("Conjured Mana Cake", 1)]
        public void UpdateQuality_Quality_Should_Not_Degrade_Below_Zero(string itemName, int currentQuality)
        {
            var items = new List<Item> { new Item { Name = itemName, SellIn = 5, Quality = currentQuality } };
            GildedRose app = new GildedRose(items);

            app.UpdateQuality();

            items[0].Quality.Should().Be(0);
        }
    }
}
