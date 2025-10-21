using Unity.VisualScripting;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;
    

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }


    public void PlayVFX(GameObject effectObject, Vector3 effectPosition)
    {
        GameObject vfxObjects = Instantiate(effectObject, effectPosition, Quaternion.identity);

        ParticleSystem[] particleSystems = vfxObjects.GetComponentsInChildren<ParticleSystem>();



        float maxLength = 0f;
        foreach (ParticleSystem individualParticleSystem in particleSystems)
        {
            float currentKnownMaxLength = individualParticleSystem.main.duration + individualParticleSystem.main.startLifetime.constantMax;

            if (currentKnownMaxLength > maxLength)
                maxLength = currentKnownMaxLength;
        }
    }

}
