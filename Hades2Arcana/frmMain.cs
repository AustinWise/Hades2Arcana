using Microsoft.Z3;

namespace Hades2Arcana;

public partial class FrmMain : Form
{
    private readonly Dictionary<string, Card> _allCards = new Dictionary<string, Card>();
    private readonly Dictionary<string, Expr> _cardEnableConstraints = new();
    private readonly Dictionary<string, BoolExpr> _awakeningConstraints = new();
    private List<Card>? lastResult = null;

    public FrmMain()
    {
        InitializeComponent();
    }

    private void cardGrid_CheckedCardsChanged(object sender, EventArgs e)
    {
        updateSolution();
    }

    private void graspUpDown_ValueChanged(object sender, EventArgs e)
    {
        updateSolution();
    }

    private void updateSolution()
    {
        var cards = cardGrid.GetCheckedCards();
        int graspUsed = cards.Want.Sum(c => c.Cost);
        lblGraspUsed.Text = $"Grasp Used: {graspUsed}";
        this.lastResult = Card.GetResult(cards.Want, cards.DontWant, (int)graspUpDown.Value);
        bool works = lastResult != null;
        this.chkValidSolutionExists.Checked = works;
        this.btnShowSolution.Enabled = works;
    }

    private void btnShowSolution_Click(object sender, EventArgs e)
    {
        using var frm = new FrmShowResult();
        frm.SetCards(lastResult!);
        frm.ShowDialog(this);
    }
}
