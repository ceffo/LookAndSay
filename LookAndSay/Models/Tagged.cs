namespace LookAndSay.Models
{
    /// <summary>
    /// Wrapper over a value with an additional tag
    /// </summary>
    public class Tagged<TValue, TTag>
    {
        public Tagged(TValue value, TTag tag)
        {
            this.Value = value;
            this.Tag = tag;

        }
        public TValue Value { get; set; }
        public TTag Tag { get; set; }

        static public implicit operator TValue(Tagged<TValue, TTag> tagged) => tagged.Value;
    }
}