using UnityEngine;

namespace Systems {
    public static class Layers {
        public static readonly int Impassible;
        public static readonly int ExtraCost;

        static Layers() {
            Impassible = LayerMask.NameToLayer("Impassible");
            ExtraCost = LayerMask.NameToLayer("ExtraCost");

            Debug.Assert(Impassible >= 0, "Layer 'Impassible' missing");
            Debug.Assert(ExtraCost >= 0, "Layer 'ExtraCost' missing");
        }
    }
}