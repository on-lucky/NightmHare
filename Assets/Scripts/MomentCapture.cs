using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MomentCapture
{

    public Vector3 position;
    public AnimatorStateInfo animationState;
    public bool lookingRight;
    public float speed;

    public MomentCapture(Vector3 _position, AnimatorStateInfo _animationState, bool _lookingRight, float _speed)
    {
        position = _position;
        animationState = _animationState;
        lookingRight = _lookingRight;
        speed = _speed;
    }
}