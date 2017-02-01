﻿using System;
using Microsoft.Xna.Framework;

namespace SmartRockets {
  class Rocket {
    const float MaxSpeed = 2;

    public Vector2 CurrentLocation { get; private set; }

    public Vector2 Velocity { get; private set; }

    public bool Crashed { get; private set; }

    private RocketController controller;
    private Rectangle operatingArea;

    public Rocket(Vector2 location, Rectangle operatingArea, RocketController controller) {
      if (controller == null) {
        throw new ArgumentNullException(nameof(controller));
      }

      CurrentLocation = location;
      this.controller = controller;
      this.operatingArea = operatingArea;
    }

    public void Update() {
      if (Crashed) {
        return;
      }
      
      var heading = controller.GetNextHeading();

      Velocity = Vector2.Add(Velocity, heading);
      Velocity = Vector2.Clamp(Velocity, new Vector2(MaxSpeed * -1), new Vector2(MaxSpeed));
      CurrentLocation = Vector2.Add(CurrentLocation, Velocity);

      if (!operatingArea.Contains(CurrentLocation)) {
        Crashed = true;
        return;
      }
    }
  }
}
