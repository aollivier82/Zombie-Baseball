using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualComponent : MonoBehaviour
{
    public GameObject visualParent;
    public GameObject parent;
    public AnimationCurve animationBounce;
    public float animationSpeed = 2f;
    public float animationScale = 1f;
    private Vector3 _transform;
    private Quaternion _rotation;
    private float timer;

    private Vector3 lastTransform;


    private void Start()
    {
        _transform = visualParent.transform.localPosition;
        _rotation = visualParent.transform.rotation;
    }

    private void Update()
    {
        timer += Time.deltaTime * animationSpeed;
        if(timer > 1)
        {
            timer = 0;
        }

        Vector3 scale = transform.localScale;
        if (lastTransform.x > transform.position.x)
        {
            scale.x = (Mathf.Abs(scale.x)); 
        }
        else
        {
            scale.x = -(Mathf.Abs(scale.x));
        }
        transform.localScale = scale;


        transform.rotation = _rotation;
        transform.position = parent.transform.position + Vector3.forward * animationScale * animationBounce.Evaluate(timer);

        lastTransform = transform.position;
    }
}
