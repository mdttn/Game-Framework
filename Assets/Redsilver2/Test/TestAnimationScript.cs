using RedSilver2.Framework.Inputs;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TestAnimationScript : MonoBehaviour
{
    [SerializeField] private AnimationClip animation01, animation02;
    private bool flip;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetKeyDown(KeyboardKey.Space)) 
        {
            flip = !flip;

            animator.CrossFadeAnimation(flip ? animation01 : animation02, 0.5f,
                     () => { Debug.LogWarning($"Start Animation " + (flip ? "Animation 01" : "Animation 02")); },
                     () => { Debug.LogWarning($"Stopped Animation " + (flip ? "Animation 01" : "Animation 02")); });
        }
    }
}
