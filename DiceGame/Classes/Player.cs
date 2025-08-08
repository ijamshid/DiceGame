using DiceGameApplication.User;

namespace DiceGameApplication.Classes;

public class Player(string name, bool isComputer)
{
    public string Name { get; } = name;
    public bool IsComputer { get; } = isComputer;
    public Dice? ChosenDie { get; private set; }
    public int? LastThrow { get; private set; }
    private string? currentKey;
    private string? currentHmac;

    public int ChooseDie(List<Dice> dice, int excludeIndex = -1)
    {
        if (IsComputer)
        {
            return ComputerChooseDie(dice, excludeIndex);
        }
        else
        {
            return HumanChooseDie(dice, excludeIndex);
        }
    }

    private int ComputerChooseDie(List<Dice> dice, int excludeIndex = -1)
    {
        Random random = new();
        int index;
        do
        {
            index = random.Next(dice.Count);
        } while (index == excludeIndex);
        ChosenDie = dice[index];
        Console.WriteLine($"{Name} chose die {string.Join(", ", dice[index].Faces)}.");
        return index;
    }

    private int HumanChooseDie(List<Dice> dice, int excludeIndex = -1)
    {
        Console.WriteLine("Choose your die:");
        for (int i = 0; i < dice.Count; i++)
        {
            if (i != excludeIndex)
                Console.WriteLine($"{i + 1} - {string.Join(", ", dice[i].Faces)}");
        }

        int choice = UserInput.GetUserChoice(dice.Count + 1);
        if (choice == -1) return -1;

        ChosenDie = dice[choice - 1];
        return choice - 1;
    }

    public void AnnounceTurn(int round)
    {
        string playerName = round == 0 ? "computer's" : "your";
        Console.WriteLine($"It's {playerName} turn");
        PrepareMove();
    }

    public void PrepareMove()
    {

        if (ChosenDie == null) throw new InvalidOperationException("Die not chosen.");
        (currentHmac, currentKey, LastThrow) = HmacGenerator.GenerateHmac(Dice.FaceNumber);
        ShowHmac();
    }

    public void ShowHmac()
    {
        if (currentHmac != null)
            Console.WriteLine($"{Name}'s HMAC: {currentHmac}");
    }

    public int MakeMove()
    {
        PlayerChooseNumber();
        return LastThrow ?? 0;
    }

    public void PlayerChooseNumber()
    {
        Console.Write($"Choose a number modulo {Dice.FaceNumber}: ");
        LastThrow = UserInput.GetUserChoice(Dice.FaceNumber);
    }

    public void ShowKey()
    {
        if (currentKey != null)
            Console.WriteLine($"{Name}'s number is {LastThrow} (Key: {currentKey})");
    }
}
