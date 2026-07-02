using System.Windows;
using System.Windows.Controls;
using CombatScore.UI.Models;
using CombatScore.UI.Services;

namespace CombatScore.UI;

public partial class FighterRegistrationWindow : Window
{
    private readonly MatchState _state;
    private int _nextBracketIndex;
    private PublicDisplayWindow? _publicWindow;
    private MatConfigWindow? _configWindow;
    private BracketDisplayWindow? _bracketDisplayWindow;

    public FighterRegistrationWindow(MatchState state)
    {
        InitializeComponent();
        _state = state;

        FightersGrid.ItemsSource = TournamentStore.Fighters;
        BracketsGrid.ItemsSource = TournamentStore.Brackets;
    }

    private void AddFighter_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text))
        {
            MessageBox.Show("Informe o nome do lutador.");
            return;
        }

        var gender = (GenderBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Masculino";
        var belt = (BeltBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Branca";

        TournamentStore.Fighters.Add(new FighterEntry
        {
            Nome = NameBox.Text.Trim(),
            Academia = TeamBox.Text.Trim(),
            Sexo = gender,
            Categoria = CategoryBox.Text.Trim(),
            Faixa = belt
        });

        NameBox.Clear();
        TeamBox.Clear();
        NameBox.Focus();
    }

    private void RemoveFighter_Click(object sender, RoutedEventArgs e)
    {
        var selected = FightersGrid.SelectedItems.Cast<FighterEntry>().ToList();

        foreach (var fighter in selected)
            TournamentStore.Fighters.Remove(fighter);

        TournamentStore.GenerateBrackets(MixedGenerationBox.IsChecked == true);
    }

    private void GenerateBrackets_Click(object sender, RoutedEventArgs e)
    {
        TournamentStore.GenerateBrackets(MixedGenerationBox.IsChecked == true);
        _nextBracketIndex = 0;
        OpenBracketDisplay();
    }

    private void OpenBracketDisplay_Click(object sender, RoutedEventArgs e)
    {
        OpenBracketDisplay();
    }

    private void OpenBracketDisplay()
    {
        if (_bracketDisplayWindow is { IsVisible: true })
        {
            _bracketDisplayWindow.Activate();
            return;
        }

        _bracketDisplayWindow = new BracketDisplayWindow();
        _bracketDisplayWindow.Show();
    }

    private void CallSelected_Click(object sender, RoutedEventArgs e)
    {
        var selected = FightersGrid.SelectedItems.Cast<FighterEntry>().Take(2).ToList();

        if (selected.Count < 2)
        {
            MessageBox.Show("Selecione dois lutadores.");
            return;
        }

        if (!CanFight(selected[0], selected[1], MixedGenerationBox.IsChecked == true))
            return;

        SetFight(selected[0], selected[1], "Luta");
    }

    private void CallNextBracket_Click(object sender, RoutedEventArgs e)
    {
        if (TournamentStore.Brackets.Count == 0)
        {
            TournamentStore.GenerateBrackets(MixedGenerationBox.IsChecked == true);
            _nextBracketIndex = 0;
            OpenBracketDisplay();
        }

        if (_nextBracketIndex >= TournamentStore.Brackets.Count)
        {
            MessageBox.Show("Não há próxima chave.");
            return;
        }

        var bracket = TournamentStore.Brackets[_nextBracketIndex++];
        CallBracket(bracket);
    }


    private void CallSelectedBracket_Click(object sender, RoutedEventArgs e)
    {
        if (BracketsGrid.SelectedItem is not BracketMatch bracket)
        {
            MessageBox.Show("Selecione uma luta na lista de chaves.");
            return;
        }

        CallBracket(bracket);
    }

    private void BracketsGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (BracketsGrid.SelectedItem is BracketMatch bracket)
            CallBracket(bracket);
    }

    private void CallBracket(BracketMatch bracket)
    {
        if (string.Equals(bracket.LutadorBranco, "BYE", StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show("Essa luta está marcada como BYE. Não há segundo competidor para chamar.");
            return;
        }

        _state.BlueName = bracket.LutadorAzul;
        _state.BlueTeam = bracket.AcademiaAzul;
        _state.RedName = bracket.LutadorBranco;
        _state.RedTeam = bracket.AcademiaBranco;
        _state.Category = string.IsNullOrWhiteSpace(bracket.Categoria) ? "SEM CATEGORIA" : bracket.Categoria;
        _state.Phase = string.IsNullOrWhiteSpace(bracket.Fase) ? "LUTA" : bracket.Fase;
        ResetFightState();
        _state.ReleasePanel();
        EnsurePanelWindowsOpen();

        bracket.Status = "Chamada";
        BracketsGrid.Items.Refresh();

        MessageBox.Show("Luta chamada. A tela de apresentação e a tela de configuração foram abertas automaticamente.");
    }


    private void EnsurePanelWindowsOpen()
    {
        if (_publicWindow is null || !_publicWindow.IsVisible)
        {
            _publicWindow = new PublicDisplayWindow(_state)
            {
                Title = $"4Combat - Painel Público - Tatame {_state.MatNumber:00}"
            };

            _publicWindow.Show();
        }
        else
        {
            _publicWindow.Activate();
        }

        if (_configWindow is null || !_configWindow.IsVisible)
        {
            _configWindow = new MatConfigWindow(_state)
            {
                Title = $"4Combat - Configuração - Tatame {_state.MatNumber:00}"
            };

            _configWindow.Show();
        }
        else
        {
            _configWindow.Activate();
        }
    }

    private bool CanFight(FighterEntry blue, FighterEntry white, bool mixed)
    {
        if (!Same(blue.Faixa, white.Faixa))
        {
            MessageBox.Show("Os lutadores precisam ser da mesma faixa.");
            return false;
        }

        if (!mixed && !Same(blue.Sexo, white.Sexo))
        {
            MessageBox.Show("Para masculino x feminino, marque a opção de geração/chamada mista.");
            return false;
        }

        return true;
    }

    private static bool Same(string a, string b)
        => string.Equals(a?.Trim(), b?.Trim(), StringComparison.OrdinalIgnoreCase);

    private void SetFight(FighterEntry blue, FighterEntry white, string phase)
    {
        _state.BlueName = blue.Nome;
        _state.BlueTeam = blue.Academia;
        _state.RedName = white.Nome;
        _state.RedTeam = white.Academia;
        _state.Category = string.IsNullOrWhiteSpace(blue.Categoria) ? "SEM CATEGORIA" : blue.Categoria;
        _state.Phase = string.IsNullOrWhiteSpace(phase) ? "LUTA" : phase;
        ResetFightState();
        _state.ReleasePanel();
        EnsurePanelWindowsOpen();

        MessageBox.Show("Lutadores enviados para o painel e telas abertas automaticamente.");
    }

    private void ResetFightState()
    {
        _state.BlueScore = 0;
        _state.RedScore = 0;
        _state.BlueAdvantage = 0;
        _state.RedAdvantage = 0;
        _state.BluePenalty = 0;
        _state.RedPenalty = 0;
        _state.ShowChampion = false;
        _state.IsPanelReleased = false;
        _state.Status = "LUTA CHAMADA - AGUARDANDO LIBERAÇÃO";
    }
}
