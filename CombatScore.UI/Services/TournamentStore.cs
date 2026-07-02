using System.Collections.ObjectModel;
using CombatScore.UI.Models;

namespace CombatScore.UI.Services;

public static class TournamentStore
{
    public static ObservableCollection<FighterEntry> Fighters { get; } = new();
    public static ObservableCollection<BracketMatch> Brackets { get; } = new();

    public static void GenerateBrackets(bool allowMixedGender)
    {
        Brackets.Clear();

        var groups = Fighters
            .Where(f => !string.IsNullOrWhiteSpace(f.Nome))
            .GroupBy(f => new
            {
                Faixa = Normalize(f.Faixa),
                Sexo = allowMixedGender ? "Misto" : Normalize(f.Sexo)
            })
            .OrderBy(g => g.Key.Faixa)
            .ThenBy(g => g.Key.Sexo);

        int number = 1;

        foreach (var group in groups)
        {
            var fighters = group
                .OrderBy(f => f.Nome)
                .ToList();

            for (int i = 0; i < fighters.Count; i += 2)
            {
                var blue = fighters[i];
                var white = i + 1 < fighters.Count ? fighters[i + 1] : null;

                Brackets.Add(new BracketMatch
                {
                    Numero = number++,
                    Fase = "Eliminatória",
                    Categoria = blue.Categoria,
                    Faixa = blue.Faixa,
                    Sexo = allowMixedGender ? "Misto" : blue.Sexo,
                    LutadorAzul = blue.Nome,
                    AcademiaAzul = blue.Academia,
                    LutadorBranco = white?.Nome ?? "BYE",
                    AcademiaBranco = white?.Academia ?? "",
                    Status = white is null ? "BYE" : "Aguardando"
                });
            }
        }
    }

    private static string Normalize(string value)
        => string.IsNullOrWhiteSpace(value) ? "Sem informação" : value.Trim().ToUpperInvariant();
}
