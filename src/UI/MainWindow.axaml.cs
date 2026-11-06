using System;
using Avalonia.Controls;
using Infraestructure.SaveJson;
using Avalonia.Interactivity;
using Avalonia.Media;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace ToDo_csharp;

public partial class MainWindow : Window
{
    private CancellationTokenSource? _cts;

    private int rowIndex = 0;
    private int panelIndex = -1;
    private string? checkBtnStatus;

    private string checkEIconPath = "M17 5H7c-1.1 0-2 .9-2 2v10c0 1.1.9 2 2 2h10c1.1 0 2-.9 2-2V7c0-1.1-.9-2-2-2M7 17V7h10v10z";
    private string checkVIconPath = "m11 12.59-1.29-1.3-1.42 1.42 2.71 2.7 4.71-4.7-1.42-1.42z";
    private StreamGeometry trashIcon = StreamGeometry.Parse("M17 6V4c0-1.1-.9-2-2-2H9c-1.1 0-2 .9-2 2v2H2v2h2v12c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V8h2V6zM9 4h6v2H9zM6 20V8h12v12z");
    private StreamGeometry? checkIcon;
    private StreamGeometry? checkVerifyIcon;

    InfraSaves dataJson = new InfraSaves();
    listCard newListCard = new listCard();


    public MainWindow()
    {
        InitializeComponent();

        checkIcon = StreamGeometry.Parse(checkEIconPath);
        checkVerifyIcon = StreamGeometry.Parse(checkVIconPath);

        dataJson.LoadData(ContainerListas, trashIcon, TaskCompleted, DeleteTask, OnTextTyped);
        dataJson.InitializeData(ref rowIndex, ref panelIndex);
    }

#region CREATING ELEMENTS
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
            Name = "Listas",
            Tag = panelIndex
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

        checkBtnStatus = checkEIconPath;

        checkBtn.Click += TaskCompleted;
        trashBtn.Click += DeleteTask;

        textInput.TextChanged += OnTextTyped;


        ContainerListas.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

        Grid.SetRow(newPanel, rowIndex);

        ContainerListas.Children.Add(newPanel);

        newPanel.Children.Add(checkBtn);
        newPanel.Children.Add(textInput);
        newPanel.Children.Add(trashBtn);

        rowIndex++;
        panelIndex++;
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
                    checkBtnStatus = checkVIconPath;
                }

                else
                {
                    icon.Data = checkIcon;
                    checkBtnStatus = checkEIconPath;
                }                
                newListCard.contentCheckIcon = checkBtnStatus;
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
                dataJson.DeleteCurrentData(panelRef.Tag);
                ContainerListas.Children.Remove(panelRef);
                
                
                rowIndex--;
                panelIndex--;
            }
        }
    }
#endregion

#region SAVE&LOAD
    public void OnTextTyped(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox txtBox)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            Task.Delay(500, token).ContinueWith(t =>
            {
            if (t.IsCanceled) return;

            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                var stackParent = txtBox.Parent;

                if (stackParent is StackPanel myPanel)
                {
                    newListCard = new listCard
                    {
                        listText = txtBox.Text,
                        rowIndex = rowIndex,
                        panelID = panelIndex,
                        contentCheckIcon = checkBtnStatus
                    };

                    dataJson.AddNewData(panelIndex, newListCard);
                }
            });
            }, token);
        }
    }
#endregion
}