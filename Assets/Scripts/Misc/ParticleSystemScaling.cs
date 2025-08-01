using UnityEngine;

// Cobbled together from an example on docs.unity3d.com for ParticleSystem.MainModule.scalingMode
// This should really just be a built-in editable setting on particle system components
public class ParticleSystemScaling : MonoBehaviour
{
    private ParticleSystem ps;
    public ParticleSystemScalingMode scaleMode;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        var main = ps.main;
        main.scalingMode = scaleMode;
    }
}
