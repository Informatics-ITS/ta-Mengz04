using UnityEngine;

namespace UnityVolumeRendering
{
    [ExecuteInEditMode]
    public class ExternalSlicingPlane : MonoBehaviour
    {
        public VolumeRenderedObject targetObject;
        [SerializeField] private Transform volumeTransform;
        private MeshRenderer meshRenderer;

        private void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            meshRenderer.sharedMaterial.SetMatrix("_parentInverseMat", volumeTransform.worldToLocalMatrix);
            meshRenderer.sharedMaterial.SetMatrix("_planeMat", transform.localToWorldMatrix); // TODO: allow changing scale
        }
    }
}
