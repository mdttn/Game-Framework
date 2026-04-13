using RedSilver2.Framework;
using RedSilver2.Framework.StateMachines;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.Stats;
using System.Linq;
using UnityEngine;

public class Stalker : AIMovementController
{
    [Space]
    [SerializeField] private Transform[] resetSpots;

    [Space]
    [SerializeField] private float damageAmount;

    [Space]
    [SerializeField] private float minAgressionTime;
    [SerializeField] private float maxAgressionTime;

    [Space]
    [SerializeField] private float runSpeed     = 200f;
    [SerializeField] private float defaultSpeed = 10f;

    [Space]
    [SerializeField] private float maxRestDistanceToPlayer;
    private float currentAggressionTime;

    private void Start() {
        Physics.IgnoreLayerCollision(GameManager.PLAYER_LAYER, GameManager.AI_LAYER, true);
        SetWaypoints(resetSpots);

        RandomizeAggressionTime();
        SetTarget(GetRandomWaypoint());
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        UpdateAggressionTime();

        if (IsCloseToTarget()) {
            if (IsTargetPlayer(out PlayerController controller))
                HitPlayer(controller);
            else
                SetTarget(GetRandomWaypoint());
        }
    }

    private void HitPlayer(PlayerController controller) {
        if(controller == null) 
            return; 

        if(controller.TryGetComponent(out PlayerHealth health)) {
            health?.Hit(damageAmount);
        }

        RandomizeAggressionTime();
        SetTarget(GetFurthestWaypointFromPlayer(controller));
    }

    private void UpdateAggressionTime() {
        currentAggressionTime = Mathf.Clamp(currentAggressionTime - Time.deltaTime, 0f, float.MaxValue);

        if (currentAggressionTime <= 0f) {
            PlayerController controller = PlayerController.Current;
            SetTarget(controller == null ? null : controller.transform);
            SetSpeed(runSpeed);
        }
        else
        {
            float distanceToPlayer = GetDistanceToPlayer();

            if (distanceToPlayer <= maxRestDistanceToPlayer) {
                SetTarget(GetFurthestWaypointFromPlayer(PlayerController.Current));
                SetSpeed(runSpeed);
            }
            else if(distanceToPlayer >= maxRestDistanceToPlayer + 10f) {
                SetSpeed(defaultSpeed);
            }
        }
    }

    private float GetDistanceToPlayer()
    {
        PlayerController controller = PlayerController.Current;
        if (controller == null) return Mathf.Infinity;

        return Vector3.Distance(transform.position, controller.transform.position);
    }

    private void RandomizeAggressionTime() {
        currentAggressionTime = Random.Range(minAgressionTime, maxAgressionTime);
    }

    public Transform GetFurthestWaypointFromPlayer(PlayerController controller)
    {
        Transform[] waypoints = GetWaypoints();
        if(waypoints == null || waypoints.Length == 0 || controller == null) return null;

        var results = waypoints.Where(x => x != null).OrderBy(x => Vector3.Distance(x.position, controller.transform.position));
        if (results.Count() > 0) return results.Last();

        return null;
    }

    public override AIMovementStateMachine GetMovementStateMachine() {
        return new AIMovementStateMachine(this);
    }
}
