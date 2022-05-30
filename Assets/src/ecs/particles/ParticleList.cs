using System.Numerics;
using System.Drawing;
using Components;
using System;
using System.Collections.Generic;

public class ParticleList
{
    public Particle2dHealthComponent[] HealthList;
    public Particle2dPositionComponent[] PositionList;
    public Particle2dRotationComponent[] RotationList;
    public Particle2dScaleComponent[] ScaleList;
    public Particle2dSpriteComponent[] SpriteList;
    public Particle2dAnimationComponent[] AnimationList;
    public ParticleStateComponent[] StateList;


    // used to Queue up the particles to be removed at the end of the frame
    private List < int > ParticlesToRemove;


    public int Capacity;
    public int Size;

    private int LastFreeParticle;

    public ParticleList()
    {
        Init();
        Expand(128);
    }

    public ParticleList(int capacity)
    {
        Init();
        Expand(capacity);
    }


 
    // returns the particle id
    public int AddParticle(float decayRate, Vector2 position, Vector2 acceleration,
        Vector2 velocity,
        float rotation, float deltaRotation, float scale, float deltaScale, int spriteId,
        Color color, float animationSpeed)
    {

        // if we dont have enough space we expand
        if (Size >= Capacity)
        {
            Expand(Capacity * 2);
        }

        // trying to find an empty particle index
        // we use LastFreeParticle for a faster insertion
        int Found = -1;
        for(int particleId = LastFreeParticle; particleId < Capacity; particleId++)
        {
            ref ParticleStateComponent state = ref StateList[particleId];

            if (state.State == ParticleState.Empty)
            {
                Found = particleId;
                break;
            }
        }

        if (Found == -1)
        {
            for(int particleId = 0; particleId < LastFreeParticle; particleId++)
            {
                ref ParticleStateComponent state = ref StateList[particleId];

                if (state.State == ParticleState.Empty)
                {
                    Found = particleId;
                    break;
                }
            }
        }

        // increment the LastFreeParticle Index
        LastFreeParticle = (LastFreeParticle + 1) % Capacity;


        // here we have found an empty index
        StateList[Found] = new ParticleStateComponent(ParticleState.Running, Found);

        HealthList[Found] = new Particle2dHealthComponent(1.0f, decayRate);
        PositionList[Found] = new Particle2dPositionComponent(position, acceleration, velocity);
        RotationList[Found] = new Particle2dRotationComponent(rotation, deltaRotation);
        ScaleList[Found] = new Particle2dScaleComponent(scale, deltaScale);
        SpriteList[Found] = new Particle2dSpriteComponent(spriteId, color);
        AnimationList[Found] = new Particle2dAnimationComponent(0.0f, animationSpeed);
        

        Size++;

        return Found;
    }

    // adds a particle id to the queue
    // the elements in the queue will be removed
    // once we call RemoveAllParticlesFromQueue()
    public void RemoveParticle(int id)
    {
        ParticlesToRemove.Add(id);
    }


    // called at the end of the frame to remove all the 
    // particles we have previously queued up
    public void RemoveAllParticlesFromQueue()
    {

        // if we have some particle to remove
        // get the id of the first one
        // so we can use it later for fast insertions
        if (ParticlesToRemove.Count > 0)
        {
            LastFreeParticle = ParticlesToRemove[0];
        }

        foreach(int particleIdToRemove in ParticlesToRemove)
        {
            ref ParticleStateComponent state = ref StateList[particleIdToRemove];

            if (state.State != ParticleState.Empty)
            {
                state.State = ParticleState.Empty;
                Size--;
            }
        }
        
        ParticlesToRemove.Clear();
    }





    // used to grow the particleList
    private void Expand(int NewCapacity)
    {
        if (NewCapacity == 0)
        {
            NewCapacity = 1;
        }

        if (NewCapacity > Capacity)
        {
            Capacity = NewCapacity;

            Array.Resize(ref StateList, Capacity);
            Array.Resize(ref HealthList, Capacity);
            Array.Resize(ref PositionList, Capacity);
            Array.Resize(ref RotationList, Capacity);
            Array.Resize(ref ScaleList, Capacity);
            Array.Resize(ref SpriteList, Capacity);
            Array.Resize(ref AnimationList, Capacity);
        }
    }

    private void Init()
    {
        LastFreeParticle = 0;
        Capacity = 0;
        Size = 0;

        StateList = new ParticleStateComponent[0];
        HealthList = new Particle2dHealthComponent[0];
        PositionList = new Particle2dPositionComponent[0];
        RotationList = new Particle2dRotationComponent[0];
        ScaleList = new Particle2dScaleComponent[0];
        SpriteList = new Particle2dSpriteComponent[0];
        AnimationList = new Particle2dAnimationComponent[0];

        ParticlesToRemove = new List<int>();

    }
}