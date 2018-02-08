//using System;
//using AppGet.Commands.List;
//using AppGet.Commands.ViewManifest;
//using AppGet.Options;
//using FluentAssertions;
//using NUnit.Framework;
//
//namespace AppGet.Tests.Options
//{
//    public class OptionsParserFixture : TestBase<OptionsParser>
//    {
//        [TestCase("list", typeof(ListOptions))]
//        [TestCase("LIST", typeof(ListOptions))]
//        public void should_parse_verb(string arg, Type optionType)
//        {
//            var option = Parse(arg);
//            option.CommandName.Should().Be(arg);
//            option.Should().BeOfType(optionType);
//        }
//
//        [TestCase("view firefox")]
//        public void should_parse_verb_with_package_name(string arg)
//        {
//            var option = (CreateManifestOptions)Parse(arg);
//            option.CommandName.Should().Be("view");
//            option.PackageId.Should().Be("firefox");
//        }
//
//        [TestCase("view /invalid")]
//        [TestCase("view firefox secondpackage")]
//        public void should_throw_with_unknow_params(string args)
//        {
//            Assert.Throws<UnknownOptionException>(() => Parse(args));
//        }
//
//        [TestCase("invalid command")]
//        [TestCase("-view")]
//        [TestCase("")]
//        public void should_throw_on_unknown_verb(string verb)
//        {
//            Assert.Throws<UnknownCommandException>(() => Parse(verb));
//        }
//
//        [TestCase("list", false)]
//        [TestCase("list -v", true)]
//        [TestCase("list -V", true)]
//        [TestCase("list --v", true)]
//        [TestCase("list --V", true)]
//        public void should_parse_common_args_from_root(string arg, bool verbose)
//        {
//            var option = Parse(arg);
////            option.Verbose.Should().Be(verbose);
//        }
//
//
//        private AppGetOption Parse(string args)
//        {
//            return Subject.Parse(args.Split(' '));
//        }
//    }
//}