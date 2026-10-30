using System;
using Avalonia.Controls;
using Infraestructure.SaveJson;
using Avalonia.Interactivity;
using Avalonia.Media;
using System.Threading;
using System.Threading.Tasks;

namespace ToDo_csharp;

public partial class MainWindow : Window
{

    private CancellationTokenSource? _cts;
    private int rowIndex = 1;
    
    private StreamGeometry checkIcon = StreamGeometry.Parse("M17 5H7c-1.1 0-2 .9-2 2v10c0 1.1.9 2 2 2h10c1.1 0 2-.9 2-2V7c0-1.1-.9-2-2-2M7 17V7h10v10z");
    private StreamGeometry trashIcon = StreamGeometry.Parse("M17 6V4c0-1.1-.9-2-2-2H9c-1.1 0-2 .9-2 2v2H2v2h2v12c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V8h2V6zM9 4h6v2H9zM6 20V8h12v12z");
    private StreamGeometry checkVerifyIcon = StreamGeometry.Parse("m11 12.59-1.29-1.3-1.42 1.42 2.71 2.7 4.71-4.7-1.42-1.42z");

    public MainWindow()
    {
        InitializeComponent();
        ShowData();
    }

#region CREATING BUTTONS
    public void CreatingNewList(object sender, RoutedEventArgs args)
    {    
        var checkI = new PathIcon
        {
            Data = checkIcon
        };

        var trashI = new PathIcon
        {
            Data = trashIcon
        };

        var newPanel = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            Margin = new Avalonia.Thickness(0, 15, 0, 0),
            Name = "Listas"
        };

        var checkBtn = new Button
        {
            Margin = new Avalonia.Thickness(125, 0, 10, 0),
            Background = Brushes.Transparent,
            Content = checkI
        };

        var textInput = new TextBox
        {
            PlaceholderText="Digite sua tarefa aqui...",
            Width = 450,
            Name = "TextoLista"
        };

        var trashBtn = new Button
        {
            Margin = new Avalonia.Thickness(10, 0, 125, 0),
            Background = Brushes.Transparent,
            Content = trashI
        };

        checkBtn.Click += TaskCompleted;
        trashBtn.Click += DeleteTask;

        textInput.TextChanged += OnTextTyped;


        ContainerListas.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

        Grid.SetRow(newPanel, rowIndex);
        rowIndex++;

        ContainerListas.Children.Add(newPanel);

        newPanel.Children.Add(checkBtn);
        newPanel.Children.Add(textInput);
        newPanel.Children.Add(trashBtn);
    }
#endregion

#region BUTTONS ACTION
    public void TaskCompleted(object? sender, RoutedEventArgs args)
    {
        if (sender is Button check)
        {
            if (check.Content is PathIcon icon)
            {
                if (icon.Data == checkIcon)
                {
                    icon.Data = checkVerifyIcon;       
                }

                else
                {
                    icon.Data = checkIcon;
                }
            }
        }
    }

    public void DeleteTask(object? sender, RoutedEventArgs args)
    {
        if (sender is Button btn)
        {
            var parentRef = btn.Parent;

            if (parentRef is StackPanel panelRef)
            {
                ContainerListas.Children.Remove(panelRef);
            }
            
        }
    }
#endregion

#region SAVE&LOAD
    public void OnTextTyped(object? sender, TextChangedEventArgs e)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        Task.Delay(500, token).ContinueWith(t =>
        {
           if (t.IsCanceled) return;

           Avalonia.Threading.Dispatcher.UIThread.Post(() =>
           {
              var dataJson = new InfraSaves();

               if (TextoLista.Text != null)
               {
                    dataJson.SaveData(TextoLista.Text);   
               }
           });
        }, token);
    }

    public void ShowData()
    {
        var dataJson = new InfraSaves();

        dataJson.LoadData(TextoLista);
    }
#endregion
}