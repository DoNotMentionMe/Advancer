using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Adv
{
    [Serializable]
    public class EnemyGenerationInformation
    {
        public GenerationObj GenerationObj;
        public float GenrationTimePoint;
        public GenerationPos GenerationPosition;
    }
}