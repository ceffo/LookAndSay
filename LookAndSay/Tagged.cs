namespace LookAndSay
{
    /// <summary>
    /// Wrapper over a value with an additional tag
    /// </summary>
    public struct Tagged<TValue, TTag>
    {
        public TValue Value {get; set;}
        public TTag Tag {get;set;}

        static public implicit operator TValue(Tagged<TValue,TTag> tagged) => tagged.Value; 
    }

    public static class Tagged
    {
        public static Tagged<TValue,TTag> Create<TValue,TTag>(TValue value, TTag tag)
        => new Tagged<TValue,TTag> {Value = value, Tag = tag};
    }
}