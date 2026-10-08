using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Microsoft.VisualBasic;

namespace ToDo_csharp;

public partial class MainWindow : Window
{

    private int rowIndex = 1;

    public MainWindow()
    {
        InitializeComponent();
    }

#region CREATING BUTTONS
    public void CreatingNewList(object sender, RoutedEventArgs args)
    {    
        var checkIcon = new PathIcon
        {
            Data = StreamGeometry.Parse("M17 5H7c-1.1 0-2 .9-2 2v10c0 1.1.9 2 2 2h10c1.1 0 2-.9 2-2V7c0-1.1-.9-2-2-2M7 17V7h10v10z")
        };

        var trashIcon = new PathIcon
        {
            Data = StreamGeometry.Parse("M17 6V4c0-1.1-.9-2-2-2H9c-1.1 0-2 .9-2 2v2H2v2h2v12c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V8h2V6zM9 4h6v2H9zM6 20V8h12v12z")
        };

        var newPanel = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            Margin = new Avalonia.Thickness(0, 15, 0, 0)
        };

        var checkBtn = new Button
        {
            Margin = new Avalonia.Thickness(125, 0, 10, 0),
            Background = Brushes.Transparent,
            Content = checkIcon
        };

        var textInput = new TextBox
        {
            PlaceholderText="Digite sua tarefa aqui...",
            Width = 450
        };

        var trashBtn = new Button
        {
            Margin = new Avalonia.Thickness(10, 0, 125, 0),
            Background = Brushes.Transparent,
            Content = trashIcon
        };

        checkBtn.Click += TaskCompleted;


        ContainerListas.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

        Grid.SetRow(newPanel, rowIndex);
        rowIndex++;

        ContainerListas.Children.Add(newPanel);

        newPanel.Children.Add(checkBtn);
        newPanel.Children.Add(textInput);
        newPanel.Children.Add(trashBtn);
    }

    public void TaskCompleted(object? sender, RoutedEventArgs args)
    {
        Console.WriteLine("Estou funcionando...");
    }

#endregion

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
}