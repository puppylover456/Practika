using System.Windows;
using System.Windows.Controls;
using CraftStore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Globalization;

namespace CraftStore.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly WorkshopManager _manager = new();

    private List<CraftMaterial> _allMaterials = new();
    private List<KitRow> _allKits = new();

    // Оформление заказа
    private Customer? _orderCustomer;
    private Customer.Project? _orderProject;
    private readonly ObservableCollection<CartEntry> _orderCart = new();

    public MainWindow()
    {
        InitializeComponent();
        StoreDataSeeder.Seed(_manager);
        Loaded += (_, _) => RefreshAll();
    }

    private void RefreshAll()
    {
        _allMaterials = _manager.GetAllMaterials().ToList();
        _allKits = _manager.GetAllKits()
            .Select(k => new KitRow(k))
            .OrderBy(k => k.Difficulty)
            .ThenBy(k => k.Name)
            .ToList();

        // Фильтры материалов
        var craftTypes = _allMaterials.Select(m => m.CraftType).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().OrderBy(s => s).ToList();
        MaterialsCraftTypeCombo.ItemsSource = new[] { "Все" }.Concat(craftTypes).ToList();
        if (MaterialsCraftTypeCombo.SelectedIndex < 0) MaterialsCraftTypeCombo.SelectedIndex = 0;

        // Фильтры наборов
        var difficulties = _allKits.Select(k => k.Difficulty).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().OrderBy(s => s).ToList();
        KitsDifficultyCombo.ItemsSource = new[] { "Все" }.Concat(difficulties).ToList();
        if (KitsDifficultyCombo.SelectedIndex < 0) KitsDifficultyCombo.SelectedIndex = 0;

        var kitCraftTypes = _allKits.Select(k => k.CraftType).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().OrderBy(s => s).ToList();
        KitsCraftTypeCombo.ItemsSource = new[] { "Все" }.Concat(kitCraftTypes).ToList();
        if (KitsCraftTypeCombo.SelectedIndex < 0) KitsCraftTypeCombo.SelectedIndex = 0;

        MaterialsApplyFilter();
        KitsApplyFilter();

        CustomersGrid.ItemsSource = _manager.GetAllCustomers().ToList();
        RefreshStats();
    }

    private void MaterialsApplyFilter()
    {
        string craftType = MaterialsCraftTypeCombo.SelectedItem as string ?? "Все";
        string q = (MaterialsSearchBox.Text ?? "").Trim();

        IEnumerable<CraftMaterial> filtered = _allMaterials;
        if (!string.Equals(craftType, "Все", StringComparison.OrdinalIgnoreCase))
        {
            filtered = filtered.Where(m => string.Equals(m.CraftType, craftType, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(q))
        {
            filtered = filtered.Where(m =>
                (m.Name ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) ||
                (m.MaterialType ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) ||
                (m.Color ?? "").Contains(q, StringComparison.OrdinalIgnoreCase));
        }

        MaterialsGrid.ItemsSource = filtered.OrderBy(m => m.CraftType).ThenBy(m => m.Name).ToList();
    }

    private void KitsApplyFilter()
    {
        string difficulty = KitsDifficultyCombo.SelectedItem as string ?? "Все";
        string craftType = KitsCraftTypeCombo.SelectedItem as string ?? "Все";

        IEnumerable<KitRow> filtered = _allKits;
        if (!string.Equals(difficulty, "Все", StringComparison.OrdinalIgnoreCase))
        {
            filtered = filtered.Where(k => string.Equals(k.Difficulty, difficulty, StringComparison.OrdinalIgnoreCase));
        }
        if (!string.Equals(craftType, "Все", StringComparison.OrdinalIgnoreCase))
        {
            filtered = filtered.Where(k => string.Equals(k.CraftType, craftType, StringComparison.OrdinalIgnoreCase));
        }

        KitsGrid.ItemsSource = filtered.ToList();

        if (KitsGrid.SelectedItem is not KitRow selected || !filtered.Contains(selected))
        {
            KitsGrid.SelectedItem = null;
            ClearKitDetails();
        }
    }

    private void ClearKitDetails()
    {
        KitDetailsTitle.Text = "";
        KitDetailsMeta.Text = "";
        KitDetailsPrice.Text = "";
        KitMaterialsList.ItemsSource = null;
        KitToolsList.ItemsSource = null;
    }

    private void RefreshStats()
    {
        RevenueText.Text = $"{_manager.GetTotalRevenue()} руб.";
        CustomersCountText.Text = $"{_manager.GetAllCustomers().Count}";

        var popular = _manager.GetPopularCraftTypes()
            .Select(kvp => new PopularCraftRow { CraftType = kvp.Key, Count = kvp.Value })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.CraftType)
            .ToList();
        PopularCraftsList.ItemsSource = popular;

        LowStockGrid.ItemsSource = _manager.GetLowStockMaterials(10)
            .OrderBy(m => m.StockQuantity)
            .ThenBy(m => m.Name)
            .ToList();
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e) => RefreshAll();

    private void MaterialsFilter_Changed(object sender, RoutedEventArgs e) => MaterialsApplyFilter();

    private void MaterialsReset_Click(object sender, RoutedEventArgs e)
    {
        MaterialsSearchBox.Text = "";
        MaterialsCraftTypeCombo.SelectedIndex = 0;
        MaterialsApplyFilter();
    }

    private void KitsFilter_Changed(object sender, RoutedEventArgs e) => KitsApplyFilter();

    private void KitsReset_Click(object sender, RoutedEventArgs e)
    {
        KitsDifficultyCombo.SelectedIndex = 0;
        KitsCraftTypeCombo.SelectedIndex = 0;
        KitsApplyFilter();
    }

    private void KitsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (KitsGrid.SelectedItem is not KitRow row)
        {
            ClearKitDetails();
            return;
        }

        var kit = row.Kit;
        KitDetailsTitle.Text = kit.Name;
        KitDetailsMeta.Text = $"{kit.CraftType}, {kit.Difficulty}, возраст: {kit.AgeRecommendation}, время: {kit.EstimatedTime} ч";
        KitDetailsPrice.Text = $"Стоимость: {row.Price} руб. | Доступность: {(row.IsAvailable ? "В наличии" : "Недостаточно материалов")}";

        KitMaterialsList.ItemsSource = kit.GetMaterials()
            .Select(item => $"{item.Key.Name} x{item.Value} {item.Key.Unit}")
            .ToList();

        var tools = kit.GetRequiredTools();
        KitToolsList.ItemsSource = tools.Count == 0
            ? new List<string> { kit.IncludesTools ? "Инструменты включены в набор" : "Инструменты не указаны" }
            : tools;
    }

    private void AddMaterial_Click(object sender, RoutedEventArgs e)
    {
        AddMaterialStatusText.Text = "";

        string name = (NewMaterialNameBox.Text ?? "").Trim();
        string color = (NewMaterialColorBox.Text ?? "").Trim();
        string materialType = (NewMaterialTypeBox.Text ?? "").Trim();
        string craftType = (NewMaterialCraftTypeBox.Text ?? "").Trim();
        string unit = (NewMaterialUnitBox.Text ?? "").Trim();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(materialType) ||
            string.IsNullOrWhiteSpace(craftType) || string.IsNullOrWhiteSpace(unit))
        {
            AddMaterialStatusText.Text = "Заполните название, тип материала, вид творчества и ед. измерения.";
            return;
        }

        if (!decimal.TryParse(NewMaterialPriceBox.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal price) || price < 0)
        {
            AddMaterialStatusText.Text = "Введите корректную цену.";
            return;
        }

        if (!int.TryParse(NewMaterialStockBox.Text, out int stock) || stock < 0)
        {
            AddMaterialStatusText.Text = "Введите корректный остаток (целое число).";
            return;
        }

        int id = GetNextMaterialId();
        string idText = (NewMaterialIdBox.Text ?? "").Trim();
        if (!string.IsNullOrWhiteSpace(idText))
        {
            if (!int.TryParse(idText, out id) || id <= 0)
            {
                AddMaterialStatusText.Text = "ID должен быть положительным числом.";
                return;
            }
            if (_manager.GetAllMaterials().Any(m => m.Id == id))
            {
                AddMaterialStatusText.Text = "Материал с таким ID уже существует.";
                return;
            }
        }

        var material = new CraftMaterial(id, name, color, materialType, price, stock, unit, craftType);
        _manager.AddMaterial(material);
        RefreshAll();

        AddMaterialStatusText.Text = $"Материал добавлен: {material.Name} (ID {material.Id})";
        NewMaterialIdBox.Text = "";
        NewMaterialNameBox.Text = "";
        NewMaterialColorBox.Text = "";
        NewMaterialTypeBox.Text = "";
        NewMaterialCraftTypeBox.Text = "";
        NewMaterialPriceBox.Text = "";
        NewMaterialStockBox.Text = "";
        NewMaterialUnitBox.Text = "";
    }

    private void AddKit_Click(object sender, RoutedEventArgs e)
    {
        AddKitStatusText.Text = "";

        string name = (NewKitNameBox.Text ?? "").Trim();
        string description = (NewKitDescriptionBox.Text ?? "").Trim();
        string craftType = (NewKitCraftTypeBox.Text ?? "").Trim();
        string difficulty = (NewKitDifficultyBox.Text ?? "").Trim();
        string age = (NewKitAgeBox.Text ?? "").Trim();
        bool includesTools = NewKitIncludesToolsBox.IsChecked == true;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) ||
            string.IsNullOrWhiteSpace(craftType) || string.IsNullOrWhiteSpace(difficulty) ||
            string.IsNullOrWhiteSpace(age))
        {
            AddKitStatusText.Text = "Заполните название, описание, вид творчества, сложность и возраст.";
            return;
        }

        if (!int.TryParse(NewKitTimeBox.Text, out int time) || time <= 0)
        {
            AddKitStatusText.Text = "Введите корректное время (часы).";
            return;
        }

        int id = GetNextKitId();
        string idText = (NewKitIdBox.Text ?? "").Trim();
        if (!string.IsNullOrWhiteSpace(idText))
        {
            if (!int.TryParse(idText, out id) || id <= 0)
            {
                AddKitStatusText.Text = "ID должен быть положительным числом.";
                return;
            }
            if (_manager.GetAllKits().Any(k => k.Id == id))
            {
                AddKitStatusText.Text = "Набор с таким ID уже существует.";
                return;
            }
        }

        var kit = new CraftKit(id, name, description, craftType, difficulty, time, includesTools, age);

        string materialsText = (NewKitMaterialsBox.Text ?? "").Trim();
        if (!string.IsNullOrWhiteSpace(materialsText))
        {
            var tokens = materialsText.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var token in tokens)
            {
                var part = token.Trim();
                if (string.IsNullOrWhiteSpace(part)) continue;

                int materialId;
                int qty = 1;
                string[] pair = part.Split(new[] { ':', 'x', 'X' }, StringSplitOptions.RemoveEmptyEntries);
                if (pair.Length >= 1)
                {
                    if (!int.TryParse(pair[0].Trim(), out materialId))
                    {
                        AddKitStatusText.Text = $"Некорректный ID материала: {part}";
                        return;
                    }
                }
                else
                {
                    AddKitStatusText.Text = $"Некорректный формат материала: {part}";
                    return;
                }

                if (pair.Length >= 2)
                {
                    if (!int.TryParse(pair[1].Trim(), out qty) || qty <= 0)
                    {
                        AddKitStatusText.Text = $"Некорректное количество: {part}";
                        return;
                    }
                }

                var material = _manager.GetAllMaterials().FirstOrDefault(m => m.Id == materialId);
                if (material == null)
                {
                    AddKitStatusText.Text = $"Материал с ID {materialId} не найден.";
                    return;
                }
                kit.AddMaterial(material, qty);
            }
        }

        string toolsText = (NewKitToolsBox.Text ?? "").Trim();
        if (!string.IsNullOrWhiteSpace(toolsText))
        {
            var tools = toolsText.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrWhiteSpace(t));
            foreach (var tool in tools)
            {
                kit.AddRequiredTool(tool);
            }
        }

        _manager.AddKit(kit);
        RefreshAll();

        AddKitStatusText.Text = $"Набор добавлен: {kit.Name} (ID {kit.Id})";
        NewKitIdBox.Text = "";
        NewKitNameBox.Text = "";
        NewKitDescriptionBox.Text = "";
        NewKitCraftTypeBox.Text = "";
        NewKitDifficultyBox.Text = "";
        NewKitAgeBox.Text = "12+";
        NewKitTimeBox.Text = "6";
        NewKitIncludesToolsBox.IsChecked = false;
        NewKitMaterialsBox.Text = "";
        NewKitToolsBox.Text = "";
    }

    private int GetNextMaterialId()
    {
        return _manager.GetAllMaterials().Count == 0 ? 1 : _manager.GetAllMaterials().Max(m => m.Id) + 1;
    }

    private int GetNextKitId()
    {
        return _manager.GetAllKits().Count == 0 ? 1 : _manager.GetAllKits().Max(k => k.Id) + 1;
    }

    private void RegisterCustomer_Click(object sender, RoutedEventArgs e)
    {
        string fullName = (CustomerNameBox.Text ?? "").Trim();
        string phone = (CustomerPhoneBox.Text ?? "").Trim();
        string email = (CustomerEmailBox.Text ?? "").Trim();
        string skillLevel = (CustomerSkillCombo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "новичок";

        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(email))
        {
            CustomerActionStatus.Text = "Заполните ФИО, телефон и email.";
            return;
        }

        if (_manager.FindCustomerByEmail(email) != null)
        {
            CustomerActionStatus.Text = "Клиент с таким email уже существует.";
            return;
        }

        var customer = _manager.RegisterCustomer(fullName, phone, email, skillLevel);
        CustomerActionStatus.Text = $"Клиент зарегистрирован: {customer.FullName} (ID {customer.Id})";

        CustomersGrid.ItemsSource = _manager.GetAllCustomers().ToList();
        CustomersGrid.SelectedItem = customer;
        RefreshStats();
    }

    private void FindCustomer_Click(object sender, RoutedEventArgs e)
    {
        string search = (CustomerSearchBox.Text ?? "").Trim();
        if (string.IsNullOrWhiteSpace(search))
        {
            CustomerActionStatus.Text = "Введите email или телефон для поиска.";
            return;
        }

        Customer? customer = _manager.FindCustomerByEmail(search) ?? _manager.FindCustomerByPhone(search);
        if (customer == null)
        {
            CustomerActionStatus.Text = "Клиент не найден.";
            return;
        }

        CustomerActionStatus.Text = $"Найден: {customer.FullName} (ID {customer.Id})";
        CustomersGrid.ItemsSource = _manager.GetAllCustomers().ToList();
        CustomersGrid.SelectedItem = customer;
        CustomersGrid.ScrollIntoView(customer);
    }

    private void CustomersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CustomersGrid.SelectedItem is not Customer c)
        {
            CustomerCardText.Text = "Выберите клиента в таблице, чтобы увидеть детали.";
            return;
        }

        var active = c.GetActiveProjects().Count;
        var finished = c.GetAllProjects().Count(p => p.Status == "Завершен");
        string interests = c.Interests.Count == 0 ? "—" : string.Join(", ", c.Interests);

        CustomerCardText.Text =
            $"ФИО: {c.FullName}\n" +
            $"Телефон: {c.Phone}\n" +
            $"Email: {c.Email}\n" +
            $"Навыки: {c.SkillLevel}\n" +
            $"Интересы: {interests}\n" +
            $"Активных проектов: {active}\n" +
            $"Завершённых проектов: {finished}";
    }

    private void MainTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (MainTabs.SelectedItem is not TabItem tab) return;
        string header = tab.Header?.ToString() ?? "";
        if (header == "Статистика") RefreshStats();
        else if (header == "Оформить заказ") RefreshOrderTab();
    }

    #region Оформить заказ

    private void RefreshOrderTab()
    {
        if (OrderProjectCombo == null || OrderCartList == null) return;
        OrderProjectCombo.ItemsSource = _orderCustomer?.GetActiveProjects() ?? new List<Customer.Project>();
        if (_orderCustomer != null && OrderProjectCombo.Items.Count > 0 && OrderProjectCombo.SelectedIndex < 0)
            OrderProjectCombo.SelectedIndex = 0;
        OrderCartList.ItemsSource = _orderCart;
        OrderType_Changed(null!, null!);
        UpdateOrderTotal();
    }

    private void OrderFindCustomer_Click(object sender, RoutedEventArgs e)
    {
        string search = (OrderCustomerSearch.Text ?? "").Trim();
        if (string.IsNullOrWhiteSpace(search))
        {
            OrderCustomerInfo.Text = "Введите email или телефон клиента.";
            _orderCustomer = null;
            return;
        }
        _orderCustomer = _manager.FindCustomerByEmail(search) ?? _manager.FindCustomerByPhone(search);
        if (_orderCustomer == null)
        {
            OrderCustomerInfo.Text = "Клиент не найден. Зарегистрируйте клиента на вкладке «Клиенты».";
            return;
        }
        OrderCustomerInfo.Text = $"Клиент: {_orderCustomer.FullName} (ID {_orderCustomer.Id})";
        OrderProjectCombo.ItemsSource = _orderCustomer.GetActiveProjects();
        if (OrderProjectCombo.Items.Count > 0) OrderProjectCombo.SelectedIndex = 0;
    }

    private void OrderProject_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _orderProject = OrderProjectCombo.SelectedItem as Customer.Project;
    }

    private void OrderCreateProject_Click(object sender, RoutedEventArgs e)
    {
        if (_orderCustomer == null)
        {
            OrderStatusText.Text = "Сначала найдите клиента.";
            return;
        }
        string name = (OrderNewProjectName.Text ?? "").Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            OrderStatusText.Text = "Введите название проекта.";
            return;
        }
        string craftType = (OrderNewProjectCraftType.Text ?? "другое").Trim();
        _orderProject = _orderCustomer.StartProject(name, craftType);
        _orderCustomer.AddInterest(craftType);
        OrderProjectCombo.ItemsSource = _orderCustomer.GetActiveProjects();
        OrderProjectCombo.SelectedItem = _orderProject;
        OrderStatusText.Text = $"Проект «{name}» создан.";
    }

    private void OrderType_Changed(object sender, RoutedEventArgs e)
    {
        if (OrderMaterialPanel == null || OrderKitPanel == null || OrderCatalogGrid == null) return;

        bool isMaterials = OrderTypeMaterials?.IsChecked == true;
        OrderMaterialPanel.Visibility = isMaterials ? Visibility.Visible : Visibility.Collapsed;
        OrderKitPanel.Visibility = isMaterials ? Visibility.Collapsed : Visibility.Visible;
        if (OrderCatalogGrid != null) OrderCatalogGrid.SelectedItem = null;
        if (OrderKitIdBox != null) OrderKitIdBox.Text = "";
        if (OrderMaterialIdBox != null) OrderMaterialIdBox.Text = "";

        var catalog = new List<OrderCatalogRow>();
        if (isMaterials)
        {
            foreach (var m in _allMaterials)
                catalog.Add(new OrderCatalogRow(m.Id, m.Name, m.CraftType ?? m.MaterialType ?? "-", m.Price, $"{m.StockQuantity} {m.Unit}", m, null));
        }
        else
        {
            foreach (var k in _allKits)
                catalog.Add(new OrderCatalogRow(k.Id, k.Name, k.CraftType ?? "-", k.Price, k.IsAvailable ? "В наличии" : "Нет в наличии", null, k.Kit));
        }
        OrderCatalogGrid.ItemsSource = catalog;
    }

    private void OrderAddMaterial_Click(object sender, RoutedEventArgs e)
    {
        if (_orderCustomer == null)
        {
            OrderStatusText.Text = "Сначала найдите клиента.";
            return;
        }
        CraftMaterial? material = null;
        string idText = (OrderMaterialIdBox.Text ?? "").Trim();
        if (!string.IsNullOrWhiteSpace(idText))
        {
            if (!int.TryParse(idText, out int id) || id <= 0)
            {
                OrderStatusText.Text = "Введите корректный ID материала.";
                return;
            }
            material = _manager.GetAllMaterials().FirstOrDefault(m => m.Id == id);
            if (material == null)
            {
                OrderStatusText.Text = "Материал с таким ID не найден.";
                return;
            }
        }
        else if (OrderCatalogGrid.SelectedItem is OrderCatalogRow row && row.Material != null)
        {
            material = row.Material;
        }
        else
        {
            OrderStatusText.Text = "Введите ID материала или выберите материал в таблице.";
            return;
        }

        int qty = 1;
        if (int.TryParse(OrderQuantityBox.Text, out int parsed) && parsed > 0) qty = parsed;
        if (!material.IsInStock(qty))
        {
            OrderStatusText.Text = $"Недостаточно на складе. В наличии: {material.StockQuantity} {material.Unit}.";
            return;
        }
        decimal cost = material.CalculateCost(qty);
        _orderCart.Add(new CartEntry(material, null, qty, cost));

        if (OrderCartList.ItemsSource == null) OrderCartList.ItemsSource = _orderCart;
        UpdateOrderTotal();
        OrderStatusText.Text = $"Добавлено: {material.Name} x{qty} = {cost} руб.";
    }

    private void OrderAddKit_Click(object sender, RoutedEventArgs e)
    {
        if (_orderCustomer == null)
        {
            OrderStatusText.Text = "Сначала найдите клиента.";
            return;
        }
        CraftKit? kit = null;
        if (OrderCatalogGrid.SelectedItem is OrderCatalogRow row && row.Kit != null)
        {
            kit = row.Kit;
        }
        else
        {
            string idText = (OrderKitIdBox.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(idText) || !int.TryParse(idText, out int id) || id <= 0)
            {
                OrderStatusText.Text = "Выберите набор в таблице или введите ID набора.";
                return;
            }
            kit = _manager.GetAllKits().FirstOrDefault(k => k.Id == id);
            if (kit == null)
            {
                OrderStatusText.Text = "Набор с таким ID не найден.";
                return;
            }
        }
        if (!kit.IsKitAvailable())
        {
            OrderStatusText.Text = "Набор недоступен. Недостаточно материалов на складе.";
            return;
        }
        decimal cost = kit.CalculateKitPrice();
        _orderCart.Add(new CartEntry(null, kit, 1, cost));

        if (OrderCartList.ItemsSource == null) OrderCartList.ItemsSource = _orderCart;
        UpdateOrderTotal();
        OrderStatusText.Text = $"Добавлен набор: {kit.Name} = {cost} руб.";
    }

    private void UpdateOrderTotal()
    {
        decimal total = _orderCart.Sum(c => c.Total);
        if (OrderTotalText != null) OrderTotalText.Text = $"{total} руб.";
        if (OrderCartList != null && OrderCartList.ItemsSource == null) OrderCartList.ItemsSource = _orderCart;
    }

    private void OrderSubmit_Click(object sender, RoutedEventArgs e)
    {
        if (_orderCustomer == null)
        {
            OrderStatusText.Text = "Сначала найдите клиента.";
            return;
        }
        if (_orderCart.Count == 0)
        {
            OrderStatusText.Text = "Добавьте материалы или наборы в корзину.";
            return;
        }

        decimal totalCost = 0;
        foreach (var item in _orderCart)
        {
            totalCost += item.Total;
            if (item.Material != null)
            {
                item.Material.Sell(item.Quantity);
                if (_orderProject != null && _orderCustomer != null)
                    _orderCustomer.AddMaterialToProject(_orderProject, item.Material, item.Quantity, _orderProject.Name);
            }
            else if (item.Kit != null)
            {
                foreach (var mat in item.Kit.GetMaterials())
                    mat.Key.Sell(mat.Value);
                if (_orderProject != null && _orderCustomer != null)
                    _orderCustomer.AddKitToProject(_orderProject, item.Kit, _orderProject.Name);
            }
        }
        _manager.RecordSale(totalCost);
        _orderCart.Clear();
        UpdateOrderTotal();
        RefreshAll();
        OrderStatusText.Text = $"Заказ оформлен! Итого к оплате: {totalCost} руб.";
    }

    #endregion

    private sealed class KitRow
    {
        public int Id { get; }
        public string Name { get; }
        public string CraftType { get; }
        public string Difficulty { get; }
        public string AgeRecommendation { get; }
        public int EstimatedTime { get; }
        public decimal Price { get; }
        public bool IsAvailable { get; }
        public CraftKit Kit { get; }

        public KitRow(CraftKit kit)
        {
            Kit = kit;
            Id = kit.Id;
            Name = kit.Name;
            CraftType = kit.CraftType;
            Difficulty = kit.Difficulty;
            AgeRecommendation = kit.AgeRecommendation;
            EstimatedTime = kit.EstimatedTime;
            Price = kit.CalculateKitPrice();
            IsAvailable = kit.IsKitAvailable();
        }
    }

    private sealed class PopularCraftRow
    {
        public string CraftType { get; set; } = "";
        public int Count { get; set; }
    }

    private sealed class OrderCatalogRow
    {
        public int Id { get; }
        public string Name { get; }
        public string ItemType { get; }
        public string Price { get; }
        public string StockInfo { get; }
        public CraftMaterial? Material { get; }
        public CraftKit? Kit { get; }

        public OrderCatalogRow(int id, string name, string itemType, decimal price, string stockInfo, CraftMaterial? material, CraftKit? kit)
        {
            Id = id; Name = name; ItemType = itemType;
            Price = $"{price} руб."; StockInfo = stockInfo;
            Material = material; Kit = kit;
        }
    }

    private sealed class CartEntry
    {
        public string DisplayText { get; }
        public CraftMaterial? Material { get; }
        public CraftKit? Kit { get; }
        public int Quantity { get; }
        public decimal Total { get; }

        public CartEntry(CraftMaterial? material, CraftKit? kit, int quantity, decimal total)
        {
            Material = material; Kit = kit; Quantity = quantity; Total = total;
            if (material != null)
                DisplayText = $"{material.Name} x{quantity} — {total} руб.";
            else if (kit != null)
                DisplayText = $"{kit.Name} — {total} руб.";
            else
                DisplayText = $"? x{quantity} — {total} руб.";
        }
    }
}