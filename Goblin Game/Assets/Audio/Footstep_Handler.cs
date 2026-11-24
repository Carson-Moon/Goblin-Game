using UnityEngine;

public class Footstep_Handler : MonoBehaviour
{
    [SerializeField] private GoblinCharacter goblinCharacter;
    [SerializeField] private float footstepIntervalMin = 0.1f; // minimum interval between footsteps
    [SerializeField] private float footstepIntervalMax = 0.8f; // maximum interval between footsteps
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepClips;

    float speedFactor;
    float currentInterval;
    float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (footstepAudioSource == null)
        {
            footstepAudioSource = GetComponent<AudioSource>();
        }
        if (footstepClips == null || footstepClips.Length == 0)
        {
            Debug.LogError("Footstep clips are not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (goblinCharacter.getStance() == Stance.Stand && goblinCharacter.HorizontalVelocityMagnitude() > 0.1f)
        {
            // play a footstep sound, speeding up as the horizontalVelocityMagnitude increases
            speedFactor = Mathf.Clamp01(goblinCharacter.HorizontalVelocityMagnitude() / goblinCharacter.walkSpeed);
            currentInterval = Mathf.Lerp(footstepIntervalMax, footstepIntervalMin, speedFactor);
            timer += Time.deltaTime;

            if (timer >= currentInterval)
            {
                footstepAudioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
                timer = 0f;
            }
            
        }
    }
}
