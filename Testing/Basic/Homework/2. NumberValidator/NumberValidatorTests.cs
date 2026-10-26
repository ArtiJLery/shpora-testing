
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void Constructor_ShouldThrowException_OnInvalidParameters()
    {
		using (new AssertionScope())
		{
			//AssertionScope выполняет все проверки и выводит отчёт что не прошло.
			Action precisionLessOrEqualThanZero = () => new NumberValidator(-1, 2, true);
			Action scaleNegative = () => new NumberValidator(1, -1, false);
			Action scaleGreaterThanPrecision = () => new NumberValidator(1, 2, false);

			precisionLessOrEqualThanZero.Should().Throw<ArgumentException>().WithMessage("precision must be a positive number");
			scaleNegative.Should().Throw<ArgumentException>().WithMessage("precision must be a non-negative number less or equal than precision");
			scaleGreaterThanPrecision.Should().Throw<ArgumentException>().WithMessage("precision must be a non-negative number less or equal than precision");
		}
    }

    [Test]
    public void IsValidNumber_ShouldReturnExpectedResult()
    {
		using (new AssertionScope())
		{
			//валидные числа
			new NumberValidator(17, 2, true).IsValidNumber("0.0").Should().BeTrue("0.0 is valid positive number");
			new NumberValidator(17, 2, true).IsValidNumber("0").Should().BeTrue("0 is valid positive number");
			new NumberValidator(17, 2, true).IsValidNumber("1,23").Should().BeTrue("commas are also allowed");
			new NumberValidator(4, 2, true).IsValidNumber("+1.23").Should().BeTrue("+1.23 fits precision 4 and scale 2");
			//невалидные числа
			new NumberValidator(17, 2, true).IsValidNumber("0.000").Should().BeFalse("0.000 scale exceeds limit of 2");
			new NumberValidator(3, 2, true).IsValidNumber("+1.23").Should().BeFalse("precision 3 less than 4 (digits + dot + sign)");
			new NumberValidator(3, 2, true).IsValidNumber("-1.23").Should().BeFalse("negative numbers not allowed");
			new NumberValidator(3, 2, true).IsValidNumber("00.00").Should().BeFalse("precision 3 less than 5 (digits + dot)");
			//строка
			new NumberValidator(3, 2, true).IsValidNumber("a.sd").Should().BeFalse("not a number format");
			//пустая строка и null
			new NumberValidator(17, 2, true).IsValidNumber("").Should().BeFalse("should be no empty strings");
			new NumberValidator(17, 2, true).IsValidNumber(null).Should().BeFalse("should be no nulls");
		}
	}
}