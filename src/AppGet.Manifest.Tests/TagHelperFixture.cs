using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Manifest.Tests
{
    [TestFixture]
    public class TagHelperFixture
    {
        [TestCase("package_tag.yaml", ExpectedResult = "tag")]
        [TestCase("package.yaml", ExpectedResult = null)]
        [TestCase("path/dir/package_TAG2.yaml", ExpectedResult = "tag2")]
        [TestCase("path/dir/package-id.yaml", ExpectedResult = null)]
        [TestCase("c:\\path\\dir\\package_TAG2.yaml", ExpectedResult = "tag2")]
        [TestCase("c:\\path\\dir\\package_1.2.3.yaml", ExpectedResult = "1.2.3")]
        [TestCase("pkg:12", ExpectedResult = "12")]
        [TestCase("branch/pkg:12", ExpectedResult = "12")]
        public string should_parse_tag(string input)
        {
            TagHelper.ParseTag(input).Should().Be(TagHelper.ParseTag(input.ToUpper()));

            return TagHelper.ParseTag(input);
        }


        [TestCase("package_tag.yaml", ExpectedResult = "package")]
        [TestCase("package.yaml", ExpectedResult = "package")]
        [TestCase("path/dir/package_tag2.yaml", ExpectedResult = "package")]
        [TestCase("path/dir/packaGE-id.yaml", ExpectedResult = "package-id")]
        [TestCase("c:\\path\\dir\\package_tag2.yaml", ExpectedResult = "package")]
        [TestCase("c:\\path\\dir\\package_1.2.3.yaml", ExpectedResult = "package")]
        [TestCase("pkg:12", ExpectedResult = "pkg")]
        [TestCase("branch/pkg:12", ExpectedResult = "branch/pkg")]
        public string should_parse_id(string input)
        {
            TagHelper.ParseId(input).Should().Be(TagHelper.ParseId(input.ToUpper()));

            return TagHelper.ParseId(input);
        }
    }
}