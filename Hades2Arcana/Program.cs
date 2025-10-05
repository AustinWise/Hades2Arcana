// See https://aka.ms/new-console-template for more information


using Microsoft.Z3;
using System.Net.Sockets;

// TODO: parameter
const int MAX_GRASP = 29;

using var ctx = new Context();

var cardGrid = new Card[][]
{
    [new Card("Sorceress", 1, true), new Card("Warway Sun", 1, true), new Card("Huntress", 2, true), new Card("Eternity", 3, false), new MoonCard()],
    [new Card("Furies", 2, true), new Card("Persistence", 2, false), new Card("Messenger", 1, true), new Card("Unseen", 5, true), new Card("Night", 2, false)],
    [new Card("Swift Runner", 1, true), new Card("Death", 4, false), new CentaurCard(), new Card("Origination", 5, false), new Card("Lovers", 3, true)],
    [new Card("Enchantress", 3, true), new Card("Boatman", 5, true), new Card("Artificer", 3, true), new Card("Excellence", 5, false), new QueenCard()],
    [new FatesCard(), new Card("Champions", 4, true), new Card("Stength", 4, false), new DivinityCard(), new JudgmentCard()],
};

for (int row = 0; row < 5; row++)
{
    var newRow = new Card[5];
    for (int col = 0; col < 5; col++)
    {
        var oldCard = cardGrid[row][col];
        var cardExpr = ctx.MkBoolConst(oldCard.Name);
        newRow[col] = oldCard with
        {
            Row = row,
            Col = col,
            CardExpr = cardExpr,
            CostExpr = (ArithExpr)ctx.MkITE(cardExpr, ctx.MkInt(oldCard.Cost), ctx.MkInt(0)),
        };
    }
    cardGrid[row] = newRow;
}

var allCards = cardGrid.SelectMany(row => row).ToArray();
var nameMap = allCards.ToDictionary(c => c.Name);

var totalCost = ctx.MkAdd(allCards.Select(c => c.CostExpr).ToArray());

var desiredCards = new string[]
{
    //"Sorceress",
    //"Warway Sun",
    //"Huntress",
    //"Eternity",
    //"Moon",
    //"Furies",
    //"Persistence",
    //"Messenger",
    //"Unseen",
    //"Night",
    //"Swift Runner",
    //"Death",
    "Centaur",
    //"Origination",
    //"Lovers",
    //"Enchantress",
    //"Boatman",
    //"Artificer",
    //"Excellence",
    "Queen",
    //"Fates",
    //"Champions",
    //"Stength",
    "Divinity",
    //"Judgment",
};

var constraints = new List<Expr>();
foreach (var name in desiredCards)
{
    var c = nameMap[name];
    constraints.Add(ctx.MkEq(c.CardExpr, ctx.MkTrue()));
    var extra = c.GetAwakeningConstraing(ctx, nameMap, cardGrid);
    if (extra is object)
    {
        constraints.Add(extra);
    }
}
constraints.Add(totalCost < MAX_GRASP);


Solver solver = ctx.MkSolver();
var status = solver.Check(constraints.ToArray());

Console.WriteLine(status);
if (status == Status.SATISFIABLE)
{
    Console.WriteLine(solver.Model);
}

//
record class Card(string Name, int Cost, bool ThePrefix)
{
    public virtual BoolExpr? GetAwakeningConstraing(Context ctx, Dictionary<string, Card> cardMap, Card[][] cardGrip)
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

    public BoolExpr CardExpr
    {
        get;
        init;
    }

    public ArithExpr CostExpr
    {
        get;
        init;
    }
}

record class MoonCard : Card
{
    public MoonCard()
        : base("Moon", 0, true)
    {
    }

    public override BoolExpr? GetAwakeningConstraing(Context ctx, Dictionary<string, Card> cardMap, Card[][] cardGrip)
    {
        var eternity = cardMap["Eternity"];
        var unseen = cardMap["Unseen"];
        var night = cardMap["Night"];
        return ctx.MkOr(eternity.CardExpr, unseen.CardExpr, night.CardExpr);
    }
}

record class CentaurCard : Card
{
    public CentaurCard()
        : base("Centaur", 0, true)
    {
    }

    public override BoolExpr? GetAwakeningConstraing(Context ctx, Dictionary<string, Card> cardMap, Card[][] cardGrip)
    {
        return ctx.MkAnd(Enumerable.Range(1, 5).Select(CreateForCost));
        BoolExpr CreateForCost(int cost)
        {
            return ctx.MkOr(cardMap.Values.Where(c => c.Cost == cost).Select(c => ctx.MkEq(c.CardExpr, ctx.MkTrue())));
        }
    }
}

record class QueenCard : Card
{
    public QueenCard()
        : base("Queen", 0, true)
    {
    }

    public override BoolExpr? GetAwakeningConstraing(Context ctx, Dictionary<string, Card> cardMap, Card[][] cardGrip)
    {
        return ctx.MkAnd(Enumerable.Range(1, 5).Select(CardsOfCostLessThanTwo));
        BoolExpr CardsOfCostLessThanTwo(int cost)
        {
            var cardCount = ctx.MkAdd(cardMap.Values.Where(c => c.Cost == cost).Select(c => (ArithExpr)ctx.MkITE(c.CardExpr, ctx.MkInt(1), ctx.MkInt(0))));
            return ctx.MkLe(cardCount, ctx.MkInt(2));
        }
    }
}

record class FatesCard : Card
{
    public FatesCard()
        : base("Fates", 0, true)
    {
    }

    public override BoolExpr? GetAwakeningConstraing(Context ctx, Dictionary<string, Card> cardMap, Card[][] cardGrip)
    {
        var enchantress = cardMap["Enchantress"].CardExpr;
        var boatman = cardMap["Boatman"].CardExpr;
        var champions = cardMap["Champions"].CardExpr;
        return ctx.MkAnd(enchantress, boatman, champions);
    }
}

record class DivinityCard : Card
{
    public DivinityCard()
        : base("Divinity", 0, false)
    {
    }

    public override BoolExpr? GetAwakeningConstraing(Context ctx, Dictionary<string, Card> cardMap, Card[][] cardGrip)
    {
        var ret = new List<BoolExpr>();
        for (int row = 0; row < 5; row++)
        {
            if (row == Row)
                continue;
            ret.Add(ctx.MkAnd(Enumerable.Range(0, 5).Select(col => cardGrip[row][col].CardExpr)));
        }
        for (int col = 0; col < 5; col++)
        {
            if (col == Col)
                continue;
            ret.Add(ctx.MkAnd(Enumerable.Range(0, 5).Select(row => cardGrip[row][col].CardExpr)));
        }
        return ctx.MkOr(ret);
    }
}

record class JudgmentCard : Card
{
    public JudgmentCard()
        : base("Judgment", 0, false)
    {
    }

    public override BoolExpr? GetAwakeningConstraing(Context ctx, Dictionary<string, Card> cardMap, Card[][] cardGrip)
    {
        var cardsAreActivated = cardMap.Values.Select(c => (ArithExpr)ctx.MkITE(c.CardExpr, ctx.MkInt(1), ctx.MkInt(0)));
        var numberOfCardsActivated = ctx.MkAdd(cardsAreActivated);
        return numberOfCardsActivated <= 3;
    }
}