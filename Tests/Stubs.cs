namespace Tests
{
    class Foo
    {
        public string PropertyFoo => "PropertyFoo";

        public string PublicFieldFoo = "PublicFieldFoo";

        private readonly string privateFieldFoo = "privateFieldFoo";

        public override string ToString() => "{\"propertyFoo\":\"PropertyFoo\"}";
    }

    class Bar : Foo
    {
        public string PropertyBar => "PropertyBar";

        public string PublicFieldBar = "PublicFieldBar";

        private readonly string privateFieldBar = "privateFieldBar";

        public override string ToString() => "{\"propertyBar\":\"PropertyBar\",\"propertyFoo\":\"PropertyFoo\"}";
    }
}
