// TODO:
// 1. Реализовать каталог материалов по видам творчества
// 2. Реализовать консультацию по подбору материалов для проекта
// 3. Реализовать мастер-классы и рекомендации

using System;
using System.Collections.Generic;

namespace CraftStore
{
    public class StoreMenu
    {
        private WorkshopManager manager;
        
        public StoreMenu()
        {
            manager = new WorkshopManager();
            InitializeStoreData();
        }
        
        private void InitializeStoreData()
        {
            // Инициализация тестовых данных - материалы
            manager.AddMaterial(new CraftMaterial(1, "Ткань хлопок", "голубой", "ткань", 450, 50, "метр", "шитье"));
            manager.AddMaterial(new CraftMaterial(2, "Пряжа акрил", "белый", "пряжа", 280, 100, "моток", "вязание"));
            manager.AddMaterial(new CraftMaterial(3, "Краски акварель", "набор 12 цветов", "краски", 850, 25, "набор", "рисование"));
            manager.AddMaterial(new CraftMaterial(4, "Глина полимерная", "бежевый", "глина", 320, 40, "пачка", "лепка"));
            manager.AddMaterial(new CraftMaterial(5, "Бисер чешский", "разноцветный", "бисер", 150, 200, "упаковка", "вышивка"));
            
            // Инициализация наборов для творчества
            CraftKit embroideryKit = new CraftKit(1, "Вышивка для начинающих", "Набор для вышивки крестиком", 
                "вышивка", "начальный", 8, true, "12+");
            // TODO: Добавить материалы в набор
            var beadMaterial = manager.GetAllMaterials().FirstOrDefault(m => m.Name.Contains("Бисер"));
            if (beadMaterial != null)
            {
                embroideryKit.AddMaterial(beadMaterial, 2);
            }
            embroideryKit.AddRequiredTool("Иголка для вышивания");
            embroideryKit.AddRequiredTool("Пяльцы");
            manager.AddKit(embroideryKit);
            
            // Добавим еще несколько наборов для разнообразия
            CraftKit sewingKit = new CraftKit(2, "Шитье платья", "Набор для пошива летнего платья", 
                "шитье", "средний", 12, false, "18+");
            var fabricMaterial = manager.GetAllMaterials().FirstOrDefault(m => m.Name.Contains("Ткань"));
            if (fabricMaterial != null)
            {
                sewingKit.AddMaterial(fabricMaterial, 3);
            }
            sewingKit.AddRequiredTool("Швейная машина");
            sewingKit.AddRequiredTool("Ножницы");
            manager.AddKit(sewingKit);
            
            CraftKit knittingKit = new CraftKit(3, "Вязание шарфа", "Набор для вязания теплого шарфа", 
                "вязание", "начальный", 6, true, "12+");
            var yarnMaterial = manager.GetAllMaterials().FirstOrDefault(m => m.Name.Contains("Пряжа"));
            if (yarnMaterial != null)
            {
                knittingKit.AddMaterial(yarnMaterial, 3);
            }
            manager.AddKit(knittingKit);
        }
        
        // TODO 1: Показать материалы по категориям
        public void ShowMaterialsByCategory()
        {
            Console.WriteLine("=== КАТАЛОГ МАТЕРИАЛОВ ===");
            
            // Получить все материалы через manager.GetAllMaterials()
            var allMaterials = manager.GetAllMaterials();
            
            // Сгруппировать материалы по видам творчества
            var groupedMaterials = allMaterials.GroupBy(m => m.CraftType);
            
            // Для каждой категории вывести материалы с детальной информацией
            foreach (var group in groupedMaterials)
            {
                Console.WriteLine($"\n--- {group.Key.ToUpper()} ---");
                foreach (var material in group)
                {
                    Console.WriteLine($"  {material}");
                }
            }
        }
        
        // TODO 1: Показать наборы для творчества
        public void ShowCraftKits()
        {
            Console.WriteLine("=== НАБОРЫ ДЛЯ ТВОРЧЕСТВА ===");
            
            // Получить все наборы через manager.GetAllKits()
            var allKits = manager.GetAllKits();
            
            // Сгруппировать наборы по сложности
            var groupedKits = allKits.GroupBy(k => k.Difficulty);
            
            // Для каждого набора вызвать ShowKitInfo()
            foreach (var group in groupedKits)
            {
                Console.WriteLine($"\n--- Сложность: {group.Key.ToUpper()} ---");
                foreach (var kit in group)
                {
                    kit.ShowKitInfo();
                    Console.WriteLine();
                }
            }
        }
        
        // TODO 2: Консультация по проекту
        public void ProvideProjectConsultation()
        {
            Console.WriteLine("=== КОНСУЛЬТАЦИЯ ПО ПРОЕКТУ ===");
            
            // 1. Найти или зарегистрировать клиента
            Console.Write("Введите email клиента (или Enter для регистрации): ");
            string email = Console.ReadLine();
            Customer customer = null;
            
            if (!string.IsNullOrEmpty(email))
            {
                customer = manager.FindCustomerByEmail(email);
            }
            
            if (customer == null)
            {
                Console.Write("Введите ФИО: ");
                string fullName = Console.ReadLine();
                Console.Write("Введите телефон: ");
                string phone = Console.ReadLine();
                if (string.IsNullOrEmpty(email))
                {
                    Console.Write("Введите email: ");
                    email = Console.ReadLine();
                }
                Console.Write("Уровень навыков (новичок/любитель/профи): ");
                string skillLevel = Console.ReadLine() ?? "новичок";
                customer = manager.RegisterCustomer(fullName, phone, email, skillLevel);
                Console.WriteLine($"Клиент зарегистрирован: {customer.FullName}");
            }
            
            // 2. Узнать тип проекта (шитье, вязание и т.д.)
            Console.Write("Какой тип проекта планируете? (шитье/вязание/вышивка/рисование/лепка): ");
            string craftType = Console.ReadLine();
            
            // 3. Узнать уровень навыков клиента (уже есть в customer.SkillLevel)
            Console.WriteLine($"Уровень навыков клиента: {customer.SkillLevel}");
            
            // 4. Предложить подходящие материалы через manager.FindMaterialsByCraftType()
            var materials = manager.FindMaterialsByCraftType(craftType);
            if (materials.Count > 0)
            {
                Console.WriteLine("\nРекомендуемые материалы:");
                foreach (var material in materials)
                {
                    Console.WriteLine($"  - {material}");
                }
            }
            
            // 5. Предложить готовые наборы через manager.RecommendKitsForCustomer()
            var recommendedKits = manager.RecommendKitsForCustomer(customer, craftType);
            if (recommendedKits.Count > 0)
            {
                Console.WriteLine("\nРекомендуемые наборы:");
                foreach (var kit in recommendedKits)
                {
                    Console.WriteLine($"  - {kit.Name} ({kit.Difficulty}, {kit.AgeRecommendation})");
                }
            }
            
            // 6. Помочь рассчитать необходимое количество материалов
            Console.Write("\nХотите создать проект? (да/нет): ");
            string createProject = Console.ReadLine();
            if (createProject?.ToLower() == "да")
            {
                Console.Write("Название проекта: ");
                string projectName = Console.ReadLine();
                
                // 7. Создать новый проект для клиента
                var project = customer.StartProject(projectName, craftType);
                customer.AddInterest(craftType);
                Console.WriteLine($"Проект '{projectName}' создан!");
            }
        }
        
        // TODO 2: Оформить заказ на материалы
        public void ProcessOrder()
        {
            Console.WriteLine("=== ОФОРМЛЕНИЕ ЗАКАЗА ===");
            
            // 1. Найти клиента по email или телефону
            Console.Write("Введите email или телефон клиента: ");
            string search = Console.ReadLine();
            Customer customer = manager.FindCustomerByEmail(search);
            if (customer == null)
            {
                customer = manager.FindCustomerByPhone(search);
            }
            
            if (customer == null)
            {
                Console.WriteLine("Клиент не найден. Сначала зарегистрируйте клиента через консультацию.");
                return;
            }
            
            // 2. Если клиент работает над проектом - предложить добавить материалы в него
            var activeProjects = customer.GetActiveProjects();
            
            Customer.Project selectedProject = null;
            if (activeProjects.Count > 0)
            {
                Console.WriteLine("\nАктивные проекты:");
                for (int i = 0; i < activeProjects.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {activeProjects[i].Name} ({activeProjects[i].CraftType})");
                }
                Console.Write("Выберите проект (или 0 для нового): ");
                if (int.TryParse(Console.ReadLine(), out int projectChoice) && projectChoice > 0 && projectChoice <= activeProjects.Count)
                {
                    selectedProject = activeProjects[projectChoice - 1];
                }
            }
            
            // 3. Иначе предложить начать новый проект
            if (selectedProject == null)
            {
                Console.Write("Создать новый проект? (да/нет): ");
                if (Console.ReadLine()?.ToLower() == "да")
                {
                    Console.Write("Название проекта: ");
                    string projectName = Console.ReadLine();
                    Console.Write("Тип творчества: ");
                    string craftType = Console.ReadLine();
                    selectedProject = customer.StartProject(projectName, craftType);
                }
            }
            
            // 4. Показать каталог материалов или наборов
            Console.WriteLine("\n1. Материалы");
            Console.WriteLine("2. Наборы");
            Console.Write("Выберите: ");
            string choice = Console.ReadLine();
            
            decimal totalCost = 0;
            
            if (choice == "1")
            {
                ShowMaterialsByCategory();
                Console.Write("\nВведите ID материала (0 для завершения): ");
                while (int.TryParse(Console.ReadLine(), out int materialId) && materialId != 0)
                {
                    var material = manager.GetAllMaterials().FirstOrDefault(m => m.Id == materialId);
                    if (material != null)
                    {
                        Console.Write("Количество: ");
                        if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                        {
                            if (material.IsInStock(quantity))
                            {
                                // 5. Добавить выбранные позиции в проект
                                if (selectedProject != null)
                                {
                                    customer.AddMaterialToProject(selectedProject, material, quantity, selectedProject.Name);
                                }
                                
                                // 6. Рассчитать стоимость
                                decimal cost = material.CalculateCost(quantity);
                                totalCost += cost;
                                Console.WriteLine($"Добавлено: {material.Name} x{quantity} = {cost} руб.");
                                
                                // 8. Обновить остатки материалов
                                material.Sell(quantity);
                            }
                            else
                            {
                                Console.WriteLine($"Недостаточно материала на складе (в наличии: {material.StockQuantity})");
                            }
                        }
                    }
                    Console.Write("Введите ID материала (0 для завершения): ");
                }
            }
            else if (choice == "2")
            {
                ShowCraftKits();
                Console.Write("\nВведите ID набора (0 для завершения): ");
                while (int.TryParse(Console.ReadLine(), out int kitId) && kitId != 0)
                {
                    var kit = manager.GetAllKits().FirstOrDefault(k => k.Id == kitId);
                    if (kit != null)
                    {
                        if (kit.IsKitAvailable())
                        {
                            // 5. Добавить выбранные позиции в проект
                            if (selectedProject != null)
                            {
                                customer.AddKitToProject(selectedProject, kit, selectedProject.Name);
                            }
                            
                            // 6. Рассчитать стоимость
                            decimal cost = kit.CalculateKitPrice();
                            totalCost += cost;
                            Console.WriteLine($"Добавлен набор: {kit.Name} = {cost} руб.");
                            
                            // Продать материалы из набора
                            foreach (var item in kit.GetMaterials())
                            {
                                item.Key.Sell(item.Value);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Набор недоступен. Недостаточно материалов:");
                            foreach (var missing in kit.GetMissingMaterials())
                            {
                                Console.WriteLine($"  - {missing}");
                            }
                        }
                    }
                    Console.Write("Введите ID набора (0 для завершения): ");
                }
            }
            
            // 7. Зафиксировать продажу через manager.RecordSale()
            if (totalCost > 0)
            {
                manager.RecordSale(totalCost);
                Console.WriteLine($"\nИтого к оплате: {totalCost} руб.");
                Console.WriteLine("Заказ оформлен!");
            }
        }
        
        // TODO 3: Мастер-классы и рекомендации
        public void ShowWorkshopsAndTips()
        {
            Console.WriteLine("=== МАСТЕР-КЛАССЫ И РЕКОМЕНДАЦИИ ===");
            
            // Вывести список предстоящих мастер-классов
            Console.WriteLine("\n--- ПРЕДСТОЯЩИЕ МАСТЕР-КЛАССЫ ---");
            Console.WriteLine("1. Шитье для начинающих - 15.02.2026, 18:00");
            Console.WriteLine("2. Вязание спицами - 20.02.2026, 19:00");
            Console.WriteLine("3. Вышивка крестиком - 25.02.2026, 17:00");
            Console.WriteLine("4. Рисование акварелью - 28.02.2026, 18:30");
            
            // Для каждого вида творчества дать рекомендации по материалам
            Console.WriteLine("\n--- РЕКОМЕНДАЦИИ ПО МАТЕРИАЛАМ ---");
            var craftTypes = new[] { "шитье", "вязание", "вышивка", "рисование", "лепка" };
            foreach (var craftType in craftTypes)
            {
                var materials = manager.FindMaterialsByCraftType(craftType);
                if (materials.Count > 0)
                {
                    Console.WriteLine($"\n{craftType.ToUpper()}:");
                    foreach (var material in materials)
                    {
                        Console.WriteLine($"  - {material.Name}: рекомендуется для начинающих");
                    }
                }
            }
            
            // Показать проекты для разных уровней сложности
            Console.WriteLine("\n--- ПРОЕКТЫ ПО УРОВНЯМ СЛОЖНОСТИ ---");
            var kitsByDifficulty = manager.GetAllKits().GroupBy(k => k.Difficulty);
            foreach (var group in kitsByDifficulty)
            {
                Console.WriteLine($"\n{group.Key.ToUpper()}:");
                foreach (var kit in group)
                {
                    Console.WriteLine($"  - {kit.Name} ({kit.AgeRecommendation}, {kit.EstimatedTime} часов)");
                }
            }
            
            // Дать советы по экономии материалов
            Console.WriteLine("\n--- СОВЕТЫ ПО ЭКОНОМИИ ---");
            Console.WriteLine("1. Покупайте материалы оптом - скидка до 15%");
            Console.WriteLine("2. Используйте остатки для небольших проектов");
            Console.WriteLine("3. Присоединяйтесь к мастер-классам - специальные цены на материалы");
            Console.WriteLine("4. Следите за акциями на наборы для творчества");
        }
        
        // TODO 3: Показать статистику мастерской
        public void ShowWorkshopStats()
        {
            Console.WriteLine("=== СТАТИСТИКА МАСТЕРСКОЙ ===");
            
            // Вывести общую выручку через manager.GetTotalRevenue()
            Console.WriteLine($"\nОбщая выручка: {manager.GetTotalRevenue()} руб.");
            
            // Вывести количество зарегистрированных клиентов
            var customers = manager.GetAllCustomers();
            Console.WriteLine($"Зарегистрированных клиентов: {customers.Count}");
            
            // Вывести самые популярные виды творчества
            var popularCraftTypes = manager.GetPopularCraftTypes();
            if (popularCraftTypes.Count > 0)
            {
                Console.WriteLine("\nПопулярные виды творчества:");
                foreach (var item in popularCraftTypes.OrderByDescending(x => x.Value))
                {
                    Console.WriteLine($"  {item.Key}: {item.Value} клиентов");
                }
            }
            
            // Вывести самые продаваемые материалы (по остатку - меньше осталось, значит больше продали)
            var materials = manager.GetAllMaterials().OrderBy(m => m.StockQuantity).Take(5);
            Console.WriteLine("\nСамые продаваемые материалы:");
            foreach (var material in materials)
            {
                Console.WriteLine($"  {material.Name} - остаток: {material.StockQuantity} {material.Unit}");
            }
            
            // Вывести материалы с низким остатком на складе
            var lowStockMaterials = manager.GetLowStockMaterials(10);
            if (lowStockMaterials.Count > 0)
            {
                Console.WriteLine("\nМатериалы с низким остатком (≤10):");
                foreach (var material in lowStockMaterials)
                {
                    Console.WriteLine($"  {material.Name} - остаток: {material.StockQuantity} {material.Unit}");
                }
            }
        }
        
        // Готовый метод - главное меню
        public void ShowMainMenu()
        {
            bool running = true;
            
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== МАГАЗИН РУКОДЕЛИЯ 'МАСТЕРСКАЯ' ===");
                Console.WriteLine("1. Каталог материалов");
                Console.WriteLine("2. Наборы для творчества");
                Console.WriteLine("3. Консультация по проекту");
                Console.WriteLine("4. Оформить заказ");
                Console.WriteLine("5. Мастер-классы и советы");
                Console.WriteLine("6. Статистика мастерской");
                Console.WriteLine("7. Поиск клиента");
                Console.WriteLine("8. Выход");
                Console.Write("Выберите: ");
                
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        ShowMaterialsByCategory();
                        break;
                    case "2":
                        ShowCraftKits();
                        break;
                    case "3":
                        ProvideProjectConsultation();
                        break;
                    case "4":
                        ProcessOrder();
                        break;
                    case "5":
                        ShowWorkshopsAndTips();
                        break;
                    case "6":
                        ShowWorkshopStats();
                        break;
                    case "7":
                        SearchCustomer();
                        break;
                    case "8":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
                
                if (running)
                {
                    Console.WriteLine("\nНажмите Enter...");
                    Console.ReadLine();
                }
            }
        }
        
        // Метод поиска клиента
        private void SearchCustomer()
        {
            Console.WriteLine("Поиск клиента:");
            Console.WriteLine("1. По email");
            Console.WriteLine("2. По телефону");
            Console.Write("Выберите способ: ");
            
            string choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.Write("Введите email: ");
                string email = Console.ReadLine();
                Customer customer = manager.FindCustomerByEmail(email);
                if (customer != null) customer.ShowCustomerInfo();
                else Console.WriteLine("Клиент не найден");
            }
            else if (choice == "2")
            {
                Console.Write("Введите телефон: ");
                string phone = Console.ReadLine();
                Customer customer = manager.FindCustomerByPhone(phone);
                if (customer != null) customer.ShowCustomerInfo();
                else Console.WriteLine("Клиент не найден");
            }
        }
    }
}