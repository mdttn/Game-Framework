using RedSilver2.Framework.StateMachines;
using UnityEngine;

public class SwayMotion : MovementStateExtension
{
    [SerializeField] private float defaultLerpSpeed;

    [Space]
    [SerializeField] private Vector2 min;
    [SerializeField] private Vector2 max;

    private Vector3 original;
    private Vector3 desired;

    protected float DefaultLerpSpeed => defaultLerpSpeed;
    protected float MinX => original.x - min.x;
    protected float MaxX => original.x + max.x;
    protected float MinY => original.y - min.y;
    protected float MaxY => original.y + max.y;
    protected Vector3 Original => original;

    protected override void Start()
    {
        base.Start();
        SetOriginal(transform.localPosition);

        if (movementController != null)
        {
            MovementStateMachine stateMachine = movementController.GetStateMachine() as MovementStateMachine;
            stateMachine?.AddOnMovedListener(OnUpdate);
            stateMachine?.AddOnLateUpdateListener(OnLateUpdate);
        }
    }

    public void SetOriginal(Vector3 localPosition)
    {
        this.original = localPosition;
    }

    public void SetMinPosition(Vector2 minPosition)
    {
        this.min = minPosition;
    }

    public void SetMaxPosition(Vector2 maxPosition)
    {
        this.max = maxPosition;
    }

    protected void OnLateUpdate()  {

        OnLateUpdate(desired);
    }

    protected void OnUpdate(Vector2 vector)
    {
        OnUpdate(vector, ref desired);
    }



    protected void OnLateUpdate(Vector3 desired) {
        transform.localPosition = desired;
    }

    protected void OnUpdate(Vector3 movementVector, ref Vector3 desired)
    {
        float x = Original.x, y = Original.y;
        movementVector.Normalize();

        if (movementVector.magnitude > 0f) UpdatePosition(ref x, ref y);

        float nextPositionX = Mathf.Lerp(desired.x, x, Time.deltaTime * DefaultLerpSpeed);
        float nextPositionY = Mathf.Lerp(desired.y, y, Time.deltaTime * DefaultLerpSpeed);

        desired = Vector3.right * nextPositionX + Vector3.up * nextPositionY + Vector3.forward * Original.z;  
    }

    protected virtual void UpdatePosition(ref float x, ref float y) 
    {
        float sin    = Mathf.Sin(Time.time * DefaultLerpSpeed);
        float absSin = Mathf.Abs(sin);

        if (sin < 0f) {
            y = Mathf.Lerp(Original.y, MinY, absSin);
            x = Mathf.Lerp(Original.x, MinX, absSin);
        }
        else if (sin > 0f)  {
            y = Mathf.Lerp(Original.y, MaxY, absSin);
            x = Mathf.Lerp(Original.x, MaxX, absSin);
        }
    }

    protected override void OnDisable()
    {
        if (movementController != null)
        {
            MovementStateMachine stateMachine = movementController.GetStateMachine() as MovementStateMachine;
            stateMachine?.RemoveOnMovedListener(OnUpdate);
            stateMachine?.RemoveOnLateUpdateListener(OnLateUpdate);
        }
    }

    protected override void OnEnable()
    {
        if (movementController != null)
        {
            MovementStateMachine stateMachine = movementController.GetStateMachine() as MovementStateMachine;
            stateMachine?.AddOnMovedListener(OnUpdate);
            stateMachine?.AddOnLateUpdateListener(OnLateUpdate);
        }
    }

    protected override void OnStateMachineAdded(MovementStateMachine stateMachine)
    {
        throw new System.NotImplementedException();
    }
}
