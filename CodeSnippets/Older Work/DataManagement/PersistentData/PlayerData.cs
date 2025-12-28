using System;
using System.Collections.Generic;

public class PlayerData
{
    [Serializable]
    public class City
    {
        public string cityName;
        public int id;
        public bool isCompleted;
        public int numOfStars;
    }

    private int cityVariations = 2;
    private string[] nameArray = new string[4] { "London", "Cairo", "Kyoto", "Rio" };

    public List<City> cityList = new List<City>();
    public string playerName; // Add player name field
    public Dictionary<string, string> keybinds = new Dictionary<string, string>(); // Add keybinds dictionary

    public PlayerData(string playerName)
    {
        this.playerName = playerName;

        // Initialize keybinds (example)
        keybinds.Add("MoveForward", "W");
        keybinds.Add("MoveBackward", "S");
        keybinds.Add("Jump", "Space");

        // Populate cityList
        for (int i = 0; i < nameArray.Length; i++)
        {
            for (int j = 0; j < cityVariations; j++)
            {
                City city = new City();

                int id = j + 1;

                city.cityName = nameArray[i];
                city.id = id;
                city.isCompleted = false;
                city.numOfStars = 0;

                cityList.Add(city);
            }
        }
    }

    public override string ToString()
    {
        return $"Player: {playerName}\nCity: {cityList[0].cityName}"; // Modify this line to display relevant information
    }
}
