using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.GraphView.GraphView;

public static class AnimationManager {
    private static readonly Dictionary<Animator, string> currentAnimations = new Dictionary<Animator, string>();

    public static void PlayAnimation(this Animator animator, string animationName) {
        PlayAnimation(animator, GetAnimationClip(animator, animationName));
    }
    public static async void PlayAnimation(this Animator animator, AnimationClip clip) {
        if (await PlayAnimationAsync(animator, clip)) ClearAnimationPlaying(animator, clip);
    }

    public static async Awaitable<bool> PlayAnimationAsync(this Animator animator, string animationName) {
        return await PlayAnimationAsync(animator, GetAnimationClip(animator, animationName));
    }

    public static async Awaitable<bool> PlayAnimationAsync(this Animator animator, AnimationClip clip)
    {
        if (animator == null || clip == null || !animator.HasAnimationClip(clip)) return false;

        SetAnimationPlaying(animator, clip);
        animator.Play(clip.name);

        await AwaitPlayAnimationAsync(animator, clip);

        if (!currentAnimations.ContainsKey(animator)) return false;
        return currentAnimations[animator] == clip.name;
    }

    private static async Awaitable AwaitPlayAnimationAsync(Animator animator, AnimationClip clip)
    {
        float currentAnimationTime = 0f;

        while (animator != null && clip != null) {
            if (!animator.CompareCurrentAnimationName(clip) || currentAnimationTime >= clip.length) break;
            if (!clip.isLooping) currentAnimationTime += Time.deltaTime;
            await Awaitable.NextFrameAsync();
        }

    }

    public static async void PlayAnimation(this Animator animator, string animationName, UnityAction onStarted)
    {
        await AsyncPlayAnimation(animator, animationName, onStarted);
    }
    public static async void PlayAnimation(this Animator animator, string animationName, UnityAction onStarted, UnityAction onFinished)
    {
        await AsyncPlayAnimation(animator, animationName, onStarted, onFinished);
    }

    public static async void PlayAnimation(this Animator animator, AnimationClip clip, UnityAction onStarted) {
        await AsyncPlayAnimation(animator, clip, onStarted);
    }
    public static async void PlayAnimation(this Animator animator, AnimationClip clip, UnityAction onStarted, UnityAction onFinished) {
        await PlayAnimationAsync(animator, clip, onStarted, onFinished);
    }

    public static async Awaitable<bool> AsyncPlayAnimation(this Animator animator, string animationName, UnityAction onStarted)
    {
        return await AsyncPlayAnimation(animator, GetAnimationClip(animator, animationName), onStarted);
    }
    public static async Awaitable<bool> AsyncPlayAnimation(this Animator animator, string animationName, UnityAction onStarted, UnityAction onFinished)
    {
        return await PlayAnimationAsync(animator, GetAnimationClip(animator, animationName), onStarted, onFinished);
    }

    public static async Awaitable<bool> AsyncPlayAnimation(this Animator animator, AnimationClip clip, UnityAction onStarted) {
        if (onStarted != null) onStarted.Invoke();
        return await PlayAnimationAsync(animator, clip);
    }
    public static async Awaitable<bool> PlayAnimationAsync(this Animator animator, AnimationClip clip, UnityAction onStarted, UnityAction onFinished)
    {
        if (!await AsyncPlayAnimation(animator, clip, onStarted)) {
            ClearAnimationPlaying(animator, clip);
            if (onFinished != null) onFinished.Invoke();
            return true;
        }

        return false;
    }

    public static void CrossFadeAnimation(this Animator animator, string animationName, float crossFadeTime)
    {
        CrossFadeAnimation(animator, GetAnimationClip(animator, animationName), crossFadeTime);
    }
    public static async void CrossFadeAnimation(this Animator animator, AnimationClip clip, float crossFadeTime)
    {
        bool isFinished = await AsyncCrossFadeAnimation(animator, clip, crossFadeTime);
        if (isFinished) ClearAnimationPlaying(animator, clip.name);
    }

    public static async Awaitable<bool> AsyncCrossFadeAnimation(this Animator animator, string animationName, float crossFadeTime)
    {
        return await AsyncCrossFadeAnimation(animator, GetAnimationClip(animator, animationName), crossFadeTime);
    }
    public static async Awaitable<bool> AsyncCrossFadeAnimation(this Animator animator, AnimationClip clip, float crossFadeTime)
    {
        if (animator == null || clip == null || !animator.HasAnimationClip(clip)) return false;

        SetAnimationPlaying(animator, clip);
        animator.CrossFade(clip.name, crossFadeTime);

        await AwaitCrossFadeAnimation(animator, clip, crossFadeTime);

        if (!currentAnimations.ContainsKey(animator)) return false;
        return currentAnimations[animator] == clip.name;
    }

    private static async Awaitable AwaitCrossFadeAnimation(Animator animator, AnimationClip clip, float crossFadeTime) {

        float currentCrossFadeTime = 0, currentAnimationTime = 0;

        while (animator != null && clip != null) {
            if (!animator.CompareCurrentAnimationName(clip) || currentAnimationTime >= clip.length)  break;
       
            currentCrossFadeTime = Mathf.Clamp(Time.deltaTime + crossFadeTime, 0f, crossFadeTime);    
            if (currentCrossFadeTime >= crossFadeTime && !clip.isLooping) currentAnimationTime += Time.deltaTime;
            await Awaitable.NextFrameAsync();
        }
    }

    public static async void CrossFadeAnimation(this Animator animator, string animationName, float crossFadeTime, UnityAction onStarted)  {
        await AsyncCrossFadeAnimation(animator, animationName, crossFadeTime, onStarted);
    }
    public static async void CrossFadeAnimation(this Animator animator, string animationName, float crossFadeTime, UnityAction onStarted, UnityAction onFinished) {
        await AsyncCrossFadeAnimation(animator, animationName, crossFadeTime, onStarted, onFinished);
    }

    public static async void CrossFadeAnimation(this Animator animator, AnimationClip clip, float crossFadeTime, UnityAction onStarted) {
        await AsyncCrossFadeAnimation(animator, clip, crossFadeTime, onStarted);
    }

    public static async void CrossFadeAnimation(this Animator animator, AnimationClip clip, float crossFadeTime, UnityAction onStarted, UnityAction onFinished) {
        await AsyncCrossFadeAnimation(animator, clip, crossFadeTime, onStarted, onFinished);
    }



    public static async Awaitable<bool> AsyncCrossFadeAnimation(this Animator animator, string animationName, float crossFadeTime, UnityAction onStarted)
    {
        return await AsyncCrossFadeAnimation(animator, GetAnimationClip(animator, animationName), crossFadeTime, onStarted);
    }
    public static async Awaitable<bool> AsyncCrossFadeAnimation(this Animator animator, string animationName, float crossFadeTime, UnityAction onStarted, UnityAction onFinished)
    {
        return await AsyncCrossFadeAnimation(animator, GetAnimationClip(animator, animationName), crossFadeTime, onStarted, onFinished);
    }

    public static async Awaitable<bool> AsyncCrossFadeAnimation(this Animator animator, AnimationClip clip, float crossFadeTime, UnityAction onStarted)
    {
        if (onStarted != null) onStarted.Invoke();
        return await AsyncCrossFadeAnimation(animator, clip, crossFadeTime);
    }
    public static async Awaitable<bool> AsyncCrossFadeAnimation(this Animator animator, AnimationClip clip, float crossFadeTime, UnityAction onStarted, UnityAction onFinished)
    {
        bool isFinished = await AsyncCrossFadeAnimation(animator, clip, crossFadeTime, onStarted);

        if (isFinished) {
            ClearAnimationPlaying(animator, clip);
            if (onFinished != null) onFinished.Invoke();
        }

        return isFinished;
    }



    public static int GetAnimationClipLayer(this Animator animator, string animationName) {
        return GetAnimationClipLayer(animator, GetAnimationClip(animator, animationName));
    }
    public static int GetAnimationClipLayer(this Animator animator, AnimationClip clip)
    {
        if (animator == null || clip == null || animator.layerCount < 0) return default;

        AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
        if (controller == null) return -1;

        for (int i = 0; i < animator.layerCount; i++)
          if(controller.layers[i].stateMachine.states.Where(x => x.state.motion as AnimationClip == clip).Count() > 0)
                return i;

        return -1;
    }

    public static AnimationClip GetAnimationClip(this Animator animator, AnimationClip clip)
    {
        if (clip == null) return null;
        return GetAnimationClip(animator, clip.name);
    }
    public static AnimationClip GetAnimationClip(this Animator animator, string animationName) {
        if (animator == null) return null;
        return GetAnimationClip(animator.runtimeAnimatorController, animationName);
    }

    public static AnimationClip GetAnimationClip(this RuntimeAnimatorController controller, AnimationClip clip)
    {
        if (clip == null) return null;
        return GetAnimationClip(controller, clip.name);
    }
    public static AnimationClip GetAnimationClip(this RuntimeAnimatorController controller, string animationName)
    {
        if (controller == null || string.IsNullOrEmpty(animationName)) return null;

        var results = controller.animationClips.Where(x => x != null)
                                               .Where(x => x.name.ToLower() == animationName.ToLower());
        if (results.Count() == 0) return null;
        return results.First();
    }

    public static string[] GetAnimationNames(this Animator animator)
    {
         List<string> results = new List<string>();
         if(animator == null) return results.ToArray();

        for(int i = 0; i < animator.layerCount; i++)
            foreach(string animationName in animator.GetAnimationNames(i))
                results.Add(animationName);

        return results.ToArray();
    }

    public static string[] GetAnimationNames(this Animator animator, int layer)
    {
        List<string> results = new List<string>();
        AnimatorController controller = animator == null ? null : animator.runtimeAnimatorController as AnimatorController;
        if (animator == null || controller == null || layer < 0 || layer >= animator.layerCount) results.ToArray();

        foreach (ChildAnimatorState state in controller.layers[layer].stateMachine.states)
            if     (state.state.motion is AnimationClip) AddAnimationName  (state.state.motion as AnimationClip, ref results);
            else if(state.state.motion is BlendTree)     AddAnimationNames (state.state.motion as BlendTree    , ref results);             
        
        return results.ToArray();
    }

    private static void AddAnimationNames(BlendTree tree, ref List<string> results) {
        if (tree == null) return;

        foreach(ChildMotion motion in tree.children) 
            AddAnimationName(motion.motion as AnimationClip, ref results);
    }

    private static void AddAnimationName(AnimationClip clip, ref List<string> results)
    {
        if(results == null) results = new List<string>();
        results.Add(clip == null ? string.Empty : clip.name);
    }

    public static bool HasAnimationClip(this Animator animator, AnimationClip clip)
    {
        if (clip == null) return false;
        return HasAnimationClip(animator, clip.name);
    }
    public static bool HasAnimationClip(this Animator animator, string animationName)
    {
        if (animator == null) return false;
        return HasAnimationClip(animator.runtimeAnimatorController, animationName);
    }

    public static bool HasAnimationClip(this Animator animator, string animationName, int layer)
    {
        return HasAnimationClip(animator, GetAnimationClip(animator, animationName), layer);
    }
    public static bool HasAnimationClip(this Animator animator, AnimationClip clip, int layer) {

        AnimatorController controller = animator == null ? null : animator.runtimeAnimatorController as AnimatorController;
        if (animator == null || controller == null || layer < 0 || layer >= animator.layerCount) return false;

        return controller.layers[layer].stateMachine.states.Where(x => x.state.motion as AnimationClip == clip)
                                                            .Count() > 0;
    }

    public static bool HasAnimationClip(this RuntimeAnimatorController controller, AnimationClip clip)
    {
        if (clip != null) return false;
        return HasAnimationClip(controller, clip.name);
    }

    public static bool HasAnimationClip(this RuntimeAnimatorController controller, string animationName)
    {
        if (controller == null || string.IsNullOrEmpty(animationName)) return false;

        return controller.animationClips
                         .Where(x => x != null)
                         .Where(x => x.name.ToLower() == animationName.ToLower())
                         .Count() > 0;
    }

    public static bool CompareCurrentAnimationName(this Animator animator, string animationName)
    {
        if(animator == null || string.IsNullOrEmpty(animationName) || !currentAnimations.ContainsKey(animator))
            return false;

        return currentAnimations[animator].ToLower() == animationName.ToLower();
    }


    public static bool CompareCurrentAnimationName(this Animator animator, AnimationClip clip)
    {
        if(animator == null || clip == null) return false;  
        return animator.CompareCurrentAnimationName(clip.name);
    }

    public static bool TryGetAnimationClip(this Animator animator, AnimationClip clip, out AnimationClip result)
    {
        result = null;
        if (clip == null) return false;
        return TryGetAnimationClip(animator, clip.name, out result);
    }
    public static bool TryGetAnimationClip(this Animator animator, string animationName, out AnimationClip result)
    {
        result = null;

        if (animator == null) return false;
        return TryGetAnimationClip(animator.runtimeAnimatorController, animationName, out result);
    }

    public static bool TryGetAnimationClip(this RuntimeAnimatorController controller, AnimationClip clip, out AnimationClip result)
    {
        result = null;
        if (clip == null) return false;
        return TryGetAnimationClip(controller, clip.name, out result);
    }
    public static bool TryGetAnimationClip(this RuntimeAnimatorController controller, string animationName, out AnimationClip result)
    {
        result = GetAnimationClip(controller, animationName);
        return result != null;
    }

    private static void SetAnimationPlaying(Animator animator, AnimationClip clip)
    {
        SetAnimationPlaying(animator, clip == null ? string.Empty : clip.name);
    }

    private static void SetAnimationPlaying(Animator animator, string animationName)
    {
        if (animator == null) return; 
        if (!currentAnimations.ContainsKey(animator)) { currentAnimations.Add(animator, string.Empty); }
        currentAnimations[animator] = string.IsNullOrEmpty(animationName) ? string.Empty : animationName;
    }

    private static void ClearAnimationPlaying(Animator animator, AnimationClip clip)
    {
        if(clip == null) return;
        ClearAnimationPlaying(animator, clip.name);
    }

    private static void ClearAnimationPlaying(Animator animator, string animationName)
    {
        if (animator == null || !currentAnimations.ContainsKey(animator)) return;

        string clipName = string.IsNullOrEmpty(animationName) ? string.Empty : animationName;
        if (currentAnimations[animator] == clipName) { SetAnimationPlaying(animator, string.Empty); }
    }
}
