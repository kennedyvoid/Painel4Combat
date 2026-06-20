using System;
using System.Media;
using System.Windows;
using System.Windows.Threading;
using CombatScore.UI.Models;

namespace CombatScore.UI;

public partial class MatConfigWindow : Window
{
    private readonly MatchState _state;
    private readonly DispatcherTimer _timer = new();

    public MatConfigWindow(MatchState state)
    {
        InitializeComponent();
        _state = state;
        DataContext = _state;
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (_state.RemainingTime.TotalSeconds <= 1)
        {
            _state.RemainingTime = TimeSpan.Zero;
            _timer.Stop();
            _state.Status = "FINALIZADA";
            SystemSounds.Exclamation.Play();
            return;
        }
        _state.RemainingTime -= TimeSpan.FromSeconds(1);
    }

    private void Start_Click(object sender, RoutedEventArgs e)
    {
        if (!_state.IsPanelReleased)
        {
            MessageBox.Show("Primeiro clique em SALVAR E LIBERAR PAINEL.");
            return;
        }
        _state.ShowChampion = false;
        _state.Status = "EM ANDAMENTO";
        _timer.Start();
    }

    private void Pause_Click(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        _state.Status = "PAUSADA";
    }

    private void ResetTime_Click(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        _state.RemainingTime = _state.ConfiguredTime;
        _state.Status = "PRONTO PARA INICIAR";
        _state.ShowChampion = false;
    }

    private void End_Click(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        _state.Status = "FINALIZADA";
        SystemSounds.Exclamation.Play();
    }

    private void ApplyTime_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(MinutesBox.Text, out int minutes) || minutes <= 0)
        {
            MessageBox.Show("Tempo inválido.");
            return;
        }
        _state.ConfiguredTime = TimeSpan.FromMinutes(minutes);
        _state.RemainingTime = _state.ConfiguredTime;
        _state.Status = "TEMPO CONFIGURADO";
    }

    private void NewMatch_Click(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        _state.NewFight();
        Activate();
        Focus();
    }

    private void ReleasePanel_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_state.BlueName) || string.IsNullOrWhiteSpace(_state.RedName))
        {
            MessageBox.Show("Informe o nome dos dois competidores antes de liberar o painel.");
            return;
        }
        _state.ReleasePanel();
    }

    private void Champion_Click(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        _state.ResultMode = "VENCEDOR";
        _state.ChampionText = _state.ResolveWinnerName();
        _state.ChampionTeam = _state.ResolveWinnerTeam();
        _state.ShowChampion = true;
        _state.Status = "VENCEDOR DEFINIDO";
        SystemSounds.Asterisk.Play();
    }

    private void WinnerBlue_Click(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        _state.ResultMode = "VENCEDOR";
        _state.ChampionText = _state.BlueName;
        _state.ChampionTeam = _state.BlueTeam;
        _state.ShowChampion = true;
        _state.Status = "VENCEDOR DEFINIDO";
        SystemSounds.Asterisk.Play();
    }

    private void WinnerRed_Click(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        _state.ResultMode = "VENCEDOR";
        _state.ChampionText = _state.RedName;
        _state.ChampionTeam = _state.RedTeam;
        _state.ShowChampion = true;
        _state.Status = "VENCEDOR DEFINIDO";
        SystemSounds.Asterisk.Play();
    }

    private void Draw_Click(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        _state.ResultMode = "EMPATE";
        _state.ChampionText = $"{_state.BlueName}  X  {_state.RedName}";
        _state.ChampionTeam = $"{_state.BlueTeam}  |  {_state.RedTeam}";
        _state.ShowChampion = true;
        _state.Status = "EMPATE";
        SystemSounds.Asterisk.Play();
    }

    private void Blue2_Click(object sender, RoutedEventArgs e) => _state.BlueScore += 2;
    private void Blue3_Click(object sender, RoutedEventArgs e) => _state.BlueScore += 3;
    private void Blue4_Click(object sender, RoutedEventArgs e) => _state.BlueScore += 4;
    private void BlueMinus2_Click(object sender, RoutedEventArgs e) => _state.BlueScore -= 2;
    private void BlueMinus3_Click(object sender, RoutedEventArgs e) => _state.BlueScore -= 3;
    private void BlueMinus4_Click(object sender, RoutedEventArgs e) => _state.BlueScore -= 4;
    private void BlueAdvPlus_Click(object sender, RoutedEventArgs e) => _state.BlueAdvantage++;
    private void BlueAdvMinus_Click(object sender, RoutedEventArgs e) => _state.BlueAdvantage--;
    private void BluePenPlus_Click(object sender, RoutedEventArgs e) => _state.BluePenalty++;
    private void BluePenMinus_Click(object sender, RoutedEventArgs e) => _state.BluePenalty--;

    private void Red2_Click(object sender, RoutedEventArgs e) => _state.RedScore += 2;
    private void Red3_Click(object sender, RoutedEventArgs e) => _state.RedScore += 3;
    private void Red4_Click(object sender, RoutedEventArgs e) => _state.RedScore += 4;
    private void RedMinus2_Click(object sender, RoutedEventArgs e) => _state.RedScore -= 2;
    private void RedMinus3_Click(object sender, RoutedEventArgs e) => _state.RedScore -= 3;
    private void RedMinus4_Click(object sender, RoutedEventArgs e) => _state.RedScore -= 4;
    private void RedAdvPlus_Click(object sender, RoutedEventArgs e) => _state.RedAdvantage++;
    private void RedAdvMinus_Click(object sender, RoutedEventArgs e) => _state.RedAdvantage--;
    private void RedPenPlus_Click(object sender, RoutedEventArgs e) => _state.RedPenalty++;
    private void RedPenMinus_Click(object sender, RoutedEventArgs e) => _state.RedPenalty--;
}
