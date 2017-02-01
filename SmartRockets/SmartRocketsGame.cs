using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SmartRockets {
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class SmartRocketsGame : Game {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    Rocket sampleRocket;
    RocketController sampleController;

    public SmartRocketsGame() {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize() {
      // TODO: Add your initialization logic here

      var bounds = GraphicsDevice.Viewport.Bounds;
      var startingPoint = new Vector2((int)(bounds.Width / 1.5), bounds.Height / 2);
      sampleController = new RocketController();
      sampleRocket = new Rocket(startingPoint, sampleController);

      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent() {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);

      // TODO: use this.Content to load your game content here
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent() {
      // TODO: Unload any non ContentManager content here
    }

    bool callUpdate;

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime) {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      // TODO: Add your update logic here

      if (callUpdate) {
        sampleRocket.Update();
        callUpdate = false;
      } else {
        callUpdate = true;
      }

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      // TODO: Add your drawing code here
      spriteBatch.Begin();
      DrawRocket(sampleRocket);
      spriteBatch.End();

      base.Draw(gameTime);
    }

    private void DrawRocket(Rocket rocket) {
      if (rocket == null) {
        throw new ArgumentNullException(nameof(rocket));
      }

      var rocketWidth = 20;
      var rocketHeight = 10;

      var shapeData = new Color[rocketWidth * rocketHeight];
      var shapeTexture = new Texture2D(GraphicsDevice, rocketWidth, rocketHeight);

      for (var i = 0; i < shapeData.Length; i++) {
        shapeData[i] = Color.White;
      }

      var velocityVector = new Vector2(sampleRocket.Velocity.X, sampleRocket.Velocity.Y);
      //velocityVector.Normalize();
      var rocketAngle = (float)Math.Atan2(velocityVector.Y, velocityVector.X);

      shapeTexture.SetData(shapeData);
      spriteBatch.Draw(shapeTexture, position: rocket.CurrentLocation, color: Color.White, rotation: rocketAngle);
    }
  }
}
