namespace CSPlang
{

    class PoisonableOne2AnyChannelIntImpl : One2AnyIntImpl
    {
        PoisonableOne2AnyChannelIntImpl(int _immunity) : base(new PoisonableOne2OneChannelIntImpl(_immunity))
        {

        }
    }
}