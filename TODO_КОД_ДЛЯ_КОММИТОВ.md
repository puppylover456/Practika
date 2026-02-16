# Руководство по коммитам: 1 TODO = 1 коммит

**Для новичка.** Скопируй код в нужный файл и выполни команду коммита.

**Важно:** Делай коммиты по порядку. Сначала CraftMaterial, потом CraftKit, потом Customer, WorkshopManager и StoreMenu.

---

## CraftMaterial.cs

### TODO 1: Добавить свойство для типа творчества

**Файл:** `CraftMaterial.cs`

**Что добавить:**

1. После `public string Unit { get; set; }` (строка 16) добавь:
```csharp
        public string CraftType { get; set; }
```

2. В конструкторе добавь параметр `string craftType` и в конец конструктора (перед `}`):
```csharp
            CraftType = craftType;
```

**Команда:**
```
git add CraftMaterial.cs
git commit -m "feat(CraftMaterial): TODO 1 - свойство CraftType"
```

---

### TODO 2: Проверка корректности данных (цена)

**Файл:** `CraftMaterial.cs`

**Что заменить:** Строку `Price = price;` в конструкторе замени на:
```csharp
            if (price < 0)
            {
                Price = 10;
            }
            else
            {
                Price = price;
            }
```

**Команда:**
```
git add CraftMaterial.cs
git commit -m "feat(CraftMaterial): TODO 2 - проверка цены"
```

---

### TODO 3: Информативное строковое представление

**Файл:** `CraftMaterial.cs`

**Что заменить:** В методе `ToString()` замени `return Name;` на:
```csharp
            string colorPart = !string.IsNullOrEmpty(Color) ? $" {Color}" : "";
            return $"{Name}{colorPart} ({CraftType}) - {Price} руб/{Unit} ({StockQuantity} {Unit})";
```

**Команда:**
```
git add CraftMaterial.cs
git commit -m "feat(CraftMaterial): TODO 3 - ToString()"
```

---

## CraftKit.cs

### TODO 1: Создание готовых наборов для хобби

**Файл:** `CraftKit.cs`

**Что добавить:** Свойство `AgeRecommendation`, параметр в конструкторе, методы `AddMaterial`, `AddRequiredTool`, `GetMaterials`, `GetRequiredTools` — см. текущий `CraftKit.cs` строки 24–37, 40–63, 114–123.

**Команда:**
```
git add CraftKit.cs
git commit -m "feat(CraftKit): TODO 1 - создание наборов"
```

---

### TODO 2: Расчёт стоимости набора

**Файл:** `CraftKit.cs`

**Код:**
```csharp
        public decimal CalculateKitPrice()
        {
            decimal total = 0;
            foreach (var item in materials)
            {
                total += item.Key.Price * item.Value;
            }
            return total * 1.2m;
        }
```

**Команда:**
```
git add CraftKit.cs
git commit -m "feat(CraftKit): TODO 2 - расчёт стоимости"
```

---

### TODO 3: Проверка комплектности и сложности

**Файл:** `CraftKit.cs`

**Код:**
```csharp
        public bool IsKitAvailable()
        {
            foreach (var item in materials)
            {
                if (!item.Key.IsInStock(item.Value))
                    return false;
            }
            return true;
        }
        
        public List<string> GetMissingMaterials()
        {
            var missing = new List<string>();
            foreach (var item in materials)
            {
                if (!item.Key.IsInStock(item.Value))
                    missing.Add($"{item.Key.Name} (нужно {item.Value}, в наличии {item.Key.StockQuantity})");
            }
            return missing;
        }
        
        public void ShowKitInfo()
        {
            Console.WriteLine($"=== НАБОР: {Name} ===");
            Console.WriteLine($"Тип творчества: {CraftType}");
            Console.WriteLine($"Сложность: {Difficulty}");
            Console.WriteLine($"Время: {EstimatedTime} ч");
            Console.WriteLine($"Возраст: {AgeRecommendation}");
            Console.WriteLine("\nСостав набора:");
            foreach (var item in materials)
                Console.WriteLine($"  {item.Key.Name} x{item.Value} {item.Key.Unit}");
            if (requiredTools.Count > 0 && !IncludesTools)
            {
                Console.WriteLine("\nИнструменты:");
                foreach (var t in requiredTools) Console.WriteLine($"  {t}");
            }
            Console.WriteLine($"\nЦена: {CalculateKitPrice()} руб.");
            Console.WriteLine($"Доступность: {(IsKitAvailable() ? "В наличии" : "Нет")}");
        }
```

**Команда:**
```
git add CraftKit.cs
git commit -m "feat(CraftKit): TODO 3 - проверка комплектности"
```

---

## Customer.cs

### TODO 1: Учёт интересов и навыков клиента

**Файл:** `Customer.cs`

**Код:**
```csharp
        public string SkillLevel { get; set; } = "новичок";
        
        public List<string> Interests { get { return interests; } }
        
        public void AddInterest(string craftType)
        {
            if (!interests.Contains(craftType))
                interests.Add(craftType);
        }
```

Добавь в `ShowCustomerInfo()` строку: `Console.WriteLine($"Навыки: {SkillLevel}");`

**Команда:**
```
git add Customer.cs
git commit -m "feat(Customer): TODO 1 - SkillLevel, Interests"
```

---

### TODO 2: Оформление заказа (проекты, материалы, наборы)

**Файл:** `Customer.cs`

**Код:**
```csharp
        public Project StartProject(string name, string craftType)
        {
            var project = new Project
            {
                Name = name,
                CraftType = craftType,
                StartDate = DateTime.Now,
                Status = "В процессе"
            };
            projects.Add(project);
            return project;
        }
        
        public void AddMaterialToProject(Project project, CraftMaterial material, int quantity, string purpose)
        {
            project.UsedMaterials.Add(new PurchaseItem { Material = material, Quantity = quantity, Purpose = purpose });
        }
        
        public void AddKitToProject(Project project, CraftKit kit, string purpose)
        {
            project.UsedMaterials.Add(new PurchaseItem { Kit = kit, Quantity = 1, Purpose = purpose });
        }
        
        public List<Project> GetActiveProjects()
        {
            return projects.Where(p => p.Status == "В процессе").ToList();
        }
        
        public List<Project> GetAllProjects()
        {
            return projects;
        }
```

**Команда:**
```
git add Customer.cs
git commit -m "feat(Customer): TODO 2 - проекты и оформление заказа"
```

---

### TODO 3: История творческих проектов

**Файл:** `Customer.cs`

**Код:**
```csharp
        public void FinishProject(Project project)
        {
            project.FinishDate = DateTime.Now;
            project.Status = "Завершен";
        }
        
        public void AddToWishList(CraftMaterial material = null, CraftKit kit = null, int priority = 2)
        {
            wishList.Add(new WishListItem
            {
                Material = material,
                Kit = kit,
                AddedDate = DateTime.Now,
                Priority = priority
            });
        }
```

**Команда:**
```
git add Customer.cs
git commit -m "feat(Customer): TODO 3 - история проектов"
```

---

## WorkshopManager.cs

### TODO 1: Регистрация клиентов

**Файл:** `WorkshopManager.cs`

**Код:**
```csharp
        private int customerIdCounter = 1;

        public Customer RegisterCustomer(string fullName, string phone, string email, string skillLevel = "новичок")
        {
            var customer = new Customer
            {
                Id = customerIdCounter++,
                FullName = fullName,
                Phone = phone,
                Email = email,
                SkillLevel = skillLevel
            };
            customers.Add(customer);
            return customer;
        }
```

**Команда:**
```
git add WorkshopManager.cs
git commit -m "feat(WorkshopManager): TODO 1 - регистрация клиентов"
```

---

### TODO 2: Поиск клиента по email или телефону

**Файл:** `WorkshopManager.cs`

**Код:**
```csharp
        public Customer FindCustomerByEmail(string email)
        {
            return customers.FirstOrDefault(c => c.Email == email);
        }

        public Customer FindCustomerByPhone(string phone)
        {
            return customers.FirstOrDefault(c => c.Phone == phone);
        }

        public List<Customer> GetAllCustomers()
        {
            return customers;
        }
```

**Команда:**
```
git add WorkshopManager.cs
git commit -m "feat(WorkshopManager): TODO 2 - поиск клиента"
```

---

### TODO 3: Подбор материалов по проекту

**Файл:** `WorkshopManager.cs`

**Код:**
```csharp
        public List<CraftMaterial> FindMaterialsByCraftType(string craftType)
        {
            return materials.Where(m => m.CraftType == craftType).ToList();
        }

        public List<CraftKit> RecommendKitsForCustomer(Customer customer, string craftType = null)
        {
            var result = kits.AsQueryable();
            if (!string.IsNullOrEmpty(craftType))
                result = result.Where(k => k.CraftType == craftType);
            if (customer.SkillLevel == "новичок")
                result = result.Where(k => k.Difficulty == "начальный");
            else if (customer.SkillLevel == "любитель")
                result = result.Where(k => k.Difficulty == "начальный" || k.Difficulty == "средний");
            return result.ToList();
        }

        public void RecordSale(decimal amount)
        {
            totalRevenue += amount;
        }

        public decimal GetTotalRevenue()
        {
            return totalRevenue;
        }

        public List<CraftMaterial> GetLowStockMaterials(int threshold = 10)
        {
            return materials.Where(m => m.StockQuantity <= threshold).ToList();
        }

        public Dictionary<string, int> GetPopularCraftTypes()
        {
            var d = new Dictionary<string, int>();
            foreach (var c in customers)
                foreach (var i in c.Interests)
                {
                    if (d.ContainsKey(i)) d[i]++;
                    else d[i] = 1;
                }
            return d;
        }
```

**Команда:**
```
git add WorkshopManager.cs
git commit -m "feat(WorkshopManager): TODO 3 - подбор материалов"
```

---

## StoreMenu.cs

### TODO 1: Каталог материалов по видам творчества

**Файл:** `StoreMenu.cs`

Добавь `using System.Linq;` и методы `InitializeStoreData`, `ShowMaterialsByCategory`, `ShowCraftKits` — см. строки 21–108 в текущем `StoreMenu.cs`.

**Команда:**
```
git add StoreMenu.cs
git commit -m "feat(StoreMenu): TODO 1 - каталог материалов"
```

---

### TODO 2: Консультация по подбору материалов

**Файл:** `StoreMenu.cs`

Добавь методы `ProvideProjectConsultation` и `ProcessOrder` — строки 110–331 в текущем `StoreMenu.cs`.

**Команда:**
```
git add StoreMenu.cs
git commit -m "feat(StoreMenu): TODO 2 - консультация и заказ"
```

---

### TODO 3: Мастер-классы и рекомендации

**Файл:** `StoreMenu.cs`

Добавь методы `ShowWorkshopsAndTips`, `ShowWorkshopStats` и `SearchCustomer` — строки 333–510 в текущем `StoreMenu.cs`.

**Команда:**
```
git add StoreMenu.cs
git commit -m "feat(StoreMenu): TODO 3 - мастер-классы и статистика"
```

---

## Порядок коммитов (15 штук)

1. CraftMaterial TODO 1  
2. CraftMaterial TODO 2  
3. CraftMaterial TODO 3  
4. CraftKit TODO 1  
5. CraftKit TODO 2  
6. CraftKit TODO 3  
7. Customer TODO 1  
8. Customer TODO 2  
9. Customer TODO 3  
10. WorkshopManager TODO 1  
11. WorkshopManager TODO 2  
12. WorkshopManager TODO 3  
13. StoreMenu TODO 1  
14. StoreMenu TODO 2  
15. StoreMenu TODO 3  
