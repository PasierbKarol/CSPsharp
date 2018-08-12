namespace CSPlang
{

    class PoisonableBufferedOne2AnyChannelInt : One2AnyIntImpl
    {
        PoisonableBufferedOne2AnyChannelInt(ChannelDataStoreInt _data, int _immunity) : base(new PoisonableBufferedOne2OneChannelInt(_data, _immunity))
        {
            
        }
    }
}