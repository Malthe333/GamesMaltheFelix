using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class DistanceMover : MonoBehaviour
{
    [SerializeField] private Transform toMove;

    [HideInInspector] public bool isMoving { get; private set; } = false;
    [Header("Step values")]
    [SerializeField, Min(0.01f)] private float distance = 0.5f;
    [SerializeField] private AnimationCurve stepCurve;
    [SerializeField] private float stepDuration = 0.2f;

    [Header("Events")]
    [SerializeField] private UnityEvent beginStep;
    [SerializeField] private UnityEvent endStep;

    private Vector3 targetStartPoint = Vector3.zero;
    private Vector3 targetStepEndPoint = Vector3.zero;
    private float stepStart = 0f;

    private bool frozenSteps = false;

    [SerializeField] private DistanceMover otherLeg;

    private void Awake()
    {
#if UNITY_EDITOR
        if (toMove == null) Debug.LogWarning($"{name} has DistanceMover component but nothing to move");
#endif
    }

    private void Update()
    {
        CheckLegDistance();

        if (!isMoving) return;

        float currentProgress = (Time.time - stepStart) / stepDuration;

        if (currentProgress >= 1f)
        {
            toMove.position = targetStepEndPoint;
            isMoving = false;
            endStep?.Invoke();
            return;
        }

        toMove.position = Vector3.Lerp(targetStartPoint, targetStepEndPoint, currentProgress) + Vector3.up * stepCurve.Evaluate(currentProgress);
    }

    /// <summary>
    /// This should be called by the movement function whenever this character moves
    /// </summary>
    public void CheckLegDistance()
    {
        CheckLegDistance(new Vector2(transform.position.x - toMove.position.x, transform.position.z - toMove.position.z));
    }

    /// <summary>
    /// This should be called by the movement function whenever this character moves
    /// </summary>
    /// <param name="direction">The direction of the character movement</param>
    public void CheckLegDistance(Vector2 direction)
    {
        if (isMoving || frozenSteps) return;

        // If the other leg is taking a step, don't take one
        if (otherLeg != null) if (otherLeg.isMoving) return;

        if (Vector3.Distance(transform.position, toMove.position) > distance)
        {
            beginStep?.Invoke();
            isMoving = true;
            direction.Normalize();
            targetStartPoint = toMove.position;
            targetStepEndPoint = transform.position + distance * new Vector3(direction.x, 0f, direction.y);
            stepStart = Time.time;
        }
    }

    public void FreezeSteps()
    {
        frozenSteps = true;
    }

    public void ThawSteps()
    {
        frozenSteps = false;
    }

    private void FinishedJump()
    {
        endStep?.Invoke();
        isMoving = false;
    }
}
