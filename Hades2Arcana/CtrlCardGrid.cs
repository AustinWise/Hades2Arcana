using System.Data;

namespace Hades2Arcana;

public partial class CtrlCardGrid : UserControl
{
    private readonly List<CheckBox> _checkboxes = [];
    private readonly Dictionary<string, CheckBox> _checkboxMap = [];

    public CtrlCardGrid()
    {
        InitializeComponent();
        if (DesignMode)
            return;

        var cardGrid = Card.s_cardGrid;
        EventHandler checkHandler = onBoxChecked;
        for (int rowNdx = 0; rowNdx < cardGrid.Length; rowNdx++)
        {
            Card[] row = cardGrid[rowNdx];
            for (int colNdx = 0; colNdx < row.Length; colNdx++)
            {
                Card c = row[colNdx];
                string prefix = c.ThePrefix ? "The " : "";
                var chk = new CheckBox()
                {
                    Text = $"{prefix}{c.Name}\n{c.Cost}",
                    Tag = c,
                    AutoSize = true,
                    ThreeState = true,
                };
                chk.CheckStateChanged += checkHandler;
                table.Controls.Add(chk, colNdx, rowNdx);
                _checkboxes.Add(chk);
                _checkboxMap.Add(c.Name, chk);
            }
        }
    }

    public event EventHandler? CheckedCardsChanged;

    private void onBoxChecked(object? sender, EventArgs e)
    {
        CheckedCardsChanged?.Invoke(this, EventArgs.Empty);
    }

    internal CheckedCards GetCheckedCards()
    {
        var want = new List<Card>();
        var dontWant = new List<Card>();
        foreach (var chk in _checkboxes)
        {
            var card = (Card)chk.Tag!;
            switch (chk.CheckState)
            {
                case CheckState.Checked:
                    want.Add(card);
                    break;
                case CheckState.Indeterminate:
                    dontWant.Add(card);
                    break;
            }
        }
        return new CheckedCards(want, dontWant);
    }

    internal void SetCheckedCards(List<Card> cards)
    {
        foreach (var chk in _checkboxes)
        {
            chk.Checked = false;
        }
        foreach (var c in cards)
        {
            var chk = _checkboxMap[c.Name];
            chk.Checked = true;
        }
    }
}