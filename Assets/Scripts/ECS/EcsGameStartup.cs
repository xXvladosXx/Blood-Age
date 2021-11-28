using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using Voody.UniLeo;

public class EcsGameStartup : MonoBehaviour
{
    private EcsWorld _ecsWorld;
    private EcsSystems _ecsSystems;

    private void Start()
    {
        _ecsWorld = new EcsWorld();
        _ecsSystems = new EcsSystems(_ecsWorld);

        AddInjections();
        AddSystems();
        AddOneFrames();
        
        _ecsSystems.ConvertScene();
        _ecsSystems.Init();
    }

    private void AddInjections()
    {
        
    }

    private void AddSystems()
    {
        _ecsSystems.Add(new StarterInputSystem()).Add(new MovementSystem());
    }

    private void AddOneFrames()
    {
        
    }
    

    private void Update()
    {
        _ecsSystems.Run();
    }

    private void OnDestroy()
    {
        if(_ecsSystems == null) return;
        
        _ecsSystems.Destroy();
        _ecsSystems = null;
        
        _ecsWorld.Destroy();
        _ecsWorld = null;
    }
    
}
