using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StabQuest.GameStates;
using StabQuest.Helpers;
using StabQuest.Services;

namespace StabQuest;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private GameState _currentState;
    private GameState _nextState;

    public OverworldState _overWorldState;

    public int _screenHeight;
    public int _screenWidth;

    public static int TILESIZE = 16;
    public static int WORLDSCALE = 1;

    private SoundService _soundService;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _screenHeight = _graphics.PreferredBackBufferHeight;
        _screenWidth = _graphics.PreferredBackBufferWidth;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _currentState = new MainMenuState(Content, GraphicsDevice, this);
        _currentState.IsActiveScene = true;
        
        _soundService = SoundService.Instance;
        _soundService.Content = Content;
        _soundService.LoadSounds();
        _soundService.PlayBackgroundMusic();
    }

    public void ChangeState(GameState state)
    {
        _nextState = state;
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardHelper.GetState();

        if (_nextState != null)
        {
            _currentState = _nextState;
            _currentState.IsActiveScene = true;
        }

        _currentState.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _currentState.Draw(gameTime, _spriteBatch);

        base.Draw(gameTime);
    }

    public GameState GetCurrentState()
    {
        return _currentState;
    }
}