using DiceGameApplication.Classes;
using DiceGameApplication.User;

if (!ValidationError.ValidateDiceInput(args, out List<Dice> dice))
{
    return;
}

new Game(dice).Start();