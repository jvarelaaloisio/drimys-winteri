using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WindVegetationController : MonoBehaviour
{
    [Range(0, 1)]
    public float WindGlobalMsak;
    public float WindGlobalSpeed;
    public float WindGlobalIntensity;
    [Range(0, 1)]
    public float WindGlobalDirX;
    [Range(0, 1)]
    public float WindGlobalDirY;
    [Range(0, 1)]
    public float WindSpeed;
    [Range(0,1)]
    public float WindTurbulence;
    [Range(0,5)]
    public float WindIntensity;

    private void Start()
    {
        
    }

    private void Update()
    {
        Shader.SetGlobalFloat("WindMaskGlobal", WindGlobalMsak);
        Shader.SetGlobalFloat("WindSpeedGlobal", WindGlobalSpeed);
        Shader.SetGlobalFloat("WindIntensityGlobal", WindGlobalIntensity);
        Shader.SetGlobalFloat("WindDirX", WindGlobalDirX);
        Shader.SetGlobalFloat("WindDirZ", WindGlobalDirY);
        Shader.SetGlobalFloat("WindSpeed", WindSpeed);
        Shader.SetGlobalFloat("WindTurbulence", WindTurbulence);
        Shader.SetGlobalFloat("WindIntensity", WindIntensity);
    }
}
