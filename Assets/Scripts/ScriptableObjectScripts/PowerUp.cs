using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PowerUp : ScriptableObject
{
    [Required("Name must be unique among all other powerups")]
    public string name = "New Powerup";

    [Multiline]
    public string description;

    public bool isFinished;

    [SerializeField]
    private FloatVariable baseChance;

    
    [SerializeField]
    private BoolData affectedBoolData;

    [SerializeField][HideIf("IsInt")]
    private FloatData affectedFloatData;

    [SerializeField][HideIf("IsFloat")]
    private FloatData affectedIntData;

    [SerializeField]
    private bool returnToDefaultOnEnd;

    private float _defaultNumberValue;
    private bool _defaultBoolValue;

    [SerializeField][ShowIf("IsBoolean")]
    private BoolVariable newValue;



    [SerializeField][ShowIf("IsNumerical")]
    private FloatVariable immediateChange;

    [SerializeField][HideIf(EConditionOperator.And,"IsNotInstant","IsNumerical")]
    private FloatVariable changeOverTime;

    [SerializeField]
    private float duration;

    private bool IsInstant => duration > 0;
    private bool IsNumerical => (affectedFloatData != null || affectedIntData != null);

    private bool IsFloat => affectedFloatData != null;
    private bool IsInt => affectedIntData != null;
    private bool IsBoolean => affectedBoolData != null;

    private float _beginningTime;
    private float _currentTime;


    public void Activate()
    {

        if(IsFloat)
        {
            _defaultNumberValue = affectedFloatData.Value;
            affectedFloatData.ApplyChange(immediateChange);
        }
        else if(IsInt)
        {
            _defaultNumberValue = affectedIntData.Value;
            affectedIntData.ApplyChange(immediateChange);
        }
        
        _defaultBoolValue = affectedBoolData.Value;
        affectedBoolData.SetValue(newValue);
        _beginningTime = Time.realtimeSinceStartup;
    }

    public void ApplyOverTime()
    {
        if(IsInstant || isFinished)
            return;
        

        _currentTime = Time.realtimeSinceStartup;
        float elapsedTime = _currentTime - _beginningTime;

        if(elapsedTime >= duration)
        {
            isFinished = true;
            if(returnToDefaultOnEnd)
            {
                if(IsFloat)
                    affectedFloatData.SetValue(_defaultNumberValue);
                else if(IsInt)
                    affectedIntData.SetValue(_defaultNumberValue);
                
                affectedBoolData.SetValue(_defaultBoolValue);
            }
            return;
        }
        else
        {
            if(IsFloat)
                    affectedFloatData.ApplyChange(changeOverTime * Time.deltaTime);
            else if(IsInt)
                    affectedIntData.ApplyChange(changeOverTime * Time.deltaTime);
                
            
        }


    }

   

    public override bool Equals(object other)
    {
        return (other as PowerUp).name == this.name;
    }


}
