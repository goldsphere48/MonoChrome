namespace MonoChrome.Core.Helpers.ComponentAttributeApplication
{
    internal interface IComponentApplicatorAcceptable
    {
        void AcceptFieldVisitor(FieldAttributeVisitor visitor);
    }
}