using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SmartRockets {
  class RocketLauncher {
    private const int RocketLifespan = 200;
    private const int RocketSwarmSwize = 50;

    public IEnumerable<Rocket> Rockets => swarm;

    private Rocket[] swarm;

    private Rectangle operationArea;
    private Vector2 location;

    private int updateTicks;


    public RocketLauncher(Vector2 location, Rectangle operationArea) {
      if (!operationArea.Contains(location)) {
        throw new ArgumentException("The launcher location must be inside the operating area", nameof(location));
      }

      this.location = location;
      this.operationArea = operationArea;

      InitSwarm();
    }

    public void Update() {
      for (var i = 0; i < RocketSwarmSwize; i++) {
        swarm[i].Update();
      }
      
      updateTicks++;
      if (updateTicks == RocketLifespan) {
        InitSwarm();
        updateTicks = 0;
      }
    }

    private void InitSwarm() {
      swarm = new Rocket[RocketSwarmSwize];
      for (var i = 0; i < RocketSwarmSwize; i++) {
        swarm[i] = new Rocket(location, operationArea, new RocketController(RocketLifespan));
      }
    }
  }
}
