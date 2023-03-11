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

    public static int TILESIZE = 16;
    public static int WORLDSCALE = 1;

    private SoundService _soundService;

    public bool IsFullScreen { get { return _graphics.IsFullScreen; } }
    public int ScreenWidth { get { return _graphics.PreferredBackBufferWidth; } }
    public int ScreenHeight { get { return _graphics.PreferredBackBufferHeight; } }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    public void SetScreenSize(int width, int height)
    {
        _graphics.PreferredBackBufferHeight = height;
        _graphics.PreferredBackBufferWidth = width;
        _graphics.ApplyChanges();
    }

    public void SetFullScreen(bool isFullScreen)
    {
        _graphics.IsFullScreen = isFullScreen;
        _graphics.ApplyChanges();
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