using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace UnityEditor.VFX
{
    class CustomSpawnerVariant : IVariantProvider
    {
        public Dictionary<string, object[]> variants
        {
            get
            {
                return new Dictionary<string, object[]>
                {
                    { "m_customType", VFXLibrary.FindConcreteSubclasses(typeof(VFXSpawnerFunction)).Select(o => new SerializableType(o) as object).ToArray() }
                };
            }
        }
    }

    [VFXInfo(category = "Spawner/Custom", variantProvider = typeof(CustomSpawnerVariant))]
    class VFXSpawnerCustomWrapper : VFXAbstractSpawner
    {
        [SerializeField, VFXSetting(hidden = true)]
        protected SerializableType m_customType;

        protected override IEnumerable<VFXPropertyWithValue> inputProperties
        {
            get
            {
                return customBehavior == null ? Enumerable.Empty<VFXPropertyWithValue>() : PropertiesFromType(customBehavior.GetRecursiveNestedType(GetInputPropertiesTypeName()));
            }
        }

        public override sealed string name { get { return m_customType == null ? "null" : ((Type)m_customType).Name; } }
        public override sealed Type customBehavior { get { return m_customType; } }
        public override sealed VFXTaskType spawnerType { get { return VFXTaskType.kSpawnerCustomCallback; } }
    }
}
