namespace MonoChrome.Core.Helpers.FieldInjection
{
    internal interface IComponentApplicatorAcceptable
    {
        void AcceptFieldVisitor(FieldAttributeVisitor visitor);
    }
}