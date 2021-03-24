using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Cheats : MonoBehaviour
{
    #region Inputs
    [Header("-----INPUTS-----")]
    [SerializeField] KeyCode immortalityCheatKey;

    #endregion

    #region Influenced Variables
    [Header("-----INFLUENCED VARIABLES-----")]
    [SerializeField] private FloatData PlayerHP;
    [SerializeField] private FloatVariable MaxPlayerHP;

    #endregion

    #region Bools

    private bool _isImmortalityActive;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isImmortalityActive) PlayerHP.SetValue(MaxPlayerHP.Value);
    }

    private void CheckForCheatInput()
    {
        if (Input.GetKeyDown(immortalityCheatKey)) ToggleBool(_isImmortalityActive);
    }

    private void ToggleBool(bool variable) { variable = !variable; }
}
