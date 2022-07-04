using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class AsteroidSplittingSystem : ReactiveSystem<GameEntity>
{
    readonly Contexts _contexts;
    readonly IGroup<GameEntity> _scores;

    public AsteroidSplittingSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _scores = contexts.game.GetGroup(GameMatcher.Score);
    }
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Hitpoints, GameMatcher.Asteroid));
    }
    protected override bool Filter(GameEntity entity)
    {
        return entity.hasAsteroid && entity.hasHitpoints;
    }
    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (e.hitpoints.hp <= 0)
            {
                Split(e);
            }
        }
    }

    private void Split(GameEntity entity)
    {
        entity.isDestroyed = true;
        ScorePoints(10);

       // _pool.CreateAsteroidDebrisEffect(entity.position.x, entity.position.y);

        if (entity.asteroid.size == AsteroidSize.Tiny)
            return;

        var newSize = AsteroidData.GetNextSizeDown(entity.asteroid.size);
        var newCollisionRadius = AsteroidData.Radii[newSize];
        var randomAngle = UnityEngine.Random.insideUnitCircle.normalized;

        CreateAsteroid(entity, newSize, randomAngle * newCollisionRadius);
        CreateAsteroid(entity, newSize, randomAngle * newCollisionRadius * -1);
    }

    private void ScorePoints(int number)
    {
        var score = _scores.GetSingleEntity();
        score.ReplaceScore(score.score.score + number);
    }

    private void CreateAsteroid(GameEntity oldAsteroid, AsteroidSize size, Vector2 vec)
    {
        var newAsteroid = _contexts.game.CreateEntity();
        newAsteroid.AddAsteroid(size);
        newAsteroid.AddPosition(new Vector3(oldAsteroid.position.value.x + vec.x, oldAsteroid.position.value.y + vec.y, 0));
        newAsteroid.AddHitpoints(1);
        newAsteroid.AddCollisionRadius(AsteroidData.Radii[size]);
        newAsteroid.isWrappedAroundGameBounds = true;
        newAsteroid.AddResource("prefabs/" + AsteroidData.Resources[size]);
        
        ImpartInitialForce(oldAsteroid, newAsteroid, vec);
    }

    private void ImpartInitialForce(GameEntity oldAsteroid, GameEntity newAsteroid, Vector2 vec)
    {
        var oldVel = oldAsteroid.rigidbody.rigidbody.velocity;
        var torque = (vec.x + UnityEngine.Random.Range(-1, 1)) * 0.2f;
        newAsteroid.ReplaceForce(new List<Vector2> { vec, oldVel * 0.5f }, torque);
    }


}
