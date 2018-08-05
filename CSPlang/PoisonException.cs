using System;
using System.Runtime.Serialization;

namespace CSPlang
{
    [Serializable]
    internal class PoisonException : ChannelDataRejectedException
    {
        private int strength;

        protected internal PoisonException(int _strength)
        {
            //super("PoisonException, strength: " + _strength);
            strength = _strength;
        }

        public int getStrength()
        {
            return strength;
        }

    }
}