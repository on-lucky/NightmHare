using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MomentCapture
{

    public Vector3 position;
    public AnimatorStateInfo animationState;
    public bool lookingRight;

    public MomentCapture(Vector3 _position, AnimatorStateInfo _animationState, bool _lookingRight)
    {
        position = _position;
        animationState = _animationState;
        lookingRight = _lookingRight;
    }
}