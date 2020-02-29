namespace ASM.DataAccess
{
    public enum DbFieldType
    {
        Text = 1,
        NonText = 2
    }

    public enum DbSearchType
    {
        Equal = 0,
        NotEqual = 1,
        Like = 2,
        NotLike = 3,
        LessThan = 4,
        GreaterThan = 5,
        LessThanOrEqual = 6,
        GreaterThanOrEqual = 7,
        In = 8,
        NotIn = 9
    }
}