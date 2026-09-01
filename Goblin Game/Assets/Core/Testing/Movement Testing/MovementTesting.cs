using System;
using UnityEngine;
using UnityEngine.UI;

public class MovementTesting : MonoBehaviour
{

    [SerializeField] GoblinCharacter goblinCharacter;
    [SerializeField] MovementSetting WalkSpeed;
    [SerializeField] MovementSetting WalkResponse;
    [SerializeField] MovementSetting CrouchSpeed;
    [SerializeField] MovementSetting CrouchResponse;
    [SerializeField] MovementSetting SlideStartSpeed;
    [SerializeField] MovementSetting SlideEndSpeed;
    [SerializeField] MovementSetting SlideFriction;
    [SerializeField] MovementSetting SlideGravity;
    [SerializeField] MovementSetting AirSpeed;
    [SerializeField] MovementSetting AirAcceleration;
    [SerializeField] MovementSetting JumpSpeed;
    [SerializeField] MovementSetting JumpSustainGravity;
    [SerializeField] MovementSetting Gravity;

    [SerializeField] MovementSettings[] presets;


    void Start()
    {
        SetSliders();
    }

    public void UpdateMovementSettings(MovementSettings settings)
    {
        goblinCharacter.UpdateMovementSettings(settings);
        SetSliders();
    }

    public void SetSliders()
    {
        MovementSettings settings = goblinCharacter.GetMovementSettings();
        WalkSpeed.slider.value = settings.WalkSpeed;
        WalkResponse.slider.value = settings.WalkResponse;
        CrouchSpeed.slider.value = settings.CrouchSpeed;
        CrouchResponse.slider.value = settings.CrouchResponse;
        SlideStartSpeed.slider.value = settings.SlideStartSpeed;
        SlideEndSpeed.slider.value = settings.SlideEndSpeed;
        SlideFriction.slider.value = settings.SlideFriction;
        SlideGravity.slider.value = settings.SlideGravity;
        AirSpeed.slider.value = settings.AirSpeed;
        AirAcceleration.slider.value = settings.AirAcceleration;
        JumpSpeed.slider.value = settings.JumpSpeed;
        JumpSustainGravity.slider.value = settings.JumpSustainGravity;
        Gravity.slider.value = settings.Gravity;
    }

    public void OnUpdateClicked()
    {
        MovementSettings settings = new MovementSettings()
        {
            WalkSpeed = WalkSpeed.slider.value,
            WalkResponse = WalkResponse.slider.value,

            CrouchSpeed = CrouchSpeed.slider.value,
            CrouchResponse = CrouchResponse.slider.value,

            SlideStartSpeed = SlideStartSpeed.slider.value,
            SlideEndSpeed = SlideEndSpeed.slider.value,
            SlideFriction = SlideFriction.slider.value,
            SlideGravity = SlideGravity.slider.value,

            AirSpeed = AirSpeed.slider.value,
            AirAcceleration = AirAcceleration.slider.value,

            JumpSpeed = JumpSpeed.slider.value,
            JumpSustainGravity = JumpSustainGravity.slider.value,
            Gravity = Gravity.slider.value
        };

        UpdateMovementSettings(settings);
    }

    public void OnPresetClicked(int presetIndex)
    {
        if(presetIndex > presets.Length - 1)
            return;

        UpdateMovementSettings(presets[presetIndex]);
    }
}

[Serializable]
public struct MovementSettings
{
    public float WalkSpeed;
    public float WalkResponse;

    public float CrouchSpeed;
    public float CrouchResponse;

    public float SlideStartSpeed;
    public float SlideEndSpeed;
    public float SlideFriction;
    public float SlideGravity;

    public float AirSpeed;
    public float AirAcceleration;

    public float JumpSpeed;
    public float JumpSustainGravity;
    public float Gravity;
}
