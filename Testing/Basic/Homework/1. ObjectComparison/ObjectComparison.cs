using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

		// Перепишите код на использование Fluent Assertions.
		// Для решения задачи нам нужен метод BeEquivalentTo.
		// Он сравнивает все публичные поля указанных объектов, поля сопоставляются по именам.
		// Засчёт такого подхода метод будет автоматически добавлять к сравнению новые поля без изменения теста.
		// Читаемость возрастает, так как вместо сравнения в несколько строк у нас используется одна строка.
		// Поля Id изначально не было в сравнении, его следует исключить.
		actualTsar.Should().BeEquivalentTo(expectedTsar, options => options.Excluding((IMemberInfo info) => info.Name == "Id"));
	}

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        // Сложности масштабирования: при появлении новых полей у Person их сравнение придётся дописывать в AreEqual вручную.
        // Если этого не сделать, то тест может выдавать положительный результат, когда это не так.
        // Сложность восприятия: чем больше кода написано, тем сложнее его воспринимать.
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}
