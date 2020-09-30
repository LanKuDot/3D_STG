using UnityEngine;

/// <summary>
/// Make the object move and rotate smoothly
/// </summary>
public class SmoothMove
{
    private readonly float _movingVelocity;
    private readonly float _movingAccelTime;
    private readonly float _rotatingAccelTime;

    private Vector2 _curMovingDirection = Vector2.zero;
    private Vector2 _movingChangingVelocity = Vector2.zero;
    private float _rotatingChangingVelocity = 0.0f;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="movingVelocity">The maximum moving velocity of the object</param>
    /// <param name="movingAccelTime">The time duration that makes the object
    /// speed up to the <c>movingVelocity</c> and the specified direction</param>
    /// <param name="rotatingAccelTime">The time duration that makes the object
    /// rotate to the specified degree</param>
    public SmoothMove(
        float movingVelocity, float movingAccelTime, float rotatingAccelTime)
    {
        _movingVelocity = movingVelocity;
        _movingAccelTime = movingAccelTime;
        _rotatingAccelTime = rotatingAccelTime;
    }

    /// <summary>
    /// Get the distance to move the object smoothly toward the specified direction
    /// </summary>
    /// <param name="toDirection">The moving direction which is an unit vector</param>
    /// <param name="deltaTime">The time interval for moving</param>
    /// <returns>The delta distance in the specified time interval</returns>
    public Vector2 MoveDelta(Vector2 toDirection, float deltaTime)
    {
        if (toDirection.magnitude < 0.01f && _curMovingDirection.magnitude < 0.01f)
            return Vector2.zero;

        _curMovingDirection = Vector2.SmoothDamp(
            _curMovingDirection, toDirection, ref _movingChangingVelocity,
            _movingAccelTime, Mathf.Infinity, deltaTime);
        return _movingVelocity * deltaTime * _curMovingDirection;
    }

    /// <summary>
    /// Get the rotating degree to rotate the object from one degree to another degree
    /// </summary>
    /// <param name="fromDeg">The degree to rotate from</param>
    /// <param name="toDeg">The degree to rotate to</param>
    /// <param name="deltaTime">The time interval for rotating</param>
    /// <returns>The delta degree in the specified time interval</returns>
    public float RotateDelta(float fromDeg, float toDeg, float deltaTime)
    {
        var nextDeg = Mathf.SmoothDampAngle(
            fromDeg, toDeg, ref _rotatingChangingVelocity,
            _rotatingAccelTime, Mathf.Infinity, deltaTime);
        return nextDeg - fromDeg;
    }
}
