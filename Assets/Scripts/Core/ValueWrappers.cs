using System.Collections.Generic;
using UnityEngine;

namespace MynetDemo.Core
{
    /// <summary>
    /// Stores stats, especially RPGish stats.
    /// </summary>
    public class Attribute
    {
        readonly float _baseValue;

        readonly List<Modifier> _modifiers;

        /// <summary>
        /// Returns the current value of the attribute.
        /// </summary>
        public float Value { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="baseValue">The base value of the attribute.</param>
        public Attribute(float baseValue)
        {
            _baseValue = baseValue;
            _modifiers = new List<Modifier>();
            CalculateValue();
        }

        /// <summary>
        /// Calculates the value of the attribute after every modification.
        /// </summary>
        void CalculateValue()
        {
            Value = _baseValue;
            foreach(Modifier modifier in _modifiers)
            {
                Value += modifier.GetModification(_baseValue);
            }
        }

        /// <summary>
        /// Adds a modifier to the attribute.
        /// </summary>
        /// <param name="modifier">The modifier to be added.</param>
        public void AddModifier(Modifier modifier)
        {
            _modifiers.Add(modifier);
            CalculateValue();
        }

        /// <summary>
        /// Removes a modifier from the attribute
        /// </summary>
        /// <param name="modifier">The modifier to be removed.</param>
        public void RemoveModifier(Modifier modifier)
        {
            _modifiers.Remove(modifier);
            CalculateValue();
        }

        /// <summary>
        /// Resets an attribute. After calling this function, the attribute is equal to the base value.
        /// </summary>
        public void Reset()
        {
            _modifiers.Clear();
            CalculateValue();
        }
    }

    /// <summary>
    /// Modifies attribute values.
    /// </summary>
    public abstract class Modifier
    {
        protected readonly float _modificationValue;

        protected readonly string _modificationName;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Modification value.</param>
        /// <param name="name">Modifier name.</param>
        public Modifier(float value, string name)
        {
            _modificationValue = value;
            _modificationName = name;
        }

        /// <summary>
        /// Returns the modification amount with respect to a baseValue.
        /// </summary>
        /// <param name="baseValue">The base value of the attribute.</param>
        /// <returns></returns>
        public abstract float GetModification(float baseValue);

        public override bool Equals(object obj)
        {
            return _modificationName.Equals(obj.ToString());
        }

        public override int GetHashCode() { return ToString().GetHashCode(); }
    }

    /// <summary>
    /// This type of modifier adds modification value directly to the attribute.
    /// </summary>
    public class AdditionModifier : Modifier
    {
        public AdditionModifier(float value, string name) : base(value, name)
        { }

        public override float GetModification(float baseValue) { return _modificationValue; }

        public override string ToString()
        {
            string sign = _modificationValue > 0 ? "+" : _modificationValue < 0 ? "-" : "";
            return _modificationName + " (" + sign + Mathf.Abs(_modificationValue) + ")";
        }
    }

    /// <summary>
    /// This type of modifier adds modification value times base value to the attribute.
    /// </summary>
    public class MultiplicationModifier : Modifier
    {
        public MultiplicationModifier(float value, string name) : base(value, name)
        { }

        public override float GetModification(float baseValue) { return baseValue * _modificationValue; }

        public override string ToString()
        {
            string sign = _modificationValue > 0 ? "+" : _modificationValue < 0 ? "-" : "";
            return _modificationName + " (" + sign + (Mathf.Abs(_modificationValue) * 100f) + "%)";
        }
    }
}
