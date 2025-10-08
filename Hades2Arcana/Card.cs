using Microsoft.Z3;
using System.Diagnostics;

namespace Hades2Arcana;

record class Card(string Name, int Cost, bool ThePrefix)
{
    public static readonly Card[][] s_cardGrid;
    public static readonly Context s_context = new Context();
    private static readonly Dictionary<string, Card> s_allCards = new();
    private static readonly Dictionary<string, BoolExpr> s_cardWantConstraints = new();
    private static readonly Dictionary<string, BoolExpr> s_cardDontWantConstraints = new();
    private static readonly List<BoolExpr> s_awakeningConstraints = new();
    private static readonly ArithExpr s_graspUsed;

    static Card()
    {
        s_cardGrid =
        [
            [new Card("Sorceress", 1, true), new Card("Wayward Son", 1, true), new Card("Huntress", 2, true), new Card("Eternity", 3, false), new MoonCard()],
            [new Card("Furies", 2, true), new Card("Persistence", 2, false), new Card("Messenger", 1, true), new Card("Unseen", 5, true), new Card("Night", 2, false)],
            [new Card("Swift Runner", 1, true), new Card("Death", 4, false), new CentaurCard(), new Card("Origination", 5, false), new Card("Lovers", 3, true)],
            [new Card("Enchantress", 3, true), new Card("Boatman", 5, true), new Card("Artificer", 3, true), new Card("Excellence", 5, false), new QueenCard()],
            [new FatesCard(), new Card("Champions", 4, true), new Card("Stength", 4, false), new DivinityCard(), new JudgmentCard()],
        ];

        for (int row = 0; row < 5; row++)
        {
            var newRow = new Card[5];
            for (int col = 0; col < 5; col++)
            {
                newRow[col] = s_cardGrid[row][col] with
                {
                    Row = row,
                    Col = col,
                };
            }
            s_cardGrid[row] = newRow;
        }

        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                var c = s_cardGrid[row][col];
                s_allCards.Add(c.Name, c);
            }
        }

        foreach (var c in s_allCards.Values)
        {
            s_cardWantConstraints.Add(c.Name, s_context.MkEq(c.CardExpr, s_context.MkTrue()));
            s_cardDontWantConstraints.Add(c.Name, s_context.MkEq(c.CardExpr, s_context.MkFalse()));
            var awakening = c.GetAwakeningConstraing(s_allCards, s_cardGrid);
            if (awakening is not null)
            {
                s_awakeningConstraints.Add(s_context.MkImplies(c.CardExpr, awakening));
            }
        }

        s_graspUsed = s_context.MkAdd(s_allCards.Values.Select(c => (ArithExpr)s_context.MkITE(c.CardExpr, s_context.MkInt(c.Cost), s_context.MkInt(0))));
    }

    internal static List<Card>? GetResult(List<Card> wantCards, List<Card> dontWantCards, int grasp)
    {
        if (grasp <= 0)
            throw new ArgumentOutOfRangeException(nameof(grasp), "Grasp must be more than zero.");

        var constraints = new List<Expr>();
        using var graspConstraint = s_graspUsed <= grasp;
        constraints.Add(graspConstraint);
        constraints.AddRange(s_awakeningConstraints);
        foreach (var c in wantCards)
        {
            constraints.Add(s_cardWantConstraints[c.Name]);
        }
        foreach (var c in dontWantCards)
        {
            constraints.Add(s_cardDontWantConstraints[c.Name]);
        }

        using var solver = s_context.MkSolver();
        if (solver.Check(constraints.ToArray()) == Status.SATISFIABLE)
        {
            using var model = solver.Model;

            var ret = new List<Card>();
            foreach (var kvp in model.Consts)
            {
                string? name = (kvp.Key.Name as StringSymbol)?.String;
                BoolExpr? expr = (kvp.Value as BoolExpr);
                if (name is not null && expr is not null && s_allCards.TryGetValue(name, out Card? card))
                {
                    // TODO: there HAS to be a better way to pull the value out
                    bool hasCard = bool.Parse(model.Eval(expr).ToString());
                    if (hasCard)
                    {
                        ret.Add(card);
                    }
                }
            }
            return ret;
        }
        else
        {
            return null;
        }
    }

    protected virtual BoolExpr? GetAwakeningConstraing(Dictionary<string, Card> cardMap, Card[][] cardGrip)
    {
        return null;
    }

    public int Row
    {
        get;
        init;
    }

    public int Col
    {
        get;
        init;
    }

    public BoolExpr CardExpr { get; } = s_context.MkBoolConst(Name);

    public ArithExpr CostExpr { get; } = s_context.MkInt(Cost);
}

record class MoonCard : Card
{
    public MoonCard()
        : base("Moon", 0, true)
    {
    }

    protected override BoolExpr? GetAwakeningConstraing(Dictionary<string, Card> cardMap, Card[][] cardGrip)
    {
        var eternity = cardMap["Eternity"];
        var unseen = cardMap["Unseen"];
        var night = cardMap["Night"];
        return s_context.MkOr(eternity.CardExpr, unseen.CardExpr, night.CardExpr);
    }
}

record class CentaurCard : Card
{
    public CentaurCard()
        : base("Centaur", 0, true)
    {
    }

    protected override BoolExpr? GetAwakeningConstraing(Dictionary<string, Card> cardMap, Card[][] cardGrip)
    {
        return s_context.MkAnd(Enumerable.Range(1, 5).Select(CreateForCost));
        BoolExpr CreateForCost(int cost)
        {
            return s_context.MkOr(cardMap.Values.Where(c => c.Cost == cost).Select(c => c.CardExpr));
        }
    }
}

record class QueenCard : Card
{
    public QueenCard()
        : base("Queen", 0, true)
    {
    }

    protected override BoolExpr? GetAwakeningConstraing(Dictionary<string, Card> cardMap, Card[][] cardGrip)
    {
        return s_context.MkAnd(Enumerable.Range(1, 5).Select(CardsOfCostLessThanTwo));
        BoolExpr CardsOfCostLessThanTwo(int cost)
        {
            var cardCount = s_context.MkAdd(cardMap.Values.Where(c => c.Cost == cost).Select(c => (ArithExpr)s_context.MkITE(c.CardExpr, s_context.MkInt(1), s_context.MkInt(0))));
            return s_context.MkLe(cardCount, s_context.MkInt(2));
        }
    }
}

record class FatesCard : Card
{
    public FatesCard()
        : base("Fates", 0, true)
    {
    }

    protected override BoolExpr? GetAwakeningConstraing(Dictionary<string, Card> cardMap, Card[][] cardGrip)
    {
        var enchantress = cardMap["Enchantress"].CardExpr;
        var boatman = cardMap["Boatman"].CardExpr;
        var champions = cardMap["Champions"].CardExpr;
        return s_context.MkAnd(enchantress, boatman, champions);
    }
}

record class DivinityCard : Card
{
    public DivinityCard()
        : base("Divinity", 0, false)
    {
    }

    protected override BoolExpr? GetAwakeningConstraing(Dictionary<string, Card> cardMap, Card[][] cardGrip)
    {
        var ret = new List<BoolExpr>();
        for (int row = 0; row < 5; row++)
        {
            if (row == Row)
                continue;
            ret.Add(s_context.MkAnd(Enumerable.Range(0, 5).Select(col => cardGrip[row][col].CardExpr)));
        }
        for (int col = 0; col < 5; col++)
        {
            if (col == Col)
                continue;
            ret.Add(s_context.MkAnd(Enumerable.Range(0, 5).Select(row => cardGrip[row][col].CardExpr)));
        }
        return s_context.MkOr(ret);
    }
}

record class JudgmentCard : Card
{
    public JudgmentCard()
        : base("Judgment", 0, false)
    {
    }

    protected override BoolExpr? GetAwakeningConstraing(Dictionary<string, Card> cardMap, Card[][] cardGrip)
    {
        var cardsAreActivated = cardMap.Values.Select(c => (ArithExpr)s_context.MkITE(c.CardExpr, s_context.MkInt(1), s_context.MkInt(0)));
        var numberOfCardsActivated = s_context.MkAdd(cardsAreActivated);
        return numberOfCardsActivated <= 3;
    }
}