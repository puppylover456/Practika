// TODO:
// 1. Реализовать создание готовых наборов для хобби
// 2. Реализовать расчет стоимости набора
// 3. Реализовать проверку комплектности и сложности

using System;
using System.Collections.Generic;

namespace CraftStore
{
    public class CraftKit
    {
        public int Id { get; set; }
        public string Name { get; set; }               // Название набора
        public string Description { get; set; }        // Описание проекта
        public string CraftType { get; set; }          // Вид творчества
        public string Difficulty { get; set; }         // Сложность (начальный, средний, сложный)
        public int EstimatedTime { get; set; }         // Примерное время выполнения (часы)
        public bool IncludesTools { get; set; }        // Включены ли инструменты
        
        private Dictionary<CraftMaterial, int> materials = new Dictionary<CraftMaterial, int>(); // Материалы и количество
        private List<string> requiredTools = new List<string>(); // Необходимые инструменты
        
        // TODO 1: Добавить свойство AgeRecommendation (возрастная рекомендация: 6+, 12+, 18+)
        public string AgeRecommendation { get; set; }
        
        public CraftKit(int id, string name, string description, string craftType, string difficulty, int time, bool tools, string ageRecommendation)
        {
            Id = id;
            Name = name;
            Description = description;
            CraftType = craftType;
            Difficulty = difficulty;
            EstimatedTime = time;
            IncludesTools = tools;
            // TODO 1: Сохранить возрастную рекомендацию
            AgeRecommendation = ageRecommendation;
        }
        
        // TODO 2: Добавить материал в набор
        public void AddMaterial(CraftMaterial material, int quantity)
        {
            // Добавить материал в словарь materials
            // Если материал уже есть - увеличить количество
            if (materials.ContainsKey(material))
            {
                materials[material] += quantity;
            }
            else
            {
                materials[material] = quantity;
            }
        }
        
        // TODO 2: Добавить необходимый инструмент
        public void AddRequiredTool(string tool)
        {
            // Добавить инструмент в список requiredTools
            if (!requiredTools.Contains(tool))
            {
                requiredTools.Add(tool);
            }
        }
        
        // TODO 2: Рассчитать стоимость набора
        public decimal CalculateKitPrice()
        {
            decimal total = 0;
            
            // Пройти по всем материалам в наборе
            // Суммировать: material.Price * quantity
            foreach (var item in materials)
            {
                total += item.Key.Price * item.Value;
            }
            
            // Добавить наценку за упаковку и инструкцию (например, 20%)
            return total * 1.2m;
        }
        
        // TODO 3: Проверить доступность всех материалов
        public bool IsKitAvailable()
        {
            // Проверить для каждого материала в наборе:
            // material.IsInStock(quantity)
            // Вернуть true только если ВСЕ материалы в наличии
            foreach (var item in materials)
            {
                if (!item.Key.IsInStock(item.Value))
                {
                    return false;
                }
            }
            return true;
        }
        
        // TODO 3: Получить список недостающих материалов
        public List<string> GetMissingMaterials()
        {
            List<string> missing = new List<string>();
            
            // Проверить каждый материал в наборе
            // Если материала недостаточно - добавить его название в список missing
            foreach (var item in materials)
            {
                if (!item.Key.IsInStock(item.Value))
                {
                    missing.Add($"{item.Key.Name} (нужно {item.Value}, в наличии {item.Key.StockQuantity})");
                }
            }
            return missing;
        }
        
        // Получить материалы набора
        public Dictionary<CraftMaterial, int> GetMaterials()
        {
            return new Dictionary<CraftMaterial, int>(materials);
        }

        // Получить список необходимых инструментов (если инструменты не включены)
        public List<string> GetRequiredTools()
        {
            return new List<string>(requiredTools);
        }
        
        // Показать информацию о наборе
        public void ShowKitInfo()
        {
            Console.WriteLine($"=== НАБОР: {Name} ===");
            Console.WriteLine($"Тип творчества: {CraftType}");
            Console.WriteLine($"Сложность: {Difficulty}");
            Console.WriteLine($"Время выполнения: {EstimatedTime} часов");
            Console.WriteLine($"Возраст: {AgeRecommendation}");
            Console.WriteLine($"Инструменты: {(IncludesTools ? "Включены" : "Не включены")}");
            
            Console.WriteLine("\nСостав набора:");
            foreach (var item in materials)
            {
                Console.WriteLine($"  {item.Key.Name} ({item.Key.Color}) x{item.Value} {item.Key.Unit}");
            }
            
            if (requiredTools.Count > 0 && !IncludesTools)
            {
                Console.WriteLine("\nНеобходимые инструменты:");
                foreach (var tool in requiredTools)
                {
                    Console.WriteLine($"  {tool}");
                }
            }
            
            Console.WriteLine($"\nСтоимость набора: {CalculateKitPrice()} руб.");
            Console.WriteLine($"Доступность: {(IsKitAvailable() ? "В наличии" : "Недостаточно материалов")}");
        }
    }
}