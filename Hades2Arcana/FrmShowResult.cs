namespace Hades2Arcana;

public partial class FrmShowResult : Form
{
    public FrmShowResult()
    {
        InitializeComponent();
    }

    internal void SetCards(List<Card> cards)
    {
        this.cardGrid.SetCheckedCards(cards);
    }
}
