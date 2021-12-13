namespace MD2Word
{
    public enum FontStyles
    {
        BodyText,
        CodeText,
        Caption,
        CodeBlock,
        [NesstingStyle(MaxLevel = 4)]
        Heading,
        [NesstingStyle(MaxLevel = 5)]
        NumberList,
        [NesstingStyle(MaxLevel = 5)]
        BulletList,
        Hyperlink,
        Title,
        Brief,
        Quote
    }
}