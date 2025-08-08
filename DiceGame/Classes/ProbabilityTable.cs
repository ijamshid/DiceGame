using Spectre.Console;

namespace DiceGameApplication.Classes;

public static class ProbabilityTable
{
    private static readonly Table probabilityTable = new();
    public static void CreateProbabilityTable(List<Dice> dice)
    {
        probabilityTable.AddColumn("   ");
        for (int i = 0; i < dice.Count; i++)
        {
            probabilityTable.AddColumn($"D{i}");
        }
        for (int i = 0; i < dice.Count; i++)
        {
            var row = new List<string> { $"D{i}" };
            for (int j = 0; j < dice.Count; j++)
            {
                double probability = CalculateProbability(dice[i], dice[j]);
                row.Add($"{probability:F2}");
            }
            probabilityTable.AddRow(row.ToArray());
        }
    }

    public static void DisplayProbabilityTable()
    {
        AnsiConsole.Write(probabilityTable);
    }

    private static double CalculateProbability(Dice dieA, Dice dieB)
    {
        int wins = 0;
        foreach (int faceA in dieA.Faces)
        foreach (int faceB in dieB.Faces)
            if (faceA > faceB) wins++;
        return (double)wins / Math.Pow(Dice.FaceNumber, 2);
    }
}
