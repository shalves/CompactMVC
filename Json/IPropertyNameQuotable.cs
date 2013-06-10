namespace System.Json
{
    internal interface IPropertyNameQuotable
    {
        bool QuotePropertyName { get; set; }
        void ByAdd();
    }
}
