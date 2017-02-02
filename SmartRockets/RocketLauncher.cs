using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SmartRockets {
  class RocketLauncher {
    private const int RocketLifespan = 200;
    private const int RocketSwarmSwize = 50;

    public IEnumerable<Rocket> Rockets => swarm;

    private Rocket[] swarm;

    private Rectangle operationArea;
    private Vector2 location;

    private Vector2 target;

    private int updateTicks;


    public RocketLauncher(Vector2 location, Rectangle operationArea, Vector2 target) {
      if (!operationArea.Contains(location)) {
        throw new ArgumentException("The launcher location must be inside the operating area", nameof(location));
      }

      this.location = location;
      this.operationArea = operationArea;
      this.target = target;

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
      if (swarm == null) {
        swarm = new Rocket[RocketSwarmSwize];
        for (var i = 0; i < RocketSwarmSwize; i++) {
          swarm[i] = new Rocket(location, operationArea, new RocketController(RocketLifespan));
        }

        return;
      }

      var rocketsWithFitness = swarm.Select(rocket => new {
        Rocket = rocket,
        Fitness = CalculateControllerFitness(rocket)
      }).ToArray();
    }

    private float CalculateControllerFitness(Rocket rocket) {
      if (rocket == null) {
        throw new ArgumentNullException(nameof(rocket));
      }

      var distanceToTarget = Vector2.Subtract(rocket.CurrentLocation, target).Length();
      float fitness;

      if (distanceToTarget != 0) {
        fitness = 1 / distanceToTarget;
      } else {
        fitness = 1;
      }

      if (fitness < 1 || rocket.Crashed) {
        fitness = 1;
      }

      return fitness;
    }
  }
}
