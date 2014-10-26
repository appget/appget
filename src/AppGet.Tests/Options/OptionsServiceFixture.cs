using System;
using AppGet.Commands.ShowFlightPlan;
using AppGet.Options;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Options
{
    [TestFixture]
    public class OptionsServiceFixture : TestBase<OptionsParser>
    {
        [TestCase("showflightplan", typeof(ShowFlightPlanOptions))]
        [TestCase("ShowFlightPlan", typeof(ShowFlightPlanOptions))]
        public void should_parse_verb(string arg, Type optionType)
        {
            var option = Parse(arg);
            option.CommandName.Should().Be(arg);
            option.Should().BeOfType(optionType);
        }

        [TestCase("ShowFlightPlan firefox")]
        public void should_parse_verb_with_package_name(string arg)
        {
            var option = (ShowFlightPlanOptions)Parse(arg);
            option.CommandName.Should().Be("ShowFlightPlan");
            option.PackageName.Should().Be("firefox");
        }

        [TestCase("showflightplan /invalid")]
        [TestCase("showflightplan firefox secondpackage")]
        public void should_throw_with_unknow_params(string args)
        {
            Assert.Throws<UnknownOptionException>(() => Parse(args));
        }

        [TestCase("invalid command")]
        [TestCase("-showflightplan")]
        [TestCase("")]
        public void should_throw_on_unknown_verb(string verb)
        {
            Assert.Throws<UnknownCommandException>(() => Parse(verb));
        }



        private AppGetOption Parse(string args)
        {
            return Subject.Parse(args.Split(' '));
        }
    }
}