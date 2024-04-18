using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using DrinksInfo.Models;
using RestSharp;
using Newtonsoft.Json;
using System.Reflection;

namespace DrinksInfo
{
    public class DrinksService
    {
        public void GetCategories()
        {
            var client = new RestClient("https://www.thecocktaildb.com/api/json/v1/1/");
            var request = new RestRequest("list.php?c=list");
            var response = client.ExecuteAsync(request);

            if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var raw = response.Result.Content;
                var serialize = JsonConvert.DeserializeObject<Categories>(raw);

                List<Category> returnedList = serialize.CategoriesList;

                TableVisualisationEngine.ShowTable(returnedList, "CategoriesMenu");
            }
        }

        public List<Drink> GetDrinksInCategory(string category)
        {
            var client = new RestClient("https://www.thecocktaildb.com/api/json/v1/1/");
            var request = new RestRequest($"filter.php?c={category}");
            var response = client.ExecuteAsync(request);
            List<Drink> returnedList = new List<Drink>();
            if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var raw = response.Result.Content;
                var serialize = JsonConvert.DeserializeObject<Drinks>(raw);

                returnedList = serialize.DrinksList;

                TableVisualisationEngine.ShowTable(returnedList, "DrinksMenu");
            }
            return returnedList;
        }

        public void GetDrinkInstructions(int? drinkId)
        {
            var client = new RestClient("https://www.thecocktaildb.com/api/json/v1/1/");
            var request = new RestRequest($"lookup.php?i={drinkId}");
            var response = client.ExecuteAsync(request);
            //List<Drink> returnedList = new List<Drink>();
            if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var raw = response.Result.Content;
                var serialize = JsonConvert.DeserializeObject<DrinkDetailObject>(raw);

                List<DrinkDetail> drinkObjects = serialize.DrinkDetailList;

                DrinkDetail drinkDetail = drinkObjects[0];

                List<object> tempObjectList = new();

                var formattedName = "";

                foreach(PropertyInfo prop in drinkDetail.GetType().GetProperties())
                {
                    if (prop.Name.Substring(0, 3) == "str")
                    {
                        formattedName = prop.Name.Substring(3);
                    }
                    else
                        formattedName = "";

                    if (!string.IsNullOrEmpty(prop.GetValue(drinkDetail)?.ToString()))
                    {
                        tempObjectList.Add(new
                        {
                            Key = formattedName,
                            Value = prop.GetValue(drinkDetail)
                        });
                    }
                }


                TableVisualisationEngine.ShowTable(tempObjectList, "DrinkDetails");
            }
        }
    }

}
