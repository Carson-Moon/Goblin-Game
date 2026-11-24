using UnityEngine;

public class Footstep_Handler : MonoBehaviour
{
    [SerializeField] private GoblinCharacter goblinCharacter;
    [SerializeField] private float footstepIntervalMin = 0.1f; // minimum interval between footsteps
    [SerializeField] private float footstepIntervalMax = 0.8f; // maximum interval between footsteps
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepClips;
    public AudioClip[] landingClips;
    public float landingVolumeSubtractAmount = 0.5f;

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
        if (landingClips == null || landingClips.Length == 0)
        {
            Debug.LogError("Landing clips are not assigned.");
        }

        goblinCharacter.Landed += OnLanded;
    }

    // Update is called once per frame
    void Update()
    {
        if (goblinCharacter.getStance() == Stance.Stand && goblinCharacter.HorizontalVelocityMagnitude() > 0.1f && goblinCharacter.getIsGrounded())
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

    private void OnLanded(float impactSpeed)
    {
        // play a landing sound when the player lands on the ground after a jump or fall
        float volumeScale = Mathf.Clamp01(1f - landingVolumeSubtractAmount);
        footstepAudioSource.PlayOneShot(landingClips[Random.Range(0, landingClips.Length)]);
        footstepAudioSource.PlayOneShot(landingClips[Random.Range(0, landingClips.Length)], volumeScale);
    }
}
