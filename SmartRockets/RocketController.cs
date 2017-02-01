using System;
using Microsoft.Xna.Framework;

namespace SmartRockets {
  class RocketController {
    float currentAngle;

    public Vector2 GetNextHeading() {
      var tangentY = (float)Math.Cos(currentAngle) * 0.1f;
      var tangentVector = new Vector2(-2f, tangentY);
      currentAngle += 0.1f;

      return tangentVector;
    }
  }
}
