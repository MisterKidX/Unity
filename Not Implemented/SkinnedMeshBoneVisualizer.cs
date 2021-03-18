// --- PBGamesStudio - PhantomBeasts ---
//   Dor Ben Dor (agesonera@gmail.com)
//   3/18/2021 9:18:15 AM
// ------------------------------------

using UnityEngine;

namespace PhantomBeasts.Utilities.Debugging
{
#if UNITY_EDITOR
    public class SkinnedMeshBoneVisualizer : MonoBehaviour
    {
        void OnDrawGizmosSelected()
        {
            var smr = GetComponentInChildren<SkinnedMeshRenderer>();
            Vector3 last = smr.bones[0].position;

            for (int i = 0; i < smr.bones.Length; i++)
            {
                Gizmos.DrawLine(smr.bones[i].position, last);
                UnityEditor.Handles.Label(smr.bones[i].transform.position, i.ToString());
                last = smr.bones[i].position;
            }
        }
    }

#endif
}
