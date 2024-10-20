namespace UniLx.Shared.Abstractions
{
    public abstract class ValueObject
    {
        public string Value { get; private set; }

        protected ValueObject(string value)
        {
            Value = value;
        }        
    }

}
