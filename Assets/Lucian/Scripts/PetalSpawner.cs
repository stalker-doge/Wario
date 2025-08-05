using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class PetalSpawnerCapsule2D : MonoBehaviour
{
    [Header("Petals")]
    public GameObject petalPrefab;
    [Min(1)] public int petalCount = 12;
    [Tooltip("How far to push petals outward from the capsule edge (world units).")]
    public float outwardOffset = 0.1f;

    private CapsuleCollider2D capsule;

    void Start()
    {
        capsule = GetComponent<CapsuleCollider2D>();

        if (petalPrefab == null)
        {
            Debug.LogError("[PetalSpawnerCapsule2D] Petal prefab not assigned.");
            return;
        }

        if (petalCount <= 0)
        {
            Debug.LogWarning("[PetalSpawnerCapsule2D] PetalCount <= 0; nothing to spawn.");
            return;
        }

        // Spawn all petals first
        for (int i = 0; i < petalCount; i++)
        {
            float t = (float)i / petalCount;
            Vector2 edgeLocal = PerimeterPointLocal(t);
            Vector3 centerLocal = capsule.offset;

            Vector3 edgeWorld = transform.TransformPoint(centerLocal + (Vector3)edgeLocal);
            Vector3 centerWorld = transform.TransformPoint(centerLocal);
            Vector2 outwardDir = (edgeWorld - centerWorld).normalized;
            Vector3 spawnWorld = edgeWorld + (Vector3)(outwardDir * outwardOffset);

            GameObject petal = Instantiate(petalPrefab, spawnWorld, Quaternion.identity, transform);

            float angleZ = Mathf.Atan2(outwardDir.y, outwardDir.x) * Mathf.Rad2Deg;
            petal.transform.rotation = Quaternion.Euler(0f, 0f, angleZ + 90f);
        }

        // AFTER spawning, register with GameManager
        if (PetalGameManager.Instance != null)
        {
            PetalGameManager.Instance.RegisterPetals(petalCount);
        }
    }

    /// <summary>
    /// Returns a point ON the capsule perimeter in LOCAL space for a given t in [0,1).
    /// Evenly walks the perimeter by arc length: top/right arc → straight → bottom/left arc → straight (for vertical),
    /// and similarly for horizontal.
    /// </summary>
    private Vector2 PerimeterPointLocal(float t)
    {
        // Local half-sizes from collider
        Vector2 size = capsule.size * 0.5f; // half-size in local units
        // Use local (pre-transform) geometry; TransformPoint will apply scale/rotation correctly.

        if (capsule.direction == CapsuleDirection2D.Vertical)
        {
            float r = size.x;                 // circle radius (short axis)
            float side = Mathf.Max(0f, size.y - r); // half of the straight section height

            // Perimeter (local units): two semicircles + two straight segments (each of length 2*side)
            float L_arc = Mathf.PI * r;       // one semicircle
            float L_side = 2f * side;         // one straight side segment
            float L = 2f * L_arc + 2f * L_side;

            float s = t * L;

            // Segments order: top arc -> right side -> bottom arc -> left side
            if (s < L_arc) // Top arc: angle 0..π, center at (0, +side)
            {
                float u = s / L_arc;               // 0..1
                float ang = Mathf.Lerp(0f, Mathf.PI, u);
                return new Vector2(Mathf.Cos(ang) * r, Mathf.Sin(ang) * r + side);
            }
            s -= L_arc;

            if (s < L_side) // Right side: from y=+side to y=-side at x=+r
            {
                float u = s / L_side;              // 0..1
                float y = Mathf.Lerp(side, -side, u);
                return new Vector2(+r, y);
            }
            s -= L_side;

            if (s < L_arc) // Bottom arc: angle π..2π, center at (0, -side)
            {
                float u = s / L_arc;               // 0..1
                float ang = Mathf.Lerp(Mathf.PI, 2f * Mathf.PI, u);
                return new Vector2(Mathf.Cos(ang) * r, Mathf.Sin(ang) * r - side);
            }
            s -= L_arc;

            // Left side: from y=-side to y=+side at x=-r
            {
                float u = Mathf.Clamp01(s / L_side);
                float y = Mathf.Lerp(-side, side, u);
                return new Vector2(-r, y);
            }
        }
        else // CapsuleDirection2D.Horizontal
        {
            float r = size.y;                 // circle radius (short axis)
            float side = Mathf.Max(0f, size.x - r); // half of the straight section width

            float L_arc = Mathf.PI * r;       // one semicircle
            float L_side = 2f * side;         // one straight side segment
            float L = 2f * L_arc + 2f * L_side;

            float s = t * L;

            // Segments order: right arc -> bottom side -> left arc -> top side
            if (s < L_arc) // Right arc: angle -π/2..+π/2, center at (+side, 0)
            {
                float u = s / L_arc;               // 0..1
                float ang = Mathf.Lerp(-Mathf.PI * 0.5f, Mathf.PI * 0.5f, u);
                return new Vector2(side + Mathf.Cos(ang) * r, Mathf.Sin(ang) * r);
            }
            s -= L_arc;

            if (s < L_side) // Bottom side: from x=+side to x=-side at y=-r
            {
                float u = s / L_side;              // 0..1
                float x = Mathf.Lerp(side, -side, u);
                return new Vector2(x, -r);
            }
            s -= L_side;

            if (s < L_arc) // Left arc: angle +π/2..+3π/2, center at (-side, 0)
            {
                float u = s / L_arc;               // 0..1
                float ang = Mathf.Lerp(Mathf.PI * 0.5f, Mathf.PI * 1.5f, u);
                return new Vector2(-side + Mathf.Cos(ang) * r, Mathf.Sin(ang) * r);
            }
            s -= L_arc;

            // Top side: from x=-side to x=+side at y=+r
            {
                float u = Mathf.Clamp01(s / L_side);
                float x = Mathf.Lerp(-side, side, u);
                return new Vector2(x, +r);
            }
        }
    }

#if UNITY_EDITOR
    // Optional: visualize points in the editor when selected
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            capsule = GetComponent<CapsuleCollider2D>();

        if (capsule == null) return;

        Gizmos.color = Color.white;
        Vector3 centerLocal = capsule.offset;

        int preview = Mathf.Max(12, petalCount);
        for (int i = 0; i < preview; i++)
        {
            float t = (float)i / preview;
            Vector3 edgeLocal = PerimeterPointLocal(t);
            Vector3 edgeWorld = transform.TransformPoint(centerLocal + edgeLocal);
            Gizmos.DrawSphere(edgeWorld, 0.03f);
        }
    }
#endif
}
