using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UI Animation Data", menuName = "UI Animation Data")]
public class UIAnimationData : ScriptableObject
{
    public TweenFunction tweenFunction;
    public Vector3 endValue;
    public float delay;
    public float duration;
    public LeanTweenType easeType;
    public float overshoot;


    public delegate LTDescr TweenFunctionDelegate(GameObject target, Vector3 endValue, float duration);
    public TweenFunctionDelegate tweenFunctionDelegate;

    private static readonly Dictionary<TweenFunction, TweenFunctionDelegate> tweenFunctionDelegates = new Dictionary<TweenFunction, TweenFunctionDelegate>()
{
    { TweenFunction.Move, (target, endValue, duration) => LeanTween.move(target, endValue, duration) },
    { TweenFunction.MoveLocal, (target, endValue, duration) => LeanTween.moveLocal(target, endValue, duration) },
    { TweenFunction.MoveX, (target, endValue, duration) => LeanTween.moveX(target, endValue.x, duration) },
    { TweenFunction.MoveY, (target, endValue, duration) => LeanTween.moveY(target, endValue.y, duration) },
    { TweenFunction.MoveZ, (target, endValue, duration) => LeanTween.moveZ(target, endValue.z, duration) },
    { TweenFunction.Rotate, (target, endValue, duration) => LeanTween.rotate(target, endValue, duration) },
    { TweenFunction.RotateLocal, (target, endValue, duration) => LeanTween.rotateLocal(target, endValue, duration) },
    { TweenFunction.RotateX, (target, endValue, duration) => LeanTween.rotateX(target, endValue.x, duration) },
    { TweenFunction.RotateY, (target, endValue, duration) => LeanTween.rotateY(target, endValue.x, duration) },
    { TweenFunction.RotateZ, (target, endValue, duration) => LeanTween.rotateZ(target, endValue.x, duration) },
    { TweenFunction.Scale, (target, endValue, duration) => LeanTween.scale(target, endValue, duration) },
    { TweenFunction.ScaleX, (target, endValue, duration) => LeanTween.scaleX(target, endValue.x, duration) },
    { TweenFunction.ScaleY, (target, endValue, duration) => LeanTween.scaleY(target, endValue.y, duration) },
    { TweenFunction.ScaleZ, (target, endValue, duration) => LeanTween.scaleZ(target, endValue.z, duration) },
};

    public void OnEnable()
    {
        tweenFunctionDelegate = tweenFunctionDelegates[tweenFunction];
    }
}


public enum TweenFunction
{
    Move,
    MoveLocal,
    MoveX,
    MoveY,
    MoveZ,
    Rotate,
    RotateLocal,
    RotateX,
    RotateY,
    RotateZ,
    Scale,
    ScaleX,
    ScaleY,
    ScaleZ
}



