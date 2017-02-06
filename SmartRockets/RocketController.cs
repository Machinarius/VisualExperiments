using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SmartRockets {
  class RocketController {
    private static Random movesCreator = new Random();

    private int lifespan;

    public Vector2[] Moves { get; private set; }

    public static Vector2 GenerateRandomMove() {
      var headingAngle = (float)movesCreator.NextDouble() * 360f;
      var yHeading = Math.Sin(headingAngle);
      var xHeading = Math.Cos(headingAngle);

      return new Vector2((float)xHeading, (float)yHeading);
    }

    public RocketController(int lifespan) {
      this.lifespan = lifespan;
      Moves = Enumerable.Range(0, lifespan).Select(_ => GenerateRandomMove()).ToArray();
    }

    public RocketController(Vector2[] moves) {
      if (moves == null) {
        throw new ArgumentNullException(nameof(moves));
      }

      lifespan = moves.Length;
      Moves = moves;
    }

    private int currentIteration;

    public Vector2 GetNextHeading() {
      return Moves[currentIteration++];
    }
  }
}
