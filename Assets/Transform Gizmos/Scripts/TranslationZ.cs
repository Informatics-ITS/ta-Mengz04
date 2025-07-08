using UnityEngine;

namespace TransformGizmos
{
    public class TranslationZ : MonoBehaviour, IGizmoTransforms
    {
        [SerializeField] private Translation translation;
        [SerializeField] Material m_defaultMaterial;
        [SerializeField] Material m_hoveredMaterial;

        float m_totalDist;
        Vector2 m_moveDirection;
        Vector2 m_initialMousePosition = Vector2.zero;
        Vector2 m_lastProjectedMousePosition = Vector2.zero;

        void Start()
        {
            translation.StartCode(GetComponent<MeshRenderer>(), m_defaultMaterial, m_hoveredMaterial, axis: 2);
        }

        public void OnMouseEnter()
        {
            translation.MouseEnterCode(axis: 2);
        }

        public void OnMouseExit()
        {
            translation.MouseExitCode(axis: 2);
        }

        public void OnMouseDown()
        {
            m_totalDist = 0;
            (m_initialMousePosition, m_lastProjectedMousePosition, m_moveDirection) = translation.MouseDownCode(axis: 2);
        }

        public void OnMouseUp()
        {
            translation.MouseUpCode();
        }

        public void OnMouseDrag()
        {
            (m_totalDist, m_lastProjectedMousePosition) = translation.MouseDragCode(m_initialMousePosition, m_moveDirection, m_lastProjectedMousePosition, m_totalDist, axis: 2);
        }
    }
}
