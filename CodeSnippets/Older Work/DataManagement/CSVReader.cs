using UnityEngine;
using System;

//This script is used to extract data from the CSVfile set it into usable arrays
//The data used is set on a time base of when the bugs are spawned and how many
//The max amount of different prefabs that can be spawned at a given time is 2
public class CSVReader : Singleton<CSVReader>
{
    public TextAsset[] csvFile;
    public NodeData[] nodeDataArray;


    [HideInInspector]
    public int tableSize;

    [System.Serializable]
    public class NodeData
    {
        public int time;
        public string bugType;
        public int amount;
        public int path;
    }

    public void ReadCSV(int index)
    {
        string[] data = csvFile[index].text.Split(new string[] { ";", "\n" }, StringSplitOptions.None);

        int columns = 4;

        tableSize = data.Length / columns - 1;
        nodeDataArray = new NodeData[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            nodeDataArray[i] = new NodeData();
            nodeDataArray[i].time = int.Parse(data[columns * (i + 1)]);
            nodeDataArray[i].bugType = data[columns * (i + 1) + 1]; 
            nodeDataArray[i].amount = int.Parse(data[columns * (i + 1) + 2]);
            nodeDataArray[i].path = int.Parse(data[columns * (i + 1) + 3]);
        }
    }
}