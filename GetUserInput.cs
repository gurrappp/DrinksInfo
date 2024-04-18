using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinksInfo
{
    public class GetUserInput
    {
        public void Menu()
        {

            var drinkService = new DrinksService();

            drinkService.GetCategories();

            var category = "";
            while (string.IsNullOrWhiteSpace(category))
            {
                Console.WriteLine("Choose a category");
                category = Console.ReadLine();
            }

            var drinkList = drinkService.GetDrinksInCategory(category);

            var drink = "";
            int? drinkId = 0;
            bool match = false;
            while (!match)
            {
                Console.WriteLine("Choose a drink");
                drink = Console.ReadLine();
                drinkId = drinkList.Where(x => x.strDrink == drink).Select(x => x.idDrink).FirstOrDefault();
                if (drinkId != 0)
                    match = true;

            }

            drinkService.GetDrinkInstructions(drinkId);

            Console.ReadLine();
            

        }
    }
}
