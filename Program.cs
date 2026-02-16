using System;

namespace CraftStore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== МАГАЗИН РУКОДЕЛИЯ 'МАСТЕРСКАЯ' ===\n");
            
            StoreMenu menu = new StoreMenu();
            menu.ShowMainMenu();
            
            Console.WriteLine("\nТворите с удовольствием!");
            Console.ReadKey();
        }
    }
}