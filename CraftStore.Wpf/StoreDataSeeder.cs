using CraftStore;
using System.Linq;

namespace CraftStore.Wpf;

internal static class StoreDataSeeder
{
    public static void Seed(WorkshopManager manager)
    {
        // Материалы
        manager.AddMaterial(new CraftMaterial(1, "Ткань хлопок", "голубой", "ткань", 450, 50, "метр", "шитье"));
        manager.AddMaterial(new CraftMaterial(2, "Пряжа акрил", "белый", "пряжа", 280, 100, "моток", "вязание"));
        manager.AddMaterial(new CraftMaterial(3, "Краски акварель", "набор 12 цветов", "краски", 850, 25, "набор", "рисование"));
        manager.AddMaterial(new CraftMaterial(4, "Глина полимерная", "бежевый", "глина", 320, 40, "пачка", "лепка"));
        manager.AddMaterial(new CraftMaterial(5, "Бисер чешский", "разноцветный", "бисер", 150, 200, "упаковка", "вышивка"));

        // Наборы
        CraftKit embroideryKit = new CraftKit(
            1,
            "Вышивка для начинающих",
            "Набор для вышивки крестиком",
            "вышивка",
            "начальный",
            8,
            true,
            "12+");

        var beadMaterial = manager.GetAllMaterials().FirstOrDefault(m => m.Name.Contains("Бисер"));
        if (beadMaterial != null)
        {
            embroideryKit.AddMaterial(beadMaterial, 2);
        }
        embroideryKit.AddRequiredTool("Иголка для вышивания");
        embroideryKit.AddRequiredTool("Пяльцы");
        manager.AddKit(embroideryKit);

        CraftKit sewingKit = new CraftKit(
            2,
            "Шитье платья",
            "Набор для пошива летнего платья",
            "шитье",
            "средний",
            12,
            false,
            "18+");

        var fabricMaterial = manager.GetAllMaterials().FirstOrDefault(m => m.Name.Contains("Ткань"));
        if (fabricMaterial != null)
        {
            sewingKit.AddMaterial(fabricMaterial, 3);
        }
        sewingKit.AddRequiredTool("Швейная машина");
        sewingKit.AddRequiredTool("Ножницы");
        manager.AddKit(sewingKit);

        CraftKit knittingKit = new CraftKit(
            3,
            "Вязание шарфа",
            "Набор для вязания теплого шарфа",
            "вязание",
            "начальный",
            6,
            true,
            "12+");

        var yarnMaterial = manager.GetAllMaterials().FirstOrDefault(m => m.Name.Contains("Пряжа"));
        if (yarnMaterial != null)
        {
            knittingKit.AddMaterial(yarnMaterial, 3);
        }
        manager.AddKit(knittingKit);
    }
}

