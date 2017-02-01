using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace SmartRockets {
  class RocketController {
    private static Random movesCreator = new Random();

    private int lifespan;
    private Vector2[] moves;

    public RocketController(int lifespan) {
      this.lifespan = lifespan;

      moves = new Vector2[lifespan];
      for (var i = 0; i < lifespan; i++) {
        var headingAngle = (float)movesCreator.NextDouble() * 360f;
        var yHeading = Math.Sin(headingAngle);
        var xHeading = Math.Cos(headingAngle);

        moves[i] = new Vector2((float)xHeading, (float)yHeading);
      }
    }

    private int currentIteration;

    public Vector2 GetNextHeading() {
      return moves[currentIteration++];
    }
  }
}
