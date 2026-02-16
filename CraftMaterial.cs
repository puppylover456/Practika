// TODO:
// 1. Добавить свойство для типа творчества (шитье, вязание, рисование, лепка)
// 2. Реализовать проверку корректности данных (цена, остаток)
// 3. Реализовать информативное строковое представление материала

namespace CraftStore
{
    public class CraftMaterial
    {
        public int Id { get; set; }                    // Артикул
        public string Name { get; set; }               // Название
        public string Color { get; set; }              // Цвет (если применимо)
        public string MaterialType { get; set; }       // Тип материала (ткань, пряжа, краски, глина)
        public decimal Price { get; set; }             // Цена
        public int StockQuantity { get; set; }         // Количество на складе
        public string Unit { get; set; }               // Единица измерения (метр, моток, набор, штука)
        
        // TODO 1: Добавить свойство CraftType (вид творчества: шитье, вязание, вышивка, рисование, лепка, скрапбукинг)
        public string CraftType { get; set; }
        
        public CraftMaterial(int id, string name, string color, string materialType, decimal price, int stock, string unit, string craftType)
        {
            Id = id;
            Name = name;
            Color = color;
            MaterialType = materialType;
            
            // TODO 2: Проверить что цена не отрицательная
            // Если цена < 0, установить минимальную цену 10
            if (price < 0)
            {
                Price = 10;
            }
            else
            {
                Price = price;
            }
            
            StockQuantity = stock;
            Unit = unit;
            
            // TODO 1: Сохранить вид творчества
            CraftType = craftType;
        }
        
        public override string ToString()
        {
            // TODO 3: Вернуть строку в формате "Ткань хлопок голубая (шитье) - 450 руб/метр (15 м)"
            string colorPart = !string.IsNullOrEmpty(Color) ? $" {Color}" : "";
            return $"{Name}{colorPart} ({CraftType}) - {Price} руб/{Unit} ({StockQuantity} {Unit})";
        }
        
        // Проверить наличие на складе
        public bool IsInStock(int quantity = 1)
        {
            return StockQuantity >= quantity;
        }
        
        // Продать материал (уменьшить остаток)
        public bool Sell(int quantity)
        {
            if (StockQuantity >= quantity)
            {
                StockQuantity -= quantity;
                return true;
            }
            return false;
        }
        
        // Получить стоимость партии
        public decimal CalculateCost(int quantity)
        {
            return Price * quantity;
        }
    }
}