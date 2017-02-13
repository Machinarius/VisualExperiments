using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SmartRockets {
  class RocketLauncher {
    private const int MaxFitness = 50;

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

    private static Random random = new Random();

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

      var totalFitness = rocketsWithFitness.Select(x => x.Fitness).Sum();
      var fitnessMap = rocketsWithFitness.SelectMany(x =>
        Enumerable.Range(0, x.Fitness).Select(_ => x.Rocket).ToArray()).ToArray();

      foreach (var i in Enumerable.Range(0, RocketSwarmSwize)) {
        var firstRocket = fitnessMap[random.Next(totalFitness)];
        var secondRocket = fitnessMap[random.Next(totalFitness)];

        var breakpoint = (int)(RocketLifespan * random.NextDouble());

        var mergedMoves = new Vector2[RocketLifespan];
        foreach (var j in Enumerable.Range(0, RocketLifespan)) {
          if (random.NextDouble() < 0.01) {
            mergedMoves[j] = RocketController.GenerateRandomMove();
            continue;
          }

          if (j <= breakpoint) {
            mergedMoves[j] = firstRocket.Controller.Moves[j];
          } else {
            mergedMoves[j] = secondRocket.Controller.Moves[j];
          }
        }

        var mergedController = new RocketController(mergedMoves);
        swarm[i] = new Rocket(location, operationArea, mergedController);
      }
    }

    private int CalculateControllerFitness(Rocket rocket) {
      if (rocket == null) {
        throw new ArgumentNullException(nameof(rocket));
      }

      var distanceToTarget = Vector2.Subtract(rocket.CurrentLocation, target).Length() / 100;
      float fitness;

      if (distanceToTarget == 0) {
        fitness = MaxFitness;
      } else {
        fitness = 1 / distanceToTarget;
      }

      if (rocket.Crashed) {
        fitness /= 10;
      }

      fitness *= 10;

      if (fitness < 1) {
        fitness = 1;
      }

      if (fitness > MaxFitness) {
        fitness = MaxFitness;
      }


      fitness = (float)Math.Floor(Math.Abs(fitness));

      System.Diagnostics.Debug.WriteLine("Calculated fitness: " + fitness);
      return (int)fitness;
    }
  }
}
