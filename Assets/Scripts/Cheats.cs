using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Cheats : MonoBehaviour
{
    #region Inputs
    [Header("------------- INPUTS -------------")]
    [SerializeField] KeyCode immortalityCheatKey;
    [SerializeField] KeyCode infiniteAmmoCheatKey;
    [SerializeField] KeyCode maxThunderCheatKey;

    #endregion

    #region Influenced Variables
    [Header("------ INFLUENCED VARIABLES ------")]

    [Header("---- Immortality ----")]
    [SerializeField] private FloatData PlayerHP;
    [SerializeField] private FloatVariable MaxPlayerHP;

    [Header("--- Infinite Ammo ---")]
    [SerializeField] private FloatData CurrentAmmo;
    [SerializeField] private FloatVariable MaxAmmo;

    [Header("---- Max Thunder ----")]
    [SerializeField] private FloatData CurrentThunderPower;
    [SerializeField] private FloatVariable MaxThunderPower;
    #endregion

    #region Bools

    private bool _isImmortalityActive = false;
    private bool _isInfiniteAmmoActive = false;
    private bool _isMaxThunderActive = false;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForCheatInputs();
        UpdateVariables();
    }

    private void CheckForCheatInputs()
    {
        if (Input.GetKeyDown(immortalityCheatKey))
        {
            _isImmortalityActive = ToggleBool(_isImmortalityActive);
        }
        
        if (Input.GetKeyDown(infiniteAmmoCheatKey))
        {
            _isInfiniteAmmoActive = ToggleBool(_isInfiniteAmmoActive);
        }
        if (Input.GetKeyDown(maxThunderCheatKey))
        {
            _isMaxThunderActive = ToggleBool(_isMaxThunderActive);
        }
    }

    private void UpdateVariables()
    {
        if (_isImmortalityActive) PlayerHP.SetValue(MaxPlayerHP.Value);

        if (_isInfiniteAmmoActive) CurrentAmmo.SetValue(MaxAmmo.Value);

        if (_isMaxThunderActive) CurrentThunderPower.SetValue(MaxThunderPower.Value);
    }

    private bool ToggleBool(bool variable) { return !variable; }
}
