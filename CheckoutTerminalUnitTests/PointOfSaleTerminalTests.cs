using CheckoutTerminal;
using Moq;

namespace CheckoutTerminalUnitTests {
	public class PointOfSaleTerminalTests {
		Mock<IPricingModel> _pricingModel;

		[SetUp]
		public void Setup() {
			_pricingModel = new Mock<IPricingModel>();
		}

		[Test]
		public void TerminalScansProductSuccessfully() {
			_pricingModel.Setup(p => p.HasPriceDataForProduct(It.IsAny<string>())).Returns(true);

			var terminal = new PointOfSaleTerminal(_pricingModel.Object);
			terminal.ScanProduct("A");
			Assert.That(terminal.GetProductCount("A"), Is.EqualTo(1), "The terminal should have scanned a single item.");
		}

		[Test]
		public void TerminalThrowsExceptionWhenProductHasNoPriceData() {
			_pricingModel.Setup(p => p.HasPriceDataForProduct(It.IsAny<string>())).Returns(false);

			var terminal = new PointOfSaleTerminal(_pricingModel.Object);
			Assert.Throws<MissingProductPriceException>(() => terminal.ScanProduct("A"), "Missing price info should result in an exception.");
		}

		[Test]
		public void EmptyProductCodeResultsInInvalidProductException() {
			_pricingModel.Setup(p => p.HasPriceDataForProduct(It.IsAny<string>())).Returns(true);

			var terminal = new PointOfSaleTerminal(_pricingModel.Object);
			Assert.Throws<InvalidProductException>(() => terminal.ScanProduct(string.Empty), "An empty string should be an invalid product code.");
		}

		[Test]
		[TestCase(2, 2, "A", "B", "B", "A")]
		[TestCase(2, 3, "B", "A", "B", "B", "A")]
		public void TerminalKeepsCorrectProductCounts(int aProductCount, int bProductCount, params string[] products) {
			_pricingModel.Setup(p => p.HasPriceDataForProduct(It.IsAny<string>())).Returns(true);
			_pricingModel.Setup(p => p.GetPrice(It.IsAny<string>(), It.IsAny<int>())).Returns(1);

			var terminal = new PointOfSaleTerminal(_pricingModel.Object);

			foreach (string product in products) {
				terminal.ScanProduct(product);
			}

			Assert.Multiple(() => {
				Assert.That(terminal.GetProductCount("A"), Is.EqualTo(aProductCount), $"Expected the amount of A products to be equal to {aProductCount}");
				Assert.That(terminal.GetProductCount("B"), Is.EqualTo(bProductCount), $"Expected the amount of B products to be equal to {bProductCount}");
			});
		}

		[Test]
		[TestCase(0)]
		[TestCase(1.25, "A")]
		[TestCase(5.50, "A", "B")]
		[TestCase(6.50, "A", "B", "C")]
		[TestCase(7.25, "A", "B", "C", "D")]
		public void TerminalGivesCorrectTotalPrice(decimal expectedPrice, params string[] products) {
			_pricingModel.Setup(p => p.HasPriceDataForProduct(It.IsAny<string>())).Returns(true);
			_pricingModel.Setup(p => p.GetPrice(It.Is<string>(p => p== "A"), It.IsAny<int>())).Returns(1.25m);
			_pricingModel.Setup(p => p.GetPrice(It.Is<string>(p => p== "B"), It.IsAny<int>())).Returns(4.25m);
			_pricingModel.Setup(p => p.GetPrice(It.Is<string>(p => p == "C"), It.IsAny<int>())).Returns(1m);
			_pricingModel.Setup(p => p.GetPrice(It.Is<string>(p => p == "D"), It.IsAny<int>())).Returns(0.75m);

			var terminal = new PointOfSaleTerminal(_pricingModel.Object);

			foreach (string product in products) {
				terminal.ScanProduct(product);
			}

			Assert.Multiple(() => {
				Assert.That(terminal.CalculateTotal(), Is.EqualTo(expectedPrice), $"Expected the total price of the products to be {expectedPrice}");
			});
		}
	}
}