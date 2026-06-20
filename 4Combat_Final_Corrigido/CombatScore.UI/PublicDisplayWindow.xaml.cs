using System.Windows;
using CombatScore.UI.Models;

namespace CombatScore.UI;

public partial class PublicDisplayWindow : Window
{
    public PublicDisplayWindow(MatchState state)
    {
        InitializeComponent();
        DataContext = state;
    }
}
