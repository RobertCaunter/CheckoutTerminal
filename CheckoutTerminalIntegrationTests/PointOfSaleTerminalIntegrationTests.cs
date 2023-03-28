using CheckoutTerminal;

namespace CheckoutTerminalIntegrationTests {
	public class PointOfSaleTerminalIntegrationTests {
		Dictionary<string, IProductPriceInfo> _pricingData;
		IPricingModel _pricingModel;

		[SetUp]
		public void Setup() {
			_pricingData = new Dictionary<string, IProductPriceInfo>();
			_pricingModel = new DictionaryPricingModel(_pricingData);
			_pricingModel.AddProductPriceInfo(new ProductPriceInfo("A", 1.25m, 3, 3.00m));
			_pricingModel.AddProductPriceInfo(new ProductPriceInfo("B", 4.25m));
			_pricingModel.AddProductPriceInfo(new ProductPriceInfo("C", 1.00m, 6, 5.00m));
			_pricingModel.AddProductPriceInfo(new ProductPriceInfo("D", 0.75m));
		}

		[Test]
		[TestCase(1.25, "A")]
		[TestCase(7.25, "A", "B", "C", "D")]
		[TestCase(6.0, "C", "C", "C", "C", "C", "C", "C")]
		[TestCase(13.25, "A", "B", "C", "D", "A", "B", "A")]
		[TestCase(15.25, "A", "B", "B", "A", "C", "D", "D", "D", "C")]
		[TestCase(2.00, "D", "A")]
		public void PointOfSaleTerminalPricesAreCorrect(decimal expectedPrice, params string[] products) {
			var terminal = new PointOfSaleTerminal(_pricingModel);

			foreach (string product in products) {
				terminal.ScanProduct(product);
			}

			Assert.That(terminal.CalculateTotal(), Is.EqualTo(expectedPrice), "The Point of Sale Terminal has calculated an unexpected price.");
		}

		[Test]
		public void InvalidProductCodeThrowsException() {
			var terminal = new PointOfSaleTerminal(_pricingModel);
			Assert.Throws<InvalidProductException>(() => terminal.ScanProduct(string.Empty), "An invalid product code should result in an exception being thrown.");
		}

		[Test]
		public void MissingPriceDataThrowsException() {
			var terminal = new PointOfSaleTerminal(_pricingModel);
			Assert.Throws<MissingProductPriceException>(() => terminal.ScanProduct("Z"), "Missing price data for a valid product code should result in an exception being thrown.");
		}
	}
}