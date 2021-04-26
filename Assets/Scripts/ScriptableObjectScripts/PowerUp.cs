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

    [SerializeField]
    public bool overwriteActive = true;

    [SerializeField][ReadOnly]
    public bool isFinished = false;

    [Range(0,1)]
    public float baseChance;

    
    [SerializeField]
    private BoolData affectedBoolData;

    [SerializeField][ShowNativeProperty]
    private bool affectedBoolValue => affectedBoolData ?? false;

    [SerializeField]
    private FloatData affectedFloatData;
    [SerializeField][ShowNativeProperty]
    private float affectedFloatValue => affectedFloatData ?? 0.0f;

    [SerializeField]
    private bool returnToDefaultOnEnd;

    [SerializeField][ReadOnly]
    private float _defaultNumberValue;

    [SerializeField][ReadOnly]
    private bool _defaultBoolValue;

    [SerializeField][ShowIf("IsBoolean")]
    private BoolVariable newValue;



    [SerializeField][ShowIf("IsNumerical")]
    private FloatVariable immediateFloatChange;

    [SerializeField][ShowIf(EConditionOperator.And,"IsNotInstant","IsNumerical")]
    private FloatVariable changeOverTime;

    [SerializeField]
    private FloatVariable duration;

    [ShowNativeProperty][SerializeField]
    private bool IsNotInstant => duration > 0;
    private bool IsNumerical => (affectedFloatData != null);


    private bool IsBoolean => affectedBoolData != null;

    private float _beginningTime;
    private float _currentTime;

    [ReadOnly]
    public float _elapsed;


    public void Activate()
    {
       // Debug.Log("Power Activation!");
        isFinished = false;
        _defaultBoolValue = affectedBoolData ?? false;
        if(IsNumerical)
        {
            _defaultNumberValue = affectedFloatData.Value;
            affectedFloatData.ApplyChange(immediateFloatChange);
        }

        
       
        affectedBoolData?.SetValue(newValue);
        
        _beginningTime = Time.realtimeSinceStartup;

        if(!IsNotInstant)
            isFinished = true;
        Debug.Log($"Activate: {powerName}");
    }

    public void ApplyOverTime()
    {
        if(!IsNotInstant || isFinished)
            return;
        

        _currentTime = Time.realtimeSinceStartup;
        float elapsedTime = _currentTime - _beginningTime;
        _elapsed = elapsedTime;
        if(elapsedTime >= duration)
        {
            Debug.Log("Time's up!");
            FinishActivation();
            return;
        }
        else
        {
            if(IsNumerical)
                affectedFloatData.ApplyChange(changeOverTime * Time.deltaTime);
        }


    }

   [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void ApplyPowerup()
    {
        PowerUpApplier.Instance.ActivatePower(this);
    }

    public GameObject SpawnPrefab(Vector3 position)
    {

        GameObject go = Instantiate(prefab,position,prefab.transform.rotation);
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

    private void OnDestroy() {
        FinishActivation();
    }
    public void FinishActivation()
    {
        
        if(returnToDefaultOnEnd)
        {
            affectedBoolData?.SetValue(_defaultBoolValue);
            if(IsNumerical)
                affectedFloatData.SetValue(_defaultNumberValue);

                
                
        }  
        isFinished = true;  
        Debug.Log($"Finishing: {powerName}");
    }
}
