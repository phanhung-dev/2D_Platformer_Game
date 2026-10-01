using UnityEngine;
using UnityEngine.Rendering;

public class PlayOneShotBehaviour : StateMachineBehaviour
{
    public AudioClip soundToPlay;
    public float volume = 1.0f;
    public bool playOnEnter = true;
    public bool playOnExit = false;
    public bool playAfterDelay = false;

    public float playDelay = 0.25f;
    private float timeSinceStarted = 0f;
    private bool hasDelayedSoundPlayer = false;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnEnter)
        {
            Play2DSound(soundToPlay, volume);
        }
        timeSinceStarted = 0f;
        hasDelayedSoundPlayer = false;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playAfterDelay && !hasDelayedSoundPlayer)
        {
            timeSinceStarted += Time.deltaTime;

            if (timeSinceStarted > playDelay)
            {
                Play2DSound(soundToPlay, volume);
                hasDelayedSoundPlayer = true;
            }
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnExit)
        {
            Play2DSound(soundToPlay, volume);
        }
    }

    private void Play2DSound(AudioClip clip, float vol)
    {
        if (clip == null) return;

        GameObject audioObject = new GameObject("TempAudio2D");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = vol;

        audioSource.spatialBlend = 0f;
        audioSource.Play();

        Destroy(audioObject, clip.length);
    }
}
