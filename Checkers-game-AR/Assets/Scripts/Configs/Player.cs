using Enums;

namespace Configs
{
    public class Player
    {
        public string       Name;
        public int          NumOfChackers = 12;
        public ColorChecker Color;

        public Player(string name, ColorChecker color)
        {
            Name  = name;
            Color = color;
        }

        public void Reset() => NumOfChackers = 12;
    }
}
