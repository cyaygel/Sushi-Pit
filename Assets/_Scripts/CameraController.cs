using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject m_CameraParent;
    [SerializeField] private float m_ScrollSpeed;
    [SerializeField] private float m_MaxHeight = 65f;
    [SerializeField] private float m_SphereRadius = 5f;
    [SerializeField] private LayerMask m_ObstacleLayers;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 direction = m_CameraParent.transform.forward;
            Vector3 nextPos = m_CameraParent.transform.position + direction * (m_ScrollSpeed * Time.deltaTime);
            if (!IsColliding(nextPos))
            {
                nextPos.y = Mathf.Min(nextPos.y, m_MaxHeight);
                m_CameraParent.transform.position = nextPos;
            }

            var depth = m_CameraParent.transform.position.y - 65;
            PlayerManager.Instance.UpdateDepth(Mathf.RoundToInt(depth));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Vector3 direction = -m_CameraParent.transform.forward;
            Vector3 nextPos = m_CameraParent.transform.position + direction * (m_ScrollSpeed * Time.deltaTime);
            nextPos.y = Mathf.Min(nextPos.y, m_MaxHeight);
            m_CameraParent.transform.position = nextPos;
            
            var depth = m_CameraParent.transform.position.y - 65;
            PlayerManager.Instance.UpdateDepth(Mathf.RoundToInt(depth));
        }
    }

    private bool IsColliding(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, m_SphereRadius, m_ObstacleLayers, QueryTriggerInteraction.Ignore);
        return hits.Length > 0;
    }
}