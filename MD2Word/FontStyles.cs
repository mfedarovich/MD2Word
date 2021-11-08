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
        NumberList,
        BulletList,
        Hyperlink,
        Title
    }
}