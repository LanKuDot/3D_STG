﻿using System;
using UnityEngine;

namespace GamePlay
{
    public class Character : MonoBehaviour
    {
        public float movingVelocity = 10.0f;
        public float movingAccelTime = 0.2f;
        public float rotatingAccelTime = 0.1f;

        private Vector2 _curMovingDirection = Vector2.zero;
        private Vector2 _curMovingVelocity = Vector2.zero;
        private float _curRotatingVelocity = 0.0f;

        /// <summary>
        /// Move the character along the <c>direction</c>.<para />
        /// It will change the direction smoothly, according to the object's <c>movingAccelTime</c>.
        /// </summary>
        /// <param name="direction">The moving direction which is an unit vector</param>
        protected void Move(Vector2 direction)
        {
            if (direction.magnitude < 0.01 && _curMovingDirection.magnitude < 0.01)
                return;

            _curMovingDirection = Vector2.SmoothDamp(
                _curMovingDirection, direction, ref _curMovingVelocity, movingAccelTime);

            var distance = _curMovingDirection * movingVelocity * Time.deltaTime;
            transform.localPosition += new Vector3(distance.x, 0, distance.y);
        }

        protected void Look(float toDeg)
        {
            var oldEulerAngle = transform.localEulerAngles;
            var curDeg = Mathf.SmoothDampAngle(
                oldEulerAngle.y, toDeg, ref _curRotatingVelocity, rotatingAccelTime);
            oldEulerAngle.y = curDeg;
            transform.localEulerAngles = oldEulerAngle;
        }
    }
}
