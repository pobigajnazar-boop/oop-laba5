using System;
using System.Linq;
using Xunit;

/// <summary>
/// Набір тестів для перевірки поліморфізму, логіки Лицаря та обробки виключень.
/// </summary>
public class UnitTest1
{
    [Fact]
    public void CalculateTotalCost_ShouldReturnSumOfAllItems()
    {
        var knight = new Knight();
        knight.Equip(new Helmet("Шолом", 2.0, 100m, 5));
        knight.Equip(new Weapon("Меч", 3.0, 250m, 20));

        decimal totalCost = knight.CalculateTotalCost();

        Assert.Equal(350m, totalCost);
    }

    [Fact]
    public void SortByWeight_ShouldReturnItemsInAscendingOrder()
    {
        var knight = new Knight();
        var armor = new Armor("Броня", 15.0, 500m, "Залізо");
        var dagger = new Weapon("Кинджал", 1.0, 50m, 10);
        var helmet = new Helmet("Шолом", 3.0, 100m, 5);
        
        knight.Equip(armor);
        knight.Equip(dagger);
        knight.Equip(helmet);

        var sorted = knight.SortByWeight().ToList();

        Assert.Equal(dagger, sorted[0]);
        Assert.Equal(helmet, sorted[1]);
        Assert.Equal(armor, sorted[2]);
    }

    [Fact]
    public void FindByPriceRange_ShouldReturnOnlyItemsWithinRange()
    {
        var knight = new Knight();
        knight.Equip(new Weapon("Дорогий меч", 3.0, 1000m, 50));
        knight.Equip(new Weapon("Звичайний меч", 3.0, 200m, 20));
        knight.Equip(new Weapon("Дешевий кинджал", 1.0, 10m, 5));

        var result = knight.FindByPriceRange(100m, 500m).ToList();

        Assert.Single(result);
        Assert.Equal("Звичайний меч", result[0].Name);
    }

    [Fact]
    public void AmmunitionConstructor_WhenWeightIsNegative_ShouldThrowException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Helmet("Шолом", -2.0, 100m, 5));
    }

    [Fact]
    public void Equip_WhenItemIsNull_ShouldThrowException()
    {
        var knight = new Knight();
        
        Assert.Throws<ArgumentNullException>(() => knight.Equip(null));
    }
}