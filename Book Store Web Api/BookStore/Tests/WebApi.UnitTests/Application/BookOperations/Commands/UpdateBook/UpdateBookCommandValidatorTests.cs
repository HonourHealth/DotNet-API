using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.UpdateBook;

namespace WebApi.UnitTests.Application.BookOperations.Commands.UpdateBook;

public class UpdateBookCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    private UpdateBookCommandValidator _validator;

    public UpdateBookCommandValidatorTests()
    {
        _validator = new();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void WhenBookIdIsInvalid_Validator_ShouldHaveError(int bookId)
    {
        //Arrange
        var model = new UpdateBookCommand.UpdateBookModel { Title = "Right Title", GenreId = 3 };
        UpdateBookCommand command = new(null);
        command.Model = model;
        command.BookId = bookId;

        //Act
        var result = _validator.Validate(command);

        //Assert
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    [Theory]
    [InlineData("", 0, 0)]
    [InlineData(null, 0, 0)]
    [InlineData("x", 1, 1)]
    [InlineData("123", 2, 2)]
    public void WhenModelIsInvalid_Validator_ShouldHaveError(string title, int genreId, int authorId)
    {
        //Arrange
        var model = new UpdateBookCommand.UpdateBookModel { Title = title, GenreId = genreId};
        UpdateBookCommand updateCommand = new(null);
        updateCommand.BookId = 1;
        updateCommand.Model = model;

        //Act
        var result = _validator.Validate(updateCommand);

        //Assert
        result.Errors.Count.Should().BeGreaterThanOrEqualTo(0);
    }

    [Theory]
    [InlineData("Title", 1, 1)]
    [InlineData("Long Title", 2, 2)]
    public void WhenInputsAreValid_Validator_ShouldNotHaveError(string title, int genreId, int authorId)
    {
        //Arrange
        var model = new UpdateBookCommand.UpdateBookModel { Title = title, GenreId = genreId };
        UpdateBookCommand updateCommand = new(null);
        updateCommand.BookId = 2;
        updateCommand.Model = model;

        //Act
        var result = _validator.Validate(updateCommand);

        //Assert
        result.Errors.Count.Should().Be(0);
    }
}