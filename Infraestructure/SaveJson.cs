using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Media;
using System.Collections.Generic;
using ToDo_csharp;

namespace Infraestructure.SaveJson;


public struct listCard
{
    public string listText { get; set; }
    public int rowIndex { get; set; }
    public int panelID { get; set; }
    public string contentCheckIcon { get; set; }
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


    public void LoadData(Grid containerRef, StreamGeometry trashIconRef)
    {
        if (File.Exists(savePath))
        {
            var fileContent = File.ReadAllText(savePath, Encoding.UTF8);
            loadCardDict = JsonSerializer.Deserialize<Dictionary<int, listCard>>(fileContent);
                    
            if (loadCardDict != null)
            {
                for (int i = 0; i < loadCardDict.Count; i++)
                {
                    StreamGeometry iconCheck = StreamGeometry.Parse(loadCardDict[i].contentCheckIcon);

                    var stackPanel = new StackPanel()
                    {
                        Orientation = Avalonia.Layout.Orientation.Horizontal,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        Margin = new Avalonia.Thickness(0, 15, 0, 0),
                        Name = "Listas",
                    };

                    var checkI = new PathIcon
                    {
                        Data = iconCheck
                    };

                    var trashI = new PathIcon
                    {
                        Data = trashIconRef
                    };

                    var textInput = new TextBox
                    {
                        Text = loadCardDict[i].listText,
                        Width = 450,
                        Name = "TextoLista"
                    };

                    var checkBtn = new Button
                    {
                        Margin = new Avalonia.Thickness(125, 0, 10, 0),
                        Background = Brushes.Transparent,
                        Content = checkI
                    };

                                
                    var trashBtn = new Button
                    {
                        Margin = new Avalonia.Thickness(10, 0, 125, 0),
                        Background = Brushes.Transparent,
                        Content = trashI
                    };

                    //checkBtn.Click += toDoRef.TaskCompleted;
                    //trashBtn.Click += toDoRef.DeleteTask;

                    //textInput.TextChanged += toDoRef.OnTextTyped;

                    containerRef.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

                    Grid.SetRow(stackPanel, loadCardDict[i].rowIndex);

                    containerRef.Children.Add(stackPanel);
                    
                    stackPanel.Children.Add(checkBtn);
                    stackPanel.Children.Add(textInput);
                    stackPanel.Children.Add(trashBtn);
                }
            }
            else
            {
                Console.WriteLine("ERRO: ARQUIVO NÃO ENCONTRADO");
                return;
            }
        }
    }
    
    public void InitializeData(ref int rowIndexRef, ref int panelIndexRef)
    {
        if (File.Exists(savePath))
        {
            if (loadCardDict != null)
            {
                rowIndexRef = loadCardDict.Count;
                panelIndexRef = loadCardDict.Count -1;   
            }
        }
    }
}