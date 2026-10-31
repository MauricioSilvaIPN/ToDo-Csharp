using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Avalonia.Controls;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Infraestructure.SaveJson;

public class ListContent
{
    public string? ListString{ get; set; }
}


public class InfraSaves
{
    private Dictionary<int, string>? textDict;
    string savePath = "SaveFile.json";


    public void AddNewText(int index, string listText)
    {
        if (textDict == null)
        {
            textDict = new Dictionary<int, string>();
        }

        if (File.Exists(savePath))
        { 
            var fileContent = File.ReadAllText(savePath);
            var value = JsonSerializer.Deserialize<Dictionary<int, string>>(fileContent);
            
            textDict = value;

            if (textDict.ContainsKey(index))
            {
                textDict[index] = listText;    
            }
            else
            {
                textDict.Add(index, listText);
            }   
        }

        SaveData();   
    }

    public void SaveData()
    {
        string jsonString = JsonSerializer.Serialize(textDict);

        File.WriteAllText(savePath, jsonString, Encoding.UTF8);
        Console.WriteLine(File.ReadAllText(savePath, Encoding.UTF8));
    }


    public void LoadData(TextBox inputList)
    {
        if (File.Exists(savePath))
        {
            var fileContent = File.ReadAllText(savePath, Encoding.UTF8);
            ListContent? listContent = JsonSerializer.Deserialize<ListContent>(fileContent);

            if (listContent != null)
            {
                inputList.Text = $"{listContent.ListString}";
            }
        }
        else
        {
            Console.WriteLine("ERRO: ARQUIVO NÃO ENCONTRADO");
            return;
        }
    }
}