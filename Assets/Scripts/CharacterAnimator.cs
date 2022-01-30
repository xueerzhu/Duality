using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;
    private bool state;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");

            if(state) _animator.SetTrigger("TriggerLower");
            else _animator.SetTrigger("TriggerLift");

            state = !state;
        }
    }
}
