using System;
using Microsoft.Xna.Framework;

namespace SmartRockets {
  class Rocket {
    const float MaxSpeed = 1;

    public Vector2 CurrentLocation { get; private set; }
    public Vector2 Velocity { get; private set; }

    private RocketController controller;

    public Rocket(Vector2 location, RocketController controller) {
      if (controller == null) {
        throw new ArgumentNullException(nameof(controller));
      }

      CurrentLocation = location;
      this.controller = controller;
    }

    public void Update() {
      var heading = controller.GetNextHeading();

      Velocity = Vector2.Add(Velocity, heading);
      CurrentLocation = Vector2.Add(CurrentLocation, Velocity);

      Velocity = Vector2.Clamp(Velocity, new Vector2(MaxSpeed * -1), new Vector2(MaxSpeed));
    }
  }
}
