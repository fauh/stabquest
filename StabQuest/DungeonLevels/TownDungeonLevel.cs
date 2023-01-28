using Microsoft.Xna.Framework.Graphics;

namespace StabQuest.DungeonLevels
{
    public class TownDungeonLevel : DungeonLevel
    {
        public TownDungeonLevel(int level, Texture2D texture) : base(level, texture)
        {
        }

        protected override void GenerateTiles(int level)
        {
            throw new System.NotImplementedException();
        }
    }
}
