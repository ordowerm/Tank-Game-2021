using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletState : GameState
{
    protected BulletData bd;
    public TrailRenderer trail;
    public ParticleSystem startParticle;
    public ParticleSystem endParticle;

    public BulletState(GameObject t, GameStateMachine s, BulletData bdata) : base(t, s)
    {
        bd = bdata;
    }
}
