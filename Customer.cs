// TODO:
// 1. Реализовать учет интересов и навыков клиента
// 2. Реализовать оформление заказа на материалы и наборы
// 3. Реализовать историю творческих проектов

using System;
using System.Collections.Generic;
using System.Linq;

namespace CraftStore
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        
        // TODO 1: Добавить свойство SkillLevel (уровень навыков: новичок, любитель, профи)
        public string SkillLevel { get; set; } = "новичок";
        
        // TODO 1: Добавить свойство Interests (интересы: список видов творчества)
        public List<string> Interests { get { return interests; } }
        
        private List<string> interests = new List<string>();
        private List<Project> projects = new List<Project>(); // Завершенные проекты
        private List<WishListItem> wishList = new List<WishListItem>(); // Список желаний
        
        public class PurchaseItem
        {
            public CraftMaterial Material { get; set; }
            public CraftKit Kit { get; set; }
            public int Quantity { get; set; }
            public string Purpose { get; set; } // Для какого проекта
        }
        
        public class Project
        {
            public string Name { get; set; }
            public string CraftType { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? FinishDate { get; set; }
            public string Status { get; set; } // В процессе, Завершен, Отложен
            public List<PurchaseItem> UsedMaterials { get; set; } = new List<PurchaseItem>();
        }
        
        public class WishListItem
        {
            public CraftMaterial Material { get; set; }
            public CraftKit Kit { get; set; }
            public DateTime AddedDate { get; set; }
            public int Priority { get; set; } // Приоритет (1-высокий, 3-низкий)
        }
        
        // TODO 2: Создать новый проект
        public Project StartProject(string name, string craftType)
        {
            // Создать новый объект Project
            var project = new Project
            {
                Name = name,
                CraftType = craftType,
                StartDate = DateTime.Now,
                Status = "В процессе"
            };
            // Добавить проект в список projects
            projects.Add(project);
            // Вернуть созданный проект
            return project;
        }
        
        // TODO 2: Добавить материал в проект
        public void AddMaterialToProject(Project project, CraftMaterial material, int quantity, string purpose)
        {
            // Создать PurchaseItem для материала
            var item = new PurchaseItem
            {
                Material = material,
                Quantity = quantity,
                Purpose = purpose
            };
            // Добавить в UsedMaterials проекта
            project.UsedMaterials.Add(item);
        }
        
        // TODO 2: Добавить набор в проект
        public void AddKitToProject(Project project, CraftKit kit, string purpose)
        {
            // Создать PurchaseItem для набора
            var item = new PurchaseItem
            {
                Kit = kit,
                Quantity = 1,
                Purpose = purpose
            };
            // Добавить в UsedMaterials проекта
            project.UsedMaterials.Add(item);
        }
        
        // TODO 3: Завершить проект
        public void FinishProject(Project project)
        {
            // Установить текущую дату как дату завершения
            project.FinishDate = DateTime.Now;
            // Изменить статус на "Завершен"
            project.Status = "Завершен";
        }
        
        // TODO 3: Добавить в список желаний
        public void AddToWishList(CraftMaterial material = null, CraftKit kit = null, int priority = 2)
        {
            // Создать WishListItem
            var item = new WishListItem
            {
                Material = material,
                Kit = kit,
                AddedDate = DateTime.Now,
                Priority = priority
            };
            // Добавить в список wishList
            wishList.Add(item);
        }
        
        // TODO 1: Добавить интерес
        public void AddInterest(string craftType)
        {
            // Добавить вид творчества в список interests если его там еще нет
            if (!interests.Contains(craftType))
            {
                interests.Add(craftType);
            }
        }
        
        // Получить активные проекты
        public List<Project> GetActiveProjects()
        {
            return projects.Where(p => p.Status == "В процессе").ToList();
        }
        
        // Получить все проекты
        public List<Project> GetAllProjects()
        {
            return projects;
        }
        
        // Показать информацию о клиенте
        public void ShowCustomerInfo()
        {
            Console.WriteLine($"Клиент: {FullName}");
            Console.WriteLine($"Телефон: {Phone}");
            Console.WriteLine($"Email: {Email}");
            // TODO 1: Вывести уровень навыков
            Console.WriteLine($"Уровень навыков: {SkillLevel}");
            Console.WriteLine($"Интересы: {string.Join(", ", interests)}");
            Console.WriteLine($"Активных проектов: {projects.Count(p => p.Status == "В процессе")}");
            Console.WriteLine($"Завершенных проектов: {projects.Count(p => p.Status == "Завершен")}");
            Console.WriteLine($"В списке желаний: {wishList.Count} позиций");
        }
    }
}