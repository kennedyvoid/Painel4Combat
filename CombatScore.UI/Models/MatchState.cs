using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CombatScore.UI.Models;

public sealed class MatchState : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private int _matNumber;
    private string _blueName = string.Empty;
    private string _redName = string.Empty;
    private string _blueTeam = string.Empty;
    private string _redTeam = string.Empty;
    private string _category = "ADULTO / PESO MÉDIO";
    private string _phase = "FINAL";
    private int _blueScore;
    private int _redScore;
    private int _blueAdvantage;
    private int _redAdvantage;
    private int _bluePenalty;
    private int _redPenalty;
    private TimeSpan _configuredTime = TimeSpan.FromMinutes(5);
    private TimeSpan _remainingTime = TimeSpan.FromMinutes(5);
    private string _status = "CONFIGURANDO LUTA";
    private bool _isPanelReleased;
    private bool _showChampion;
    private string _resultMode = "VENCEDOR";
    private string _championText = string.Empty;
    private string _championTeam = string.Empty;

    public MatchState(int matNumber)
    {
        _matNumber = matNumber;
    }

    public int MatNumber
    {
        get => _matNumber;
        set
        {
            Set(ref _matNumber, value);
            OnPropertyChanged(nameof(MatText));
        }
    }

    public string MatText => $"TATAME {MatNumber:00}";

    public string BlueName { get => _blueName; set => Set(ref _blueName, value); }
    public string RedName { get => _redName; set => Set(ref _redName, value); }
    public string BlueTeam { get => _blueTeam; set => Set(ref _blueTeam, value); }
    public string RedTeam { get => _redTeam; set => Set(ref _redTeam, value); }
    public string Category { get => _category; set => Set(ref _category, value); }
    public string Phase { get => _phase; set => Set(ref _phase, value); }

    public int BlueScore { get => _blueScore; set => Set(ref _blueScore, Math.Max(0, value)); }
    public int RedScore { get => _redScore; set => Set(ref _redScore, Math.Max(0, value)); }
    public int BlueAdvantage { get => _blueAdvantage; set => Set(ref _blueAdvantage, Math.Max(0, value)); }
    public int RedAdvantage { get => _redAdvantage; set => Set(ref _redAdvantage, Math.Max(0, value)); }
    public int BluePenalty { get => _bluePenalty; set => Set(ref _bluePenalty, Math.Max(0, value)); }
    public int RedPenalty { get => _redPenalty; set => Set(ref _redPenalty, Math.Max(0, value)); }

    public TimeSpan ConfiguredTime { get => _configuredTime; set => Set(ref _configuredTime, value); }

    public TimeSpan RemainingTime
    {
        get => _remainingTime;
        set
        {
            Set(ref _remainingTime, value);
            OnPropertyChanged(nameof(TimeText));
        }
    }

    public string TimeText => RemainingTime.ToString(@"mm\:ss");
    public string Status { get => _status; set => Set(ref _status, value); }
    public bool IsPanelReleased { get => _isPanelReleased; set => Set(ref _isPanelReleased, value); }
    public bool ShowChampion { get => _showChampion; set => Set(ref _showChampion, value); }
    public string ResultMode { get => _resultMode; set => Set(ref _resultMode, value); }
    public string ChampionText { get => _championText; set => Set(ref _championText, value); }
    public string ChampionTeam { get => _championTeam; set => Set(ref _championTeam, value); }

    public void NewFight()
    {
        BlueName = string.Empty;
        RedName = string.Empty;
        BlueTeam = string.Empty;
        RedTeam = string.Empty;
        BlueScore = 0;
        RedScore = 0;
        BlueAdvantage = 0;
        RedAdvantage = 0;
        BluePenalty = 0;
        RedPenalty = 0;
        RemainingTime = ConfiguredTime;
        Status = "CONFIGURANDO LUTA";
        IsPanelReleased = false;
        ShowChampion = false;
        ResultMode = "VENCEDOR";
        ChampionText = string.Empty;
        ChampionTeam = string.Empty;
    }

    public void ReleasePanel()
    {
        IsPanelReleased = true;
        ShowChampion = false;
        ResultMode = "VENCEDOR";
        Status = "PRONTO PARA INICIAR";
    }

    public string ResolveWinnerName()
    {
        if (BlueScore > RedScore) return BlueName;
        if (RedScore > BlueScore) return RedName;
        if (BlueAdvantage > RedAdvantage) return BlueName;
        if (RedAdvantage > BlueAdvantage) return RedName;
        if (BluePenalty < RedPenalty) return BlueName;
        if (RedPenalty < BluePenalty) return RedName;
        return "DECISÃO DO ÁRBITRO";
    }

    public string ResolveWinnerTeam()
    {
        string winner = ResolveWinnerName();
        if (winner == BlueName) return BlueTeam;
        if (winner == RedName) return RedTeam;
        return string.Empty;
    }

    private void Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
