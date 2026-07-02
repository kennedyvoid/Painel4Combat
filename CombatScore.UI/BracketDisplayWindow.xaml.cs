using System.Windows;
using CombatScore.UI.Services;

namespace CombatScore.UI;

public partial class BracketDisplayWindow : Window
{
    public BracketDisplayWindow()
    {
        InitializeComponent();

        if (TournamentStore.Brackets.Count == 0)
            TournamentStore.GenerateBrackets(false);

        BracketItems.ItemsSource = TournamentStore.Brackets;
    }
}
