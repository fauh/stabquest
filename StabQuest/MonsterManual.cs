using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rpgcombatsim
{
    public static class MonsterManual
    {
        public static Dictionary<string, int[]> StatLines = new Dictionary<string, int[]>
        {
            { "Goblin", new int[] { 1, 2, 0, 0, 0, 0}},
            { "Ork",    new int[] { 4, 0, 3, 0, 0, 0}},
            { "Bandit", new int[] { 2, 2, 2, 1, 1, 1}}
        };

    }
}