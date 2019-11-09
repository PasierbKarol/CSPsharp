using System;

namespace CSPlang
{
    [Serializable]
    public class PoisonException : ChannelDataRejectedException
    {
        private int strength;

        protected internal PoisonException(int _strength)
        {
            //TODO check why this was commented and if it is needed
            //super("PoisonException, strength: " + _strength);
            strength = _strength;
        }

        public int getStrength()
        {
            return strength;
        }
    }
}