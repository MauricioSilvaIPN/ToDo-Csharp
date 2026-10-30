using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Avalonia.Controls;

namespace Infraestructure.SaveJson;

public class ListContent
{
    public string? ListString{ get; set; }
}


public class InfraSaves
{
    string savePath = "SaveFile.json";

    public void SaveData(string inputList)
    {
        var listContent = new ListContent()
        {
            ListString = inputList
        };

        string jsonString = JsonSerializer.Serialize(listContent);

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