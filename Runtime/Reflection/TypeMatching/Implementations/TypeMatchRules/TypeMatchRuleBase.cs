using System;

namespace EasyToolKit.Core.Reflection.Implementations
{
    public abstract class TypeMatchRuleBase : ITypeMatchRule
    {
        private string _name;

        public virtual string Name
        {
            get
            {
                if (_name == null)
                {
                    _name = GetType().Name;
                }

                return _name;
            }
        }

        /// <inheritdoc/>
        public abstract bool CanMatch(TypeMatchCandidate candidate, Type[] targets);

        /// <inheritdoc/>
        public abstract Type Match(TypeMatchCandidate candidate, Type[] targets);

        public override string ToString()
        {
            return Name;
        }
    }
}
