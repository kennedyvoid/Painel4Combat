using System.Windows;
using System.Windows.Controls;
using CombatScore.UI.Models;

namespace CombatScore.UI;

public partial class MainWindow : Window
{
    private PublicDisplayWindow? _publicWindow;
    private MatConfigWindow? _configWindow;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OpenTatame_Click(object sender, RoutedEventArgs e)
    {
        ClosePanels();

        var selected = MatCombo.SelectedItem as ComboBoxItem;
        int matNumber = int.Parse(selected?.Content?.ToString() ?? "1");
        var state = new MatchState(matNumber);

        _publicWindow = new PublicDisplayWindow(state)
        {
            Title = $"4Combat - Painel Público - Tatame {matNumber:00}"
        };

        _configWindow = new MatConfigWindow(state)
        {
            Title = $"4Combat - Configuração - Tatame {matNumber:00}"
        };

        _publicWindow.Show();
        _configWindow.Show();

        StatusText.Text = $"Painel público e configuração abertos para o Tatame {matNumber:00}.";
    }

    private void ClosePanels_Click(object sender, RoutedEventArgs e)
    {
        ClosePanels();
        StatusText.Text = "Nenhum painel aberto.";
    }

    private void ClosePanels()
    {
        try { _publicWindow?.Close(); } catch { }
        try { _configWindow?.Close(); } catch { }
        _publicWindow = null;
        _configWindow = null;
    }
}
