using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StabQuest.DungeonLevels;
using StabQuest.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Color = Microsoft.Xna.Framework.Color;
using static StabQuest.Helpers.DiceHelper;
using StabQuest.Services;

namespace StabQuest
{
    public class Player : GameComponent
    {
        private Vector2 _position;
        private Texture2D _texture;
        private SimpleRandomWalkDungeonLevel _currentDungeonLevel;
        private CardinalDirections _direction;
        private bool _hasMoved;
        private SoundService _soundService;
        private List<Character> _characters;

        public Player(Vector2 startPosition, Texture2D texture)
        {
            _position = startPosition;
            _texture = texture;
            _hasMoved = false;
            _characters = new List<Character>()
            {
                new Character("Sven", RollDice(4), RollDice(4), RollDice(4), RollDice(4), RollDice(4), RollDice(4), true),
                new Character("Not Sven", RollDice(3), RollDice(3), RollDice(3), RollDice(3), RollDice(6), RollDice(6), true)
            };
            _soundService = SoundService.Instance;
        }


        public Player()
        {
        }

        public Player(Texture2D characterSpriteSheet)
        {
            _texture = characterSpriteSheet;
            _hasMoved = false;
            _characters = new List<Character>();
        }

        public List<Character> Characters { get { return _characters; } }

        public Vector2 Position { get => _position; set => _position = value; }

        public SimpleRandomWalkDungeonLevel CurrentDungeonLevel
        {
            get => _currentDungeonLevel; set
            {
                if (value.Equals(_currentDungeonLevel))
                {
                    return;
                }
                else
                {
                    _currentDungeonLevel = value;
                }
            }
        }

        public Vector2 WorldPosition { get { return new Vector2(this.Position.X * Game1.TILESIZE, this.Position.Y * Game1.TILESIZE); ; } }

        public CardinalDirections Direction { get => _direction; set => _direction = value; }

        public bool HasMoved { get => _hasMoved; set => _hasMoved = value; }

        public override void Update(GameTime gameTimel)
        {
            _hasMoved = false;

            HandlePlayerMovement();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {   
            var playerSpriteEffect = SpriteEffects.None;
            if (this.Direction.Equals(CardinalDirections.LEFT))
            {
                playerSpriteEffect = SpriteEffects.FlipHorizontally;
            }
            spriteBatch.Draw(_texture, position: WorldPosition, new Rectangle(3 * Game1.TILESIZE, 0, Game1.TILESIZE, Game1.TILESIZE), Color.White, 0, Vector2.One, scale: Game1.WORLDSCALE, playerSpriteEffect, 0);

        }

        private void HandlePlayerMovement()
        {
            var newPosition = _position;
            if (KeyboardHelper.CheckKeyPress(Keys.Up))
            {
                newPosition = _position + Direction2D.Get(CardinalDirections.UP);
            }
            if (KeyboardHelper.CheckKeyPress(Keys.Down))
            {
                newPosition = _position + Direction2D.Get(CardinalDirections.DOWN);
            }
            if (KeyboardHelper.CheckKeyPress(Keys.Left))
            {
                newPosition = _position + Direction2D.Get(CardinalDirections.LEFT);
                _direction = CardinalDirections.LEFT;
            }
            if (KeyboardHelper.CheckKeyPress(Keys.Right))
            {
                newPosition = _position + Direction2D.Get(CardinalDirections.RIGHT);
                _direction = CardinalDirections.RIGHT;
            }

            if (!newPosition.Equals(_position)) // has actually moved
            {
                if (_currentDungeonLevel.Tiles.Any(tile => tile.Position.Equals(newPosition) && tile.Walkable))
                {
                    _position = newPosition;
                    _soundService.PlaySoundEffectInstance(SoundService.SoundEffects.Step);
                    _hasMoved = true;
                }
            }
        }
    }
}
