using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {

    // This is the blackboard container shared between all nodes.
    // Use this to store temporary data that multiple nodes need read and write access to.
    // Add other properties here that make sense for your specific use case.
    [System.Serializable]
    public class Blackboard {

        //Checking VARIABLES
        public Collider[] collidersInArea;
        public List<Collider> collidersInLOS;

        //timer for when to attack
        public float attackTimer;

        public Vector3 jumpLocation;
        
        public float timer = 10f;

        public GameObject selectedTarget;
    }
}