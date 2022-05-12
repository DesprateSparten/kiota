using System.Linq;
using Kiota.Builder.Writers;
using Kiota.Builder.Writers.Go;
using Kiota.Builder.Writers.Php;
using Xunit;

namespace Kiota.Builder.Refiners.Tests
{
    public class PhpLanguageRefinerTests
    {
        private readonly CodeNamespace root = CodeNamespace.InitRootNamespace();

        [Fact]
        public void ReplacesRequestBuilderPropertiesByMethods()
        {
            var model = root.AddClass(new CodeClass()
            {
                Name = "userRequestBuilder",
                Kind = CodeClassKind.RequestBuilder
            }).First();

            var requestBuilder = model.AddProperty(new CodeProperty()
            {
                Name = "breaks", 
                Kind = CodePropertyKind.RequestBuilder,
                Type = new CodeType()
                {
                    Name = "string"
                }
            }).First();
            ILanguageRefiner.Refine(new GenerationConfiguration {Language = GenerationLanguage.PHP}, root);
            Assert.Equal("breaks", requestBuilder.Name);
            Assert.Equal("userRequestBuilder", model.Name);
        }

        [Fact]
        public void PrefixReservedWordPropertyNamesWith()
        {
            var model = root.AddClass(new CodeClass()
            {
                Name = "userRequestBuilder",
                Kind = CodeClassKind.RequestBuilder
            }).First();

            var property = model.AddProperty(new CodeProperty()
            {
                Name = "continue", 
                Kind = CodePropertyKind.RequestBuilder,
                Type = new CodeType()
                {
                    Name = "string"
                }
            }).First();
            
            ILanguageRefiner.Refine(new GenerationConfiguration {Language = GenerationLanguage.PHP}, root);
            Assert.Equal("EscapedContinue",property.Name);
        }
        
        [Fact]
        public void ReplacesBinaryWithNativeType()
        {
            var model = root.AddClass(new CodeClass () {
                Name = "model",
                Kind = CodeClassKind.Model
            }).First();
            var method = model.AddMethod(new CodeMethod() {
                Name = "method"
            }).First();
            method.ReturnType = new CodeType() {
                Name = "binary"
            };
            ILanguageRefiner.Refine(new GenerationConfiguration { Language = GenerationLanguage.PHP}, root);
            Assert.Equal("StreamInterface", method.ReturnType.Name);
        }

        [Fact]
        public void AddsDefaultImports() {
            var model = root.AddClass(new CodeClass () {
                Name = "model",
                Kind = CodeClassKind.Model
            }).First();
            var requestBuilder = root.AddClass(new CodeClass() {
                Name = "rb",
                Kind = CodeClassKind.RequestBuilder,
            }).First();
            ILanguageRefiner.Refine(new GenerationConfiguration { Language = GenerationLanguage.PHP }, root);
            Assert.NotEmpty(model.StartBlock.Usings);
        }

        [Fact]
        public void ReplacesInt64On32BitTarget()
        {
            var property = new CodeProperty() { Name = "prop", Type = new CodeType() { Name = "int64"}};
            var parentClass = new CodeClass() { Name = "model", Kind = CodeClassKind.Model };
            root.AddClass(parentClass).First().AddProperty(property);
            ILanguageRefiner.Refine(new GenerationConfiguration() { Language =  GenerationLanguage.PHP }, root);
            Assert.Equal("int64", property.Type.Name);
            
            ILanguageRefiner.Refine(new GenerationConfiguration() { Language =  GenerationLanguage.PHP, Platform = GenerationConfiguration.PlatformArchitecture.Bit32}, root);
            Assert.Equal("float", property.Type.Name);
            
            var param = new CodeParameter() { Name = "param", Type = new CodeType() { Name = "int64"}};
            var method = new CodeMethod() { Name = "test", ReturnType = new CodeType() { Name = "int64" }};
            method.AddParameter(param);
            parentClass.AddMethod(method);
            ILanguageRefiner.Refine(new GenerationConfiguration() { Language =  GenerationLanguage.PHP }, root);
            Assert.Equal("int64", param.Type.Name);
            Assert.Equal("int64", method.ReturnType.Name);
            
            ILanguageRefiner.Refine(new GenerationConfiguration() { Language =  GenerationLanguage.PHP, Platform = GenerationConfiguration.PlatformArchitecture.Bit32}, root);
            Assert.Equal("float", param.Type.Name);
            Assert.Equal("float", method.ReturnType.Name);
        }
    }
}
