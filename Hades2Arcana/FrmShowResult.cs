namespace Hades2Arcana;

public partial class FrmShowResult : Form
{
    public FrmShowResult()
    {
        InitializeComponent();
    }

    internal void SetCards(List<Card> cards)
    {
        this.Text += " - Grasp Used: " + cards.Sum(c => c.Cost).ToString();
        this.cardGrid.SetCheckedCards(cards);
    }
}
