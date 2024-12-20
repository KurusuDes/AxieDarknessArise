using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


namespace ADR.Core
{
    public class Entity
    {
        [BoxGroup("Entity"), SerializeField] private int _id;
        [BoxGroup("Entity"), SerializeField] private string _name;
        [BoxGroup("Entity"), SerializeField,TextArea(5,10)] private string _description;
        [BoxGroup("Entity"), SerializeField,PreviewField(150,alignment: ObjectFieldAlignment.Left)] private Sprite _icon;

        public int ID => _id;
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;

    }
}
