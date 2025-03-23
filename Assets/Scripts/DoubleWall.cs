using System.Collections.Generic;
using UnityEngine;

public class DoubleWall : MonoBehaviour
{
    public List<DoubleWallRing> rings;
    public bool allPassed => rings.TrueForAll(r => r.hasBeenPassed);
    public void ResetWall() {
    foreach (var ring in rings) {
        ring.hasBeenPassed = false;
    }
    }
}
