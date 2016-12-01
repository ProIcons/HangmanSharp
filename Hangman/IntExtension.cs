namespace devian.gr.Hangman
{
    public static class IntExtension
    {
        public static int LimitToRange(this int value, int inclusiveMinimum, int inclusiveMaximum) =>
            value < inclusiveMinimum ? inclusiveMaximum : (value > inclusiveMaximum ? inclusiveMaximum : value);
    }
}
