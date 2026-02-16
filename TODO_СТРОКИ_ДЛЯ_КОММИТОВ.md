# Строки кода для каждого TODO (1 коммит = 1 TODO)

---

## CraftMaterial.cs

### TODO 1: Добавить свойство для типа творчества
| Файл | Строки | Описание |
|------|--------|----------|
| CraftMaterial.cs | 18-19 | Свойство `CraftType` |
| CraftMaterial.cs | 21 | Параметр `craftType` в конструкторе |
| CraftMaterial.cs | 41-43 | Сохранение `CraftType` в конструкторе |

**Коммит:** `feat(CraftMaterial): TODO 1 - свойство CraftType`

---

### TODO 2: Проверка корректности данных (цена, остаток)
| Файл | Строки | Описание |
|------|--------|----------|
| CraftMaterial.cs | 27-37 | Проверка цены: если < 0, то Price = 10 |

**Коммит:** `feat(CraftMaterial): TODO 2 - проверка цены`

---

### TODO 3: Информативное строковое представление
| Файл | Строки | Описание |
|------|--------|----------|
| CraftMaterial.cs | 46-50 | Метод `ToString()` с форматированием |

**Коммит:** `feat(CraftMaterial): TODO 3 - ToString()`

---

## CraftKit.cs

### TODO 1: Реализовать создание готовых наборов для хобби
| Файл | Строки | Описание |
|------|--------|----------|
| CraftKit.cs | 24-25 | Свойство `AgeRecommendation` |
| CraftKit.cs | 36-37 | Сохранение `AgeRecommendation` в конструкторе |
| CraftKit.cs | 40-53 | Метод `AddMaterial()` |
| CraftKit.cs | 55-63 | Метод `AddRequiredTool()` |
| CraftKit.cs | 114-117 | Метод `GetMaterials()` |
| CraftKit.cs | 119-123 | Метод `GetRequiredTools()` |

**Коммит:** `feat(CraftKit): TODO 1 - создание наборов (AgeRecommendation, AddMaterial, AddRequiredTool, GetMaterials, GetRequiredTools)`

---

### TODO 2: Реализовать расчёт стоимости набора
| Файл | Строки | Описание |
|------|--------|----------|
| CraftKit.cs | 65-78 | Метод `CalculateKitPrice()` |

**Коммит:** `feat(CraftKit): TODO 2 - расчёт стоимости набора`

---

### TODO 3: Реализовать проверку комплектности и сложности
| Файл | Строки | Описание |
|------|--------|----------|
| CraftKit.cs | 81-95 | Метод `IsKitAvailable()` |
| CraftKit.cs | 97-112 | Метод `GetMissingMaterials()` |
| CraftKit.cs | 126-153 | Метод `ShowKitInfo()` |

**Коммит:** `feat(CraftKit): TODO 3 - проверка комплектности (IsKitAvailable, GetMissingMaterials, ShowKitInfo)`

---

## Customer.cs

### TODO 1: Учёт интересов и навыков клиента
| Файл | Строки | Описание |
|------|--------|----------|
| Customer.cs | 19-20 | Свойство `SkillLevel` |
| Customer.cs | 22-23 | Свойство `Interests` |
| Customer.cs | 125-132 | Метод `AddInterest()` |
| Customer.cs | 152-153 | Вывод `SkillLevel` в `ShowCustomerInfo()` |

**Коммит:** `feat(Customer): TODO 1 - SkillLevel, Interests, AddInterest`

---

### TODO 2: Оформление заказа на материалы и наборы
| Файл | Строки | Описание |
|------|--------|----------|
| Customer.cs | 55-70 | Метод `StartProject()` |
| Customer.cs | 73-84 | Метод `AddMaterialToProject()` |
| Customer.cs | 87-98 | Метод `AddKitToProject()` |
| Customer.cs | 135-138 | Метод `GetActiveProjects()` |
| Customer.cs | 141-144 | Метод `GetAllProjects()` |

**Коммит:** `feat(Customer): TODO 2 - StartProject, AddMaterialToProject, AddKitToProject, GetActiveProjects, GetAllProjects`

---

### TODO 3: История творческих проектов
| Файл | Строки | Описание |
|------|--------|----------|
| Customer.cs | 101-107 | Метод `FinishProject()` |
| Customer.cs | 110-122 | Метод `AddToWishList()` |

**Коммит:** `feat(Customer): TODO 3 - FinishProject, AddToWishList`

---

## WorkshopManager.cs
*(В файле нет комментариев TODO, но реализованы соответствующие задачи)*

### Логический TODO 1: Регистрация клиентов
| Файл | Строки | Описание |
|------|--------|----------|
| WorkshopManager.cs | 80-94 | Метод `RegisterCustomer()` |

**Коммит:** `feat(WorkshopManager): RegisterCustomer`

---

### Логический TODO 2: Поиск клиента
| Файл | Строки | Описание |
|------|--------|----------|
| WorkshopManager.cs | 70-74 | Метод `FindCustomerByEmail()` |
| WorkshopManager.cs | 76-80 | Метод `FindCustomerByPhone()` |
| WorkshopManager.cs | 94-98 | Метод `GetAllCustomers()` |

**Коммит:** `feat(WorkshopManager): FindCustomerByEmail, FindCustomerByPhone, GetAllCustomers`

---

### Логический TODO 3: Подбор материалов и учёт выручки
| Файл | Строки | Описание |
|------|--------|----------|
| WorkshopManager.cs | 34-38 | Метод `FindMaterialsByCraftType()` |
| WorkshopManager.cs | 39-59 | Метод `RecommendKitsForCustomer()` |
| WorkshopManager.cs | 61-65 | Метод `RecordSale()` |
| WorkshopManager.cs | 66-70 | Метод `GetTotalRevenue()` |
| WorkshopManager.cs | 99-104 | Метод `GetLowStockMaterials()` |
| WorkshopManager.cs | 105-119 | Метод `GetPopularCraftTypes()` |

**Коммит:** `feat(WorkshopManager): FindMaterialsByCraftType, RecommendKitsForCustomer, RecordSale, GetLowStockMaterials, GetPopularCraftTypes`

---

## StoreMenu.cs

### TODO 1: Каталог материалов по видам творчества
| Файл | Строки | Описание |
|------|--------|----------|
| StoreMenu.cs | 21-62 | Метод `InitializeStoreData()` — материалы и наборы |
| StoreMenu.cs | 65-85 | Метод `ShowMaterialsByCategory()` |
| StoreMenu.cs | 87-108 | Метод `ShowCraftKits()` |

**Коммит:** `feat(StoreMenu): TODO 1 - InitializeStoreData, ShowMaterialsByCategory, ShowCraftKits`

---

### TODO 2: Консультация и оформление заказа
| Файл | Строки | Описание |
|------|--------|----------|
| StoreMenu.cs | 110-184 | Метод `ProvideProjectConsultation()` |
| StoreMenu.cs | 186-331 | Метод `ProcessOrder()` |

**Коммит:** `feat(StoreMenu): TODO 2 - ProvideProjectConsultation, ProcessOrder`

---

### TODO 3: Мастер-классы и рекомендации
| Файл | Строки | Описание |
|------|--------|----------|
| StoreMenu.cs | 333-379 | Метод `ShowWorkshopsAndTips()` |
| StoreMenu.cs | 381-421 | Метод `ShowWorkshopStats()` |
| StoreMenu.cs | 484-510 | Метод `SearchCustomer()` |

**Коммит:** `feat(StoreMenu): TODO 3 - ShowWorkshopsAndTips, ShowWorkshopStats, SearchCustomer`

---

## Рекомендуемый порядок коммитов

1. CraftMaterial (TODO 1, 2, 3)
2. CraftKit (TODO 1, 2, 3)
3. Customer (TODO 1, 2, 3)
4. WorkshopManager (TODO 1, 2, 3)
5. StoreMenu (TODO 1, 2, 3)

**Важно:** Один файл может содержать код нескольких TODO. Используй `git add -p <файл>` для выборочного добавления строк, если делаешь коммиты по одному TODO.
