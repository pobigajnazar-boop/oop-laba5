using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Базовий абстрактний клас, що представляє будь-яку амуніцію лицаря.
/// </summary>
public abstract class Ammunition
{
    public string Name { get; }
    public double Weight { get; }
    public decimal Price { get; }

    /// <summary>
    /// Конструктор базового класу з обробкою виключних ситуацій (валідація даних).
    /// </summary>
    protected Ammunition(string name, double weight, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Назва амуніції не може бути порожньою.", nameof(name));
        if (weight <= 0)
            throw new ArgumentOutOfRangeException(nameof(weight), "Вага повинна бути більшою за нуль.");
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Ціна не може бути від'ємною.");

        Name = name;
        Weight = weight;
        Price = price;
    }

    public override string ToString()
    {
        return $"{GetType().Name} '{Name}' (Вага: {Weight} кг, Ціна: {Price} монет)";
    }
}

/// <summary>
/// Клас-нащадок, що описує шолом.
/// </summary>
public class Helmet : Ammunition
{
    public int DefenseLevel { get; }

    public Helmet(string name, double weight, decimal price, int defenseLevel) 
        : base(name, weight, price)
    {
        DefenseLevel = defenseLevel;
    }
}

/// <summary>
/// Клас-нащадок, що описує броню.
/// </summary>
public class Armor : Ammunition
{
    public string Material { get; }

    public Armor(string name, double weight, decimal price, string material) 
        : base(name, weight, price)
    {
        Material = material;
    }
}

/// <summary>
/// Клас-нащадок, що описує зброю.
/// </summary>
public class Weapon : Ammunition
{
    public int Damage { get; }

    public Weapon(string name, double weight, decimal price, int damage) 
        : base(name, weight, price)
    {
        Damage = damage;
    }
}

/// <summary>
/// Клас, що описує Лицаря, який містить колекцію своєї амуніції.
/// </summary>
public class Knight
{
    private readonly List<Ammunition> _equipment = new List<Ammunition>();

    public IReadOnlyList<Ammunition> Equipment => _equipment.AsReadOnly();

    /// <summary>
    /// Метод для екіпірування лицаря новим предметом.
    /// </summary>
    public void Equip(Ammunition item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "Неможливо екіпірувати неіснуючий предмет (null).");
            
        _equipment.Add(item);
    }

    /// <summary>
    /// Рахує загальну вартість усієї екіпірованої амуніції.
    /// </summary>
    public decimal CalculateTotalCost()
    {
        return _equipment.Sum(item => item.Price);
    }

    /// <summary>
    /// Сортує амуніцію за вагою (зростання).
    /// </summary>
    public IEnumerable<Ammunition> SortByWeight()
    {
        return _equipment.OrderBy(item => item.Weight);
    }

    /// <summary>
    /// Знаходить елементи амуніції, що потрапляють у заданий діапазон цін.
    /// </summary>
    public IEnumerable<Ammunition> FindByPriceRange(decimal minPrice, decimal maxPrice)
    {
        if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
            throw new ArgumentException("Неправильно заданий діапазон цін.");

        return _equipment.Where(item => item.Price >= minPrice && item.Price <= maxPrice);
    }
}

/// <summary>
/// Виконавчий клас програми.
/// </summary>
public class Lab5
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        try
        {
            Knight arthur = new Knight();

            arthur.Equip(new Helmet("Сталевий шолом", 2.5, 150m, 10));
            arthur.Equip(new Armor("Кольчуга", 12.0, 500m, "Сталь"));
            arthur.Equip(new Weapon("Довгий меч", 3.2, 300m, 50));
            arthur.Equip(new Weapon("Кинджал", 0.8, 50m, 15));
            arthur.Equip(new Armor("Шкіряні поножі", 1.5, 80m, "Шкіра"));

            Console.WriteLine("--- Екіпірування лицаря ---");
            foreach (var item in arthur.Equipment)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"\nЗагальна вартість амуніції: {arthur.CalculateTotalCost()} монет");

            Console.WriteLine("\n--- Амуніція, відсортована за вагою ---");
            foreach (var item in arthur.SortByWeight())
            {
                Console.WriteLine(item);
            }

            decimal minPrice = 100m;
            decimal maxPrice = 400m;
            Console.WriteLine($"\n--- Пошук амуніції в діапазоні від {minPrice} до {maxPrice} монет ---");
            foreach (var item in arthur.FindByPriceRange(minPrice, maxPrice))
            {
                Console.WriteLine(item);
            }
            
            // Демонстрація обробки виключних ситуацій (спроба створити зброю з від'ємною вагою)
            Console.WriteLine("\n--- Демонстрація обробки помилок ---");
            Weapon invalidWeapon = new Weapon("Зламана шабля", -5.0, 10m, 5);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Помилка валідації даних: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася непередбачена помилка: {ex.Message}");
        }
    }
}