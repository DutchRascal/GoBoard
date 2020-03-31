using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EndScreen : MonoBehaviour
{
    public PostProcessProfile
        blurProfile,
        normalProfile;
    public PostProcessVolume postProcessVolume;

    public void EnableCameraBlur(bool state)
    {
        if (postProcessVolume && blurProfile && normalProfile)
        {
            postProcessVolume.profile = (state) ? blurProfile : normalProfile;
        }
    }
}
