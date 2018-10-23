using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MomentCapture
{

    public Vector3 position;
    public AnimatorStateInfo animationState;
    public float speed;
    public bool lookingRight;

    public MomentCapture(Vector3 _position, AnimatorStateInfo _animationState, float _speed, bool _lookingRight)
    {
        position = _position;
        animationState = _animationState;
        speed = _speed;
        lookingRight = _lookingRight;
    }
}