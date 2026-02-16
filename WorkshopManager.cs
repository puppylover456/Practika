using System;
using System.Collections.Generic;
using System.Linq;

namespace CraftStore
{
    public class WorkshopManager
    {
        private List<CraftMaterial> materials = new List<CraftMaterial>();
        private List<CraftKit> kits = new List<CraftKit>();
        private List<Customer> customers = new List<Customer>();
        private decimal totalRevenue = 0;
        private int customerIdCounter = 1;

        public void AddMaterial(CraftMaterial material)
        {
            materials.Add(material);
        }

        public void AddKit(CraftKit kit)
        {
            kits.Add(kit);
        }

        public List<CraftMaterial> GetAllMaterials()
        {
            return materials;
        }

        public List<CraftKit> GetAllKits()
        {
            return kits;
        }

        public List<CraftMaterial> FindMaterialsByCraftType(string craftType)
        {
            return materials.Where(m => m.CraftType == craftType).ToList();
        }

        public List<CraftKit> RecommendKitsForCustomer(Customer customer, string craftType = null)
        {
            var recommendedKits = kits.AsQueryable();

            if (!string.IsNullOrEmpty(craftType))
            {
                recommendedKits = recommendedKits.Where(k => k.CraftType == craftType);
            }

            // Фильтруем по уровню сложности в зависимости от навыков клиента
            if (customer.SkillLevel == "новичок")
            {
                recommendedKits = recommendedKits.Where(k => k.Difficulty == "начальный");
            }
            else if (customer.SkillLevel == "любитель")
            {
                recommendedKits = recommendedKits.Where(k => k.Difficulty == "начальный" || k.Difficulty == "средний");
            }

            return recommendedKits.ToList();
        }

        public void RecordSale(decimal amount)
        {
            totalRevenue += amount;
        }

        public decimal GetTotalRevenue()
        {
            return totalRevenue;
        }

        public Customer FindCustomerByEmail(string email)
        {
            return customers.FirstOrDefault(c => c.Email == email);
        }

        public Customer FindCustomerByPhone(string phone)
        {
            return customers.FirstOrDefault(c => c.Phone == phone);
        }

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

        public List<Customer> GetAllCustomers()
        {
            return customers;
        }

        public List<CraftMaterial> GetLowStockMaterials(int threshold = 10)
        {
            return materials.Where(m => m.StockQuantity <= threshold).ToList();
        }

        public Dictionary<string, int> GetPopularCraftTypes()
        {
            var craftTypeCounts = new Dictionary<string, int>();
            foreach (var customer in customers)
            {
                foreach (var interest in customer.Interests)
                {
                    if (craftTypeCounts.ContainsKey(interest))
                        craftTypeCounts[interest]++;
                    else
                        craftTypeCounts[interest] = 1;
                }
            }
            return craftTypeCounts;
        }
    }
}
