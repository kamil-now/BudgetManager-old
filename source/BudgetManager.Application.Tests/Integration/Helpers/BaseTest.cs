using System;
using MediatR;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

public abstract class BaseTest : TestBed<TestFixture>
{
  protected IMediator mediator;

  public BaseTest(
    ITestOutputHelper testOutputHelper,
    TestFixture fixture
    ) : base(testOutputHelper, fixture)
  {
    mediator = fixture.GetService<IMediator>(testOutputHelper) ?? throw new ArgumentNullException("IMediator instance is not registered");
  }
}
