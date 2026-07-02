namespace CombatScore.UI.Models;

public class BracketMatch
{
    public int Numero { get; set; }
    public string Fase { get; set; } = "Luta";
    public string Categoria { get; set; } = "";
    public string Faixa { get; set; } = "";
    public string Sexo { get; set; } = "";
    public string LutadorAzul { get; set; } = "";
    public string AcademiaAzul { get; set; } = "";
    public string LutadorBranco { get; set; } = "";
    public string AcademiaBranco { get; set; } = "";
    public string Status { get; set; } = "Aguardando";
}
