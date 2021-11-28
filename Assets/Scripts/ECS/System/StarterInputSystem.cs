using Leopotam.Ecs;
using Movable;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed  class StarterInputSystem : IEcsRunSystem
{
    private readonly EcsFilter<PlayerTag, DirectionComponent> inputFiler = null;

    private Vector3 _direction;
    public void Run()
    {
        SetDirection();
        
        foreach (var filter in inputFiler)
        {
            ref var directionComponent = ref inputFiler.Get2(filter);
            ref var direction = ref directionComponent.Direction;

            direction = _direction;
        } 
    }

    private void SetDirection()
    {
        RaycastHit raycastHit;

        if (Mouse.current.leftButton.isPressed)
        {
            if (Physics.Raycast(Camera.main
                    .ScreenPointToRay(Mouse.current.position.ReadValue()),
                out raycastHit, 1000))
            {
                _direction = raycastHit.point;
            }
        }
    }
}