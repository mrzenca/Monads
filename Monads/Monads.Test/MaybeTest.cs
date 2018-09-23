using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Monads.Test
{
    [TestClass]
    public class MaybeTest
    {
        private readonly EmployeeRepository repo = new EmployeeRepository();

        [TestMethod]
        public void TestMap()
        {
            var result = Maybe.Invoke(() => repo.Create("Kees"))
                .Map(e => e.Name)
                .Map(s => s.EndsWith("s"))
                .Value;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestFlatMap()
        {
            var result = Maybe.Invoke(() => repo.Create("Kees"))
                .FlatMap(e => e.IsNameKees())
                .Value;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestFailureOnMap()
        {
            var result = Maybe.Invoke(() => repo.Create("Kees"))
                .Map(e => e.WillThrowException());

            Assert.IsTrue(result.IsFailure);
        }

        [TestMethod]
        public void TestRecoverOnFlatMap()
        {
            var result = Maybe.Invoke(() => repo.Create("Kees"))
                .FlatMap(e => e.WillThrowException())
                .Recover(ex => repo.Create("Jaap"));

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Value.Name, "Jaap");
        }

        [TestMethod]
        public void TestSpecificRecoverOnFlatMap()
        {
            var result = Maybe.Invoke(() => repo.Create("Kees"))
                .FlatMap(e => e.WillThrowTryException())
                .Recover<TryException>(e => repo.Create("Jaap"))
                .Recover(ex => repo.Create("Jan"));

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Get().Name, "Jaap");
        }

        [TestMethod]
        public void TestRecoverSkippingSpecificRecoverOnFlatMap()
        {
            var result = Maybe.Invoke(() => repo.Create("Kees"))
                .FlatMap(e => e.WillThrowException())
                .Recover<TryException>(e => repo.Create("Jaap"))
                .Recover(ex => repo.Create("Jan"));

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Value.Name, "Jan");
        }

        [TestMethod]
        public void TestFilter()
        {
            var result = Maybe.Invoke(() => repo.Create("Kees"))
                .Filter(e => e.IsNameKees().Value);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void TestFilterNegative()
        {
            var result = Maybe.Invoke(() => repo.Create("Frits"))
                .Filter(e => e.IsNameKees().Value);

            Assert.IsTrue(result.IsFailure);
        }

        [TestMethod]
        public void TestGetOrElse()
        {
            var result = Maybe.Invoke(() => repo.Create("Kees"))
                .GetOrElse(repo.Create("Hans"));

            Assert.AreEqual(result.Name, "Kees");
        }

        [TestMethod]
        public void TestGetOrElseFails()
        {
            var result = Maybe.Invoke(() => repo.Create("Frits"))
                .Filter(e => e.IsNameKees().Value)
                .GetOrElse(repo.Create("Hans"));

            Assert.AreEqual(result.Name, "Hans");
        }

        [TestMethod]
        public void TestOrElse()
        {
            var result = Maybe.Invoke(() => repo.Create("Kees"))
                .GetOrElse(repo.Create("Hans"));

            Assert.AreEqual(result.Name, "Kees");
        }

        [TestMethod]
        public void TestOrElseFails()
        {
            var result = Maybe.Invoke(() => repo.Create("Frits"))
                .Filter(e => e.IsNameKees().Value)
                .GetOrElse(repo.Create("Hans"));

            Assert.AreEqual(result.Name, "Hans");
        }

        [TestMethod]
        public void TestDoSuccess()
        {
            var result = Maybe.Invoke(() => repo.Create("Frits"))
                .Set(e => e.Name = "Walter")
                .Set(e => e.Age = new DateTime(1979, 4, 15))
                .Get();

            Assert.AreEqual("Walter", result.Name);
            Assert.AreEqual(new DateTime(1979, 4, 15), result.Age);
        }

        [TestMethod]
        public void TestWithSuccess()
        {
            var result = Maybe.Invoke(() => repo.Create("Frits"))
                .Is(e => e.Name == "Walter", i => i.Age = new DateTime(1979, 4, 15))
                .Is(e => e.Name == "Frits", i => i.Age = new DateTime(1979, 4, 16))
                .Get();

            Assert.AreEqual(new DateTime(1979, 4, 16), result.Age);
        }

        [TestMethod]
        public void TestWithSuccess2()
        {
            var result = Maybe.Invoke(() => repo.Create("Walter"))
                .Is(e => e.Name == "Walter", i => i.Age = new DateTime(1979, 4, 15))
                .Is(e => e.Name == "Frits", i => i.Age = new DateTime(1979, 4, 16))
                .Get();

            Assert.AreEqual(new DateTime(1979, 4, 15), result.Age);
        }

        [TestMethod]
        public void TestWithSuccess3()
        {
            var test = 1;

            var result = Maybe.Invoke(() => repo.Create("Frits"))
                .Set(e => e.Age = new DateTime(1979, 4, 15))
                .Is(e => e.Name == "Walter", i => i.Age = new DateTime(1979, 4, 15))
                .Is(e => e.Name == "Frits", _ => test = 2)
                .Get();

            Assert.AreEqual("Frits", result.Name);
            Assert.AreEqual(new DateTime(1979, 4, 15), result.Age);
            Assert.AreEqual(2, test);
        }

        [TestMethod]
        public void TestWithSuccess4()
        {
            var result = Maybe.Invoke(() => repo.Create("Frits"))
                .Is(e => e.Name == "Walter", i => i.Age = new DateTime(1979, 4, 15))
                .Is(e => e.Name == "Frits", i => i.Age = new DateTime(1979, 4, 16))
                .Is(e => e.Name == "Frits", i => i.Age = new DateTime(1979, 4, 17))
                .Get();

            Assert.AreEqual(new DateTime(1979, 4, 16), result.Age);
        }
    }
}
