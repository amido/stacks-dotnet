﻿using TestStack.BDDfy;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Fixtures;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.steps;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Functional
{
    //Define the story/feature being tested
    [Story(
        AsA = "restaurant administrator",
        IWant = "to be able to create menus",
        SoThat = "customers know what we have on offer")]

    public class CreateMenuTests : IClassFixture<AuthFixture>
    {
        private readonly MenuSteps steps;
        private readonly AuthFixture fixture;

        public CreateMenuTests(AuthFixture fixture)
        {
            //Get instances of the fixture and steps required for the test
            this.fixture = fixture;
            steps = new MenuSteps();
        }

        //Add all tests that make up the story to this class.
        [Fact]
        public void Create_a_menu()
        {
            this.Given(step => fixture.GivenAUser())
                .Given(step => steps.GivenIHaveSpecfiedAFullMenu())
                .When(step => steps.WhenICreateTheMenu())
                .Then(step => steps.ThenTheMenuHasBeenCreated())
                .BDDfy();
        }
    }
}
