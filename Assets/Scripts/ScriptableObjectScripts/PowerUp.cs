using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "New Powerup")]
public class PowerUp : ScriptableObject
{

    [SerializeField]
    private GameObject prefab;

   [Label("Power Name (Used for lookups)")]
   [Tooltip("This string will be used to search for this power in the powerups list. Make sure it is unique and unlikely to change.")]
    public string powerName = "Name must be unique among all other powerups";

    [Multiline]
    public string description;

    public bool overwriteActive = true;
    public bool isFinished = false;

    [Range(0,1)]
    public float baseChance;

    
    [SerializeField]
    private BoolData affectedBoolData;

    [SerializeField][HideIf("IsInt")]
    private FloatData affectedFloatData;

    [SerializeField][HideIf("IsFloat")]
    private IntData affectedIntData;

    [SerializeField]
    private bool returnToDefaultOnEnd;

    private float _defaultNumberValue;
    private bool _defaultBoolValue;

    [SerializeField][ShowIf("IsBoolean")]
    private BoolVariable newValue;



    [SerializeField][ShowIf("IsNumerical")]
    private FloatVariable immediateChange;

    [SerializeField][ShowIf(EConditionOperator.And,"IsNotInstant","IsNumerical")]
    private FloatVariable changeOverTime;

    [SerializeField]
    private FloatVariable duration;

    private bool IsNotInstant => duration > 0;
    private bool IsNumerical => (affectedFloatData != null || affectedIntData != null);

    private bool IsFloat => affectedFloatData != null;
    private bool IsInt => affectedIntData != null;
    private bool IsBoolean => affectedBoolData != null;

    private float _beginningTime;
    private float _currentTime;


    public void Activate()
    {
        isFinished = false;
        if(IsFloat)
        {
            _defaultNumberValue = affectedFloatData.Value;
            affectedFloatData.ApplyChange(immediateChange);
        }
        else if(IsInt)
        {
            _defaultNumberValue = affectedIntData.Value;
            affectedIntData.ApplyChange((int)immediateChange.Value);
        }
        
        _defaultBoolValue = affectedBoolData.Value;
        affectedBoolData.SetValue(newValue);
        
        _beginningTime = Time.realtimeSinceStartup;

        if(!IsNotInstant)
            isFinished = true;
    }

    public void ApplyOverTime()
    {
        if(!IsNotInstant || isFinished)
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
                    affectedIntData.SetValue((int)_defaultNumberValue);
                
                affectedBoolData.SetValue(_defaultBoolValue);
            }
            return;
        }
        else
        {
            if(IsFloat)
                    affectedFloatData.ApplyChange(changeOverTime * Time.deltaTime);
            else if(IsInt)
                    affectedIntData.ApplyChange((int)(changeOverTime * Time.deltaTime));
                
            
        }


    }

   [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void ApplyPowerup()
    {
        PowerUpApplier.Instance.ActivatePower(this);
    }

    public GameObject SpawnPrefab(Vector3 position, Quaternion rotation)
    {

        GameObject go = Instantiate(prefab,position,rotation);
        PowerUpHolder pp = go.GetComponent<PowerUpHolder>();
        if(pp == null)
        {
            pp = go.AddComponent<PowerUpHolder>();
            pp.powerUp = this;
        }
        else
        {
            pp.powerUp = this;
        }

        return go;
    }

    public override bool Equals(object other)
    {
        return (other as PowerUp).name == this.name;
    }


}
