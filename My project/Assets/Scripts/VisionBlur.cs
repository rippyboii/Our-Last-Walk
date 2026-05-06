using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // use HighDefinition for HDRP
public class VisionBlur : MonoBehaviour
{
    public Volume volume;
    private DepthOfField dof;

    void Start()
    {
        volume.profile.TryGet(out dof);
    }

    public void SetBlurry(bool isBlurry)
    {
        if (dof != null)
        {
            if (isBlurry)
            {
                dof.active = true;
            }
            else
            {
                dof.active = false;
            }
        }
    }
}
