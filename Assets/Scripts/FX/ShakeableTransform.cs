
///////////////////////////////////////
// Taken from: https://github.com/IronWarrior/UnityCameraShake
// Unlincense License
///////////////////////////////////////////


using UnityEngine;
using System;
using System.Collections;
using NaughtyAttributes;

public class ShakeableTransform : MonoBehaviour 
{
    [SerializeField] private FloatVariable _defaultImpact;
    [SerializeField] private FloatVariable _shockRadius;
    [SerializeField] private GameObject _targetOverride;

    /// <summary>
    /// Maximum distance in each direction the transform
    /// with translate during shaking.
    /// </summary>
    [SerializeField]
    Vector3 maximumTranslationShake = Vector3.one;

    /// <summary>
    /// Maximum angle, in degrees, the transform will rotate
    /// during shaking.
    /// </summary>
    [SerializeField]
    Vector3 maximumAngularShake = Vector3.one * 15;

    /// <summary>
    /// Frequency of the Perlin noise function. Higher values
    /// will result in faster shaking.
    /// </summary>
    [SerializeField]
    float frequency = 25;

    /// <summary>
    /// <see cref="trauma"/> is taken to this power before
    /// shaking is applied. Higher values will result in a smoother
    /// falloff as trauma reduces.
    /// </summary>
    [SerializeField]
    float traumaExponent = 1;

    /// <summary>
    /// Amount of trauma per second that is recovered.
    /// </summary>
    [SerializeField]
    float recoverySpeed = 1;

    /// <summary>
    /// Value between 0 and 1 defining the current amount
    /// of stress this transform is enduring.
    /// </summary>
    private float trauma;

    private float seed;
    private float _forceFactor;

    private GameObject _target;
    private void Awake()
    {
        seed = UnityEngine.Random.value;
        if(_targetOverride == null)
        {
            _target = gameObject;
        }
        else
            _target = _targetOverride;
    }

   /* private void Update()
    {
        // Taking trauma to an exponent allows the ability to smoothen
        // out the transition from shaking to being static.
        float shake = Mathf.Pow(trauma, traumaExponent);

        // This x value of each Perlin noise sample is fixed,
        // allowing a vertical strip of noise to be sampled by each dimension
        // of the translational and rotational shake.
        // PerlinNoise returns a value in the 0...1 range; this is transformed to
        // be in the -1...1 range to ensure the shake travels in all directions.
        transform.localPosition = new Vector3(
            maximumTranslationShake.x * (Mathf.PerlinNoise(seed, Time.time * frequency) * 2 - 1),
            maximumTranslationShake.y * (Mathf.PerlinNoise(seed + 1, Time.time * frequency) * 2 - 1),
            maximumTranslationShake.z * (Mathf.PerlinNoise(seed + 2, Time.time * frequency) * 2 - 1)
        ) * shake;

        transform.localRotation = Quaternion.Euler(new Vector3(
            maximumAngularShake.x * (Mathf.PerlinNoise(seed + 3, Time.time * frequency) * 2 - 1),
            maximumAngularShake.y * (Mathf.PerlinNoise(seed + 4, Time.time * frequency) * 2 - 1),
            maximumAngularShake.z * (Mathf.PerlinNoise(seed + 5, Time.time * frequency) * 2 - 1)
        ) * shake);

        trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.deltaTime);
    }*/

    public void Shake()
    {
        _forceFactor = 1;
        StartCoroutine(ApplyShake(_defaultImpact));
       
        
    }

    public void Shake(float stress)
    {
        _forceFactor = 1;
        StartCoroutine(ApplyShake(stress));
       
        
    }

    public void ShakeDistanceAffected(Vector3 sourcePosition)
    {
        
        float distance = Vector3.Distance(_target.transform.position, sourcePosition);
        float distance01 = Mathf.Clamp01(distance / _shockRadius);
        _forceFactor = (1-distance01);
        //print(_forceFactor);
        //print(_defaultImpact * _forceFactor);
        StartCoroutine(ApplyShake(_defaultImpact));
    }

    public void ShakeDirectionAffected(Vector3 sourcePosition)
    {
        _forceFactor = 1;
        StartCoroutine(ApplyDirectionalShake( transform.position - sourcePosition));
    }

    public void ShakeDirectionDistanceAffected(Vector3 sourcePosition)
    {
        float distance = Vector3.Distance(_target.transform.position, sourcePosition);
        float distance01 = Mathf.Clamp01(distance / _shockRadius);
        _forceFactor = (1-distance01);

        
        StartCoroutine(ApplyDirectionalShake( transform.position - sourcePosition));
    }


    private IEnumerator ApplyShake(float amount)
    {
        Vector3 startingPos = _target.transform.localPosition;
        Quaternion startingRot = _target.transform.localRotation;

        trauma = Mathf.Clamp01(trauma + (amount * _forceFactor));
        while(trauma > 0.000f)
        {
            // Taking trauma to an exponent allows the ability to smoothen
            // out the transition from shaking to being static.
            float shake = Mathf.Pow(trauma, traumaExponent);

            // This x value of each Perlin noise sample is fixed,
            // allowing a vertical strip of noise to be sampled by each dimension
            // of the translational and rotational shake.
            // PerlinNoise returns a value in the 0...1 range; this is transformed to
            // be in the -1...1 range to ensure the shake travels in all directions.
            _target.transform.localPosition = startingPos + new Vector3(
            maximumTranslationShake.x * (Mathf.PerlinNoise(seed, Time.time *    frequency) * 2 - 1),
            maximumTranslationShake.y * (Mathf.PerlinNoise(seed + 1, Time.time  * frequency) * 2 - 1),
            maximumTranslationShake.z * (Mathf.PerlinNoise(seed + 2, Time.time * frequency) * 2 - 1)
                ) * shake;

            _target.transform.localRotation = startingRot * Quaternion.Euler(new Vector3(
            maximumAngularShake.x * (Mathf.PerlinNoise(seed + 3, Time.time * frequency) * 2 - 1),
            maximumAngularShake.y * (Mathf.PerlinNoise(seed + 4, Time.time * frequency) * 2 - 1),
            maximumAngularShake.z * (Mathf.PerlinNoise(seed + 5, Time.time * frequency) * 2 - 1)
                ) * shake);

            trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.deltaTime);

            yield return null;

        }

    }

    private IEnumerator ApplyDirectionalShake(Vector3 direction)
    {
        Vector3 startingPos = _target.transform.localPosition;
        Quaternion startingRot = _target.transform.localRotation;
        trauma = Mathf.Clamp01(trauma + (_defaultImpact * _forceFactor));

        float factorHorizontal = Mathf.Clamp(Vector3.Dot(direction, transform.right), -1, 1);
        float factorVertical = Mathf.Clamp(Vector3.Dot(direction, transform.up), -1, 1);
        float factorForward = Mathf.Clamp(Vector3.Dot(direction, transform.forward), -1, 1);
        //print(factorHorizontal + ", " + factorVertical + ", " + factorForward );

        while(trauma > 0.000f)
        {
            // Taking trauma to an exponent allows the ability to smoothen
            // out the transition from shaking to being static.
            float shake = Mathf.Pow(trauma, traumaExponent);

            // This x value of each Perlin noise sample is fixed,
            // allowing a vertical strip of noise to be sampled by each dimension
            // of the translational and rotational shake.
            // PerlinNoise returns a value in the 0...1 range; this is transformed to
            // be in the -1...1 range to ensure the shake travels in all directions.
            /*transform.localPosition = startingPos + new Vector3(
            maximumTranslationShake.x * (Mathf.PerlinNoise(seed, Time.time *    frequency) * 2 - 1),
            maximumTranslationShake.y * (Mathf.PerlinNoise(seed + 1, Time.time  * frequency) * 2 - 1),
            maximumTranslationShake.z * (Mathf.PerlinNoise(seed + 2, Time.time * frequency) * 2 - 1)
                ) * shake;*/

            _target.transform.localRotation = startingRot * Quaternion.Euler(new Vector3(
            maximumAngularShake.x * factorVertical * (Mathf.PerlinNoise(seed + 3, Time.time * frequency) * 2 - 1),
            maximumAngularShake.y * factorHorizontal * (Mathf.PerlinNoise(seed + 4, Time.time * frequency) * 2 - 1),
            maximumAngularShake.z * (Mathf.PerlinNoise(seed + 5, Time.time * frequency) * 2 - 1)
                ) * shake);

            trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.deltaTime);

            yield return null;

        }

    }
}
