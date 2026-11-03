using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Avalonia.Controls;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Infraestructure.SaveJson;


public struct listCard
{
    public string listText { get; set; }
    public int rowIndex { get; set; }
    public string listName { get; set; }
    public object listTag { get; set; }
    public Avalonia.Layout.Orientation listOrientation { get; set; }
    public Avalonia.Layout.HorizontalAlignment listAlignment { get; set; }
    public Avalonia.Thickness listMargin { get; set; }
}



public class InfraSaves
{
    private Dictionary<int, listCard>? listCardDict;
    public Dictionary<int, listCard>? loadCardDict;
    string savePath = "SaveFile.json";

    public void AddNewData(int index, listCard listAllContent)
    {
        if (listCardDict == null)
        {
            listCardDict = new Dictionary<int, listCard>();
        }

        if (File.Exists(savePath))
        { 
            var fileContent = File.ReadAllText(savePath);
            var value = JsonSerializer.Deserialize<Dictionary<int, listCard>>(fileContent);
            
            listCardDict = value;

            if (listCardDict.ContainsKey(index))
            {
                listCardDict[index] = listAllContent;    
            }
            else
            {
                listCardDict.Add(index, listAllContent);
            }   
        }

        SaveData();   
    }

    public void SaveData()
    {
        string jsonString = JsonSerializer.Serialize(listCardDict);

        File.WriteAllText(savePath, jsonString, Encoding.UTF8);
        Console.WriteLine(File.ReadAllText(savePath, Encoding.UTF8));
    }


    public void LoadData(Grid containerRef)
    {
        if (File.Exists(savePath))
        {
            var fileContent = File.ReadAllText(savePath, Encoding.UTF8);
            loadCardDict = JsonSerializer.Deserialize<Dictionary<int, listCard>>(fileContent);
                    
            if (loadCardDict != null)
            {
                for (int i = 0; i < loadCardDict.Count; i++)
                {
                    var stackPanel = new StackPanel()
                    {
                        Orientation = loadCardDict[i].listOrientation,
                        HorizontalAlignment = loadCardDict[i].listAlignment,
                        Margin = loadCardDict[i].listMargin,
                        Name = loadCardDict[i].listName,
                        Tag = loadCardDict[i].listTag
                    };

                    var textInput = new TextBox
                    {
                        Text = loadCardDict[i].listText,
                        Width = 450,
                        Name = "TextoLista"
                    };

                    //textInput.TextChanged += OnTextTyped;

                    containerRef.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

                    Grid.SetRow(stackPanel, loadCardDict[i].rowIndex);

                    containerRef.Children.Add(stackPanel);
                    stackPanel.Children.Add(textInput);
                }
            }
            else
            {
                Console.WriteLine("ERRO: ARQUIVO NÃO ENCONTRADO");
                return;
            }
        }
    }
}