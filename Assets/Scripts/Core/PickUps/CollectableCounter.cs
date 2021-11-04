using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{


    public class CollectableCounter : MonoBehaviour
    {
        public int totaalCount;
        [HideInInspector] public int repairCount;
        [HideInInspector] public int speedBoostCount;
        [HideInInspector] public int boomCount;
        [HideInInspector] public int speedTurnCount;

    }
}
