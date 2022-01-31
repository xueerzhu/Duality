using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private float animationSpeed;
    private bool state;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.speed = animationSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        _animator.speed = animationSpeed;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");

            if(state) _animator.SetTrigger("TriggerLower");
            else _animator.SetTrigger("TriggerLift");

            state = !state;
        }
    }
}
