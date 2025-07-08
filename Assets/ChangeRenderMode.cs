using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityVolumeRendering
{
    public class ChangeRenderMode : MonoBehaviour
    {
        [SerializeField] private UnityEvent<RenderMode> renderEvents;

        public void ChangeRenderModeDVR(){
            renderEvents.Invoke(RenderMode.DirectVolumeRendering);
        }
        public void ChangeRenderModeIsosurface(){
            renderEvents.Invoke(RenderMode.IsosurfaceRendering);
        }
    }
}
