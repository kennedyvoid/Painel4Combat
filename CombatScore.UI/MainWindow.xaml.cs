using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows;
using CombatScore.UI.Models;

namespace CombatScore.UI;

public partial class MainWindow : Window
{
    private PublicDisplayWindow? _publicWindow;
    private MatConfigWindow? _configWindow;
    private FighterRegistrationWindow? _fighterWindow;
    private BracketDisplayWindow? _bracketWindow;
    private MatchState _currentState = new MatchState(1);

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnlyNumbers_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
    }

    private void OpenTatame_Click(object sender, RoutedEventArgs e)
    {
        ClosePanels();

        if (!int.TryParse(MatNumberBox.Text, out int matNumber) || matNumber <= 0)
        {
            MessageBox.Show("Informe um número de tatame válido.");
            return;
        }

        var state = new MatchState(matNumber);
        _currentState = state;

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
        try { _fighterWindow?.Close(); } catch { }
        try { _bracketWindow?.Close(); } catch { }
        _publicWindow = null;
        _configWindow = null;
        _fighterWindow = null;
        _bracketWindow = null;
    }

    private void Fighters_Click(object sender, RoutedEventArgs e)
    {
        if (_fighterWindow is { IsVisible: true })
        {
            _fighterWindow.Activate();
            return;
        }

        _fighterWindow = new FighterRegistrationWindow(_currentState);
        _fighterWindow.Show();
    }

    private void Brackets_Click(object sender, RoutedEventArgs e)
    {
        if (_bracketWindow is { IsVisible: true })
        {
            _bracketWindow.Activate();
            return;
        }

        _bracketWindow = new BracketDisplayWindow();
        _bracketWindow.Show();
    }

}
